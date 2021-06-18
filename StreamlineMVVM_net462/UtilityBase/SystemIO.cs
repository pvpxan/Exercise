using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StreamlineMVVM
{
    // START SystemIO Class -------------------------------------------------------------------------------------------------------------
    public enum PathType
    {
        Invalid,
        File,
        Directory,
    }

    public class FileCopyResult
    {
        public string FilePath { get; set; } = "";
        public bool Success { get; set; } = true;
    }

    public class DirectoryCreateResult
    {
        public string DirectoryPath { get; set; } = "";
        public bool Success { get; set; } = true;
    }

    public class CopyResults
    {
        public FileCopyResult[] FileResults { get; set; }
        public DirectoryCreateResult[] DirectoryResults { get; set; }
        public bool Errors { get; set; } = false;
    }

    // Safely wrapped System.IO stuff that is commonly used.
    public static class SystemIO
    {
        public static PathType GetPathType(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return PathType.Invalid;
            }

            if (File.Exists(path))
            {
                return PathType.File;
            }

            if (Directory.Exists(path))
            {
                return PathType.Directory;
            }

            return PathType.Invalid;
        }

        public static bool Delete(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                    return true;
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error deleting file: " + file, Ex);
                    return false;
                }
            }

            return false;
        }

        public static bool Copy(string fileSource, string fileTarget, bool overwrite)
        {
            try
            {
                File.Copy(fileSource, fileTarget, overwrite);
                return true;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error copying source file: " + fileSource + " to target desitnation path: " + fileTarget, Ex);
                return false;
            }
        }

        public static bool CreateDirectory(string directory)
        {
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error creating directory: : " + directory, Ex);
                return false;
            }
        }

        private class CopyWorkResults
        {
            public List<FileCopyResult> FileResults { get; set; } = new List<FileCopyResult>();
            public List<DirectoryCreateResult> DirectoryResults { get; set; } = new List<DirectoryCreateResult>();
            public bool Errors { get; set; } = false;
        }

        public static CopyResults CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            CopyResults copyResults = new CopyResults();

            try
            {
                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

                CopyWorkResults copyWorkResults = copyDirecoryWork(diSource, diTarget);
                copyResults.Errors = copyWorkResults.Errors;
                copyResults.DirectoryResults = copyWorkResults.DirectoryResults.ToArray();
                copyResults.FileResults = copyWorkResults.FileResults.ToArray();
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error processing Copy method.", Ex);
            }

            return copyResults;
        }

        private static CopyWorkResults copyDirecoryWork(DirectoryInfo source, DirectoryInfo target)
        {
            CopyWorkResults output = copyFiles(source, target);

            DirectoryInfo[] sourceSubDirectoryList = null;
            try
            {
                sourceSubDirectoryList = source.GetDirectories();
            }
            catch (Exception Ex)
            {
                output.Errors = true;
                LogWriter.Exception("Error processing Copy method.", Ex);
                return output;
            }

            // Copy each source subdirectory using recursion.
            foreach (DirectoryInfo subDirectoryInfo in sourceSubDirectoryList)
            {
                DirectoryInfo newSubDirectoryInfo = null;
                try
                {
                    newSubDirectoryInfo = new DirectoryInfo(Path.Combine(target.FullName, subDirectoryInfo.Name));
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error creating new subdirectory info object.", Ex);
                    continue;
                }

                if (newSubDirectoryInfo == null)
                {
                    LogWriter.LogEntry("Unable to process current source subdirectory.");
                    continue;
                }

                CopyWorkResults copyWorkResults = copyDirecoryWork(subDirectoryInfo, newSubDirectoryInfo);
                output.Errors = copyWorkResults.Errors;
                try
                {
                    output.DirectoryResults.AddRange(copyWorkResults.DirectoryResults);
                    output.FileResults.AddRange(copyWorkResults.FileResults);
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error with updating return results with recursive subdirectory function.", Ex);
                }
            }

            return output;
        }

        private static CopyWorkResults copyFiles(DirectoryInfo source, DirectoryInfo target)
        {
            CopyWorkResults copyWorkResults = new CopyWorkResults();

            string targetPath = "";
            try
            {
                targetPath = target.FullName;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error attempting to read target directory path. Operation cancelled.", Ex);

                copyWorkResults.Errors = true;
                return copyWorkResults;
            }

            if (target.Exists == false)
            {
                try
                {
                    target.Create();
                    copyWorkResults.DirectoryResults.Add(new DirectoryCreateResult() { DirectoryPath = targetPath, Success = true, });
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error creating base directory. Copy operation cancelled. Directory Name: " + targetPath, Ex);

                    copyWorkResults.Errors = true;
                    return copyWorkResults;
                }
            }

            FileInfo[] directoryFiles = null;
            try
            {
                directoryFiles = source.GetFiles();
            }
            catch (Exception Ex)
            {
                // This is safety in case the source path FullName cannot be read.
                string sourcePath = "";
                try
                {
                    sourcePath = source.FullName;
                }
                catch
                {
                    // Do nothing.
                }

                LogWriter.Exception("Source directory not found unable to copy files. Operation cancelled. Source Directory: " + sourcePath, Ex);
                return null;
            }

            if (directoryFiles.Length < 1)
            {
                return copyWorkResults;
            }

            FileCopyResult[] copyResults = new FileCopyResult[directoryFiles.Length];

            // Copy each file FROM the source directory into the target path.
            for (int i = 0; i < directoryFiles.Length; i++)
            {
                FileCopyResult fileResult = new FileCopyResult();

                try
                {
                    fileResult.FilePath = directoryFiles[i].FullName;
                    directoryFiles[i].CopyTo(Path.Combine(targetPath, directoryFiles[i].Name), true);
                }
                catch (Exception Ex)
                {
                    fileResult.Success = false;
                    LogWriter.Exception("Failed to copy file: " + directoryFiles[i].FullName, Ex);
                }

                copyWorkResults.FileResults.Add(fileResult);
            }

            return copyWorkResults;
        }
    }
    // END SystemIO Class ---------------------------------------------------------------------------------------------------------------
}
