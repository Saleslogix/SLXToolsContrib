//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sage Software">
//     Copyright (c) Sage Software. All rights reserved.
//		This code may not be copied or used, except as set out in a written licence agreement
// 		between the user and Sage Software, which specifically permits the user to use
// 		this code. 
// </copyright>
//-----------------------------------------------------------------------


using System;
using log4net;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using SalesLogix.Deployment.Properties;

namespace SalesLogix.Deployment
{
    /// <summary>
    /// Main entry point for the application
    /// </summary>
    class Program
    {

        #region Private Fields
        private static readonly ILog log = LogManager.GetLogger("DeploymentUtility.Program");
        #endregion

        #region Entry Point Support 
        public static void Main(string[] args)
        {
            try
            {

                if (args.Length == 1)
                {

                    string manifestPath = args[0];
                    BuildManifest manifest = GetBuildManifest(manifestPath);
                    if (manifest != null)
                    {
                        List<string> errors = ValidateManifest(manifest);
                        if (errors.Count == 0)
                        {
                            // Create and execute the build manager 
                            BuildManager manager = new BuildManager(manifest);
                            manager.Build();
                        }
                        else
                        {
                            Console.WriteLine("The Provided Manifest has some invalid or missing settings. The following validation errors were generated.");
                            errors.ForEach(e => Console.WriteLine(e));
                            Environment.Exit(1);
                        }
                    }

                } else {
                    Console.WriteLine("Manifest file was not specified for processing.");
                    Environment.Exit(1);
                }

            }
            catch (Exception ex)
            {
                log.Error("An error occurred attempting to build interfaces", ex);
                Environment.Exit(1);
            }

            Environment.Exit(0);

        }
        #endregion


        #region Manifest Support 
        /// <summary>
        /// Reads the build manifest from the file specified in the command line arguments
        /// </summary>
        /// <param name="manifestPath"></param>
        /// <returns></returns>
        private static BuildManifest GetBuildManifest(string manifestPath)
        {
            BuildManifest manifest = null;

            if (String.IsNullOrEmpty(manifestPath))
                throw new ArgumentException("manifestPath is null or empty.", "manifestPath");

            if (!File.Exists(manifestPath))
                throw new FileNotFoundException("Manifest file was not found in the path specified");

            XmlSerializer serializer = new XmlSerializer(typeof(BuildManifest));
            using (var reader = new StreamReader(manifestPath))
            {
                manifest = (BuildManifest)serializer.Deserialize(reader);
            }

            return manifest;
        }


        private static List<string> ValidateManifest(BuildManifest manifest)
        {

            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(manifest.ConnectionString))
                errors.Add(Resources.err_connection_not_provided);

            // Validate that the manifest contains valid information 


            return errors;

        }
        #endregion

    }
}
