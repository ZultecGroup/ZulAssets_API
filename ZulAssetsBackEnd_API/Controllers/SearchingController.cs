using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchingController : ControllerBase
    {
        #region Declaration & Constructor

        private static string SP_ServerSideSearching = "[dbo].[SP_ServerSideSearching]";

        #endregion


        #region Server-Side Searching
        /// <summary>
        /// Get all Asset Coding Definitions by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <param name="serverSearchingParams"></param>
        /// <returns>This will return all the Asset Coding Definitions</returns>
        [HttpPost("ServerSideSearching")]
        [Authorize]
        public IActionResult ServerSideSearching([FromBody] ServerSideSearchingParams serverSearchingParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.ServerSideSearching(serverSearchingParams, SP_ServerSideSearching);
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
