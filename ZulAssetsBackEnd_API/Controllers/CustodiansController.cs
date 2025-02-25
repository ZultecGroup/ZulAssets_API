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
    [Route("api/[controller]")]
    [ApiController]
    public class CustodiansController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteCustodian = "[dbo].[SP_GetInsertUpdateDeleteCustodian]";

        #endregion

        #region Get All Custodians
        /// <summary>
        /// Get all Asset Coding Definitions by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <param name="custReqParams"></param>
        /// <returns>This will return all the Asset Coding Definitions</returns>
        [HttpPost("GetAllCustodians")]
        [Authorize]
        public IActionResult GetAllCustodians([FromBody] CustodianReqParams custReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllCustodians(custReqParams, SP_GetInsertUpdateDeleteCustodian);
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
                            new {
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

        #region Add Custodian

        /// <summary>
        /// Insert a Custodian by passing Name, Code, Phone, Email, Fax, Cell, Address, OrgHierID, DesignationID with "Add = 1" and others as empty
        /// </summary>
        /// /// <param name="custReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertCustodian")]
        [Authorize]
        public IActionResult InsertCustodian([FromBody] CustodianReqParams custReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertCustodian(custReqParams, SP_GetInsertUpdateDeleteCustodian);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Custodian", "Added", custReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Custodian", 1, "Inserted", custReqParams.LoginName, "dbo.Custodian");
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

        #region Update Custodian

        /// <summary>
        /// Update a Custodian by passing Name, Code, Phone, Email, Fax, Cell, Address, OrgHierID, DeisignationID with "Update = 1"
        /// </summary>
        /// /// <param name="custReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateCustodian")]
        [Authorize]
        public IActionResult UpdateCustodian([FromBody] CustodianReqParams custReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateCustodian(custReqParams, SP_GetInsertUpdateDeleteCustodian);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Custodian", "Added", custReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Custodian", 1, "Updated", custReqParams.LoginName, "dbo.custodian");
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

        #region Delete Custodian

        /// <summary>
        /// Delete a Custodian by passing "Delete = 1" and CustodianID as parameters and others as empty
        /// </summary>
        /// /// <param name="custReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteCustodian")]
        [Authorize]
        public IActionResult DeleteCustodian([FromBody] CustodianReqParams custReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteCustodian(custReqParams, SP_GetInsertUpdateDeleteCustodian);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Custodian", "Added", custReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Custodian", 1, "Deleted", custReqParams.LoginName, "dbo.Custodian");
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
