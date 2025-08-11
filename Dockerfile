# --- Stage 1: The Build Stage ---
# We start with a base image that contains the full .NET 8 SDK, which is needed to build our project.
# We give this stage a name, "build", so we can refer to it later.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container.
WORKDIR /src

# Copy all the .csproj files first. This is a Docker optimization.
# If these files don't change, Docker can reuse this layer from its cache, making future builds much faster.
COPY ["SentimentAnalysis.Api/SentimentAnalysis.Api.csproj", "SentimentAnalysis.Api/"]
COPY ["SentimentAnalysis.Core/SentimentAnalysis.Core.csproj", "SentimentAnalysis.Core/"]
COPY ["SentimentAnalysis.Infrastructure/SentimentAnalysis.Infrastructure.csproj", "SentimentAnalysis.Infrastructure/"]
COPY ["SentimentAnalysis.Tests/SentimentAnalysis.Tests.csproj", "SentimentAnalysis.Tests/"]
COPY ["ModelTrainer/ModelTrainer.csproj", "ModelTrainer/"]
COPY ["SentimentAnalysis.sln", "."]

# Restore all NuGet packages.
RUN dotnet restore "SentimentAnalysis.sln"

# Copy the rest of the source code into the container.
COPY . .

# Set the working directory to the API project folder.
WORKDIR "/src/SentimentAnalysis.Api"

# Build and publish the application, creating a "release" version.
# The output will be placed in the /app/publish folder.
RUN dotnet publish "SentimentAnalysis.Api.csproj" -c Release -o /app/publish

# --- Stage 2: The Final/Runtime Stage ---
# We now start fresh with a much smaller base image that only contains the ASP.NET runtime, not the SDK.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory.
WORKDIR /app

# Expose port 8080. This tells Docker that our application inside the container will be listening on this port.
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 

ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the compiled application from the "build" stage (from /app/publish) into our new, small final image.
COPY --from=build /app/publish .

# This is the command that will be executed when the container starts.
# It runs our application's DLL.
ENTRYPOINT ["dotnet", "SentimentAnalysis.Api.dll"]