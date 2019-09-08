﻿using BIS.PBO;
using System;
using System.Collections.Generic;
using System.IO;

namespace DokanPbo
{
    public class ArchiveManager
    {

        public Dictionary<string, FileEntry> FilePathToFileEntry { get; internal set; }

        public long TotalBytes { get; internal set; }

        public ArchiveManager(string[] folderPaths)
        {
            FilePathToFileEntry = new Dictionary<string, FileEntry>();
            TotalBytes = 0;

            foreach (var folderPath in folderPaths)
            {
                try
                {
                    ReadPboFiles(Directory.GetFiles(folderPath, "*.pbo"));
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void ReadPboFiles(string[] filePaths)
        {
            foreach(var filePath in filePaths)
            {
                var archive = new PBO(filePath);

                foreach (var file in archive.FileEntries)
                {
                    var prefix = "";
                    if (!string.IsNullOrEmpty(archive.Prefix))
                    {
                        prefix = "\\" + archive.Prefix;
                    }

                    var wholeFilePath = (prefix + "\\" + file.FileName).ToLower();
                    FilePathToFileEntry[wholeFilePath] = file;
                    TotalBytes += (long) file.DataSize;
                }
            }
        }
    }
}
