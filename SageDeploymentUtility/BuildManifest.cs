//-----------------------------------------------------------------------
// <copyright file="BuildManifest.cs" company="Sage Software">
//     Copyright (c) Sage Software. All rights reserved.
//		This code may not be copied or used, except as set out in a written licence agreement
// 		between the user and Sage Software, which specifically permits the user to use
// 		this code. 
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace SalesLogix.Deployment
{
    /// <summary>
    /// Provides access to the build settings manifest
    /// </summary>
    [Serializable]
    public class BuildManifest
    {

        #region Public Properties
        /// <summary>
        /// gets/sets SalesLogix Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets/Sets the VFS path. If this not set it will use the Database
        /// VFS by default
        /// </summary>
        public string VFSPath { get; set; }

        /// <summary>
        /// Gets/Sets the output path generated files
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets/Sets Path to deploy the portal out. Used when the deployment name
        /// is not provided
        /// </summary>
        public string DeploymentPath { get; set; }

        /// <summary>
        /// Gets/Sets the name of the portal to deploy 
        /// </summary>
        public string PortalName { get; set; }

        /// <summary>
        /// Gets/sets the name of the deployment (multiple portals/configuration to deploy)
        /// </summary>
        public string DeploymentName { get; set; }

        /// <summary>
        /// Gets/Sets if a full build of core components should occur
        /// </summary>
        public bool BuildCore { get; set; }

        /// <summary>
        /// Gets/Sets if a precompile should occurr on a file based deployment 
        /// </summary>
        public bool Precompile { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the BuildManifest class.
        /// </summary>
        public BuildManifest()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the BuildManifest class.
        /// </summary>
        public BuildManifest(string connectionString, string vFSPath, string outputPath, string deploymentPath, string portalName, string deploymentName, bool buildCore, bool precompile)
        {
            ConnectionString = connectionString;
            VFSPath = vFSPath;
            OutputPath = outputPath;
            DeploymentPath = deploymentPath;
            PortalName = portalName;
            DeploymentName = deploymentName;
            BuildCore = buildCore;
            Precompile = precompile;
        }
    }
}
