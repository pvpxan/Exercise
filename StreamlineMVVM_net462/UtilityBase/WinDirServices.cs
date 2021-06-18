using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace StreamlineMVVM
{
    public static class WinDirServices
    {
        private static string directoryServicesPath = "WinNT://" + Environment.MachineName + ",computer";

        public static List<string> GetUserList()
        {
            List<string> users = new List<string>();
            DirectoryEntry directoryEntry = null;
            try
            {
                directoryEntry = new DirectoryEntry(directoryServicesPath);

                foreach (DirectoryEntry childEntry in directoryEntry.Children)
                {
                    if (childEntry.SchemaClassName == "User")
                    {
                        users.Add(childEntry.Name);
                    }
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error getting local system user list.", Ex);
            }
            finally
            {
                if (directoryEntry != null)
                {
                    directoryEntry.Dispose();
                }
            }

            return users;
        }

        public static bool CheckUserExists(string username)
        {
            bool exists = false;
            List<string> users = GetUserList();

            try
            {
                exists = users.Any(u => u.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error checking if local system user exists.", Ex);
            }

            return exists;
        }
    }
}
