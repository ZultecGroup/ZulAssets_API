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

        #region Standard Reports

        #region CompanyAssets


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("CompanyAssets")]
        //[Authorize]
        public IActionResult CompanyAssets([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_CompanyAssets);
                if (dt.Rows.Count > 0)
                {
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

        #region AssetDetails


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetDetails")]
        //[Authorize]
        public IActionResult AssetDetails([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_AssetDetails);
                if (dt.Rows.Count > 0)
                {
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

        #region AssetLedger


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetLedger")]
        //[Authorize]
        public IActionResult AssetLedger([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_AssetLedger);
                if (dt.Rows.Count > 0)
                {
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

        #region AssetLog


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetLog")]
        //[Authorize]
        public IActionResult AssetLog([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_AssetLog);
                if (dt.Rows.Count > 0)
                {
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

        #region AssetTagging


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("AssetTagging")]
        //[Authorize]
        public IActionResult AssetTagging([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_AssetTagging);
                if (dt.Rows.Count > 0)
                {
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

        #region DisposedAssets


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DisposedAssets")]
        //[Authorize]
        public IActionResult DisposedAssets([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_DisposedAssets);
                if (dt.Rows.Count > 0)
                {
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

        #region ItemsInventory


        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ItemsInventory")]
        //[Authorize]
        public IActionResult ItemsInventory([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_ItemsInventory);
                if (dt.Rows.Count > 0)
                {
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

        #region NewTags

        /// /// <param name="reportReqParams"></param>
        /// <returns>This will return a message of success</returns>
        [HttpPost("NewTags")]
        //[Authorize]
        public IActionResult NewTags([FromBody] ReportReqParams reportReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.StandardReport(reportReqParams, SP_Report_NewTags);
                if (dt.Rows.Count > 0)
                {
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

                                return Ok( new
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

    }
}