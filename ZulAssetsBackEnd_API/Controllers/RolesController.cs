using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using ZulAssetsBackEnd_API.BAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteRole = "[dbo].[SP_GetInsertUpdateDeleteRole]";
        private static string SP_UpdateRole = "[dbo].[SP_UpdateRole]";
        private static string SP_GetMenuOrRightsAgainstRoleId = "[dbo].[SP_GetMenuOrRightsAgainstRoleId]";
        private static string Sp_RoleDetail = "[dbo].[Sp_RoleDetail]";

        #endregion

        #region Get All Roles
        /// <summary>
        /// Get all Roles by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Roles</returns>
        [HttpPost("GetAllRoles")]
        [Authorize]
        public IActionResult GetAllRoles([FromBody] RoleRequest roleReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllRoles(roleReq, SP_GetInsertUpdateDeleteRole);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        #region Replace Table Names

                        DataTable table = ds.Tables["Table"];
                        DataTable table1 = ds.Tables["Table1"];

                        table.TableName = "TotalRowsCount";
                        table1.TableName = "data";

                        #endregion

                        int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);

                        return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                data = table1
                            });
                        //return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get Role By ID
        /// <summary>
        /// Get all rights against a specific RoleID by passing the RoleID and GetByID parameter in the payload
        /// </summary>
        /// <returns>This will return all the Roles</returns>
        [HttpPost("GetRoleByID")]
        [Authorize]
        public IActionResult GetRoleByID([FromBody] RoleRequest roleReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetRoleByID(roleReq, SP_GetInsertUpdateDeleteRole);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        #region Replace Table Names

                        DataTable table = ds.Tables["Table"];
                        DataTable table1 = ds.Tables["Table1"];
                        DataTable table2 = ds.Tables["Table2"];

                        table.TableName = "MasterMenu";
                        table1.TableName = "MenuOptions";
                        table2.TableName = "RoleAssignOptions";

                        #endregion

                        //int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);

                        //return Ok(
                        //    new
                        //    {
                        //        totalRowsCount = totalRowsCounts,
                        //        data = table1
                        //    });
                        return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Add Role

        /// <summary>
        /// Insert a role by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertRole")]
        [Authorize]
        public IActionResult InsertRole([FromBody] RoleRequest roleReq)
        {
            Message msg = new Message();
            try
            {

                if (roleReq.Description == "" || roleReq.Description == null)
                {
                    msg.message = "Please provide the Role Description.";
                    msg.status = "401";
                    return BadRequest(msg);
                }

                DataTable dt = DataLogic.InsertRole(roleReq, SP_GetInsertUpdateDeleteRole);

                return Ok(dt);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }


        #endregion

        #region Update Role

        /// <summary>
        /// Update a role by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateRole")]
        [Authorize]
        public IActionResult UpdateRole([FromBody] RoleRequest roleReq)
        {
            Message msg = new Message();
            try
            {

                DataTable dt = DataLogic.UpdateRole(roleReq, SP_UpdateRole);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Role", "Updated", roleReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Role", 1, "Updated", roleReq.LoginName, "dbo.UserRoles");
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

        #region Update Role Rights

        /// <summary>
        /// Update a role by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateRoleRights")]
        [Authorize]
        public IActionResult UpdateRoleRights([FromBody] RoleRequest roleReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("RoleId"); dtMain.Columns.Add("OptionId"); dtMain.Columns.Add("MenuId"); dtMain.Columns.Add("Value");
                if (roleReq.roleAssignOptions_list != null)
                {
                    dtMain = ListintoDataTable.ToDataTable(roleReq.roleAssignOptions_list);
                }

                DataTable dt = DataLogic.UpdateRoleRights(roleReq, dtMain, SP_GetInsertUpdateDeleteRole);

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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Role", "Updated", roleReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Role", 1, "Updated", roleReq.LoginName, "dbo.UserRoles");
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

        #region Delete Role

        /// <summary>
        /// Delete a role by passing "Delete = 1" and RoleID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteRole")]
        [Authorize]
        public IActionResult DeleteRole([FromBody] RoleRequest roleReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteRole(roleReq, SP_GetInsertUpdateDeleteRole);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Role", "Deleted", roleReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Role", 1, "Deleted", roleReq.LoginName, "dbo.UserRoles");
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

        #region Get Assigned Menus
        /// <summary>
        /// Get all rights against a specific RoleID by passing the RoleID and GetByID parameter in the payload
        /// </summary>
        /// <returns>This will return all the Roles</returns>
        [HttpPost("GetAssignedMenus")]
        [Authorize]
        public IActionResult GetAssignedMenus([FromBody] RightsParams rightParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAssignedMenus(rightParams, SP_GetMenuOrRightsAgainstRoleId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                       
                        return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get Assigned Menu Options
        /// <summary>
        /// Get all rights against a specific RoleID by passing the RoleID and GetByID parameter in the payload
        /// </summary>
        /// <returns>This will return all the Roles</returns>
        [HttpPost("GetAssignedMenuOptions")]
        [Authorize]
        public IActionResult GetAssignedMenuOptions([FromBody] RightsParams rightParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAssignedMenuOptions(rightParams, SP_GetMenuOrRightsAgainstRoleId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                       
                        return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
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
