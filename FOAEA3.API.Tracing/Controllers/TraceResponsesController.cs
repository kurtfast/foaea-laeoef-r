﻿using FOAEA3.Business.Areas.Application;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Base;
using FOAEA3.Model.Interfaces;
using FOAEA3.Resources.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace FOAEA3.API.Tracing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TraceResponsesController : ControllerBase
    {
        private readonly CustomConfig config;

        public TraceResponsesController(IOptions<CustomConfig> config)
        {
            this.config = config.Value;
        }

        [HttpGet("{id}")]
        public ActionResult<DataList<TraceResponseData>> GetTraceResults([FromRoute] string id,
                                                                         [FromServices] IRepositories repositories)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var applKey = new ApplKey(id);

            var manager = new TracingManager(repositories, config);

            if (manager.LoadApplication(applKey.EnfSrv, applKey.CtrlCd))
                return Ok(manager.GetTraceResults());
            else
                return NotFound();
        }

        [HttpPost("bulk")]
        public ActionResult<int> CreateTraceResponsesBulk([FromServices] IRepositories repositories)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var responseData = APIBrokerHelper.GetDataFromRequestBody<List<TraceResponseData>>(Request);

            var tracingManager = new TracingManager(repositories, config);

            tracingManager.CreateResponseData(responseData);

            var rootPath = "http://" + HttpContext.Request.Host.ToString();

            return Created(rootPath, new TraceResponseData());

        }

        [HttpPut("MarkResultsAsViewed")]
        public ActionResult<int> MarkTraceResponsesAsViewed([FromServices] IRepositories repositories,
                                                            [FromQuery] string enfService)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var tracingManager = new TracingManager(repositories, config);

            tracingManager.MarkResponsesAsViewed(enfService);

            return Ok();
        }
    }
}
