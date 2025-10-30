# 🔄 CI/CD Pipeline Documentation

## Overview

This document describes the Continuous Integration and Continuous Deployment pipeline for the Authentication Service.

## Build Pipeline

The project uses GitHub Actions for automated builds and testing.

### Pipeline Stages

1. **Build**: Compile the .NET solution
2. **Test**: Run unit and integration tests
3. **Code Quality**: Run code analysis and linting
4. **Package**: Create deployment artifacts

## Workflow Files

The CI/CD workflows are defined in `.github/workflows/` directory.

## CI/CD Fixes

For information about CI/CD fixes and improvements, see [FIXES.md](FIXES.md).

## Build Status

Current build status can be checked at: [GitHub Actions](https://github.com/maiconcardozo/Authentication/actions)

## Deployment

Deployment strategies and procedures are documented in the [Deployment Guide](../guides/DEPLOYMENT.md).

## Troubleshooting

For CI/CD issues and solutions, refer to:
- [CI/CD Fixes](FIXES.md)
- [Troubleshooting Guide](../reference/TROUBLESHOOTING.md)
- [Project Status](../status/PROJECT_STATUS.md)

---

For the latest pipeline configuration details, check the workflow files in the `.github/workflows/` directory.
