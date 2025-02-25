using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Backend Inventory")]
    [Route("api/[controller]")]
    [ApiController]
    public class BackendInvController : ControllerBase
    {

        #region Declaration

        private static string SP_GetBackendInventoryData = "[dbo].[SP_GetBackendInventoryData]";

        #endregion

        #region Get All Backend Inventory Against LocationID and InventorySchdulerCode 
        /// <summary>
        /// Get all Assets In Backend Inventory by passing a parameter LocationID and InvSchCode
        /// </summary>
        /// <returns>This will return all the Purchase Orders</returns>
        [HttpPost("GetAllAssetsAgainstLocID_InvSchCode")]
        [Authorize]
        public IActionResult GetAllAssetsAgainstLocID_InvSchCode([FromBody] BackendInvReqParams backendInvReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllAssetsAgainstLocID_InvSchCode(backendInvReqParams, SP_GetBackendInventoryData);
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
