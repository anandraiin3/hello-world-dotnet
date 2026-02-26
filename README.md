# hello-world-dotnet

A "hello world" application written in .NET Core.

## Running the Application

From the `hello-world-dotnet` subdirectory, run:

```bash
$ dotnet run
```

### Docker

From the `hello-world-dotnet` subdirectory, run:

```bash
$ docker build . -t hello-world-dotnet:latest
$ docker run -it --rm -e ASPNETCORE_URLS=http://+:5000 -e ASPNETCORE_ENVIRONMENT=Development -p 5000:5000 hello-world-dotnet:latest
```

### Docker Compose

From the `hello-world-dotnet` subdirectory, run:

```bash
$ docker-compose up --build
```

## Using the Application

This application serves a simple JSON payload at the root directory:

```bash
$ curl http://localhost:5000
```

## Pulling from the Registry

A Docker image for this application is also available on GitHub Container Registry. You can pull this image like so:

```bash
$ docker pull ghcr.io/liatrio/hello-world-dotnet:latest
```

Similar to the instructions above, this can be ran locally like this:

```bash
$ docker run -it --rm -e ASPNETCORE_URLS=http://+:5000 -e ASPNETCORE_ENVIRONMENT=Development -p 5000:5000 ghcr.io/liatrio/hello-world-dotnet:latest
```
