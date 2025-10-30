# 🚀 Deployment Guide

This guide covers deployment strategies for the Authentication service across different environments and platforms. The service includes comprehensive authentication and Role-Based Access Control (RBAC) capabilities.

## 🏗️ Deployment Overview

The Authentication service can be deployed in various configurations:

- **Development**: Local development environment with full CRUD API access
- **Staging**: Pre-production testing environment  
- **Production**: Live production environment with secure RBAC
- **Docker**: Containerized deployment
- **Cloud**: Azure, AWS, Google Cloud Platform
- **Kubernetes**: Container orchestration

## 🔐 New RBAC Features Deployment Considerations

The service now includes a comprehensive RBAC system that requires additional configuration:

### Database Schema Updates

Ensure your database includes all RBAC tables:
- `Claims` - Permission and role definitions
- `Actions` - System action definitions  
- `ClaimActions` - Permission-to-action mappings
- `AccountClaimActions` - User permission assignments

### API Endpoints Security

The new CRUD endpoints require proper authorization:
- All `/Claim/*`, `/Action/*`, `/ClaimAction/*`, and `/AccountClaimAction/*` endpoints require valid JWT tokens
- Consider implementing role-based access for administrative endpoints
- Set up proper CORS configuration for frontend access

### Default Permissions Setup

For first deployment, consider creating default:
- Basic user claims (`user:read`, `user:write`)
- Administrative claims (`admin:full`)
- System actions (`CreateUser`, `UpdateUser`, `DeleteUser`)
- Default claim-action mappings

## 📋 Prerequisites

### System Requirements

- **.NET 8.0 Runtime**: Required for running the application
- **MySQL 8.0+**: Database server
- **Redis** (Optional): For caching and session storage
- **Load Balancer** (Production): For high availability
- **SSL Certificate**: For HTTPS in production

### Hardware Requirements

#### Minimum (Development/Testing)
- **CPU**: 2 cores
- **RAM**: 4 GB
- **Storage**: 20 GB
- **Network**: 100 Mbps

#### Recommended (Production)
- **CPU**: 4-8 cores
- **RAM**: 8-16 GB  
- **Storage**: 100 GB SSD
- **Network**: 1 Gbps

### Pre-Deployment Checklist (Enhanced for PR #40)

Before deploying to any environment, ensure:

- [ ] **Database backup created** (especially important for unique constraint migration)
- [ ] **Duplicate usernames resolved** in existing data
- [ ] **Migration tested** in staging environment
- [ ] **Error handling verified** for 409 Conflict responses
- [ ] **Internationalization configured** for target language (English/Portuguese)
- [ ] **Status code compliance tested** for all endpoints

## 🔐 Database Schema Updates (PR #40)

### Important: Unique Constraint Migration

**PR #40** introduced a UNIQUE constraint on the `Account.UserName` column to prevent duplicate usernames. This requires special attention during deployment:

#### Pre-Migration Requirements
Before applying the migration in production:

1. **Identify Duplicate Usernames:**
```sql
-- Check for existing duplicate usernames
SELECT UserName, COUNT(*) as DuplicateCount 
FROM Account 
GROUP BY UserName 
HAVING COUNT(*) > 1;
```

2. **Resolve Duplicates:**
```sql
-- Option 1: Update duplicate usernames with suffixes
UPDATE Account 
SET UserName = CONCAT(UserName, '_', Id) 
WHERE UserName IN (
    SELECT UserName FROM (
        SELECT UserName FROM Account 
        GROUP BY UserName 
        HAVING COUNT(*) > 1
    ) AS duplicates
) AND Id NOT IN (
    SELECT MIN(Id) FROM Account 
    GROUP BY UserName
);

-- Option 2: Manual resolution (recommended for production)
-- Review and manually update duplicate usernames case by case
```

#### Migration Files
- **Development**: `20250805000000_AddUniqueConstraintToUserName.cs`
- **Production**: `20250805000000_AddUniqueConstraintToUserName.cs`

