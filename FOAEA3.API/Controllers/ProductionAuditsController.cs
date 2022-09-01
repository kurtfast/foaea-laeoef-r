﻿using FOAEA3.Business.Utilities;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FOAEA3.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductionAuditsController : ControllerBase
    {
        [HttpGet("Version")]
        public ActionResult<string> GetVersion() => Ok("APIConfigurations API Version 1.0");

        [HttpGet("DB")]
        public ActionResult<string> GetDatabase([FromServices] IRepositories repositories) => Ok(repositories.MainDB.ConnectionString);

        [HttpPost]
        public async Task<ActionResult<ProductionAuditData>> InsertNotification([FromServices] IRepositories repositories)
        {
            var productionAuditData = await APIBrokerHelper.GetDataFromRequestBodyAsync<ProductionAuditData>(Request);

            var productionAuditManager = new ProductionAuditManager(repositories);
            await productionAuditManager.InsertAsync(productionAuditData);

            return Ok(productionAuditData);
        }
    }
}