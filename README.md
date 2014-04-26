CreateUnicodeDB
Tool to convert a ANSI single byte database to Unicode - varchar to nvarchar, etc.
Also will convert the project model types to Unicode types.

Note, that if your database already contains multiple multi-byte data it will not convert the data to unicode.  This will need to be modified to handle those situations.



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

VFSPath - This setting points the targeted model folder. The folder is directory that your VFS lives in PLUS \model. If this path is incorrect the build manager cannot resolve your entities and templates.
OutputPath - Points to where the utility should generate all of the customization assemblies - This needs to also be set for resolution of references and code that is generated. 
BuildCore - Tells the utility if it needs to regenerate the Interfaces and Snippet assemblies. It is recommended that you leave this set as true
PortalName - is the name of the portal that should be deployed (if a single deployment is required). With this options the deployment is based on a File System deployment and no IIS settings will be written
Deployment Path - Is where you what the final portal to be deployed to
Precompile - Determines if the utility should run a precompile operation on the deployed portal. If precompile is selected the portal will be deployed locally, precompiled and then deployed to its final destination
DeploymentName - If more than one portal is to be deployed and/or IIS configuration is required the Deployment name should be set instead of the PortalName

**************************************************
**************************************************

QuickDeploymentModule.csproj
This project should work with SalesLogix 7.5.2 or 7.5.3 and possibly other versions.

This is an admin module that contains an optimized and stripped down version of the normal deployment command in the application architect.
It introduces a new main menu item labeled "Deploy" with a child menu item "Deploy to portal target".
A dialog will popup to let you choose a target portal from your debug deployment (typically core portals).
This is meant to be used by developers as a more efficient incremental deployment tool.

There are several things missing from this deployment command:
There is no support for cancelling a deployment,
Virtual directories are not configured,
Deployment manifests are not used (in fact they are deleted if found),
There is no cleanup step (obsolete files like renames are not removed)
No ServiceHosts.xml file is generated (used for process orchestration???),
No progress dialog is shown - messages are shown in output window instead

To try it out, drop the build dll into the application architect's Modules folder.

**************************************************
