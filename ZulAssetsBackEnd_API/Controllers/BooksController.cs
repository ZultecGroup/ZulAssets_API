using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("2")]
    //[ApiExplorerSettings(GroupName = "v2")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Books")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        #region Declaration & Constructor

        private static string SP_GetInsertUpdateDeleteBooks = "[dbo].[SP_GetInsertUpdateDeleteBooks]";
        private static string SP_GetBooksAgainstCompanyID = "[dbo].[SP_GetBooksAgainstCompanyID]";

        public BooksController()
        {
            
        }

        #endregion

        #region Get All Books
        /// <summary>
        /// Get all Books by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Books</returns>
        [HttpPost("GetAllBooks")]
        //[Authorize]
        public IActionResult GetAllBooks([FromBody] BooksReqParam booksReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllBooks(booksReqParam, SP_GetInsertUpdateDeleteBooks);

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

        #region Add Book

        /// <summary>
        /// Insert a Book by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertBook")]
        [Authorize]
        public IActionResult InsertBook([FromBody] BooksReqParam booksReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertBook(booksReqParam, SP_GetInsertUpdateDeleteBooks);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Books", "Added", booksReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Books", "Added", booksReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Books", 1, "Inserted", booksReqParam.LoginName, "dbo.Books");
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

        #region Update Book

        /// <summary>
        /// Update a Book by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateBook")]
        [Authorize]
        public IActionResult UpdateBook([FromBody] BooksReqParam booksReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateBook(booksReqParam, SP_GetInsertUpdateDeleteBooks);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Books", 1, "Updated", booksReqParam.LoginName, "dbo.Books");
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

        #region Delete Books

        /// <summary>
        /// Delete a Books by passing "Delete = 1" and BookID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteBook")]
        [Authorize]
        public IActionResult DeleteBook([FromBody] BooksReqParam booksReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteBook(booksReqParam, SP_GetInsertUpdateDeleteBooks);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Books", 1, "Deleted", booksReqParam.LoginName, "dbo.Books");
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

        #region Get Book against CompanyID
        /// <summary>
        /// Get Book by passing a parameter "CompanyID and others as empty"
        /// </summary>
        /// <returns>This will return all the Books</returns>
        [HttpPost("GetBookAgainstCompanyID")]
        [Authorize]
        public IActionResult GetBookAgainstCompanyID(string CompanyID)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetBookAgainstCompanyID(CompanyID, SP_GetBooksAgainstCompanyID);
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
