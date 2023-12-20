

namespace FOAEA3.IVR.Helpers
{
    public class IVRConfigurationHelper
    {
        public string FoaeaConnection { get; }
        
        public List<string> ProductionServers { get; }

        public IVRConfigurationHelper(string[] args = null)
        {
            string aspnetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                //.AddJsonFile("FoaeaConfiguration.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"FoaeaConfiguration.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{aspnetCoreEnvironment}.json", optional: true, reloadOnChange: true);
            ;

            if (args is not null)
                builder = builder.AddCommandLine(args);

            IConfiguration configuration = builder.Build();

            FoaeaConnection = configuration.GetConnectionString("FOAEAMain").ReplaceVariablesWithEnvironmentValues();

            //test only
            //FoaeaIVRConnection = configuration.GetConnectionString("FOAEAIVRMain").ReplaceVariablesWithEnvironmentValues();

            //Recipients = configuration.GetSection("RecipientsConfig").Get<RecipientsConfig>();
   
        }

    }
}
