using FOAEA3.Business.Areas.Application;
using FOAEA3.Model;
using FOAEA3.Model.Constants;
using FOAEA3.Model.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FOAEA3.API.Areas.Application.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ApplicationSearchesController : ControllerBase
{
    [HttpGet("Version")]
    public ActionResult<string> GetVersion() => Ok("ApplicationSearches API Version 1.0");

    [HttpGet("DB")]
    [Authorize(Roles = Roles.Admin)]
    public ActionResult<string> GetDatabase([FromServices] IRepositories repositories) => Ok(repositories.MainDB.ConnectionString);

    [HttpPost]
    public async Task<ActionResult<List<ApplicationSearchResultData>>> DoSearch([FromBody] QuickSearchData quickSearchCriteria,
                                                                                [FromServices] IRepositories repositories,
                                                                                [FromQuery] int page = 1,
                                                                                [FromQuery] int perPage = 1000,
                                                                                [FromQuery] string orderBy = "EnforcementService, ControlCode")
    {
        try
        {
            var searchManager = new ApplicationSearchManager(repositories);
            List<ApplicationSearchResultData> result;
            int totalCount;
            (result, totalCount) = await searchManager.Search(quickSearchCriteria, page, perPage, orderBy);

            if (!Response.Headers.ContainsKey("Page"))
            {
                Response.Headers.Append("Page", page.ToString());
                Response.Headers.Append("PerPage", perPage.ToString());
                Response.Headers.Append("OrderBy", orderBy);
                Response.Headers.Append("TotalCount", totalCount.ToString());
            }

            if (string.IsNullOrEmpty(searchManager.LastError))
                return Ok(result);
            else
                return UnprocessableEntity(searchManager.LastError);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

}