#### Migration Commands
```bash
# Development environment
dotnet ef database update --context ApiContextDevelopment

# Production environment  
dotnet ef database update --context ApiContextProduction
```

#### Rollback Plan
If migration fails, rollback using:
```sql
-- Remove unique constraint manually if needed
DROP INDEX IX_Account_UserName ON Account;
```

### Development Environment

1. **Install .NET 8.0 SDK**
```bash
# Windows (using winget)
winget install Microsoft.DotNet.SDK.8

# macOS (using Homebrew)
brew install --cask dotnet

# Linux (Ubuntu/Debian)
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install dotnet-sdk-8.0
```

2. **Install MySQL**
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install mysql-server-8.0

# macOS
brew install mysql

# Windows
# Download and install from: https://dev.mysql.com/downloads/mysql/
```

3. **Configure Database**
```sql
CREATE DATABASE AuthenticationDB;
CREATE USER 'authuser'@'localhost' IDENTIFIED BY 'secure_password';
GRANT ALL PRIVILEGES ON AuthenticationDB.* TO 'authuser'@'localhost';
FLUSH PRIVILEGES;
```

4. **Configure Application**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB;Uid=authuser;Pwd=secure_password;"
  },
  "JwtSettings": {
    "Issuer": "AuthenticationService",
    "Audience": "AuthenticationClients",
    "SecretKey": "your-256-bit-secret-key-here-minimum-32-characters-long",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

5. **Run Migrations**
```bash
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment
```

6. **Start Application**
```bash
cd Solution
dotnet run --project ../Src/Authentication.API
```

### Production Environment

#### 1. Application Configuration

**appsettings.Production.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-db-server;Database=AuthenticationDB;Uid=authuser;Pwd=${DB_PASSWORD};"
  },
  "JwtSettings": {
    "Issuer": "AuthenticationService",
    "Audience": "AuthenticationClients", 
    "SecretKey": "${JWT_SECRET_KEY}",
    "ExpirationMinutes": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com,*.yourdomain.com"
}
```

#### 2. Environment Variables

```bash
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS="https://0.0.0.0:443;http://0.0.0.0:80"
export DB_PASSWORD="your_secure_database_password"
export JWT_SECRET_KEY="your-super-secure-256-bit-jwt-secret-key-here"
```

#### 3. SSL Certificate Configuration

```bash
# Generate self-signed certificate (development only)
dotnet dev-certs https --export-path ./certificates/aspnetapp.pfx --password mypassword123

# For production, use Let's Encrypt or commercial certificate
certbot certonly --standalone -d api.yourdomain.com
```

## 🐳 Docker Deployment

### Important: Foundation Dependency

Before building the Docker image, ensure both repositories are cloned to the same parent directory:

```bash
# Clone both repositories
git clone https://github.com/maiconcardozo/Foundation.git
git clone https://github.com/maiconcardozo/Authentication.git

# Directory structure should be:
# Parent Directory/
# ├── Foundation/
# │   └── Src/Foundation.Base/
# └── Authentication/
#     └── Src/
```

### Dockerfile

The Docker build must be executed from the parent directory containing both repositories. Create `Dockerfile` in the Authentication repository root, but build from the parent directory:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files (paths relative to parent directory)
COPY ["Authentication/Src/Authentication.API/Authentication.API.csproj", "Authentication/Src/Authentication.API/"]
COPY ["Authentication/Src/Authentication.Login/Authentication.Login.csproj", "Authentication/Src/Authentication.Login/"]
COPY ["Foundation/Src/Foundation.Base/Foundation.Base.csproj", "Foundation/Src/Foundation.Base/"]
COPY ["Authentication/Solution/Authentication.sln", "Authentication/Solution/"]

# Restore dependencies
RUN dotnet restore "Authentication/Solution/Authentication.sln"

# Copy source code
COPY Authentication/ Authentication/
COPY Foundation/ Foundation/

