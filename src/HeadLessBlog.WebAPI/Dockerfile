# Use the official .NET 8 SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the csproj and restore as distinct layers
COPY src/HeadLessBlog.WebAPI/*.csproj ./src/HeadLessBlog.WebAPI/
COPY src/HeadLessBlog.Application/*.csproj ./src/HeadLessBlog.Application/
COPY src/HeadLessBlog.Infrastructure/*.csproj ./src/HeadLessBlog.Infrastructure/
COPY src/HeadLessBlog.Domain/*.csproj ./src/HeadLessBlog.Domain/

RUN dotnet restore src/HeadLessBlog.WebAPI/HeadLessBlog.WebAPI.csproj

# Copy everything else and build
COPY . .

RUN dotnet publish src/HeadLessBlog.WebAPI/HeadLessBlog.WebAPI.csproj -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HeadLessBlog.WebAPI.dll"]
