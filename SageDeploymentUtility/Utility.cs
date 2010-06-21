//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="Sage Software">
//     Copyright (c) Sage Software. All rights reserved.
//		This code may not be copied or used, except as set out in a written licence agreement
// 		between the user and Sage Software, which specifically permits the user to use
// 		this code. 
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Data.OleDb;

namespace SalesLogix.Deployment
{
    public class Utility
    {
        /// <summary>
        /// Determines if the server can be connected to
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        protected internal static bool CanConnect(string connString)
        {
            bool connect = false;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();

                    using (OleDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "Select count(*) from sysdba.Account";
                        command.ExecuteScalar();

                        connect = true;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot connect to the underlying database");
            }

            return connect;
        }
    }
}
