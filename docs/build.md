# Goal of Build Automation
The build automation shall label and tag a release artifact for publishing to Azure Artifacts.

## Initialization
- Checkout source code
- Install DotNet 6x
- Initialize build variables: version, build date, build branch, commit id

## Update info.json
- Generate JSON file named 'info.json' and add to root of Mako project.
- Insert build variables into info.json.

## Package
- Use Build Configuration and output project
- Tag codebase with current version
- Zip archive
- label build artifact with version

## Publish to Azure Artifacts
- Push artifact to Azure Artifacts as DEV promotion
- Inform team new artifact has been published
