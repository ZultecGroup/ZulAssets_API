using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        #region Declaration

        private static string SP_GetAllCategories = "[dbo].[SP_GetAllCategories]";
        private static string SP_GetInsertUpdateDeleteCategory = "[dbo].[SP_GetInsertUpdateDeleteCategory]";

        #endregion

        #region All Category
        /// <summary>
        /// Get All Category API
        /// </summary>
        /// <returns>Returns a message ""</returns>
        [HttpGet("GetAllCategory")]
        [Authorize]
        public IActionResult GetAllCategory()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllCategory(SP_GetAllCategories);
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

        #region All Categories For TreeView
        /// <summary>
        /// Get All Categories by passing the "Get = 1" and others as blank
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("GetAllCategoriesTreeView")]
        [Authorize]
        public IActionResult GetAllCategoriesTreeView(CategoryTree catTreeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllCategoriesTree(catTreeReq, SP_GetInsertUpdateDeleteCategory);
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

        #region Add Category

        /// <summary>
        /// Insert a Category by passing all parameters with "Add = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertCategor")]
        [Authorize]
        public IActionResult InsertCategory([FromBody] CategoryTree catTreeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertCategory(catTreeReq, SP_GetInsertUpdateDeleteCategory);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Supplier", "Added", suppReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Category", 1, "Inserted", catTreeReq.LoginName, "dbo.Category");
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

        #region Update Category

        /// <summary>
        /// Update a Category by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateCategory")]
        [Authorize]
        public IActionResult UpdateCategory([FromBody] CategoryTree catTreeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateCategory(catTreeReq, SP_GetInsertUpdateDeleteCategory);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Supplier", "Updated", suppReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Category", 1, "Updated", catTreeReq.LoginName, "dbo.Category");
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

        #region Delete Category

        /// <summary>
        /// Delete a Category by passing "Delete = 1" and CatID as parameters
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteCategory")]
        [Authorize]
        public IActionResult DeleteCategory([FromBody] CategoryTree catTreeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteCategory(catTreeReq, SP_GetInsertUpdateDeleteCategory);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Supplier", "Deleted", suppReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Category", 1, "Deleted", catTreeReq.LoginName, "dbo.Category");
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
