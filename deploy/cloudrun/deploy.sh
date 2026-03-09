#!/usr/bin/env bash
##############################################################################
# deploy.sh — Build, push, and deploy hello-world-dotnet to Cloud Run
#
# Usage:
#   ./deploy/cloudrun/deploy.sh <environment> <region> [image-tag]
#
# Examples:
#   ./deploy/cloudrun/deploy.sh staging us-central1
#   ./deploy/cloudrun/deploy.sh production australia-southeast1
#   ./deploy/cloudrun/deploy.sh production australia-southeast1 v1.2.3
#
# Prerequisites:
#   - gcloud CLI authenticated: gcloud auth login
#   - Docker configured for Artifact Registry:
#       gcloud auth configure-docker REGION-docker.pkg.dev
#   - PROJECT_ID environment variable set, or passed via gcloud config
##############################################################################
set -euo pipefail

ENVIRONMENT="${1:?Usage: deploy.sh <staging|production> <region> [image-tag]}"
REGION="${2:?Usage: deploy.sh <staging|production> <region> [image-tag]}"
IMAGE_TAG="${3:-$(git rev-parse --short HEAD)}"

PROJECT_ID="${PROJECT_ID:-$(gcloud config get-value project)}"
REGISTRY="${REGION}-docker.pkg.dev/${PROJECT_ID}/hello-world-dotnet"
IMAGE="${REGISTRY}/hello-world-dotnet:${IMAGE_TAG}"
REPO_ROOT="$(git rev-parse --show-toplevel)"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Select service manifest
case "$ENVIRONMENT" in
  staging)
    SERVICE_YAML="${SCRIPT_DIR}/service.staging.yaml"
    ;;
  production)
    SERVICE_YAML="${SCRIPT_DIR}/service.production.yaml"
    ;;
  *)
    echo "ERROR: unknown environment '${ENVIRONMENT}'. Use staging or production." >&2
    exit 1
    ;;
esac

echo "==> Environment : ${ENVIRONMENT}"
echo "==> Region      : ${REGION}"
echo "==> Project     : ${PROJECT_ID}"
echo "==> Image       : ${IMAGE}"
echo "==> Manifest    : ${SERVICE_YAML}"
echo ""

# ── 1. Build & push image ─────────────────────────────────────────────────
echo "==> Building Docker image..."
docker build \
  --file "${REPO_ROOT}/deploy/docker/Dockerfile" \
  --tag "${IMAGE}" \
  "${REPO_ROOT}"

echo "==> Pushing image to Artifact Registry..."
docker push "${IMAGE}"

# Also tag as latest for convenience
docker tag "${IMAGE}" "${REGISTRY}/hello-world-dotnet:latest"
docker push "${REGISTRY}/hello-world-dotnet:latest"

# ── 2. Patch image tag into service YAML and deploy ───────────────────────
echo "==> Deploying to Cloud Run (${REGION})..."
sed \
  -e "s|REGION-docker.pkg.dev/PROJECT_ID/hello-world-dotnet/hello-world-dotnet:IMAGE_TAG|${IMAGE}|g" \
  "${SERVICE_YAML}" \
| gcloud run services replace - \
    --region "${REGION}" \
    --project "${PROJECT_ID}"

# ── 3. Allow unauthenticated access (idempotent) ──────────────────────────
SERVICE_NAME="hello-world-dotnet$([ "$ENVIRONMENT" = "staging" ] && echo "-staging" || echo "")"
gcloud run services add-iam-policy-binding "${SERVICE_NAME}" \
  --region "${REGION}" \
  --project "${PROJECT_ID}" \
  --member="allUsers" \
  --role="roles/run.invoker" \
  2>/dev/null || true

# ── 4. Print service URL ───────────────────────────────────────────────────
SERVICE_URL=$(gcloud run services describe "${SERVICE_NAME}" \
  --region "${REGION}" \
  --project "${PROJECT_ID}" \
  --format="value(status.url)")

echo ""
echo "==> Deployment complete."
echo "==> Service URL: ${SERVICE_URL}"
