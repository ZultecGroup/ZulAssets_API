using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;
using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {

        #region Declaration & Constructor

        private static string SP_AllLocations = "[dbo].[SP_AllLocation]";
        private static string SP_GetInsertUpdateDeleteLocation = "[dbo].[SP_GetInsertUpdateDeleteLocation]";
        private static string SP_GetAssetsByLocationID = "[dbo].[SP_GetAssetsByLocID]";
        private static string SP_CheckAssetsCountAgainstLocID = "[dbo].[SP_CheckAssetsCountAgainstLocID]";
        private static string SP_CheckChildForLocation = "[dbo].[SP_CheckChildForLocation]";
        private static string SP_GetInvLocAgainstDevSerialNo = "[dbo].[SP_GetInvLocAgainstDevSerialNo]";
        private static string SP_GetLocationsOfLocLevel0 = "[dbo].[SP_GetLocationsOfLocLevel0]";

        private readonly IConfiguration _configuration;

        public LocationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region All Locations
        /// <summary>
        /// Get All Locations API
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAllLocations")]
        //[Authorize]
        public IActionResult GetAllLocations()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllLocations(SP_AllLocations);
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

        #region All Locations With Params
        /// <summary>
        /// Get All Locations API against DeviceID
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("GetAllLocationsByHardwareID")]
        [Authorize]
        public IActionResult GetAllLocationsByHardwareID([FromBody] DeviceReg deviceReq)
        {
            Message msg = new Message();
            try
            {

                DataTable dtInvLoc = DataLogic.GetInvLocAgainstDevSerialNo(deviceReq.DeviceSerialNo, SP_GetInvLocAgainstDevSerialNo);

                DataTable mergeDT = GeneralFunctions.MergeRows(dtInvLoc);

                string LocIDs = GeneralFunctions.MergeAllRowsWithQuotes(mergeDT).Replace("|", "','");
                DataTable dataTable = new DataTable();
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                string query = @"SELECT locid, LocationFullPath AS [Values], LocLevel, REPLACE(CompCode, '\', '-') AS ID FROM Location WHERE LocLevel = (SELECT MAX(locLevel) FROM location WHERE locid IN (@LocIDs)) AND locid IN (@LocIDs) ORDER BY LocID;";

                // Replace @LocIDs with the actual string in the query
                query = query.Replace("@LocIDs", LocIDs);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Load the reader into the DataTable
                            dataTable.Load(reader);
                        }
                    }
                }

                return Ok(dataTable);

            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Location By ID
        /// <summary>
        /// Get Location By ID API
        /// </summary>
        /// <returns>Returns a detail against Loc ID</returns>
        [HttpPost("GetAssetsByLocationID")]
        [Authorize]
        public IActionResult GetAssetsByLocationID([FromBody] LocationRequest locReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAssetsByLocationID(locReq, SP_GetAssetsByLocationID);
                //DataTable dt = DataLogic.GetLocationByID(locReq, SP_GetLocationByLocID);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        DataSet newDS = new DataSet();
                        DataTable newDT = new DataTable();
                        DataRow newDR = newDT.NewRow();
                        newDT.Columns.Add("ErrorMessage");
                        newDT.Columns.Add("Status");
                        newDR["ErrorMessage"] = msg.message;
                        newDR["Status"] = msg.status;
                        newDT.Rows.Add(newDR);
                        //newDS.Tables.Add(newDT);
                        return Ok(newDT);
                    }
                    else
                    {

                        DataTable dtforDSTable1 = new DataTable();
                        DataTable dtforDSTable2 = new DataTable();
                        dtforDSTable1 = ds.Tables[0];
                        dtforDSTable2 = ds.Tables[1];

                        List<DataRow> rows_to_remove = new List<DataRow>();
                        foreach (DataRow row1 in dtforDSTable1.Rows)
                        {
                            foreach (DataRow row2 in dtforDSTable2.Rows)
                            {
                                if (row1["Barcode"].ToString() == row2["Barcode"].ToString())
                                {
                                    rows_to_remove.Add(row1);
                                }
                            }
                        }

                        foreach (DataRow row in rows_to_remove)
                        {
                            dtforDSTable1.Rows.Remove(row);
                            dtforDSTable1.AcceptChanges();
                        }

                        dtforDSTable1.Merge(dtforDSTable2);

                        List<DataRow> rows_to_remove2 = new List<DataRow>();
                        foreach (DataRow newRow in dtforDSTable1.Rows)
                        {
                            if (newRow["CompleteCode"].ToString() == locReq.LocID && (newRow["Status"].ToString() == "2" || newRow["Status"].ToString() == "3"))
                            {
                                rows_to_remove2.Add(newRow);
                            }
                        }

                        foreach (DataRow newRowsToRemove in rows_to_remove2)
                        {
                            dtforDSTable1.Rows.Remove(newRowsToRemove);
                            dtforDSTable1.AcceptChanges();
                        }

                        for (int i = 0; i < dtforDSTable1.Rows.Count; i++)
                        {
                            dtforDSTable1.Rows[i]["CompleteCode"] = locReq.LocID;
                        }

                        return Ok(dtforDSTable1);
                        
                    }
                }
                else
                {
                    DataTable noDataDT = new DataTable();
                    noDataDT = ds.Tables[0];
                    return Ok(noDataDT);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region All Locations For TreeView
        /// <summary>
        /// Get All Locations by passing the "Get = 1" and others as blank
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("GetAllLocationsTreeView")]
        [Authorize]
        public IActionResult GetAllLocationsTreeView(LocationTree locTree)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllLocationsTree(locTree, SP_GetInsertUpdateDeleteLocation);
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

        #region Add Location

        /// <summary>
        /// Insert a Location by passing all parameters with "Add = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertLocation")]
        [Authorize]
        public IActionResult InsertLocation([FromBody] LocationTree locTreeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertLocation(locTreeReq, SP_GetInsertUpdateDeleteLocation);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Location", 1, "Inserted", locTreeReq.LoginName, "dbo.Location");
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

        #region Update Location

        /// <summary>
        /// Update a Location by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateLocation")]
        //[Authorize]
        public IActionResult UpdateLocation([FromBody] LocationTree locTreeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateLocation(locTreeReq, SP_GetInsertUpdateDeleteLocation);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Location", 1, "Updated", locTreeReq.LoginName, "dbo.Location");
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

        #region Delete Location

        /// <summary>
        /// Delete a Location by passing "Delete = 1" and LocationID as parameters
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteLocation")]
        [Authorize]
        public IActionResult DeleteLocation([FromBody] LocationTree locTreeReq)
        {
            Message msg = new Message();
            try
            {
                #region Check Child against Location ID

                DataTable checkChildCountAgainstLocID_DT = DataLogic.CheckChildForLocation(locTreeReq, SP_CheckChildForLocation);
                int childCount = Convert.ToInt32(checkChildCountAgainstLocID_DT.Rows[0]["Child_Count"].ToString());

                #endregion

                if (childCount > 0)
                {
                    msg.message = "Unable to delete the location. Child(s) are concern with this location.";
                    msg.status = "404";
                    return Ok(msg);
                }
                else
                {
                    #region Check Assets Count against Location ID

                    DataTable checkAssetCountAgainstLocID_DT = DataLogic.CheckAssetCountAgainstLocID(locTreeReq, SP_CheckAssetsCountAgainstLocID);
                    int assetsCount = Convert.ToInt32(checkAssetCountAgainstLocID_DT.Rows[0]["Count"].ToString());

                    #endregion

                    if (assetsCount > 0)
                    {
                        msg.message = "Unable to delete the location. (" + assetsCount + ") assets are concerned with the selected location.";
                        msg.status = "404";
                        return Ok(msg);
                    }
                    else
                    {
                        DataTable dt = DataLogic.DeleteLocation(locTreeReq, SP_GetInsertUpdateDeleteLocation);
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
                                    DataTable dt2 = DataLogic.InsertAuditLogs("Location", 1, "Deleted", locTreeReq.LoginName, "dbo.Location");
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
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get All Locations of LocLevel = 0

        /// <summary>
        /// Get All Locations of LocLevel 0 by passing the "LoginName" and others as blank to get Locations of current user company
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("GetAllLocationsOfLocLevel0")]
        [Authorize]
        public IActionResult GetAllLocationsOfLocLevel0(QuarterlyReportRequestParams quarterlyReportRequestParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllLocationsOfLocLevel0(quarterlyReportRequestParams, SP_GetLocationsOfLocLevel0);
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

    }
}
