using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Inventory Schedules")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvSchController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteInvSch = "[dbo].[SP_GetInsertUpdateDeleteInvSch]";
        private static string SP_CheckCountForAstHistoryAgainstInvSchCode = "[dbo].[SP_CheckCountForAstHistoryAgainstInvSchCode]";
        private static string SP_DeleteAstHistoryAgainstInvSchCode = "[dbo].[SP_DeleteAstHistoryAgainstInvSchCode]";
        private static string SP_GetInProcessInventorySchedules = "[dbo].[SP_GetInProcessInventorySchedules]";
        private static string SP_ValidateAst_HistoryData = "[dbo].[SP_ValidateAst_HistoryData]";

        #endregion

        #region Get All Inventory Schedules
        /// <summary>
        /// Get all Inventory Schedules by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Inventory Schedules</returns>
        [HttpPost("GetAllInvSchs")]
        [Authorize]
        public IActionResult GetAllInvSchs([FromBody] InvSchReqParam invSchReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllInvSchs(invSchReqParam, SP_GetInsertUpdateDeleteInvSch);
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

                        // Set table names for clarity
                        table.TableName = "TotalRowsCount";
                        table1.TableName = "data";

                        #endregion

                        #region Splitting InvLoc & InvDev with "|" and convert into a list and include it in the response

                        // Create a list to store the final result
                        var resultList = new List<object>();

                        // Iterate through the rows of the DataTable `table1` to modify the `invLoc` and `invDev` columns
                        foreach (DataRow row in table1.Rows)
                        {
                            // Create the locTrees and deviceTrees by splitting invLoc and invDev
                            var locTrees = row["invLoc"].ToString()
                                .Split('|')
                                .Select(loc => new { locID = loc })
                                .ToList();

                            var deviceTrees = row["invDev"].ToString()
                                .Split('|')
                                .Select(dev => new { deviceHardwareID = dev })
                                .ToList();

                            // Create a new object with the desired structure
                            var resultItem = new
                            {
                                rowNo = row["rowNo"],
                                invSchCode = row["invSchCode"],
                                invDesc = row["invDesc"],
                                invStartDate = row["invStartDate"],
                                invEndDate = row["invEndDate"],
                                isDeleted = row["isDeleted"],
                                closed = row["closed"],
                                schType = row["schType"],
                                locTrees = locTrees, // Include the transformed locTrees
                                deviceTrees = deviceTrees // Include the transformed deviceTrees
                            };

                            // Add the result item to the list
                            resultList.Add(resultItem);
                        }

                        #endregion

                        #region Total Rows Count

                        // Convert the TotalRowsCount to the integer
                        int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);

                        #endregion


                        // Return the final structured response
                        return Ok(new
                        {
                            totalRowsCount = totalRowsCounts,
                            data = resultList
                        });
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

        #region Get All In Process Inventory Schedules
        /// <summary>
        /// Get all In Process Inventory Schedules
        /// </summary>
        /// <returns>This will return all the Inventory Schedules</returns>
        [HttpPost("GetInProcessInvSchs")]
        [Authorize]
        public IActionResult GetInProcessInvSchs()
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetInProcessInvSchs(SP_GetInProcessInventorySchedules);
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

                        // Set table names for clarity
                        table.TableName = "data";

                        #endregion

                        #region Total Rows Count

                        // Convert the TotalRowsCount to the integer
                        int totalRowsCounts = Convert.ToInt32(table.Rows.Count);

                        #endregion

                        // Return the final structured response
                        return Ok(new
                        {
                            totalRowsCount = totalRowsCounts,
                            data = ds
                        });
                    }
                }
                else
                {
                    // Return the final structured response
                    return Ok(new
                    {
                        totalRowsCount = 0,
                        data = ds
                    });
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Add Inventory Scheduler

        /// <summary>
        /// Insert a Inventory Scheduler by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertInvSch")]
        [Authorize]
        public IActionResult InsertInvSch([FromBody] InvSchReqParam invSchReqParam)
        {
            Message msg = new Message();
            string locIDs = "", deviceHardwareIDs = "";
            try
            {
                if (invSchReqParam.locTrees.Count > 0)
                {
                    locIDs = string.Join("|", invSchReqParam.locTrees.Select(tree => tree.LocID.ToString()));
                }
                if (invSchReqParam.deviceTrees.Count > 0)
                {
                    deviceHardwareIDs = string.Join("|", invSchReqParam.deviceTrees.Select(tree => tree.DeviceHardwareID.ToString()));
                }

                var locIDArray = locIDs.Split('|');
                var formattedLocIDs = string.Join(", ", locIDArray.Select(id => $"'{id}'"));

                DataTable dt = DataLogic.InsertInvSch(invSchReqParam, locIDs, deviceHardwareIDs, formattedLocIDs, SP_GetInsertUpdateDeleteInvSch);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Inventory Schedule", "Added", invSchReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Inventory Schedule", 1, "Inserted", invSchReqParam.LoginName, "dbo.Ast_INV_Schedule");
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

        #region Update Inventory Scheduler

        /// <summary>
        /// Update a Inventory Scheduler by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateInvSch")]
        [Authorize]
        public IActionResult UpdateInvSch([FromBody] InvSchReqParam invSchReqParam)
        {
            Message msg = new Message();
            try
            {
                if (Convert.ToInt32(DataLogic.CheckCountOfAstHistory(invSchReqParam, SP_CheckCountForAstHistoryAgainstInvSchCode).Rows[0]["TotalScannedAssets"]) == 0)  //If count = 0 means no data is processed yet
                {

                    DataTable deleteAstHistoryAgainstInvSchCode = DataLogic.DeleteAstHistoryAgainstInvSchCode(invSchReqParam, SP_DeleteAstHistoryAgainstInvSchCode);    //Delete Ast_History against InvSchCode to insert updated selected Location data

                    string locIDs = "", deviceHardwareIDs = "";
                    if (invSchReqParam.locTrees.Count > 0)
                    {
                        locIDs = string.Join("|", invSchReqParam.locTrees.Select(tree => tree.LocID.ToString()));
                    }
                    if (invSchReqParam.deviceTrees.Count > 0)
                    {
                        deviceHardwareIDs = string.Join("|", invSchReqParam.deviceTrees.Select(tree => tree.DeviceHardwareID.ToString()));
                    }

                    var locIDArray = locIDs.Split('|');
                    var formattedLocIDs = string.Join(", ", locIDArray.Select(id => $"'{id}'"));

                    DataTable dt = DataLogic.UpdateInvSch(invSchReqParam, locIDs, deviceHardwareIDs, formattedLocIDs, SP_GetInsertUpdateDeleteInvSch);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Inventory Schedule", "Updated", invSchReqParam.LoginName);
                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            if (msgFromDB.Contains("successfully"))
                            {
                                DataTable dt2 = DataLogic.InsertAuditLogs("Inventory Schedule", 1, "Updated", invSchReqParam.LoginName, "dbo.Ast_INV_Schedule");
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
                else
                {
                    msg.message = "Inventory Schedule cannot be updated as it has Inventory Data.";
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

        #region Delete Inventory Scheduler

        /// <summary>
        /// Delete a Inventory Scheduler by passing "Delete = 1" and InvSchCode as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteInvSch")]
        [Authorize]
        public IActionResult DeleteInvSch([FromBody] InvSchReqParam invSchReqParam)
        {
            Message msg = new Message();
            try
            {
                if (Convert.ToInt32(DataLogic.CheckCountOfAstHistory(invSchReqParam, SP_CheckCountForAstHistoryAgainstInvSchCode).Rows[0]["TotalScannedAssets"]) == 0)  //If count = 0 means no data is processed yet
                {

                    DataTable deleteAstHistoryAgainstInvSchCode = DataLogic.DeleteAstHistoryAgainstInvSchCode(invSchReqParam, SP_DeleteAstHistoryAgainstInvSchCode);    //Delete Ast_History against InvSchCode

                    DataTable dt = DataLogic.DeleteInvSch(invSchReqParam, SP_GetInsertUpdateDeleteInvSch);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Inventory Schedule", "Deleted", invSchReqParam.LoginName);
                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            if (msgFromDB.Contains("successfully"))
                            {
                                DataTable dt2 = DataLogic.InsertAuditLogs("Inventory Schedule", 1, "Deleted", invSchReqParam.LoginName, "dbo.Ast_INV_Schedule");
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
                else
                {
                    msg.message = "Inventory Schedule cannot be deleted as it has Inventory Data.";
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

        #region Validate Inventory Scheduler

        /// <summary>
        /// Delete a Inventory Scheduler by passing "Delete = 1" and InvSchCode as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ValidateInvSch")]
        [Authorize]
        public IActionResult ValidateInvSch([FromBody] InvSchReqParam invSchReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.ValidateInvSch(invSchReqParam, SP_ValidateAst_HistoryData);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Inventory Schedule", "Deleted", invSchReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Inventory Schedule", 1, "Insert", invSchReqParam.LoginName, "dbo.Ast_History");
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
