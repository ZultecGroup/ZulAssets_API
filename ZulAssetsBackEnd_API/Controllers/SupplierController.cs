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
    public class SupplierController : ControllerBase
    {

        #region Declaration

        private static string SP_GetSuppliers = "[dbo].[SP_GetInsertUpdateDeleteSupplier]";
        private static string SP_InsertSuppliers = "[dbo].[SP_GetInsertUpdateDeleteSupplier]";
        private static string SP_UpdateSuppliers = "[dbo].[SP_GetInsertUpdateDeleteSupplier]";
        private static string SP_DeleteSuppliers = "[dbo].[SP_GetInsertUpdateDeleteSupplier]";

        #endregion

        #region Get All Suppliers
        /// <summary>
        /// Get all suppliers by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Suppliers</returns>
        [HttpPost("GetAllSuppliers")]
        [Authorize]
        public IActionResult GetAllSuppliers([FromBody] SupplierRequest suppReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllSuppliers(suppReq, SP_GetSuppliers);
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

        #region Add Supplier

        /// <summary>
        /// Insert a supplier by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertSupplier")]
        [Authorize]
        public IActionResult InsertSupplier([FromBody] SupplierRequest suppReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertSupplier(suppReq, SP_InsertSuppliers);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Supplier", 1, "Inserted", suppReq.LoginName, "dbo.Supplier");
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

        #region Update Supplier

        /// <summary>
        /// Update a supplier by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateSupplier")]
        [Authorize]
        public IActionResult UpdateSupplier([FromBody] SupplierRequest suppReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateSupplier(suppReq, SP_UpdateSuppliers);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Supplier", 1, "Updated", suppReq.LoginName, "dbo.Supplier");
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

        #region Delete Supplier

        /// <summary>
        /// Delete a supplier by passing "Delete = 1" and SupplierID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteSupplier")]
        [Authorize]
        public IActionResult DeleteSupplier([FromBody] SupplierRequest suppReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteSupplier(suppReq, SP_DeleteSuppliers);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Supplier", 1, "Deleted", suppReq.LoginName, "dbo.Supplier");
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