# Build application
WORKDIR "/src/Authentication/Src/Authentication.API"
RUN dotnet build "Authentication.API.csproj" -c Release -o /app/build

# Publish application
FROM build AS publish
RUN dotnet publish "Authentication.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user
RUN groupadd -r authgroup && useradd -r -g authgroup authuser
USER authuser

# Copy published application
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Start application
ENTRYPOINT ["dotnet", "Authentication.API.dll"]
```

### Building the Docker Image

Build the Docker image from the parent directory containing both repositories:

```bash
# Navigate to the parent directory containing both repositories
cd /path/to/parent-directory

# Build the Docker image
docker build -f Authentication/Dockerfile -t authentication-api .

# Run the container
docker run -d \
  --name authentication-api \
  -p 5001:80 \
  -p 5443:443 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e "ConnectionStrings__DefaultConnection=Server=localhost;Database=AuthenticationDB;Uid=root;Pwd=password;" \
  authentication-api
```

### Docker Compose

Create `docker-compose.yml`:

```yaml
version: '3.8'

services:
  authentication-api:
    build: 
      context: .  # Build from parent directory containing both repositories
      dockerfile: Authentication/Dockerfile
    ports:
      - "5001:80"
      - "5443:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - ConnectionStrings__DefaultConnection=Server=db;Database=AuthenticationDB;Uid=root;Pwd=${MYSQL_ROOT_PASSWORD};
      - JwtSettings__SecretKey=${JWT_SECRET_KEY}
      - JwtSettings__Issuer=AuthenticationService
      - JwtSettings__Audience=AuthenticationClients
      - JwtSettings__ExpirationMinutes=30
    depends_on:
      db:
        condition: service_healthy
    volumes:
      - ./certificates:/https:ro
    networks:
      - auth-network
    restart: unless-stopped
    
  db:
    image: mysql:8.0
    environment:
      - MYSQL_ROOT_PASSWORD=${MYSQL_ROOT_PASSWORD}
      - MYSQL_DATABASE=AuthenticationDB
      - MYSQL_USER=authuser
      - MYSQL_PASSWORD=${MYSQL_PASSWORD}
    ports:
      - "3306:3306"
    volumes:
      - mysql_data:/var/lib/mysql
      - ./scripts/init.sql:/docker-entrypoint-initdb.d/init.sql:ro
    networks:
      - auth-network
    healthcheck:
      test: ["CMD", "mysqladmin", "ping", "-h", "localhost", "-u", "root", "-p${MYSQL_ROOT_PASSWORD}"]
      interval: 30s
      timeout: 10s
      retries: 5
      start_period: 30s
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    networks:
      - auth-network
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 30s
      timeout: 10s
      retries: 5
    restart: unless-stopped

volumes:
  mysql_data:
    driver: local
  redis_data:
    driver: local

networks:
  auth-network:
    driver: bridge
```

### Environment File

Create `.env` file:

```bash
# Database
MYSQL_ROOT_PASSWORD=your_secure_root_password
MYSQL_PASSWORD=your_secure_user_password

# JWT
JWT_SECRET_KEY=your-super-secure-256-bit-jwt-secret-key-here-minimum-32-characters

# Application
ASPNETCORE_ENVIRONMENT=Production
```

### Build and Deploy

```bash
# Build and start services
docker-compose up -d

# View logs
docker-compose logs -f authentication-api

# Run database migrations
docker-compose exec authentication-api dotnet ef database update

# Scale application (multiple instances)
docker-compose up -d --scale authentication-api=3
```

## ☁️ Cloud Deployment

### Azure App Service

#### 1. Create Azure Resources

```bash
# Login to Azure
az login

# Create resource group
az group create --name rg-authentication --location eastus

# Create App Service plan
az appservice plan create \
  --name plan-authentication \
  --resource-group rg-authentication \
  --sku P1V2 \
  --is-linux

# Create web app
az webapp create \
  --name app-authentication \
  --resource-group rg-authentication \
  --plan plan-authentication \
  --runtime "DOTNETCORE:9.0"

