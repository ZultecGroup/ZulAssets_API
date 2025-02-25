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
    [Tags("Organization Hierarchy")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrgHierController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteOrgHier = "[dbo].[SP_GetInsertUpdateDeleteOrgHier]";

        #endregion

        #region Get All Organization Hier for TreeView
        /// <summary>
        /// Get all Organization Hierarchy by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Organization Hierarchy</returns>
        [HttpPost("GetAllOrgHier")]
        //[Authorize]
        public IActionResult GetAllOrgHier([FromBody] OrgHierReqParams orgHierReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllOrgHierTreeView(orgHierReqParam, SP_GetInsertUpdateDeleteOrgHier);
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

        #region Add Organization Hierarchy

        /// <summary>
        /// Insert an Organization Hierarchy by passing LvlID from DropDown, OrgHierName, ParentId, and Add=1 and else as empty.
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertOrgHier")]
        [Authorize]
        public IActionResult InsertOrgHier([FromBody] OrgHierReqParams orgHierReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertOrgHier(orgHierReqParam, SP_GetInsertUpdateDeleteOrgHier);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Organization Hierarchy", "Added", orgHierReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Organization Hierarchy", 1, "Inserted", orgHierReqParam.LoginName, "dbo.OrganizationHier");
                        }
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
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

        #region Update Organization Hierarchy

        /// <summary>
        /// Update a Organization Hierarchy by passing the OrgHierName, and LvlCode with Update = 1 and else as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateOrgHier")]
        //[Authorize]
        public IActionResult UpdateOrgHier([FromBody] OrgHierReqParams orgHierReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateOrgHier(orgHierReqParam, SP_GetInsertUpdateDeleteOrgHier);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Organization Hierarchy", "Added", orgHierReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Organization Hierarchy", 1, "Updated", orgHierReqParam.LoginName, "dbo.OrganizationHier");
                        }
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
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

        #region Delete Organization Hierarchy

        /// <summary>
        /// Delete a Organization Hierarchy by passing "Delete = 1" and OrgHierID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteOrgHier")]
        //[Authorize]
        public IActionResult DeleteOrgHier([FromBody] OrgHierReqParams orgHierReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteOrgHier(orgHierReqParam, SP_GetInsertUpdateDeleteOrgHier);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Organization Hierarchy", "Added", orgHierReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Organization Hierarchy", 1, "Deleted", orgHierReqParam.LoginName, "dbo.OrganizationHier");
                        }
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
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
