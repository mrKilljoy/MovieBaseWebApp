using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace CinemaStorage.Infrastructure
{
    public static class PosterLocationHelper
    {
        private static string _folderName = "posters";

        public static string PostersDirectoryPath
        {
            get { return Path.Combine(HttpRuntime.AppDomainAppPath, "Content", _folderName); }
        }

        #region Methods
        public static bool IsFolderExists()
        {
            string core_folder_path = HttpRuntime.AppDomainAppPath;
            string poster_folder_path = Path.Combine(core_folder_path, "Content", _folderName);

            return Directory.Exists(poster_folder_path);
        }

        public static void FolderInitialize()
        {
            Directory.CreateDirectory(PostersDirectoryPath);
        }

        public static bool IsFileUploaded(string short_file_name)
        {
            string file_path = MakeAbsoluteImagePath(short_file_name);

            return File.Exists(file_path);
        }
        
        public static string MakeAbsoluteImagePath(string short_file_name)
        {
            return Path.Combine(PostersDirectoryPath, short_file_name);
        }

        public static string MakeRelativeImagePath(string short_file_name)
        {
            return Path.Combine("\\Content", _folderName, short_file_name);
        }
        #endregion
    }
}