# Create MySQL database
az mysql server create \
  --name mysql-authentication \
  --resource-group rg-authentication \
  --location eastus \
  --admin-user authuser \
  --admin-password YourSecurePassword123 \
  --sku-name GP_Gen5_2
```

#### 2. Configure Application Settings

```bash
# Set connection string
az webapp config connection-string set \
  --name app-authentication \
  --resource-group rg-authentication \
  --connection-string-type MySql \
  --settings DefaultConnection="Server=mysql-authentication.mysql.database.azure.com;Database=AuthenticationDB;Uid=authuser@mysql-authentication;Pwd=YourSecurePassword123;SslMode=Required;"

# Set application settings
az webapp config appsettings set \
  --name app-authentication \
  --resource-group rg-authentication \
  --settings \
    JwtSettings__SecretKey="your-super-secure-256-bit-jwt-secret-key-here" \
    JwtSettings__Issuer="AuthenticationService" \
    JwtSettings__Audience="AuthenticationClients" \
    JwtSettings__ExpirationMinutes="30"
```

#### 3. Deploy Application

```bash
# Build and publish
dotnet publish -c Release -o ./publish

# Create deployment package
cd publish
zip -r ../deployment.zip .

# Deploy to Azure
az webapp deployment source config-zip \
  --name app-authentication \
  --resource-group rg-authentication \
  --src deployment.zip
```

### AWS Elastic Beanstalk

#### 1. Prepare Application

Create `aws-windows-deployment-manifest.json`:

```json
{
  "manifestVersion": 1,
  "deployments": {
    "aspNetCoreWeb": [
      {
        "name": "authentication-api",
        "parameters": {
          "appBundle": "Authentication.API.zip",
          "iisPath": "/",
          "iisWebSite": "Default Web Site"
        }
      }
    ]
  }
}
```

#### 2. Deploy with EB CLI

```bash
# Install EB CLI
pip install awsebcli

# Initialize Elastic Beanstalk application
eb init authentication-api --platform "64bit Amazon Linux 2 v2.2.0 running .NET Core" --region us-east-1

# Create environment
eb create production-auth --database.engine mysql --database.username authuser

# Deploy application
eb deploy
```

### Google Cloud Platform

#### 1. Create GKE Cluster

```bash
# Create cluster
gcloud container clusters create authentication-cluster \
  --zone=us-central1-a \
  --num-nodes=3 \
  --machine-type=e2-medium

# Get credentials
gcloud container clusters get-credentials authentication-cluster --zone=us-central1-a
```

#### 2. Deploy with Kubernetes

Create `k8s/deployment.yaml`:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: authentication-api
spec:
  replicas: 3
  selector:
    matchLabels:
      app: authentication-api
  template:
    metadata:
      labels:
        app: authentication-api
    spec:
      containers:
      - name: authentication-api
        image: gcr.io/your-project/authentication-api:latest
        ports:
        - containerPort: 80
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: auth-secrets
              key: connection-string
        - name: JwtSettings__SecretKey
          valueFrom:
            secretKeyRef:
              name: auth-secrets
              key: jwt-secret
---
apiVersion: v1
kind: Service
metadata:
  name: authentication-service
spec:
  selector:
    app: authentication-api
  ports:
    - protocol: TCP
      port: 80
      targetPort: 80
  type: LoadBalancer
```

```bash
# Deploy to Kubernetes
kubectl apply -f k8s/
```

## 🔄 CI/CD Pipeline

### GitHub Actions

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy Authentication API

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
        
    - name: Restore dependencies
      run: dotnet restore Solution/Authentication.sln
      
    - name: Build
      run: dotnet build Solution/Authentication.sln --no-restore
      
    - name: Test
      run: dotnet test Solution/Authentication.sln --no-build --verbosity normal

  build-and-deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
        
    - name: Build and publish
      run: |
        dotnet publish Src/Authentication.API/Authentication.API.csproj \
          -c Release \
          -o ./publish \
          --no-restore
          
    - name: Build Docker image
      run: |
        docker build -t authentication-api:${{ github.sha }} .
        docker tag authentication-api:${{ github.sha }} authentication-api:latest
        
    - name: Deploy to production
      run: |
        # Add your deployment commands here
        echo "Deploying to production..."
