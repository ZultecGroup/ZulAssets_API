using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;
using ZulAssetsBackEnd_API.BAL;
using Microsoft.Extensions.Options;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        #region Declaration & Contructor

        private static string SP_GetInsertUpdateDeleteCompany = "[dbo].[SP_GetInsertUpdateDeleteCompany]";
        private static string SP_GetInsertUpdateCompanyInfo = "[dbo].[SP_GetInsertUpdateCompanyInfo]";
        private static string SP_UpdateCompanyInfoLogo = "[dbo].[SP_UpdateCompanyInfoLogo]";
        private static string SP_GetUpdateInterCompanyTransfer = "[dbo].[SP_GetUpdateInterCompanyTransfer]";
        private static string SP_GetGLCodesAgainstCompanyID = "[dbo].[SP_GetGLCodesAgainstCompanyID]";

        private Constants _constants;
        public CompanyController()
        {
            
        }

        #endregion

        #region Companies APIs

        #region Get All Companies
        /// <summary>
        /// Get all Companies by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <param name="compReqParam"></param>
        /// <returns>This will return all the Companies</returns>
        [HttpPost("GetAllCompanies")]
        [Authorize]
        public IActionResult GetAllCompanies([FromBody] CompReqParam compReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllCompanies(compReqParam, SP_GetInsertUpdateDeleteCompany);
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

        #region Add Company

        /// <summary>
        /// Insert a Company by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertCompany")]
        [Authorize]
        public IActionResult InsertCompany([FromBody] CompReqParam compReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertCompany(compReqParam, SP_GetInsertUpdateDeleteCompany);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Company", "Added", compReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Company", 1, "Inserted", compReqParam.LoginName, "dbo.Companies");
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

        #region Update Company

        /// <summary>
        /// Update a Company by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateCompany")]
        [Authorize]
        public IActionResult UpdateCompany([FromBody] CompReqParam compReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateCompany(compReqParam, SP_GetInsertUpdateDeleteCompany);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Company", "Added", compReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Company", 1, "Updated", compReqParam.LoginName, "dbo.Companies");
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

        #region Delete Company

        /// <summary>
        /// Delete a Company by passing "Delete = 1" and CompanyId as parameters and others as empty
        /// </summary>
        /// <param name="compReqParam"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteCompany")]
        [Authorize]
        public IActionResult DeleteCompany([FromBody] CompReqParam compReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteCompany(compReqParam, SP_GetInsertUpdateDeleteCompany);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Company", "Added", compReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Company", 1, "Deleted", compReqParam.LoginName, "dbo.Companies");
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

        #region Company APIs

        #region Get Company Info
        /// <summary>
        /// Get Company Info by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <param name="compInfoReqParam"></param>
        /// <returns>This will return all the Companies</returns>
        [HttpPost("GetCompanyInfo")]
        [Authorize]
        public IActionResult GetCompanyInfo([FromBody] CompInfoReqParam compInfoReqParam)
        {
            Message msg = new Message();

            try
            {
                ImageFunctionality imgFun = new ImageFunctionality();
                DataSet ds = DataLogic.GetCompanyInfo(compInfoReqParam, SP_GetInsertUpdateCompanyInfo);
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

                        #region If image is allow to safe in Database or Folder

                        var imgStoageLoc = ds.Tables[1].Rows[0]["ImgStorageLoc"].ToString();

                        if (imgStoageLoc != "Database")
                        {
                            var compName = ds.Tables[1].Rows[0]["Name"].ToString();
                            var base64string = imgFun.ConvertImgFromServer_toBase64(compName);

                            foreach (DataRow dr in ds.Tables[1].Rows) // search whole table
                            {
                                if (Convert.ToInt32(dr["Id"]) == 1) // if id==2
                                {
                                    dr["ImageBase64"] = base64string; //change the name
                                                                      //break; break or not depending on you
                                }
                            }
                        }

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
                //if (dt.Rows.Count > 0)
                //{
                //    if (dt.Columns.Contains("ErrorMessage"))
                //    {
                //        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                //        msg.status = "401";
                //        return Ok(msg);
                //    }
                //    else
                //    {
                //        #region If image is allow to safe in Database or Folder

                //        var imgStoageLoc = dt.Rows[0]["ImgStorageLoc"].ToString();

                //        if (imgStoageLoc != "Database")
                //        {
                //            var compName = dt.Rows[0]["Name"].ToString();
                //            var base64string = imgFun.ConvertImgFromServer_toBase64(compName);

                //            foreach (DataRow dr in dt.Rows) // search whole table
                //            {
                //                if (Convert.ToInt32(dr["Id"]) == 1) // if id==2
                //                {
                //                    dr["ImageBase64"] = base64string; //change the name
                //                                                      //break; break or not depending on you
                //                }
                //            }
                //        }

                //        #endregion

                //        return Ok(dt);
                //    }
                //}
                //else
                //{
                //    return Ok(dt);
                //}
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Add Company Info

        /// <summary>
        /// Insert a Company Info by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <param name="compInfoReqParam"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertCompanyInfo")]
        [Authorize]
        public IActionResult InsertCompanyInfo([FromBody] CompInfoReqParam compInfoReqParam)
        {
            Message msg = new Message();
            try
            {
                ImageFunctionality imgFun = new ImageFunctionality();
                DataTable dt = DataLogic.InsertCompanyInfo(compInfoReqParam, SP_GetInsertUpdateCompanyInfo);
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

                        if (dt.Rows[0]["Message"].ToString().Contains("already created"))
                        {
                            msg.message = dt.Rows[0]["Message"].ToString();
                            msg.status = "404";
                            return Ok(msg);
                        }
                        else
                        {
                            #region If image is allow to safe in Database or Folder

                            var imgStoageLoc = dt.Rows[0]["ImgStorageLoc"].ToString();

                            if (imgStoageLoc == "Database")
                            {
                                DataTable dt2 = DataLogic.UpdateCompanyInfoLogo(compInfoReqParam.ImageToBase64, SP_UpdateCompanyInfoLogo);
                            }
                            else
                            {
                                bool aa = imgFun.ConvertBase64toImg_SavetoServer(compInfoReqParam.ImageToBase64, compInfoReqParam.Name, "CompanyLogo");
                            }

                            #endregion

                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Company Info", "Added", compInfoReqParam.LoginName);
                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            if (msgFromDB.Contains("successfully"))
                            {
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Company Info", 1, "Inserted", compInfoReqParam.LoginName, "dbo.CompanyInfo");
                            }
                            msg.message = dt.Rows[0]["Message"].ToString();
                            msg.status = dt.Rows[0]["Status"].ToString();
                            return Ok(msg);
                        }

                        
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

        #region Update Company Info

        /// <summary>
        /// Update a Company Info by passing all parameters with "Update = 1"
        /// </summary>
        /// <param name="compInfoReqParam"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateCompanyInfo")]
        [Authorize]
        public IActionResult UpdateCompanyInfo([FromBody] CompInfoReqParam compInfoReqParam)
        {
            Message msg = new Message();
            try
            {
                ImageFunctionality imgFun = new ImageFunctionality();
                DataTable dt = DataLogic.UpdateCompanyInfo(compInfoReqParam, SP_GetInsertUpdateCompanyInfo);
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

                        #region If image is allow to safe in Database or Folder

                        var imgStoageLoc = dt.Rows[0]["ImgStorageLoc"].ToString();

                        if (imgStoageLoc == "Database")
                        {
                            DataTable dt2 = DataLogic.UpdateCompanyInfoLogo(compInfoReqParam.ImageToBase64, SP_UpdateCompanyInfoLogo);
                        }
                        else
                        {
                            bool aa = imgFun.ConvertBase64toImg_SavetoServer(compInfoReqParam.ImageToBase64, compInfoReqParam.Name, "CompanyLogo");
                        }

                        #endregion

                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Company Info", "Added", compInfoReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Company Info", 1, "Updated", compInfoReqParam.LoginName, "dbo.CompanyInfo");
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

        //#region Delete Company Info

        ///// <summary>
        ///// Delete a Company Info by passing "Delete = 1" and CompanyId as parameters and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("DeleteCompanyInfo")]
        //[Authorize]
        //public IActionResult DeleteCompanyInfo([FromBody] CompReqParam compReqParam)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.DeleteCompanyInfo(compReqParam, SP_GetInsertUpdateDeleteCompany);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Columns.Contains("ErrorMessage"))
        //            {
        //                msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                msg.status = "401";
        //                return Ok(msg);
        //            }
        //            else
        //            {
        //                msg.message = dt.Rows[0]["Message"].ToString();
        //                msg.status = dt.Rows[0]["Status"].ToString();
        //                return Ok(msg);
        //            }
        //        }
        //        else
        //        {
        //            return Ok(dt);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.message = ex.Message;
        //        return Ok(msg);
        //    }
        //}

        //#endregion

        #endregion

        #region Inter-Company Transfer

        #region Get GLCodes Against CompanyID

        /// <summary>
        /// This API is used to get All GLCodes against CompanyID
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetGLCodesAgainstCompanyID")]
        [Authorize]
        public IActionResult GetGLCodesAgainstCompanyID([FromBody] CompInfoReqParam compInfoReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetGLCodesAgainstCompanyID(compInfoReqParam.ID, SP_GetGLCodesAgainstCompanyID);
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

        #region Get Ast Info

        /// <summary>
        /// This API is used to transfer the asset/s from a Company to sister Company
        /// </summary>
        /// <param name="interCompanyTransferReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetAstInfo")]
        [Authorize]
        public IActionResult GetAstInfo([FromBody] InterCompanyTransferReqParams interCompanyTransferReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAstInfo(interCompanyTransferReqParams, SP_GetUpdateInterCompanyTransfer);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {

                        DataTable table = ds.Tables["Table"];
                        DataTable table1 = ds.Tables["Table1"];

                        table.TableName = "AssetGeneralInfo";
                        table1.TableName = "AssetBookInformation";

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

        #region Update Ast Company

        /// <summary>
        /// This API is used to transfer the asset from a Company to sister Company
        /// </summary>
        /// <param name="interCompanyTransferReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateAstCompany")]
        [Authorize]
        public IActionResult UpdateAstCompany([FromBody] InterCompanyTransferReqParams interCompanyTransferReqParams)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                long astNum = 0;
                bool astNumFlag = false;
                string astNumRef = string.Empty;
                if (interCompanyTransferReqParams.AstID == "")
                {
                    msg.message = "Please select an Asset to transfer";
                    msg.status = "404";
                    return BadRequest(msg);
                }
                if (interCompanyTransferReqParams.CurrentBV == 0.00)
                {
                    msg.message = "Please enter a value for CurrentBV greater than zero";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                #region Checking Asset Coding Definition

                SysConfigReqParams sysConfigReqParams = new SysConfigReqParams();
                sysConfigReqParams.Get = 1;
                DataTable checkAstCodingMode = DataLogic.GetSysConfigInfo(sysConfigReqParams, "[dbo].[SP_GetInsertUpdateSysSettings]");

                if (Convert.ToBoolean(checkAstCodingMode.Rows[0]["CodingMode"]))
                {
                    long StartRange = 0, EndRange = 0;

                    DataTable assetCodingDT = DataLogic.GetAstCodingDefAgainstCompanyID(interCompanyTransferReqParams.NewCompanyID, "[dbo].[SP_GetInsertUpdateDeleteAstCoding]");

                    if (assetCodingDT.Rows.Count > 0)
                    {
                        StartRange = Convert.ToInt64(assetCodingDT.Rows[0]["StartSerial"]);
                        EndRange = Convert.ToInt64(assetCodingDT.Rows[0]["EndSerial"]);
                    }

                    if (EndRange != 0 && StartRange != 0)
                    {
                        DataTable lastAstNoDT = DataLogic.GetCompanyLastAssetNumber(interCompanyTransferReqParams.NewCompanyID, "[dbo].[SP_GetLastAssetNumberAgainstCompanyID]");
                        long lastAssetNumber = Convert.ToInt64(lastAstNoDT.Rows[0]["LastAssetNumber"]);

                        astNum = lastAssetNumber >= StartRange ? lastAssetNumber + 1 : StartRange;

                        if (EndRange < astNum)
                        {
                            msg.message = "Range exceeded for the Transferred Company. Please create new Asset Coding Range";
                            msg.status = "404";
                            DataLogic.CloseAstCodingRange(interCompanyTransferReqParams.NewCompanyID, "[dbo].[SP_CloseAstCodingRangeAgainstCompanyID]");
                            return BadRequest(msg);
                        }
                        astNumFlag = true;
                    }
                    else
                    {
                        msg.message = "Asset Range for Transferred Company must be defined first.";
                        msg.status = "404";
                        return BadRequest(msg);
                    }

                }

                #endregion
                
                if (interCompanyTransferReqParams.NewCompanyID != "")
                {
                    if (interCompanyTransferReqParams.GLCodes != "")
                    {
                        int countDisposedAst = Convert.ToInt32(DataLogic.IsAssetDisposed(interCompanyTransferReqParams.AstID, "[dbo].[SP_CheckIsAssetDisposed]").Rows[0]["Count"].ToString());
                        if (countDisposedAst > 0)
                        {
                            msg.message = "Asset can't be transferred if it is disposed";
                            msg.status = "404";
                            return Ok(msg);
                        }
                        else
                        {
                            if (interCompanyTransferReqParams.OldCompanyID == interCompanyTransferReqParams.NewCompanyID)
                            {
                                msg.message = "Asset can't be transferred in same company";
                                msg.status = "404";
                                return Ok(msg);
                            }
                            else
                            {
                                var aa = GF.AddNew_AssetDetails_ForNewCompanyID(interCompanyTransferReqParams.NewCompanyID, interCompanyTransferReqParams.AstID, interCompanyTransferReqParams.RefNo,
                                    interCompanyTransferReqParams.CurrentBV, interCompanyTransferReqParams.TransRemarks, interCompanyTransferReqParams.GLCodes, interCompanyTransferReqParams.SalYr,
                                    interCompanyTransferReqParams.ImageToBase64, interCompanyTransferReqParams.SalValue, "[dbo].[SP_GetAssetDetailsAgainstAstID]");
                                if (aa != null)
                                {
                                    if (astNumFlag)
                                    {
                                        astNumRef = astNum.ToString();
                                    }
                                    else
                                    {
                                        astNumRef = aa.Item3;
                                    }
                                    if (aa.Item1.Contains("success"))
                                    {
                                        msg.message = aa.Item1;
                                        msg.status = "200";
                                        return Ok(new
                                        {
                                            message = aa.Item1,
                                            status = "200",
                                            newAstID = aa.Item2,
                                            astNum = astNumRef
                                        });
                                    }
                                    else
                                    {
                                        msg.message = aa.Item1;
                                        msg.status = "404";
                                        return Ok(new
                                        {
                                            message = aa.Item1,
                                            status = "404",
                                            newAstID = aa.Item2,
                                            astNum = astNumRef
                                        });
                                    }
                                }
                                else
                                {
                                    msg.message = "Something went wrong!";
                                    msg.status = "400";
                                    return Ok(new
                                    {
                                        message = msg.message,
                                        status = "400",
                                        newAstID = aa.Item2,
                                        astNum = astNumRef
                                    });
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        msg.message = "GL Code is not defined";
                        msg.status = "404";
                        return Ok(new
                        {
                            message = msg.message,
                            status = "404",
                            newAstID = "",
                            astNum = ""
                        });
                    }
                }
                else
                {
                    msg.message = "Company is not defined";
                    msg.status = "404";
                    return Ok(new
                    {
                        message = msg.message,
                        status = "404",
                        newAstID = "",
                        astNum = ""
                    });
                }
                
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(new
                {
                    message = ex.Message,
                    status = "404",
                    newAstID = "",
                    astNum = ""
                });
            }
        }

        #endregion

        #endregion

    }
}
