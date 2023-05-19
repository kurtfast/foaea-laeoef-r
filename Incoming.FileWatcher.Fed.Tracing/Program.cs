﻿using DBHelper;
using FileBroker.Model;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Resources.Helpers;
using Incoming.Common;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Incoming.FileWatcher.Fed.Tracing
{
    class Program
    {
        private static IncomingFederalTracingFile FederalFileManager;

        static void Main(string[] args)
        {
            ColourConsole.WriteEmbeddedColorLine("Starting [cyan]Ontario[/cyan] File Monitor");

            string aspnetCoreEnvironment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args);

            IConfiguration configuration = builder.Build();

            var fileBrokerDB = new DBTools(configuration.GetConnectionString("MessageBroker").ReplaceVariablesWithEnvironmentValues());
            var apiRootForFiles = configuration.GetSection("APIroot").Get<ApiConfig>();
            var apiAction = new APIBrokerHelper();

            FederalFileManager = new(fileBrokerDB, apiRootForFiles, apiAction);

            string ftpRoot = configuration["FTProot"];

            var allNewFiles = new Dictionary<string, FileTableData>();
            AppendNewFilesFrom(ref allNewFiles, ftpRoot + @"\EI3STS"); // NETP
            AppendNewFilesFrom(ref allNewFiles, ftpRoot + @"\HR3STS"); // EI Tracing
            AppendNewFilesFrom(ref allNewFiles, ftpRoot + @"\RC3STS"); // CRA Tracing

            if (allNewFiles.Count > 0)
            {
                ColourConsole.WriteEmbeddedColorLine($"Found [green]{allNewFiles.Count}[/green] file(s)");
                foreach (var newFile in allNewFiles)
                {
                    ColourConsole.WriteEmbeddedColorLine($"Processing [green]{newFile.Key}[/green]...");
                    FederalFileManager.ProcessNewFile(newFile.Key);
                }
            }
            else
                ColourConsole.WriteEmbeddedColorLine("[yellow]No new files found.[/yellow]");
        }

        private static void AppendNewFilesFrom(ref Dictionary<string, FileTableData> allNewFiles, string filePath)
        {
            var newFiles = FederalFileManager.GetNewFiles(filePath);
            foreach (var newFile in newFiles)
                allNewFiles.Add(newFile.Key, newFile.Value);
        }
    }
}
