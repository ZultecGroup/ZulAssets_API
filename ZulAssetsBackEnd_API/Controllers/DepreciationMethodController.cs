using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZulAssetsBackEnd_API.BAL;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Depreciation Methods")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepreciationMethodController : ControllerBase
    {

        #region Declaration

        private static string SP_GetDepMethods = "[dbo].[SP_GetInsertUpdateDeleteDepreciation_Method]";
        private static string SP_InsertDepMethod = "[dbo].[SP_GetInsertUpdateDeleteDepreciation_Method]";
        private static string SP_UpdateDepMethod = "[dbo].[SP_GetInsertUpdateDeleteDepreciation_Method]";
        private static string SP_DeleteDepMethod = "[dbo].[SP_GetInsertUpdateDeleteDepreciation_Method]";
        private static string SP_GetInsertUpdateDelete_DepPolicy = "[dbo].[SP_GetInsertUpdateDelete_DepPolicy]";
        private static string SP_GetUpdateDepreciationEngine = "[dbo].[SP_GetUpdateDepreciationEngine]";

        #endregion

        #region Deprecition Method APIs

        #region Get All Depreciation Methods
        /// <summary>
        /// Get all Deprecation Methods by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Depreciation Methods</returns>
        [HttpPost("GetAllDepMethods")]
        [Authorize]
        public IActionResult GetAllDepMethods([FromBody] DepreciationMethodsRequest depMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllDepreciationMethods(depMethodReqParam, SP_GetDepMethods);
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

        #region Add Depreciation Method

        /// <summary>
        /// Insert a Depreciation Method by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertDepMethod")]
        [Authorize]
        public IActionResult InsertDepMethod([FromBody] DepreciationMethodsRequest depMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertDepreciationMethod(depMethodReqParam, SP_InsertDepMethod);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Depreciation Method", "Added", depMethodReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Method", 1, "Inserted", depMethodReqParam.LoginName, "dbo.Depreciation_Method");
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

        #region Update Depreciation Method

        /// <summary>
        /// Update a Depreciation Method by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateDepMethod")]
        [Authorize]
        public IActionResult UpdateDepMethod([FromBody] DepreciationMethodsRequest depMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateDepreciationMethod(depMethodReqParam, SP_UpdateDepMethod);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Depreciation Method", "Updated", depMethodReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Method", 1, "Updated", depMethodReqParam.LoginName, "dbo.Depreciation_Method");
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

        #region Delete Depreciation Method

        /// <summary>
        /// Delete a Depreciation Method by passing "Delete = 1" and DesigatnionID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteDepMethod")]
        [Authorize]
        public IActionResult DeleteDepMethod([FromBody] DepreciationMethodsRequest depMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteDepreciationMethod(depMethodReqParam, SP_DeleteDepMethod);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Depreciation Method", "Deleted", depMethodReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Method", 1, "Deleted", depMethodReqParam.LoginName, "dbo.Depreciation_Method");
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

        #endregion

        #region Depreciation Policy APIs

        #region Get Depreciation Policy Against AstCatID

        /// <summary>
        /// Get Depreciation Policy of a Category by passing "Get = 1 and AstCatID"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetDepPolicyAgainstAstCatID")]
        [Authorize]
        public IActionResult GetDepPolicyAgainstAstCatID([FromBody] DepPolicyReqParams depPolicyReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetDepPolicyAgainstAstCatID(depPolicyReqParams, SP_GetInsertUpdateDelete_DepPolicy);
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

        #region Insert Depreciation Policy

        /// <summary>
        /// Insert a Depreciation Policy by passing all parameters with "Add = 1" and without CatDepID parameter
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertDepPolicy")]
        [Authorize]
        public IActionResult InsertDepPolicy([FromBody] DepPolicyReqParams depPolicyReqParams)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {

                depPolicyReqParams.SalvagePercent = GF.CalculatePercent(depPolicyReqParams.SalvageYear, depPolicyReqParams.SalvageMonth);

                if (Convert.ToDouble(depPolicyReqParams.SalvagePercent) < 0 || Convert.ToDouble(depPolicyReqParams.SalvagePercent) > 1200)
                {
                    msg.message = "Salvage Percentage value is not valid";
                    msg.status = "400";
                    return Ok(msg);
                }
                else
                {

                    if (depPolicyReqParams.IsSalvageValuePercentage)
                    {
                        depPolicyReqParams.SalvageValue = Convert.ToDouble(depPolicyReqParams.SalvageValuePercent);
                    }
                    else
                    {
                        depPolicyReqParams.SalvageValue = depPolicyReqParams.SalvageValue;
                    }

                    DataTable dt = DataLogic.InsertDepPolicy(depPolicyReqParams, SP_GetInsertUpdateDelete_DepPolicy);
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
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", depPolicyReqParams.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Policy", 1, "Insert", depPolicyReqParams.LoginName, "dbo.DepPolicy");
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
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Update Depreciation Policy

        /// <summary>
        /// Update Depreciation Policy by passing all parameters with "Update = 1" and CatDepID as parameter
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateDepPolicy")]
        [Authorize]
        public IActionResult UpdateDepPolicy([FromBody] DepPolicyReqParams depPolicyReqParams)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {

                depPolicyReqParams.SalvagePercent = GF.CalculatePercent(depPolicyReqParams.SalvageYear, depPolicyReqParams.SalvageMonth);

                if (Convert.ToDouble(depPolicyReqParams.SalvagePercent) < 0 || Convert.ToDouble(depPolicyReqParams.SalvagePercent) > 1200)
                {
                    msg.message = "Salvage Percentage value is not valid";
                    msg.status = "400";
                    return Ok(msg);
                }
                else
                {

                    if (depPolicyReqParams.IsSalvageValuePercentage)
                    {
                        depPolicyReqParams.SalvageValue = Convert.ToDouble(depPolicyReqParams.SalvageValuePercent);
                    }
                    else
                    {
                        depPolicyReqParams.SalvageValue = depPolicyReqParams.SalvageValue;
                    }

                    DataTable dt = DataLogic.UpdateDepPolicy(depPolicyReqParams, SP_GetInsertUpdateDelete_DepPolicy);
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
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", depPolicyReqParams.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Policy", 1, "Insert", depPolicyReqParams.LoginName, "dbo.DepPolicy");
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
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #endregion

        #region Deprcation Engine APIs

        #region Get Depreciation Engine

        /// <summary>
        /// Get Asset Books to be applied on Assets for Depreciation Engine by passing "Get = 1 and CompanyID" as a parameter.
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetAstBooksAgainstCompanyIDForDepreciation")]
        [Authorize]
        public IActionResult GetAstBooksAgainstCompanyIDForDepreciation([FromBody] DeprecitionEngineReqParams depEngineReqParams)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                DataSet ds = DataLogic.GetAstBookAgainstCompanyIDForDeprecation(depEngineReqParams, SP_GetUpdateDepreciationEngine);
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

        #region Apply Depreciation Engine on Assets

        /// <summary>
        /// this api is used to apply depreciation engine on assets
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("RunDepreciationEngineonAssets")]
        [Authorize]
        public IActionResult RunDepreciationEngineonAssets([FromBody] DeprecitionEngineReqParams depEngineReqParams)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            string msg1 = string.Empty;
            try
            {

                DataTable selectedBooksDT = new DataTable();
                selectedBooksDT.Columns.Add("BookIDs");

                if (selectedBooksDT != null)
                {
                    selectedBooksDT = ListintoDataTable.ToDataTable(depEngineReqParams.depEngTree);
                }

                if (selectedBooksDT.Rows.Count > 0 && selectedBooksDT.Rows[0]["BookIDs"].ToString() != "" )
                {

                    msg1 = GF.ThrBreakMethod(selectedBooksDT, depEngineReqParams.UpdateBookTillDate, selectedBooksDT.Rows.Count);

                    if (msg1.Contains("ErrorMessage"))
                    {
                        msg.message = msg1;
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = msg1;
                        msg.status = "200";
                        return Ok(msg);
                    }
                }
                else
                {
                    msg.message = "Please select at least one Book";
                    msg.status = "401";
                    return Ok(msg);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #endregion

    }
}
