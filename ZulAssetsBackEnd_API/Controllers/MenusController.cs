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
    public class MenusController : ControllerBase
    {

        #region Declaration

        private static string SP_AssignMenusAgainstRole = "[dbo].[SP_AssignMenusAgainstRole]";
        private static string SP_GetAllMenu_WithBlankRights = "[dbo].[SP_GetAllMenu_WithBlankRights]";
        private static string SP_GetOptionsAgainstRoleAndMenuID = "[dbo].[SP_GetOptionsAgainstRoleAndMenuID]";

        #endregion

        #region Get All Menus

        /// <summary>
        /// Get all Menus by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Menus</returns>
        [HttpPost("GetAllMenus")]
        [Authorize]
        public IActionResult GetAllMenus([FromBody] Menus menu)
        {
            Message msg = new Message();
            try
            {

                DataTable dtAssignMenus = DataLogic.GetAllMenus(menu.RoleId, SP_AssignMenusAgainstRole);

                if (dtAssignMenus.Rows.Count > 0)
                {
                    if (dtAssignMenus.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dtAssignMenus.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        return Ok(dtAssignMenus);
                    }
                }
                else
                {
                    return Ok(dtAssignMenus);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get All Menus With Blank Rights

        /// <summary>
        /// Get all Menus by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Menus</returns>
        [HttpPost("GetAllMenusWithNoRights")]
        [Authorize]
        public IActionResult GetAllMenusWithNoRights()
        {
            Message msg = new Message();
            try
            {
                //DataTable dtAllMenus = DataLogic.GetMenus(SP_MenusAgainstParentMenu);   //Names for Menus
                DataTable menusWithNoRights = DataLogic.GetAllMenusWithNoRights(SP_GetAllMenu_WithBlankRights);

                if (menusWithNoRights.Rows.Count > 0)
                {
                    if (menusWithNoRights.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = menusWithNoRights.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        return Ok(menusWithNoRights);
                    }
                }
                else
                {
                    return Ok(menusWithNoRights);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get Menu Options With Rights

        /// <summary>
        /// Get all Menus by passing a parameter "RoleId as parameter"
        /// </summary>
        /// <returns>This will return all the Menus</returns>
        [HttpPost("GetMenuOptionsWithRights")]
        [Authorize]
        public IActionResult GetMenuOptionsWithRights([FromBody] MenuOptions menuOptions)
        {
            Message msg = new Message();
            try
            {

                DataTable menusWithRights = DataLogic.GetMenuOptionsWithRights(menuOptions, SP_GetOptionsAgainstRoleAndMenuID);

                if (menusWithRights.Rows.Count > 0)
                {
                    if (menusWithRights.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = menusWithRights.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        return Ok(menusWithRights);
                    }
                }
                else
                {
                    return Ok(menusWithRights);
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