```

### Azure DevOps

Create `azure-pipelines.yml`:

```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'

stages:
- stage: Build
  jobs:
  - job: Build
    steps:
    - task: UseDotNet@2
      inputs:
        packageType: 'sdk'
        version: '9.0.x'
        
    - task: DotNetCoreCLI@2
      displayName: 'Restore packages'
      inputs:
        command: 'restore'
        projects: 'Solution/Authentication.sln'
        
    - task: DotNetCoreCLI@2
      displayName: 'Build application'
      inputs:
        command: 'build'
        projects: 'Solution/Authentication.sln'
        arguments: '--configuration $(buildConfiguration) --no-restore'
        
    - task: DotNetCoreCLI@2
      displayName: 'Run tests'
      inputs:
        command: 'test'
        projects: 'Solution/Authentication.sln'
        arguments: '--configuration $(buildConfiguration) --no-build'
        
    - task: DotNetCoreCLI@2
      displayName: 'Publish application'
      inputs:
        command: 'publish'
        projects: 'Src/Authentication.API/Authentication.API.csproj'
        arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
        
    - task: PublishBuildArtifacts@1
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'
        ArtifactName: 'drop'

- stage: Deploy
  dependsOn: Build
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  jobs:
  - deployment: Deploy
    environment: 'production'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            inputs:
              azureSubscription: 'Azure Subscription'
              appType: 'webApp'
              appName: 'authentication-api'
              package: '$(Pipeline.Workspace)/drop/**/*.zip'
```

## 📊 Monitoring and Logging

### Application Insights (Azure)

Add to `Program.cs`:

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

### Prometheus Metrics

Install `prometheus-net.AspNetCore`:

```csharp
app.UseHttpMetrics();
app.MapMetrics();
```

### Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddDbContext<LoginContext>()
    .AddMySql(connectionString);

app.MapHealthChecks("/health");
```

## 🔒 Security Considerations

### Production Security Checklist

- [ ] Use HTTPS everywhere
- [ ] Secure JWT secret keys (256-bit minimum)
- [ ] Enable request rate limiting
- [ ] Configure CORS appropriately
- [ ] Use secure password hashing (Argon2)
- [ ] Enable security headers
- [ ] Regular security updates
- [ ] Database connection encryption
- [ ] Implement audit logging
- [ ] Use secrets management (Azure Key Vault, AWS Secrets Manager)

### Network Security

```bash
# Firewall rules (Ubuntu/CentOS)
sudo ufw allow 22/tcp    # SSH
sudo ufw allow 80/tcp    # HTTP
sudo ufw allow 443/tcp   # HTTPS
sudo ufw deny 3306/tcp   # MySQL (internal only)
sudo ufw enable
```

## 🔧 Troubleshooting

### Common Issues

1. **Database Connection Failed**
   - Check connection string
   - Verify MySQL server is running
   - Check firewall rules
   - Verify user permissions

2. **JWT Token Invalid**
   - Check secret key length (minimum 256 bits)
   - Verify issuer and audience settings
   - Check token expiration time

3. **Application Won't Start**
   - Check .NET 8.0 runtime is installed
   - Verify all required environment variables
   - Check application logs
   - Ensure ports are available

### Logging Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    },
    "Console": {
      "IncludeScopes": true
    },
    "File": {
      "Path": "./logs/app.log",
      "RollingInterval": "Day"
    }
  }
}
```

## 📞 Support

For deployment support:

- **Documentation**: [Deployment Wiki](../../wiki/deployment)
- **Issues**: [GitHub Issues](../../issues)
- **DevOps Support**: devops@yourdomain.com