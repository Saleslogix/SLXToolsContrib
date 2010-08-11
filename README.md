**************************************************
QuickFormDiff.csproj
This project is based on SalesLogix 7.5.2.
In order to build it, you must have 7.5.2 versions of your reference assemblies in your [Program Files]\SalesLogix\ReferenceAssemblies folder.
**************************************************
**************************************************
SalesLogixPortalDeploymentUtility.csproj
This project is based on SalesLogix 7.5.2.

The deployment utility requires many of the core platform assemblies to perform correctly. It is recommended that you point the build to your <Program Files>\SalesLogix folder. 
This will allow for the engine to resolve the many needed models and code generation components to successfully deploy the portal (s)

The utility takes a single argument which is a path to the manifest file. The manifest file contains the settings required to build and deploy the portal. The following is some details on the 
settings

VFSPath - This setting points the the targeted model folder. The folder is directory that your VFS lives in PLUS \model. If this path is incorrect the build manager cannot resolve your entities and templates.
OutputPath - Points to where the utility should generate all of the customization assemblies - This needs to also be set for resolution of references and code that is generated. 
BuildCore - Tells the utility if it needs to regenerate the Interfaces and Snippet assemblies. It is recommended that you leave this set as true
PortalName - is the name of the portal that should be deployed (if a single deployment is required). With this options the deployment is based on a File System deployment and no IIS settings will be written
Deployment Path - Is where you what the final portal to be deployed to
Precompile - Determines if the utility should run a precompile operation on the deployed portal. If precompile is selected the portal will be deployed locally, precompiled and then deployed to its final destination
DeploymentName - If more then one portal is to be deployed and/or IIS configuration is required the Deployment name should be set instead of the PortalName

**************************************************