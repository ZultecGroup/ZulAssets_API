using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        #region Declaration

        private static string SP_GetCompLevels = "[dbo].[SP_GetInsertUpdateDeleteCompanyLevel]";
        private static string SP_InsertCompLevel = "[dbo].[SP_GetInsertUpdateDeleteCompanyLevel]";
        private static string SP_UpdateCompLevel = "[dbo].[SP_GetInsertUpdateDeleteCompanyLevel]";
        private static string SP_DeleteCompLevel = "[dbo].[SP_GetInsertUpdateDeleteCompanyLevel]";

        #endregion

        #region Get All Company Levels
        /// <summary>
        /// Get all Company Levels by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Company Levels</returns>
        [HttpPost("GetAllLevels")]
        [Authorize]
        public IActionResult GetAllLevels([FromBody] LevelsParams lvlParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllLvls(lvlParams, SP_GetCompLevels);
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
                msg.status = "500";
                return Ok(msg);
            }
        }

        #endregion

        #region Add Company Level

        /// <summary>
        /// Insert an Company Level by passing all parameters with "Add = 1" and without LevelID parameter
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertLevel")]
        [Authorize]
        public IActionResult InsertLevel([FromBody] LevelsParams lvlParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertLvl(lvlParams, SP_InsertCompLevel);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Company Level", 1, "Insert", lvlParams.LoginName, "dbo.CompLvl");
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
                msg.status = "500";
                return Ok(msg);
            }
        }

        #endregion

        #region Update Company Level

        /// <summary>
        /// Update an Company Level by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateLevel")]
        [Authorize]
        public IActionResult UpdateLevel([FromBody] LevelsParams lvlParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateLvl(lvlParams, SP_UpdateCompLevel);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Company Level", 1, "Update", lvlParams.LoginName, "dbo.CompLvl");
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
                msg.status = "500";
                return Ok(msg);
            }
        }

        #endregion

        #region Delete Company Level

        /// <summary>
        /// Delete an Company Level by passing "Delete = 1" and LvlID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteLevel")]
        [Authorize]
        public IActionResult DeleteLevel([FromBody] LevelsParams lvlParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteLvl(lvlParams, SP_DeleteCompLevel);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Compayn Level", 1, "Delete", lvlParams.LoginName, "dbo.CompLvl");
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
                msg.status = "500";
                return Ok(msg);
            }
        }

        #endregion

    }
}
