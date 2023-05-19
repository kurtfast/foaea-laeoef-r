﻿using FOAEA3.Business.Security;
using FOAEA3.Common.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FOAEA3.API.Areas.Administration.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubjectsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<SubjectData>> GetSubjects([FromServices] IRepositories repositories, [FromQuery] string submCd)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            if (submCd != null)
                return Ok(repositories.SubjectRepository.GetSubjectsForSubmitter(submCd));
            else
                return UnprocessableEntity("Missing submCd parameter"); // not allowed to get all subjects currently
        }

        [HttpGet("{subjectName}")]
        public ActionResult<SubjectData> GetSubject([FromServices] IRepositories repositories, [FromRoute] string subjectName)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var data = repositories.SubjectRepository.GetSubject(subjectName);
            return Ok(data);
        }

        [HttpPut("AcceptTermsOfReference")]
        public ActionResult<SubjectData> AcceptTermsOfReference([FromServices] IRepositories repositories, [FromBody] SubjectData subject)
        {
            APIHelper.ApplyRequestHeaders(repositories, Request.Headers);
            APIHelper.PrepareResponseHeaders(Response.Headers);

            var loginManager = new LoginManager(repositories);
            loginManager.AcceptNewTermsOfReferernce(subject.SubjectName);

            return Ok(subject);
        }

    }
}
