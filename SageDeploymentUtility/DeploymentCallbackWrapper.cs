//-----------------------------------------------------------------------
// <copyright file="DeploymentCallbackWrapper.cs" company="Sage Software">
//     Copyright (c) Sage Software. All rights reserved.
//		This code may not be copied or used, except as set out in a written licence agreement
// 		between the user and Sage Software, which specifically permits the user to use
// 		this code. 
// </copyright>
//-----------------------------------------------------------------------


using System;
using System.ComponentModel;
using System.Reflection;
using log4net;
using Sage.Platform.Deployment;

namespace SalesLogix.Deployment
{

    /// <summary>
    /// Allows access to async process
    /// </summary>
    public class DeploymentCallbackWrapper
    {

        bool Running = false;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region CTOR
        /// <summary>
        /// Initializes a new instance of the DeploymentCallbackWrapper class.
        /// </summary>
        public DeploymentCallbackWrapper()
        {
        }
        #endregion
        
        /// <summary>
        /// Executes the portal deployment process         
        /// </summary>
        /// <param name="deployment"></param>
        public void DeployPortal(Sage.Platform.Deployment.Deployment deployment) {

            PortalDeploymentManager manager = new PortalDeploymentManager();
            BackgroundWorker worker = manager.GetDeploymentBackgroundWorker();
            DeploymentArgs deploymentArgs = new DeploymentArgs(deployment, false);

            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            Running = true;
            worker.RunWorkerAsync(deploymentArgs);

            while (Running)
            {
                System.Threading.Thread.Sleep(1000);
            }
            
        }

        /// <summary>
        /// Log any messages out to the log for next info window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string message = (string)e.UserState;
            log.Info(message);
        }

        /// <summary>
        /// Deployment is Completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Running = false;
        }


    }
}
