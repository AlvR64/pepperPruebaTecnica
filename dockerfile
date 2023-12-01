#BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Library_Pepper_TechnicalTest/LibraryPepper.API.csproj"
RUN dotnet publish "./Library_Pepper_TechnicalTest/LibraryPepper.API.csproj" -c Release -o /app


#DEPLOY STAGE
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 4300

ENTRYPOINT ["dotnet", "LibraryPepper.API.dll", "--environment=Production"]