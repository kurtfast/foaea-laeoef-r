﻿using FOAEA3.Business.Areas.Application;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Enums;
using FOAEA3.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace FOAEA3.API.Tracing.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OutgoingFederalTracingRequestsController : ControllerBase
    {
        private readonly CustomConfig config;

        public OutgoingFederalTracingRequestsController(IOptions<CustomConfig> config)
        {
            this.config = config.Value;
        }

        [HttpGet("")]
        public ActionResult<List<TracingOutgoingFederalData>> GetFederalOutgoingData(
                                                                [FromQuery] int maxRecords,
                                                                [FromQuery] string activeState,
                                                                [FromQuery] int lifeState,
                                                                [FromQuery] string enfServiceCode,
                                                                [FromServices] IRepositories repositories)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var manager = new TracingManager(repositories, config);

            var data = manager.GetFederalOutgoingData(maxRecords, activeState, (ApplicationState)lifeState,
                                                      enfServiceCode);

            return Ok(data);
        }
    }
}
