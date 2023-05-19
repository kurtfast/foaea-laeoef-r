﻿using FOAEA3.Business.Areas.Application;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using FOAEA3.Resources.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FOAEA3.API.Interception.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InterceptionsController : ControllerBase
    {
        private readonly CustomConfig config;

        public InterceptionsController(IOptions<CustomConfig> config)
        {
            this.config = config.Value;
        }

        [HttpGet("Version")]
        public ActionResult<string> Version()
        {
            return Ok("FOAEA3.API.Interception API Version 1.4");
        }

        [HttpGet("{key}")]
        public ActionResult<InterceptionApplicationData> GetApplication([FromRoute] string key,
                                                                        [FromServices] IRepositories repositories,
                                                                        [FromServices] IRepositories_Finance repositoriesFinance)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var applKey = new ApplKey(key);

            var manager = new InterceptionManager(repositories, repositoriesFinance, config);

            bool success = manager.LoadApplication(applKey.EnfSrv, applKey.CtrlCd);
            if (success)
            {
                if (manager.InterceptionApplication.AppCtgy_Cd == "I01")
                    return Ok(manager.InterceptionApplication);
                else
                    return NotFound($"Error: requested I01 application but found {manager.InterceptionApplication.AppCtgy_Cd} application.");
            }
            else
                return NotFound();

        }

        [HttpPut("{key}/Vary")]
        public ActionResult<InterceptionApplicationData> Vary([FromRoute] string key,
                                                              [FromServices] IRepositories repositories,
                                                              [FromServices] IRepositories_Finance repositoriesFinance)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var applKey = new ApplKey(key);

            var application = APIBrokerHelper.GetDataFromRequestBody<InterceptionApplicationData>(Request);

            var appManager = new InterceptionManager(application, repositories, repositoriesFinance, config);
            if (appManager.VaryApplication())
                return Ok(application);
            else
                return UnprocessableEntity(application);
        }

        [HttpPut("{key}/SINbypass")]
        public ActionResult<InterceptionApplicationData> SINbypass([FromRoute] string key,
                                                                   [FromServices] IRepositories repositories,
                                                                   [FromServices] IRepositories_Finance repositoriesFinance)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var applKey = new ApplKey(key);

            var sinBypassData = APIBrokerHelper.GetDataFromRequestBody<SINBypassData>(Request);

            var application = new InterceptionApplicationData();

            var appManager = new InterceptionManager(application, repositories, repositoriesFinance, config);
            appManager.LoadApplication(applKey.EnfSrv, applKey.CtrlCd);

            var sinManager = new ApplicationSINManager(application, appManager);
            sinManager.SINconfirmationBypass(sinBypassData.NewSIN, repositories.CurrentSubmitter, false, sinBypassData.Reason);

            return Ok(application);
        }

    }
}
