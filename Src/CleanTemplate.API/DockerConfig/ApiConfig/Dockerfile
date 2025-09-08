FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Copiar todo o código mantendo a estrutura que as referências esperam
COPY Authentication ./Authentication
COPY Foundation ./Foundation

# Restaurar dependências do projeto API (que referencia Login e Foundation via Login)
RUN dotnet restore Authentication/Src/Authentication.API/Authentication.API.csproj

# Publicar API no modo Release
RUN dotnet publish Authentication/Src/Authentication.API/Authentication.API.csproj -c Release -o /app/out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "Authentication.API.dll"]
