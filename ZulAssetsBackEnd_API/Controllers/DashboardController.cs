using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        #region Declaration

        private static string SP_DashboardCounts = "[dbo].[SP_DashboardCounts]";

        #endregion

        #region Get All Counts
        /// <summary>
        /// Get all Counts of AppUsers, AssetDetails, BrandCount, CategoryCount, CustodianCount, DepartmentCount, LocationCount and POCount without passing any parameter
        /// </summary>
        /// <returns>This will return all the counts</returns>
        [HttpGet("GetAllDashboardCounts")]
        [Authorize]
        public IActionResult GetAllDashboardCounts()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllDashboardCounts(SP_DashboardCounts);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        return Ok(dt);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

    }
}
