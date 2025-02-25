using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Brands")]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteBrand = "[dbo].[SP_GetInsertUpdateDeleteBrand]";

        #endregion

        #region Get All Brands
        /// <summary>
        /// Get all Brands by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Brands</returns>
        [HttpPost("GetAllBrands")]
        [Authorize]
        public IActionResult GetAllBrands([FromBody] BrandRequest brandReq)
        {
            Message msg = new Message();
            try
            {

                DataSet ds = DataLogic.GetAllBrands(brandReq, SP_GetInsertUpdateDeleteBrand);

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

        #region Add Brand

        /// <summary>
        /// Insert a Brand by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertBrand")]
        [Authorize]
        public IActionResult InsertBrand([FromBody] BrandRequest brandReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertBrand(brandReq, SP_GetInsertUpdateDeleteBrand);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Brand", 1, "Inserted", brandReq.LoginName, "dbo.Brand");
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

        #region Update Brand

        /// <summary>
        /// Update a Brand by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateBrand")]
        [Authorize]
        public IActionResult UpdateBrand([FromBody] BrandRequest brandReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateBrand(brandReq, SP_GetInsertUpdateDeleteBrand);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Brand", 1, "Updated", brandReq.LoginName, "dbo.Brand");
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

        #region Delete Brand

        /// <summary>
        /// Delete a Brand by passing "Delete = 1" and AstBrandID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteBrand")]
        [Authorize]
        public IActionResult DeleteBrand([FromBody] BrandRequest brandReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteBrand(brandReq, SP_GetInsertUpdateDeleteBrand);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Brand", 1, "Deleted", brandReq.LoginName, "dbo.Brand");
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
