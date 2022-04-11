﻿using DBHelper;
using FileBroker.Business;
using FileBroker.Data;
using FileBroker.Data.DB;
using FOAEA3.Common.Brokers;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Resources.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Outgoing.FileCreator.Fed.LicenceDenial
{
    class Program
    {
        static void Main(string[] args)
        {
            ColourConsole.WriteEmbeddedColorLine("Starting Federal Outgoing Licence Denial Files Creator");

            string aspnetCoreEnvironment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args);

            IConfiguration configuration = builder.Build();

            string fileBrokerConnectionString = configuration.GetConnectionString("MessageBroker").ReplaceVariablesWithEnvironmentValues();
            var fileBrokerDB = new DBTools(fileBrokerConnectionString);
            var apiRootForFiles = configuration.GetSection("APIroot").Get<ApiConfig>();

            CreateOutgoingFederalLicenceDenialFiles(fileBrokerDB, apiRootForFiles);

            ColourConsole.Write("Completed.");
            ColourConsole.WriteEmbeddedColorLine("[yellow]Press any key to close[/yellow]");
            Console.ReadKey();

        }

        private static void CreateOutgoingFederalLicenceDenialFiles(DBTools fileBrokerDB, ApiConfig apiRootForFiles)
        {
            var apiBrokers = new APIBrokerList
            {
                ApplicationEventAPIBroker = new ApplicationEventAPIBroker(new APIBrokerHelper(apiRootForFiles.FoaeaApplicationRootAPI)),
                LicenceDenialApplicationAPIBroker = new LicenceDenialApplicationAPIBroker(new APIBrokerHelper(apiRootForFiles.FoaeaLicenceDenialRootAPI)),
                LicenceDenialResponseAPIBroker = new LicenceDenialResponseAPIBroker(new APIBrokerHelper(apiRootForFiles.FoaeaLicenceDenialRootAPI)),
                LicenceDenialEventAPIBroker = new LicenceDenialEventAPIBroker(new APIBrokerHelper(apiRootForFiles.FoaeaLicenceDenialRootAPI)),
                SinAPIBroker = new SinAPIBroker(new APIBrokerHelper(apiRootForFiles.FoaeaApplicationRootAPI))
            };

            var repositories = new RepositoryList
            {
                FileTable = new DBFileTable(fileBrokerDB),
                FlatFileSpecs = new DBFlatFileSpecification(fileBrokerDB),
                OutboundAuditDB = new DBOutboundAudit(fileBrokerDB),
                ErrorTrackingDB = new DBErrorTracking(fileBrokerDB),
                ProcessParameterTable = new DBProcessParameter(fileBrokerDB),
                MailServiceDB = new DBMailService(fileBrokerDB)
            };

            var federalFileManager = new OutgoingFederalLicenceDenialManager(apiBrokers, repositories);

            var federalLicenceDenialOutgoingSources = repositories.FileTable.GetFileTableDataForCategory("LICOUT")
                                                                  .Where(s => s.Active == true);

            var allErrors = new Dictionary<string, List<string>>();
            foreach (var federalLicenceDenialOutgoingSource in federalLicenceDenialOutgoingSources)
            {
                string filePath = federalFileManager.CreateOutputFile(federalLicenceDenialOutgoingSource.Name,
                                                                      out List<string> errors);
                allErrors.Add(federalLicenceDenialOutgoingSource.Name, errors);
                if (errors.Count == 0)
                    ColourConsole.WriteEmbeddedColorLine($"Successfully created [cyan]{filePath}[/cyan]");
            }

            if (allErrors.Count > 0)
                foreach (var error in allErrors)
                    ColourConsole.WriteEmbeddedColorLine($"Error creating [cyan]{error.Key}[/cyan]: [red]{error.Value}[/red]");
        }
    }
}
