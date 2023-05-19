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

namespace Outgoing.FileCreator.Fed.Tracing
{
    class Program
    {
        static void Main(string[] args)
        {
            ColourConsole.WriteEmbeddedColorLine("Starting Federal Outgoing File Creator");

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

            CreateOutgoingFederalTracingFiles(fileBrokerDB, apiRootForFiles);

            ColourConsole.Write("Completed.");
            ColourConsole.WriteEmbeddedColorLine("[yellow]Press any key to close[/yellow]");
            Console.ReadKey();

        }

        private static void CreateOutgoingFederalTracingFiles(DBTools fileBrokerDB, ApiConfig apiRootForFiles)
        {
            var apiBrokers = new APIBrokerList
            {
                ApplicationEventAPIBroker = new ApplicationEventAPIBroker(new APIBrokerHelper(apiRootForFiles.ApplicationRootAPI)),
                TracingApplicationAPIBroker = new TracingApplicationAPIBroker(new APIBrokerHelper(apiRootForFiles.TracingRootAPI)),
                TraceResponseAPIBroker = new TraceResponseAPIBroker(new APIBrokerHelper(apiRootForFiles.TracingRootAPI)),
                TracingEventAPIBroker = new TracingEventAPIBroker(new APIBrokerHelper(apiRootForFiles.TracingRootAPI)),
                SinAPIBroker = new SinAPIBroker(new APIBrokerHelper(apiRootForFiles.ApplicationRootAPI))
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

            var federalFileManager = new OutgoingFederalTracingManager(apiBrokers, repositories);

            var federalTraceOutgoingSources = repositories.FileTable.GetFileTableDataForCategory("TRCOUT")
                                                .Where(s => s.Active == true);

            var allErrors = new Dictionary<string, List<string>>();
            foreach (var federalTraceOutgoingSource in federalTraceOutgoingSources)
            {
                string filePath = federalFileManager.CreateOutputFile(federalTraceOutgoingSource.Name,
                                                                      out List<string> errors);
                allErrors.Add(federalTraceOutgoingSource.Name, errors);
                if (errors.Count == 0)
                    ColourConsole.WriteEmbeddedColorLine($"Successfully created [cyan]{filePath}[/cyan]");
            }

            if (allErrors.Count > 0)
                foreach (var error in allErrors)
                    ColourConsole.WriteEmbeddedColorLine($"Error creating [cyan]{error.Key}[/cyan]: [red]{error.Value}[/red]");
        }
    }
}
