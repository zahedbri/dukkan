﻿using System;
using System.IO;
using System.Linq;
using Abp.Reflection.Extensions;

namespace Dukkan.Web
{
    /// <summary>
    /// This class is used to find root path of the web project in;
    /// unit tests (to find views) and entity framework core command line commands (to find conn string).
    /// </summary>
    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(DukkanCoreSharedModule).GetAssembly().Location);
            if (coreAssemblyDirectoryPath == null)
            {
                throw new Exception("Could not find location of Dukkan.Core assembly!");
            }

            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
            while (!DirectoryContains(directoryInfo.FullName, "Dukkan.sln"))
            {
                directoryInfo = directoryInfo.Parent ?? throw new Exception("Could not find content root folder!");
            }

            var webPublicFolder = Path.Combine(directoryInfo.FullName, "src", "Dukkan.Web.Public");
            if (Directory.Exists(webPublicFolder))
            {
                return webPublicFolder;
            }

            var webApiFolder = Path.Combine(directoryInfo.FullName, "src", "Dukkan.Web.Api");
            if (Directory.Exists(webApiFolder))
            {
                return webApiFolder;
            }

            throw new Exception("Could not find root folder of the web project!");
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }
}
