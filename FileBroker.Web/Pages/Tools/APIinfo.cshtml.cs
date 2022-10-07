using FileBroker.Common.Brokers;
using FileBroker.Common;
using FOAEA3.Common.Brokers;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using FOAEA3.Model.Interfaces.Broker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FileBroker.Web.Pages.Tools
{
    public class APIinfoModel : PageModel
    {
        private ApiConfig ApiConfig { get; }

        public Dictionary<string, ApiInfo> ApiData { get; }

        public APIinfoModel(IOptions<ApiConfig> apiConfig)
        {
            ApiConfig = apiConfig.Value;
            ApiData = new Dictionary<string, ApiInfo>();
        }

        public async Task OnGet([FromServices] IConfiguration config)
        {
            var applicationApiHelper = new APIBrokerHelper(ApiConfig.FoaeaApplicationRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var interceptionApiHelper = new APIBrokerHelper(ApiConfig.FoaeaInterceptionRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var licenceDenialApiHelper = new APIBrokerHelper(ApiConfig.FoaeaLicenceDenialRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var tracingApiHelper = new APIBrokerHelper(ApiConfig.FoaeaTracingRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");

            var fb_accountApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerAccountRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");

            var fb_interceptionApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerMEPInterceptionRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var fb_licenceDenialApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerMEPLicenceDenialRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var fb_tracingApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerMEPTracingRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");

            var fb_FederalInterceptionApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerFederalInterceptionRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var fb_FederalLicenceDenialApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerFederalLicenceDenialRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var fb_FederalTracingApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerFederalTracingRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");
            var fb_FederalSINApiHelper = new APIBrokerHelper(ApiConfig.FileBrokerFederalSINRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");

            var fb_BackendProcessesHelper = new APIBrokerHelper(ApiConfig.BackendProcessesRootAPI, currentSubmitter: "MSGBRO", currentUser: "MSGBRO");

            string foaeaToken = await TokenHelper.GetFoaeaApiTokenAsync(applicationApiHelper, config);

            await AddVersionFor("FOAEA3.API", applicationApiHelper, new ApplicationAPIBroker(applicationApiHelper, foaeaToken));
            await AddVersionFor("FOAEA3.API.Interception", interceptionApiHelper, new InterceptionApplicationAPIBroker(interceptionApiHelper, foaeaToken));
            await AddVersionFor("FOAEA3.API.LicenceDenial", licenceDenialApiHelper, new LicenceDenialApplicationAPIBroker(licenceDenialApiHelper, foaeaToken));
            await AddVersionFor("FOAEA3.API.Tracing", tracingApiHelper, new TracingApplicationAPIBroker(tracingApiHelper, foaeaToken));

            await AddVersionFor("BackendProcesses.API", fb_BackendProcessesHelper, new BackendProcessesAPIBroker(fb_BackendProcessesHelper, foaeaToken));

            string fileBrokerToken = await TokenHelper.GetFileBrokerApiTokenAsync(fb_accountApiHelper, config);

            await AddVersionFor("FileBroker.API.MEP.Interception", fb_interceptionApiHelper, new MEPInterceptionAPIBroker(fb_interceptionApiHelper, fileBrokerToken));
            await AddVersionFor("FileBroker.API.MEP.LicenceDenial", fb_licenceDenialApiHelper, new MEPLicenceDenialAPIBroker(fb_licenceDenialApiHelper, fileBrokerToken));
            await AddVersionFor("FileBroker.API.MEP.Tracing", fb_tracingApiHelper, new MEPTracingAPIBroker(fb_tracingApiHelper, fileBrokerToken));

            await AddVersionFor("FileBroker.API.FED.Interception", fb_FederalInterceptionApiHelper, new FEDInterceptionAPIBroker(fb_FederalInterceptionApiHelper, fileBrokerToken));
            await AddVersionFor("FileBroker.API.FED.LicenceDenial", fb_FederalLicenceDenialApiHelper, new FEDLicenceDenialAPIBroker(fb_FederalLicenceDenialApiHelper, fileBrokerToken));
            await AddVersionFor("FileBroker.API.FED.Tracing", fb_FederalTracingApiHelper, new FEDTracingAPIBroker(fb_FederalTracingApiHelper, fileBrokerToken));
            await AddVersionFor("FileBroker.API.FED.SIN", fb_FederalSINApiHelper, new FEDSINAPIBroker(fb_FederalSINApiHelper, fileBrokerToken));
        }

        private async Task AddVersionFor(string apiName, IAPIBrokerHelper helper, IVersionSupport broker)
        {

            // TODO: fix token
            bool isError = !GetVersionDescription(await broker.GetVersionAsync(), out string versionDescription);
            string connectionString = string.Empty;
            if (!isError)
                connectionString = await broker.GetConnectionAsync();

            var apiInfo = new ApiInfo
            {
                ApiRoot = helper.APIroot,
                ApiVersion = versionDescription,
                IsError = isError,
                ConnectionString = GetDatabaseInfo(connectionString)
            };

            ApiData.Add(apiName, apiInfo);
        }

        private static bool GetVersionDescription(string version, out string versionDescription)
        {
            if (!string.IsNullOrEmpty(version))
            {
                versionDescription = version;
                return true;
            }
            else
            {
                versionDescription = $"Version could not be found. Is API running?";
                return false;
            }
        }

        private static string GetDatabaseInfo(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString.Trim()))
                return string.Empty;

            static string GetDBValue(string info)
            {
                string result = info;

                string[] values = info.Split('=');
                if (values.Length == 2)
                    result = values[1];

                return result;
            }

            string[] dbInfo = connectionString.Split(';');
            string server = string.Empty;
            string database = string.Empty;
            foreach (string info in dbInfo)
            {
                if (info.StartsWith("server", StringComparison.OrdinalIgnoreCase))
                    server = GetDBValue(info);
                else if (info.StartsWith("database", StringComparison.OrdinalIgnoreCase))
                    database = GetDBValue(info);
            }

            return $@"{database.ToUpper()} on {server.ToUpper()}";
        }
    }
}
