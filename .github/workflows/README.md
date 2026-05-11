# GitHub Actions Workflows for PyTrain

This repository uses GitHub Actions to build, test, and deploy PyTrain.

## Available Workflows

### 1. Build and Deploy (Main Application)

The main workflow (`build-and-deploy.yml`) handles building the PyTrain application and deploying it to various targets.

#### Triggering the Workflow

This workflow is manually triggered. To start it:

1. Go to the "Actions" tab in your repository
2. Select "Build and Deploy" from the list of workflows
3. Click "Run workflow"
4. Choose your deployment target:
   - **preview**: Deploys to Docker Hub with the `preview` tag
   - **production**: Deploys to Docker Hub with the `latest` tag
   - **github_release**: Creates a GitHub release with artifacts
5. Optionally specify a custom version number
6. Choose whether to create a GitHub Release

#### Workflow Steps

1. Builds the PyTrain application using the Build.ps1 script
2. Runs tests
3. Signs the executables (if code signing certificate is available)
4. Creates artifacts
5. Deploys to the selected target

### 2. Relay Server

The relay server workflow (`relay-server.yml`) handles building and deploying the WebSocket Relay server.

#### Triggering the Workflow

This workflow can be triggered:

- Automatically on push to the `main` branch
- Manually with workflow dispatch

For manual triggering:

1. Go to the "Actions" tab in your repository
2. Select "Relay Server" from the list of workflows
3. Click "Run workflow"
4. Choose your deployment target:
   - **preview**: Deploys to Docker Hub with the `preview` tag
   - **production**: Deploys to Docker Hub with the `latest` tag

#### Workflow Steps

1. Builds the WebSocket Relay Server
2. Creates and pushes Docker images with appropriate tags

## Required Secrets

For these workflows to function properly, you need to set up the following repository secrets:

- `DOCKER_USERNAME`: Your Docker Hub username
- `DOCKER_PAT`: Your Docker Hub access token
- `CERTIFICATE_THUMBPRINT`: Thumbprint of the code signing certificate (optional)
- `SIGNTOOL_BINARY`: Base64-encoded SignTool executable (optional)

## GitHub Release Assets

When creating a GitHub Release, the following assets are included:

- `PyTrain.Server.[version].zip`: Server application
- `docker-compose.yaml`: Docker Compose file

## Docker Images

Docker images are published to a private container registry. Update the
publish-docker workflow with your registry path before running it.

- `<registry>/pytrain:preview` - Preview/development version
- `<registry>/pytrain:latest` - Production version
- `<registry>/pytrain:[version]` - Specific version
- `<registry>/pytrain-relay:preview` - Preview relay server
- `<registry>/pytrain-relay:latest` - Production relay server
- `<registry>/pytrain-relay:[version]` - Specific version of relay server
