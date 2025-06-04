using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZulAssetsBackEnd_API.BAL;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{  //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {

        #region Declaration

        private static string SP_GetAllAuditReportsDD = "[dbo].[SP_GetAllAuditReportsDD]";
        private static string SP_GetAllStandardReportsDD = "[dbo].[SP_GetAllStandardReportsDD]";

        #region Standard Reports

        private static string SP_Report_CompanyAssets = "[dbo].[SP_Report_CompanyAssets]";
        private static string SP_Report_AssetDetails = "[dbo].[SP_Report_AssetDetails]";
        private static string SP_Report_AssetLedger = "[dbo].[SP_Report_AssetLedger]";
        private static string SP_Report_AssetLog = "[dbo].[SP_Report_AssetLog]";
        private static string SP_Report_AssetTagging = "[dbo].[SP_Report_AssetTagging]";
        private static string SP_Report_DisposedAssets = "[dbo].[SP_Report_DisposedAssets]";
        private static string SP_Report_ItemsInventory = "[dbo].[SP_Report_ItemsInventory]";
        private static string SP_Report_NewTags = "[dbo].[SP_Report_NewTags]";

        #endregion

        #region Audit Reports

        private static string SP_AnnonymousAssets = "[dbo].[SP_Report_AnnonymousAssets]";
        private static string SP_MissingAssets = "[dbo].[SP_MissingAssets_AuditReport]";

        private static string SP_FoundAssets = "[dbo].[SP_Report_FoundMisplacedTransferredAssets]";
        private static string SP_MisplacedAssets = "[dbo].[SP_Report_FoundMisplacedTransferredAssets]";
        private static string SP_TransferredAssets = "[dbo].[SP_Report_FoundMisplacedTransferredAssets]";

        private static string SP_AllocatedAssets = "[dbo].[SP_Report_AllocatedAssets]";
        private static string SP_AllAssets = "[dbo].[SP_AllAssets_AuditReport]";
        private static string SP_CostCenterAudit = "[dbo].[SP_Report_CostCenterAudit]";

        #endregion

        #region Extended Reports

        private static string SP_ExtendedAssetStatementReport = "[dbo].[SP_ExtendedAssetStatementReport]";
        private static string SP_GetInvSchsForExtendedReport = "[dbo].[SP_GetInvSchsForExtendedReport]";
        private static string SP_GetLocationsOfLocLevel0 = "[dbo].[SP_GetLocationsOfLocLevel0]";
        private static string SP_GetAst_HistoryAssets = "[dbo].[SP_GetAst_HistoryAssets]";
        private static string SP_GetLocAgainstLocID = "[dbo].[SP_GetLocAgainstLocID]";
        private static string SP_GetAllAssetsAgainstLocID = "[dbo].[SP_GetAllAssetsAgainstLocID]";

        #endregion

        #endregion

        #region Get All Audit Reports

        /// <summary>
        /// This API is used to get All Audit Reports for Dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllAuditReportsDD")]
        [Authorize]
        public IActionResult GetAllAuditReportsDD()
        {
            Message msg = new Message();
            try
            {

                DataTable dt = DataLogic.GetAllAuditReportsDD(SP_GetAllAuditReportsDD);

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
                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get All Standard Reports
        /// <summary>
        /// This API is used to get All Audit Reports for Dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllStandardReportsDD")]
        [Authorize]
        public IActionResult GetAllStandardReportsDD()
        {
            Message msg = new Message();
            try
            {

                DataTable dt = DataLogic.GetAllAuditReportsDD(SP_GetAllStandardReportsDD);

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
                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }
        #endregion

        #region Standard Reports

        #region Company Assets

        /// <param name="reportReqParams"></param>
        /// <summary>
        /// This API is used to get Company Assets
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("CompanyAssets")]
        [Authorize]
        public IActionResult CompanyAssets([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_CompanyAssets);
                if (ds.Tables[0].Rows.Count > 0)
                {
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
                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                totalAssetsCost = totalAssetsCost,
                                data = table1
                            });
                        }
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

        #region Asset Details

        /// <param name="reportReqParams"></param>
        /// <summary>
        /// This API is used to get Complete Asset Details
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetDetails")]
        [Authorize]
        public IActionResult AssetDetails([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_AssetDetails);
                if (ds.Tables[0].Rows.Count > 0)
                {
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
                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                totalAssetsCost = totalAssetsCost,
                                data = table1
                            });
                        }
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

        #region Asset Ledger

        /// <param name="reportReqParams"></param>
        /// <summary>
        /// This API is used to get Asset Ledger Details
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetLedger")]
        [Authorize]
        public IActionResult AssetLedger([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_AssetLedger);
                if (ds.Tables[0].Rows.Count > 0)
                {
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

                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                totalAssetsCost = totalAssetsCost,
                                data = table1
                            });
                        }
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

        //#region Asset Log

        ///// <param name="reportReqParams"></param>

        ///// <returns>This will return a message of success</returns>
        //[HttpPost("AssetLog")]
        ////[Authorize]
        //public IActionResult AssetLog([FromBody] ReportReqParams reportReqParams)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_AssetLog);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                if (ds.Tables[0].Columns.Contains("ErrorMessage"))
        //                {
        //                    msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //                    msg.status = "401";
        //                    return Ok(msg);
        //                }
        //                else
        //                {
        //                    //return Ok(ds);

        //                    #region Replace Table Names

        //                    DataTable table = ds.Tables["Table"];
        //                    DataTable table1 = ds.Tables["Table1"];

        //                    table.TableName = "TotalRowsCount";
        //                    table1.TableName = "data";

        //                    #endregion

        //                    int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
        //                    string totalAssetsCost = table.Rows[0][1].ToString();

        //                    return Ok(
        //                    new
        //                    {
        //                        totalRowsCount = totalRowsCounts,
        //                        totalAssetsCost = totalAssetsCost,
        //                        data = table1
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                return Ok(ds);
        //            }
        //        }
        //        else
        //        {
        //            return Ok(ds);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.message = ex.Message;
        //        return Ok(msg);
        //    }
        //}

        //#endregion

        #region Asset Tagging

        /// <param name="reportReqParams"></param>
        /// <summary>
        /// This API is used to generate Asset Tagging Report
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetTagging")]
        [Authorize]
        public IActionResult AssetTagging([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_AssetTagging);
                if (ds.Tables[0].Rows.Count > 0)
                {
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

                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                totalAssetsCost = totalAssetsCost,
                                data = table1
                            });
                        }
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

        #region Disposed Assets

        /// <param name="disposedAssetsReportReqParams"></param>
        /// <summary>
        /// This API is used to generate Disposed Assets Report
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DisposedAssets")]
        [Authorize]
        public IActionResult DisposedAssets([FromBody] DisposedAssetsReportReqParams disposedAssetsReportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.DisposedAssetsReport(disposedAssetsReportReqParams, SP_Report_DisposedAssets);
                if (ds.Tables[0].Rows.Count > 0)
                {
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

                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                totalAssetsCost = totalAssetsCost,
                                data = table1
                            });
                        }
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

        #region Items Inventory

        /// <param name="reportReqParams"></param>
        /// <summary>
        /// This API is used to generate Asset items count Report
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ItemsInventory")]
        [Authorize]
        public IActionResult ItemsInventory([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_ItemsInventory);
                if (ds.Tables[0].Rows.Count > 0)
                {
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
                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalAssetCount = totalRowsCounts,
                                totalAssetItemsCount = totalAssetsCost,
                                data = table1
                            });
                        }
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

        #region New Tags

        /// <param name="reportReqParams"></param>
        /// <summary>
        /// This API is used to generate New Tag Assets Report
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("NewTags")]
        [Authorize]
        public IActionResult NewTags([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.StandardReport(reportReqParams, SP_Report_NewTags);
                if (ds.Tables[0].Rows.Count > 0)
                {
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
                            //return Ok(ds);

                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);
                            string totalAssetsCost = table.Rows[0][1].ToString();

                            return Ok(
                            new
                            {
                                totalAssetCount = totalRowsCounts,
                                totalAssetsCost = totalAssetsCost,
                                data = table1
                            });
                        }
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

        #endregion

        #region Audit Reports

        #region Missing Assets 

        /// <summary>
        /// This API is used to get Missing Assets against Inventory Schedule and Inventory Location
        /// </summary>
        /// <param name="reportAllAssetsReqParams"></param>
        /// <returns></returns>
        [HttpPost("MissingAssets")]
        [Authorize]
        public IActionResult MissingAssets([FromBody] ReportAllAssetsReqParams reportAllAssetsReqParams)
        {
            Message msg = new Message();
            try
            {
                //DataSet ds = new DataSet();

                //var result = GeneralFunctions.ConvertInvSchCodesInvLocs(reportAllAssetsReqParams.InvSchCode, reportAllAssetsReqParams.InvLoc);

                DataSet ds = new DataSet();

                DataTable invSchInvLocDT = new DataTable();

                invSchInvLocDT.Columns.Add("InvSchCode");
                invSchInvLocDT.Columns.Add("InvLoc");

                if (reportAllAssetsReqParams.rptAllAstsAuditTree != null)
                {
                    invSchInvLocDT = ListintoDataTable.ToDataTable2(reportAllAssetsReqParams.rptAllAstsAuditTree);
                }

                List<string> invSchCodeList = new List<string>();
                List<string> invLocList = new List<string>();

                for (int i = 0; i < invSchInvLocDT.Rows.Count; i++)
                {

                    string invSchCode = invSchInvLocDT.Rows[i]["InvSchCode"].ToString();
                    invSchCodeList.Add(invSchCode);

                    string invLoc = invSchInvLocDT.Rows[i]["InvLoc"].ToString();
                    invLocList.Add(invLoc);

                }

                var result = GeneralFunctions.ConvertInvSchCodesInvLocs(invSchCodeList, invLocList);

                string SelInvCode = result.SelInvCode; string SelLoc = result.SelLoc; int rowCount = result.rowCount;

                if (rowCount > 0)
                {
                    ds = DataLogic.MissingAuditReport(SelInvCode, SelLoc, reportAllAssetsReqParams.paginationParam.PageIndex,
                        reportAllAssetsReqParams.paginationParam.PageSize, SP_MissingAssets);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                            }
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
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Found Assets

        /// <summary>
        /// This API is used to get Found Assets against Inventory Schedule and Inventory Location
        /// and posted or unposted data
        /// </summary>
        /// <param name="rptFMTAReqParams"></param>
        /// <returns></returns>
        [HttpPost("FoundAssets")]
        [Authorize]
        public IActionResult FoundAssets([FromBody] ReportFoundMisplacedTransferredAssetsReqParams rptFMTAReqParams)
        {
            Message msg = new Message();
            try
            {

                DataSet ds = new DataSet();

                DataTable invSchInvLocDT = new DataTable();

                invSchInvLocDT.Columns.Add("InvSchCode");
                invSchInvLocDT.Columns.Add("InvLoc");

                if (rptFMTAReqParams.rptAllAstsAuditTree != null)
                {
                    invSchInvLocDT = ListintoDataTable.ToDataTable2(rptFMTAReqParams.rptAllAstsAuditTree);
                }

                List<string> invSchCodeList = new List<string>();
                List<string> invLocList = new List<string>();

                for (int i = 0; i < invSchInvLocDT.Rows.Count; i++)
                {

                    string invSchCode = invSchInvLocDT.Rows[i]["InvSchCode"].ToString();
                    invSchCodeList.Add(invSchCode);

                    string invLoc = invSchInvLocDT.Rows[i]["InvLoc"].ToString();
                    invLocList.Add(invLoc);

                }

                var result = GeneralFunctions.ConvertInvSchCodesInvLocs(invSchCodeList, invLocList);

                string SelInvCode = result.SelInvCode; string SelLoc = result.SelLoc; int rowCount = result.rowCount;

                if (rowCount > 0)
                {
                    ds = DataLogic.FoundAuditReport(SelInvCode, SelLoc, rptFMTAReqParams.paginationParam.PageIndex,
                        rptFMTAReqParams.paginationParam.PageSize, rptFMTAReqParams.Posted, SP_FoundAssets);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                            }
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
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Misplaced Assets

        /// <summary>
        /// This API is used to get all Misplaced Assets against Inventory Schedule and Inventory Location
        /// and posted or unposted data
        /// </summary>
        /// <param name="rptFMTAReqParams"></param>
        /// <returns></returns>
        [HttpPost("MisplacedAssets")]
        [Authorize]
        public IActionResult MisplacedAssets([FromBody] ReportFoundMisplacedTransferredAssetsReqParams rptFMTAReqParams)
        {
            Message msg = new Message();
            try
            {

                DataSet ds = new DataSet();

                DataTable invSchInvLocDT = new DataTable();

                invSchInvLocDT.Columns.Add("InvSchCode");
                invSchInvLocDT.Columns.Add("InvLoc");

                if (rptFMTAReqParams.rptAllAstsAuditTree != null)
                {
                    invSchInvLocDT = ListintoDataTable.ToDataTable2(rptFMTAReqParams.rptAllAstsAuditTree);
                }

                List<string> invSchCodeList = new List<string>();
                List<string> invLocList = new List<string>();

                for (int i = 0; i < invSchInvLocDT.Rows.Count; i++)
                {

                    string invSchCode = invSchInvLocDT.Rows[i]["InvSchCode"].ToString();
                    invSchCodeList.Add(invSchCode);

                    string invLoc = invSchInvLocDT.Rows[i]["InvLoc"].ToString();
                    invLocList.Add(invLoc);

                }

                var result = GeneralFunctions.ConvertInvSchCodesInvLocs(invSchCodeList, invLocList);

                string SelInvCode = result.SelInvCode; string SelLoc = result.SelLoc; int rowCount = result.rowCount;

                if (rowCount > 0)
                {
                    ds = DataLogic.MisplacedAuditReport(SelInvCode, SelLoc, rptFMTAReqParams.paginationParam.PageIndex,
                        rptFMTAReqParams.paginationParam.PageSize, rptFMTAReqParams.Posted, SP_MisplacedAssets);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                            }
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
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Transferred Assets

        /// <summary>
        /// This API is used to get all Transferred Assets against Inventory Schedule and Inventory Location
        /// and posted or unposted data
        /// </summary>
        /// <param name="rptFMTAReqParams"></param>
        /// <returns></returns>
        [HttpPost("TransferredAssets")]
        [Authorize]
        public IActionResult TransferredAssets([FromBody] ReportFoundMisplacedTransferredAssetsReqParams rptFMTAReqParams)
        {
            Message msg = new Message();
            try
            {

                DataSet ds = new DataSet();

                DataTable invSchInvLocDT = new DataTable();

                invSchInvLocDT.Columns.Add("InvSchCode");
                invSchInvLocDT.Columns.Add("InvLoc");

                if (rptFMTAReqParams.rptAllAstsAuditTree != null)
                {
                    invSchInvLocDT = ListintoDataTable.ToDataTable2(rptFMTAReqParams.rptAllAstsAuditTree);
                }

                List<string> invSchCodeList = new List<string>();
                List<string> invLocList = new List<string>();

                for (int i = 0; i < invSchInvLocDT.Rows.Count; i++)
                {

                    string invSchCode = invSchInvLocDT.Rows[i]["InvSchCode"].ToString();
                    invSchCodeList.Add(invSchCode);

                    string invLoc = invSchInvLocDT.Rows[i]["InvLoc"].ToString();
                    invLocList.Add(invLoc);

                }

                var result = GeneralFunctions.ConvertInvSchCodesInvLocs(invSchCodeList, invLocList);

                string SelInvCode = result.SelInvCode; string SelLoc = result.SelLoc; int rowCount = result.rowCount;

                if (rowCount > 0)
                {
                    ds = DataLogic.TransferredAuditReport(SelInvCode, SelLoc, rptFMTAReqParams.paginationParam.PageIndex,
                        rptFMTAReqParams.paginationParam.PageSize, rptFMTAReqParams.Posted, SP_TransferredAssets);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                            }
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
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region All Assets Audit Report
        /// <summary>
        /// This API is used to get All Assets against Inventory Schedule and Inventory Location
        /// </summary>
        /// <param name="reportAllAssetsReqParams"></param>
        /// <returns></returns>
        [HttpPost("AllAssetsAuditReport")]
        [Authorize]
        public IActionResult AllAssetsAuditReport([FromBody] ReportAllAssetsReqParams reportAllAssetsReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = new DataSet();

                DataTable invSchInvLocDT = new DataTable();

                invSchInvLocDT.Columns.Add("InvSchCode");
                invSchInvLocDT.Columns.Add("InvLoc");

                #region Conversion of List Tree to DataTable

                if (reportAllAssetsReqParams.rptAllAstsAuditTree != null)
                {
                    invSchInvLocDT = ListintoDataTable.ToDataTable2(reportAllAssetsReqParams.rptAllAstsAuditTree);
                }

                #endregion

                #region Convert of DataTable to List string

                List<string> invSchCodeList = new List<string>();
                List<string> invLocList = new List<string>();

                for (int i = 0; i < invSchInvLocDT.Rows.Count; i++)
                {

                    string invSchCode = invSchInvLocDT.Rows[i]["InvSchCode"].ToString();
                    invSchCodeList.Add(invSchCode);

                    string invLoc = invSchInvLocDT.Rows[i]["InvLoc"].ToString();
                    invLocList.Add(invLoc);

                }

                #endregion

                var result = GeneralFunctions.ConvertInvSchCodesInvLocs(invSchCodeList, invLocList);

                string SelInvCode = result.SelInvCode; string SelLoc = result.SelLoc; int rowCount = result.rowCount;

                if (rowCount > 0)
                {
                    ds = DataLogic.AllAssetsAuditReport(SelInvCode, SelLoc, reportAllAssetsReqParams.paginationParam.PageIndex,
                        reportAllAssetsReqParams.paginationParam.PageSize, SP_AllAssets);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                                DataTable data = ds.Tables["Table1"];
                                DataTable summaryCountData = ds.Tables["Table2"];

                                table.TableName = "TotalRowsCount";
                                data.TableName = "data";
                                summaryCountData.TableName = "summaryCountData";

                                #endregion

                                #region Adding Total column in summary count data

                                // Calculate the sum of the StatusCount column
                                int totalCount = 0;
                                foreach (DataRow row in summaryCountData.Rows)
                                {
                                    // Ensure the column exists and is not null or empty
                                    if (row["StatusCount"] != DBNull.Value)
                                    {
                                        totalCount += Convert.ToInt32(row["StatusCount"]);
                                    }
                                }

                                // Create a new row for the "Total" entry
                                DataRow totalRow = summaryCountData.NewRow();
                                totalRow["StatusDesc"] = "Total"; // Set the "StatusDesc" as "Total"
                                totalRow["StatusCount"] = totalCount; // Set the sum for StatusCount

                                // Add the "Total" row to the DataTable
                                summaryCountData.Rows.Add(totalRow);

                                #endregion

                                int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);

                                return Ok(new
                                {
                                    totalRowsCount = totalRowsCounts,
                                    data = data,
                                    summaryCountData = summaryCountData
                                });
                            }
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
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        //#region AnnonymousAssets
        ///// /// <param name="reportauditReqParams"></param>
        //[HttpPost("AnnonymousAssets")]
        ////[Authorize]
        //public IActionResult AnnonymousAssets([FromBody] ReportAuditReqParams reportauditReqParams)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.AuditReport(reportauditReqParams, SP_AnnonymousAssets);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                if (dt.Columns.Contains("ErrorMessage"))
        //                {
        //                    msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                    msg.status = "401";
        //                    return Ok(msg);
        //                }
        //                else
        //                {
        //                    return Ok(dt);
        //                }
        //            }
        //            else
        //            {
        //                return Ok(dt);
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

        //#region AllocatedAssets
        ///// /// <param name="reportauditReqParams"></param>
        //[HttpPost("AllocatedAssets")]
        ////[Authorize]
        //public IActionResult AllocatedAssets([FromBody] ReportAuditReqParams reportauditReqParams)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.AuditReport(reportauditReqParams, SP_AllocatedAssets);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                if (dt.Columns.Contains("ErrorMessage"))
        //                {
        //                    msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                    msg.status = "401";
        //                    return Ok(msg);
        //                }
        //                else
        //                {
        //                    return Ok(dt);
        //                }
        //            }
        //            else
        //            {
        //                return Ok(dt);
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

        //#region CostCenterAudit
        ///// /// <param name="reportauditReqParams"></param>
        //[HttpPost("CostCenterAudit")]
        ////[Authorize]
        //public IActionResult CostCenterAudit([FromBody] ReportAuditReqParams reportauditReqParams)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.AuditReport(reportauditReqParams, SP_CostCenterAudit);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                if (dt.Columns.Contains("ErrorMessage"))
        //                {
        //                    msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                    msg.status = "401";
        //                    return Ok(msg);
        //                }
        //                else
        //                {
        //                    return Ok(dt);
        //                }
        //            }
        //            else
        //            {
        //                return Ok(dt);
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

        #region Extended Reports

        #region Asset Statement Report

        /// <summary>
        /// This API is used to get all the asset against the selected custodian(s). This report will be consider as a part of Extended Reports
        /// </summary>
        /// <param name="bulkSelectionParametersReq"></param>
        /// <returns></returns>
        [HttpPost("AssetStatementReport")]
        [Authorize]
        public IActionResult AssetStatementReport([FromBody] BulkSelectionParameters bulkSelectionParametersReq)
        {
            Message msg = new Message();
            try
            {
                //DataSet ds = new DataSet();

                //var result = GeneralFunctions.ConvertInvSchCodesInvLocs(reportAllAssetsReqParams.InvSchCode, reportAllAssetsReqParams.InvLoc);

                DataSet ds = new DataSet();

                DataTable CustodiansDT = new DataTable();

                CustodiansDT.Columns.Add("CustodianID");

                if (bulkSelectionParametersReq.Custodian != null)
                {
                    CustodiansDT = ListintoDataTable.ToDataTable2(bulkSelectionParametersReq.Custodian);
                }

                List<string> CustodianList = new List<string>();

                for (int i = 0; i < CustodiansDT.Rows.Count; i++)
                {

                    string custodianID = CustodiansDT.Rows[i]["CustodianID"].ToString();
                    CustodianList.Add(custodianID);

                }

                var result = GeneralFunctions.ConvertCustodianIDs(CustodianList);

                string selCustodians = result.SelCustodians; int rowCount = result.rowCount;

                if (rowCount > 0)
                {
                    ds = DataLogic.AssetStatementReport(selCustodians, SP_ExtendedAssetStatementReport);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
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
                                table.TableName = "data";

                                #endregion

                                return Ok(ds);
                            }
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
                return Ok(ds);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Quarterly Report

        /// <summary>
        /// This API is used to get all the asset against the selected custodian(s). This report will be consider as a part of Extended Reports
        /// </summary>
        /// <param name="quarterlyReportRequestParams"></param>
        /// <returns></returns>
        [HttpPost("QuarterlyReport")]
        [Authorize]
        public IActionResult QuarterlyReport([FromBody] QuarterlyReportRequestParams quarterlyReportRequestParams)
        {
            Message msg = new Message();
            try
            {
                //DateTime startTime = DateTime.Now;
                DataTable dt = new DataTable();

                if (quarterlyReportRequestParams.Year == "" || quarterlyReportRequestParams.Year == null)
                {
                    quarterlyReportRequestParams.Year = DateTime.Now.Year.ToString();
                }

                if (quarterlyReportRequestParams.Quarterly == "")
                {
                    quarterlyReportRequestParams.Quarterly = null;
                }


                dt = DataLogic.QuarterlyReport(quarterlyReportRequestParams.Quarterly, quarterlyReportRequestParams.Year, SP_GetInvSchsForExtendedReport);

                List<string> invSchList = new List<string>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string invSchCode = dt.Rows[i]["InvSchCode"].ToString();
                    invSchList.Add(invSchCode);

                }

                var result = GeneralFunctions.ConvertInvSchs(invSchList);

                string selInvSchs = result.SelInvSchs; int rowCount = result.rowCount;

                // Final SQL query string

                DataTable dt2 = DataLogic.GetAllLocationsOfLocLevel0AgainstLocID(quarterlyReportRequestParams, SP_GetLocAgainstLocID);  //LocID, LocDesc

                #region Finding Inventory Schedules

                dt2.Columns.Add("PhysicalCount", typeof(string));
                dt2.Columns.Add("LocationCount", typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] invLocSplit = dt.Rows[i]["InvLoc"].ToString().Split("|");  //3-1-1|3-2-1|15-1-1
                    string invScCode = dt.Rows[i]["InvSchCode"].ToString();
                    string invSchDesc = dt.Rows[i]["InvDesc"].ToString();
                    string locDesc = string.Empty;
                    int inc = 0;
                    string previousSubString = string.Empty;
                    for (int j = 0; j < invLocSplit.Length; j++)    //3-1-1     3-2-1    15-1-1
                    {
                        if (j > 0)
                        {
                            string previousValue = invLocSplit[j - 1];
                            previousSubString = previousValue.Substring(0, previousValue.IndexOf("-"));
                        }
                        string invLocSubString = invLocSplit[j].Substring(0, invLocSplit[j].IndexOf("-"));  //3 3 
                        if (previousSubString != invLocSubString)
                        {
                            DataRow[] foundRows = dt2.Select("LocID = '" + invLocSubString + "'");
                            if (foundRows.Length > 0)
                            {
                                string numOfPhyCount = foundRows[0]["PhysicalCount"].ToString();
                                if (numOfPhyCount == "")
                                {
                                    inc = 1;
                                    foundRows[0]["PhysicalCount"] = inc;
                                }
                                else
                                {
                                    foundRows[0]["PhysicalCount"] = Convert.ToInt32(foundRows[0]["PhysicalCount"]) + 1;
                                }
                            }
                        }


                    };
                };

                #endregion

                #region Finding Total Locations

                //DataTable originalTable = /* your input table */;
                // Step 2: Flatten all locations into another table
                DataTable locationSummaryTable = new DataTable();
                locationSummaryTable.Columns.Add("Location", typeof(string));
                locationSummaryTable.Columns.Add("Count", typeof(int));

                foreach (DataRow row in dt.Rows)
                {
                    string invLoc = row["InvLoc"]?.ToString();
                    if (string.IsNullOrWhiteSpace(invLoc)) continue;

                    foreach (string loc in invLoc.Split('|'))
                    {
                        if (!string.IsNullOrWhiteSpace(loc))
                        {
                            locationSummaryTable.Rows.Add(loc.Trim(), 1);
                        }
                    }
                }

                // Step 3: Group by Location and Sum the Count
                DataTable groupedTable = new DataTable();
                groupedTable.Columns.Add("Location", typeof(string));
                groupedTable.Columns.Add("TotalCount", typeof(int));

                var grouped1 = locationSummaryTable.AsEnumerable()
                    .GroupBy(r => r.Field<string>("Location"))
                    .Select(g => new
                    {
                        Location = g.Key,
                        TotalCount = g.Sum(r => r.Field<int>("Count"))
                    });

                foreach (var item in grouped1)
                {
                    groupedTable.Rows.Add(item.Location, item.TotalCount);
                }

                // Step 3: Group by the value before the first hyphen and sum the TotalCounts
                DataTable finalGroupTable = new DataTable();
                finalGroupTable.Columns.Add("RegionID", typeof(string));      // The left-most value
                finalGroupTable.Columns.Add("RegionCount", typeof(int));      // Summed count

                var regionGrouped = groupedTable.AsEnumerable()
                    .Select(row => new
                    {
                        RegionID = row.Field<string>("Location").Split('-')[0],        // Get value before first hyphen
                        Count = row.Field<int>("TotalCount")
                    })
                    .GroupBy(x => x.RegionID)
                    .Select(g => new
                    {
                        RegionID = g.Key,
                        RegionCount = g.Sum(x => x.Count)
                    });

                foreach (var item in regionGrouped)
                {
                    finalGroupTable.Rows.Add(item.RegionID, item.RegionCount);
                }

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    string locIDfrom_DT2 = dt2.Rows[i]["LocID"].ToString();
                    for (int j = 0; j < finalGroupTable.Rows.Count; j++)
                    {
                        string locIDfrom_finalGroupTable = finalGroupTable.Rows[j]["RegionID"].ToString();

                        if (locIDfrom_finalGroupTable == locIDfrom_DT2)
                        {
                            dt2.Rows[i]["LocationCount"] = finalGroupTable.Rows[j]["RegionCount"].ToString();
                        }
                    }

                }

                #endregion

                #region Finding Total Assets Found, Transferred, Missing and Found Assets Percentage in a Main Location

                //using this condition so that in any case of assetFoundColumn, assetTransferredColumn or assetMissingColumn it will fill the ast_History_Table
                DataTable ast_History_Table = new DataTable();

                quarterlyReportRequestParams.AssetFoundColumn = (quarterlyReportRequestParams.AssetFoundPercentage == 1) ? 1 : quarterlyReportRequestParams.AssetFoundColumn;

                if (quarterlyReportRequestParams.AssetFoundColumn == 1 || quarterlyReportRequestParams.AssetTransferredColumn == 1 || quarterlyReportRequestParams.AssetMissingColumn == 1 || quarterlyReportRequestParams.AssetFoundPercentage == 1)
                {
                    ast_History_Table = DataLogic.GetAllAssetsAgainstInvSchs(selInvSchs, SP_GetAst_HistoryAssets);
                }

                #region Assets Count Column

                if (quarterlyReportRequestParams.AssetsCountColumn == 1)
                {
                    dt2.Columns.Add("AssetsCount", typeof(string));
                }

                DataTable totalAssetsLocWise = new DataTable();

                if (quarterlyReportRequestParams.AssetFoundPercentage == 1 || quarterlyReportRequestParams.AssetsCountColumn == 1)
                {
                    totalAssetsLocWise = DataLogic.GetAllAssetsAgainstLocID(quarterlyReportRequestParams, SP_GetAllAssetsAgainstLocID);
                }

                if (quarterlyReportRequestParams.AssetsCountColumn == 1)
                {
                    for (int percentageLoop = 0; percentageLoop < totalAssetsLocWise.Rows.Count; percentageLoop++)
                    {
                        int? mainLocationValue = Convert.ToInt32(totalAssetsLocWise.Rows[percentageLoop]["MainLocation"]);
                        int? totalAssetsLocWiseValue = Convert.ToInt32(totalAssetsLocWise.Rows[percentageLoop]["TotalNumberOfAssetsExists"]);

                        for (int dt2Loop = 0; dt2Loop < dt2.Rows.Count; dt2Loop++)
                        {
                            int? dt2_LocIDValue = Convert.ToInt32(dt2.Rows[dt2Loop]["LocID"]);

                            if (mainLocationValue == dt2_LocIDValue)
                            {
                                string locID = dt2.Rows[dt2Loop]["LocID"].ToString();

                                DataRow[] targetRows = dt2.Select($"LocID = '{locID}'");

                                if (targetRows.Length > 0)
                                {
                                    targetRows[0]["AssetsCount"] = totalAssetsLocWiseValue;
                                }

                            }
                        }

                    }
                }

                #endregion

                if (selInvSchs == "")
                {
                    if (quarterlyReportRequestParams.AssetFoundColumn == 1)
                    {
                        dt2.Columns.Add("AssetsFound", typeof(string));
                    }
                    if (quarterlyReportRequestParams.AssetTransferredColumn == 1)
                    {
                        dt2.Columns.Add("AssetsTransferred", typeof(string));
                    }
                    if (quarterlyReportRequestParams.AssetMissingColumn == 1)
                    {
                        dt2.Columns.Add("AssetsMissing", typeof(string));
                    }
                    if (quarterlyReportRequestParams.AssetFoundPercentage == 1)
                    {
                        dt2.Columns.Add("%OfAssetsFound", typeof(string));
                    }

                    return Ok(dt2);
                }

                #region Asset Found Column Case

                if (quarterlyReportRequestParams.AssetFoundColumn == 1)
                {
                    if (quarterlyReportRequestParams.AssetFoundPercentage == 1)
                    {
                        if (!dt2.Columns.Contains("AssetsCount"))
                        {
                            dt2.Columns.Add("AssetsCount", typeof(string));
                        }

                    }
                    dt2.Columns.Add("AssetsFound", typeof(string));

                    DataTable ast_history_TempDT = new DataTable();
                    ast_history_TempDT.Columns.Add("MainLocation", typeof(string));
                    ast_history_TempDT.Columns.Add("AssetCount", typeof(int));

                    for (int i = 0; i < ast_History_Table.Rows.Count; i++)
                    {
                        string? fullLoc = ast_History_Table.Rows[i]["CurrLoc"]?.ToString();
                        int? Status = Convert.ToInt32(ast_History_Table.Rows[i]["Status"]);

                        if (Status == 1)
                        {

                            if (!string.IsNullOrWhiteSpace(fullLoc) && fullLoc.Contains("-"))
                            {
                                string[] parts = fullLoc.Split('-');
                                string mainLocation = parts[0]; // safer than Substring

                                // Check if mainLocation already exists in the temp table
                                DataRow[] existingRows = ast_history_TempDT.Select($"MainLocation = '{mainLocation}'");

                                if (existingRows.Length > 0)
                                {
                                    // Increment the existing count
                                    existingRows[0]["AssetCount"] = Convert.ToInt32(existingRows[0]["AssetCount"]) + 1;
                                }
                                else
                                {
                                    // Add a new row
                                    ast_history_TempDT.Rows.Add(mainLocation, 1);
                                }
                            }
                        }
                    }

                    // Loop through ast_history_TempDT to update dt2
                    foreach (DataRow sourceRow in ast_history_TempDT.Rows)
                    {
                        string mainLocation = sourceRow["MainLocation"].ToString();
                        int assetCount = Convert.ToInt32(sourceRow["AssetCount"]);

                        // Match LocID with MainLocation
                        DataRow[] targetRows = dt2.Select($"LocID = '{mainLocation}'");

                        if (targetRows.Length > 0)
                        {
                            targetRows[0]["AssetsFound"] = assetCount;
                        }
                    }
                }

                #endregion

                #region Asset Transferred Column Case

                if (quarterlyReportRequestParams.AssetTransferredColumn == 1)
                {

                    dt2.Columns.Add("AssetsTransferred", typeof(string));

                    DataTable ast_history_TempDT = new DataTable();
                    ast_history_TempDT.Columns.Add("MainLocation", typeof(string));
                    ast_history_TempDT.Columns.Add("AssetCount", typeof(int));

                    for (int i = 0; i < ast_History_Table.Rows.Count; i++)
                    {
                        string? fullLoc = ast_History_Table.Rows[i]["CurrLoc"]?.ToString();
                        int? Status = Convert.ToInt32(ast_History_Table.Rows[i]["Status"]);

                        if (Status == 3)
                        {

                            if (!string.IsNullOrWhiteSpace(fullLoc) && fullLoc.Contains("-"))
                            {
                                string[] parts = fullLoc.Split('-');
                                string mainLocation = parts[0]; // safer than Substring

                                // Check if mainLocation already exists in the temp table
                                DataRow[] existingRows = ast_history_TempDT.Select($"MainLocation = '{mainLocation}'");

                                if (existingRows.Length > 0)
                                {
                                    // Increment the existing count
                                    existingRows[0]["AssetCount"] = Convert.ToInt32(existingRows[0]["AssetCount"]) + 1;
                                }
                                else
                                {
                                    // Add a new row
                                    ast_history_TempDT.Rows.Add(mainLocation, 1);
                                }
                            }
                        }
                    }

                    // Loop through ast_history_TempDT to update dt2
                    foreach (DataRow sourceRow in ast_history_TempDT.Rows)
                    {
                        string mainLocation = sourceRow["MainLocation"].ToString();
                        int assetCount = Convert.ToInt32(sourceRow["AssetCount"]);

                        // Match LocID with MainLocation
                        DataRow[] targetRows = dt2.Select($"LocID = '{mainLocation}'");

                        if (targetRows.Length > 0)
                        {
                            targetRows[0]["AssetsTransferred"] = assetCount;
                        }
                    }
                }

                #endregion

                #region Asset Missing Column Case

                if (quarterlyReportRequestParams.AssetMissingColumn == 1)
                {

                    dt2.Columns.Add("AssetsMissing", typeof(string));

                    DataTable ast_history_TempDT = new DataTable();
                    ast_history_TempDT.Columns.Add("MainLocation", typeof(string));
                    ast_history_TempDT.Columns.Add("AssetCount", typeof(int));

                    for (int i = 0; i < ast_History_Table.Rows.Count; i++)
                    {
                        string? fullLoc = ast_History_Table.Rows[i]["CurrLoc"]?.ToString();
                        int? Status = Convert.ToInt32(ast_History_Table.Rows[i]["Status"]);

                        if (Status == 0)
                        {

                            if (!string.IsNullOrWhiteSpace(fullLoc) && fullLoc.Contains("-"))
                            {
                                string[] parts = fullLoc.Split('-');
                                string mainLocation = parts[0]; // safer than Substring

                                // Check if mainLocation already exists in the temp table
                                DataRow[] existingRows = ast_history_TempDT.Select($"MainLocation = '{mainLocation}'");

                                if (existingRows.Length > 0)
                                {
                                    // Increment the existing count
                                    existingRows[0]["AssetCount"] = Convert.ToInt32(existingRows[0]["AssetCount"]) + 1;
                                }
                                else
                                {
                                    // Add a new row
                                    ast_history_TempDT.Rows.Add(mainLocation, 1);
                                }
                            }
                        }
                    }

                    // Loop through ast_history_TempDT to update dt2
                    foreach (DataRow sourceRow in ast_history_TempDT.Rows)
                    {
                        string mainLocation = sourceRow["MainLocation"].ToString();
                        int assetCount = Convert.ToInt32(sourceRow["AssetCount"]);

                        // Match LocID with MainLocation
                        DataRow[] targetRows = dt2.Select($"LocID = '{mainLocation}'");

                        if (targetRows.Length > 0)
                        {
                            targetRows[0]["AssetsMissing"] = assetCount;
                        }
                    }
                }

                #endregion

                #region Finding Percentage of Assets Found

                if (quarterlyReportRequestParams.AssetFoundPercentage == 1)
                {

                    dt2.Columns.Add("%OfAssetsFound", typeof(string));

                    for (int percentageLoop = 0; percentageLoop < totalAssetsLocWise.Rows.Count; percentageLoop++)
                    {
                        int? mainLocationValue = Convert.ToInt32(totalAssetsLocWise.Rows[percentageLoop]["MainLocation"]);
                        int? totalAssetsLocWiseValue = Convert.ToInt32(totalAssetsLocWise.Rows[percentageLoop]["TotalNumberOfAssetsExists"]);

                        for (int dt2Loop = 0; dt2Loop < dt2.Rows.Count; dt2Loop++)
                        {
                            int? dt2_LocIDValue = Convert.ToInt32(dt2.Rows[dt2Loop]["LocID"]);
                            int? dt2_AssetFoundColumnValue = dt2.Rows[dt2Loop]["AssetsFound"] != DBNull.Value ? Convert.ToInt32(dt2.Rows[dt2Loop]["AssetsFound"]) : 0;

                            if (mainLocationValue == dt2_LocIDValue)
                            {
                                //double percentageValue = Convert.ToDouble((dt2_AssetFoundColumnValue / totalAssetsLocWiseValue) * 100);
                                double percentageValue = ((double)(dt2_AssetFoundColumnValue ?? 0) / (double)(totalAssetsLocWiseValue ?? 1)) * 100;

                                string locID = dt2.Rows[dt2Loop]["LocID"].ToString();

                                DataRow[] targetRows = dt2.Select($"LocID = '{locID}'");

                                if (targetRows.Length > 0)
                                {
                                    double assetsFoundPercentage = Math.Round(percentageValue, 2);
                                    targetRows[0]["%OfAssetsFound"] = assetsFoundPercentage.ToString() + " %";
                                    targetRows[0]["AssetsCount"] = totalAssetsLocWiseValue;
                                }

                            }
                        }

                    }

                    if (quarterlyReportRequestParams.AssetsCountColumn != 1)
                    {
                        if (dt2.Columns.Contains("AssetsCount"))
                        {
                            dt2.Columns.Remove("AssetsCount");
                        }
                    }
                }

                #endregion

                #endregion

                //DateTime endTime = DateTime.Now;

                //TimeSpan duration = endTime - startTime;

                return Ok(dt2);

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