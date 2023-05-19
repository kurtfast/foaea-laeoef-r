﻿using DBHelper;
using FileBroker.Data.DB;
using FileBroker.Model;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using FOAEA3.Resources.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Incoming.Common
{
    public class IncomingFederalTracingFile
    {
        private DBFileTable FileTable { get; }
        private ApiConfig ApiFilesConfig { get; }
        private IAPIBrokerHelper APIHelper { get; }

        public IncomingFederalTracingFile(IDBTools fileBrokerDB,
                                          ApiConfig apiFilesConfig,
                                          IAPIBrokerHelper apiHelper)
        {
            ApiFilesConfig = apiFilesConfig;
            APIHelper = apiHelper;
            FileTable = new DBFileTable(fileBrokerDB);
        }

        public Dictionary<string, FileTableData> GetNewFiles(string rootPath)
        {
            var newFiles = new Dictionary<string, FileTableData>();

            var directory = new DirectoryInfo(rootPath);
            var allFiles = directory.GetFiles("*IT.*");
            var last31days = DateTime.Now.AddDays(-31);
            var files = allFiles.Where(f => f.LastWriteTime > last31days).OrderByDescending(f => f.LastWriteTime);

            foreach (var fileInfo in files)
            {
                int cycle = FileHelper.GetCycleFromFilename(fileInfo.Name);
                var fileNameNoCycle = Path.GetFileNameWithoutExtension(fileInfo.Name); // remove cycle
                var fileTableData = FileTable.GetFileTableDataForFileName(fileNameNoCycle);

                if ((cycle == fileTableData.Cycle) && (fileTableData.Active.HasValue) && (fileTableData.Active.Value))
                    newFiles.Add(fileInfo.FullName, fileTableData);
            }

            return newFiles;
        }

        public bool ProcessNewFile(string fullPath)
        {
            bool fileProcessedSuccessfully = false;

            string fileNameNoPath = Path.GetFileName(fullPath);

            if (fullPath.ToUpper()[6] == 'I') // incoming file have a I in 7th position (e.g. EI3STSIT.000022)
            {                                 //                                                    ↑ 

                string flatFile;
                using (var streamReader = new StreamReader(fullPath, Encoding.UTF8))
                {
                    flatFile = streamReader.ReadToEnd();
                }

                // send json to processor api

                var response = APIHelper.PostFlatFile($"api/v1/FederalTracingFiles?fileName={fileNameNoPath}", flatFile, ApiFilesConfig.IncomingFederalTracingRootAPI);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    ColourConsole.WriteEmbeddedColorLine($"[red]Error: {response.Content?.ReadAsStringAsync().Result}[/red]");
                else
                    ColourConsole.WriteEmbeddedColorLine($"[green]{response.Content?.ReadAsStringAsync().Result}[/green]");

                fileProcessedSuccessfully = true;

            }
            else
            {
                // TODO: generate Unknown file name exception or ignore?
            }

            return fileProcessedSuccessfully;
        }

    }
}
