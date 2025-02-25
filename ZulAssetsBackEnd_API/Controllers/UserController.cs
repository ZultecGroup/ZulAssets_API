using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using Microsoft.Extensions.Options;
using ZulAssetsBackEnd_API.Services;
using ZulAssetsBackEnd_API.Settings;
using ZulAssetsBackEnd_API.Model;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace ZulAssetsBackEnd_API.Controllers
{

    /// <summary>
    /// User Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        #region Declarations & Contruction

        public readonly static string SP_UserLogin = "[dbo].[SP_UserLogin]";
        public readonly static string SP_VerifyOldPassword = "[dbo].[SP_NewPasswordwithVerification]";
        public readonly static string UpdateRefreshToken_SP = "[dbo].[UpdateRefreshToken]";
        public readonly static string SP_GetUserByID = "[dbo].[GetUserByID]";
        public readonly static string SP_VerifyUser = "[dbo].[SP_VerifyUser]";
        public readonly static string SP_ForgetPasswordTest = "[dbo].[SP_ForgetPasswordTest]";
        public readonly static string SP_GetPasswordResetTokenDate = "[dbo].[SP_GetPasswordResetTokenDate]";
        public readonly static string SP_GetInsertUpdateDeleteUser = "[dbo].[SP_GetInsertUpdateDeleteUser]";
        public readonly static string SP_CreateAdminUserRole = "[dbo].[SP_CreateAdminUserRole]";
        public readonly static string SP_UpdatePassword = "[dbo].[SP_UpdatePassword]";

        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;

        public UserController(IMailService mailService, IOptions<MailSettings> options)
        {
            _mailService = mailService;
            _mailSettings = options.Value;
        }

        #endregion

        #region User Login

        /// <summary>
        /// Login API
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        /// 

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Loginparam loginParam)
        {
            Message msg = new Message();
            LoginRes loginRes;
            try
            {
                //if (loginParam.LoginName == "webuser")
                //{
                    loginParam.Password = EncryptDecryptPassword.EncryptQueryString(loginParam.LoginName + "|" + loginParam.Password);
                //}

                DataTable dt = DataLogic.LoginDetails(loginParam, SP_UserLogin);

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
                        if (loginParam.LoginName == "WebUser")
                        {
                            var loginNamefromDB = EncryptDecryptPassword.DecryptQueryString(dt.Rows[0]["Password"].ToString()).Split("|");
                        }
                        
                        var loginName = dt.Rows[0]["LoginName"].ToString();
                        var userName = dt.Rows[0]["UserName"].ToString();
                        var roleID = dt.Rows[0]["RoleID"].ToString();
                        var status = dt.Rows[0]["Status"].ToString();

                        #region Return Parameters

                        if ( status == "200")
                        //if (loginNamefromDB[0] == loginParam.LoginName && status == "200")
                        {

                            #region JWT Authentication

                            var tokenString = JWTBuilder.Generation(loginName, userName, roleID, status);

                            var refreshToken = JWTBuilder.GenerateRefreshToken();
                            DataTable dt2 = DataLogic.UpdateRefreshToken(loginParam.LoginName, refreshToken, UpdateRefreshToken_SP);

                            #endregion


                            loginRes = new LoginRes();

                            loginRes.UserName = userName;
                            loginRes.LoginName = loginName;
                            loginRes.RoleID = roleID;
                            loginRes.Status = status;
                            loginRes.Message = "User authenticated";
                            loginRes.RefreshToken = dt.Rows[0]["refreshToken"].ToString();
                            loginRes.RefreshTokenNew = refreshToken;
                            loginRes.Token = tokenString.Item1.ToString();
                            loginRes.ValidTill = tokenString.Item2.ToString();

                            DataTable auditDT = DataLogic.InsertAuditLogs("Login", 1, "Authenticated", loginParam.LoginName, "dbo.AppUsers");

                            return Ok(loginRes);
                        }
                        else if( status == "401")
                        {
                            loginRes = new LoginRes();

                            loginRes.UserName = "";
                            loginRes.LoginName = "";
                            loginRes.RoleID = "";
                            loginRes.Status = status;
                            loginRes.Message = "User not authenticated";
                            loginRes.RefreshToken = "";
                            loginRes.RefreshTokenNew = "";
                            loginRes.Token = "";
                            loginRes.ValidTill = "";
                            DataTable auditDT = DataLogic.InsertAuditLogs("Login", 1, "Not Authenticated", loginParam.LoginName, "dbo.AppUsers");

                            return Ok(loginRes);
                        }
                        else
                        {
                            loginRes = new LoginRes();

                            loginRes.UserName = "";
                            loginRes.LoginName = "";
                            loginRes.RoleID = "";
                            loginRes.Status = status;
                            loginRes.Message = "User not found";
                            loginRes.RefreshToken = "";
                            loginRes.RefreshTokenNew = "";
                            loginRes.Token = "";
                            loginRes.ValidTill = "";

                            DataTable auditDT = DataLogic.InsertAuditLogs("Login", 1, "Not Found", loginParam.LoginName, "dbo.AppUsers");

                            return Ok(loginRes);
                        }

                        #endregion

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

        #region RefreshToken
        /// <summary>
        /// Refreshing Token when Last token is valid
        /// </summary>
        /// <param name="refreshTokenRequest"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                LoginRes loginRes = new LoginRes();
                var principal = JWTBuilder.GetClaimsFromExpiredToken(refreshTokenRequest.JWTToken);
                var loginName_ = principal?.Claims?.SingleOrDefault(p => p.Type == UserClaimParameters.LOGINNAME.ToString())?.Value;
                DataTable dt = DataLogic.GetUserByID(loginName_.ToString(), SP_GetUserByID);
                var status = dt.Rows[0]["Status"].ToString();
                if (dt.Columns.Contains("ErrorMessage"))
                {
                    return Ok(dt);
                }
                else
                {

                    #region Return Params

                    if (status == "200")
                    {

                        #region Refreshing Token

                        var refreshToken = dt.Rows[0]["RefreshToken"].ToString();
                        if (refreshToken != refreshTokenRequest.RefreshToken)
                            throw new SecurityTokenException("Invalid refresh token");
                        var loginName = dt.Rows[0]["LoginName"].ToString();
                        var userName = dt.Rows[0]["UserName"].ToString();
                        var roleID = dt.Rows[0]["RoleID"].ToString();
                        status = dt.Rows[0]["Status"].ToString();
                        var newJwtToken = JWTBuilder.Generation(loginName, userName, roleID, status);
                        var newRefreshToken = JWTBuilder.GenerateRefreshToken();

                        #endregion

                        DataTable dt2 = DataLogic.UpdateRefreshToken(loginName, newRefreshToken, UpdateRefreshToken_SP);
                        DataSet ds2 = DataLogic.GetDetails(loginName, SP_GetUserByID);


                        loginRes.UserName = userName;
                        loginRes.LoginName = loginName;
                        loginRes.RoleID = roleID;
                        loginRes.Status = status;
                        loginRes.Message = "User authenticated";
                        loginRes.RefreshToken = dt.Rows[0]["refreshToken"].ToString();
                        loginRes.RefreshTokenNew = newRefreshToken;
                        loginRes.Token = newJwtToken.Item1.ToString();
                        loginRes.ValidTill = newJwtToken.Item2.ToString();


                        return Ok(loginRes);
                    }
                    else
                    {
                        loginRes.UserName = "";
                        loginRes.LoginName = "";
                        loginRes.RoleID = "";
                        loginRes.Status = status;
                        loginRes.Message = "User not found";
                        loginRes.RefreshToken = "";
                        loginRes.RefreshTokenNew = "";
                        loginRes.Token = "";
                        loginRes.ValidTill = "";

                        return Ok(loginRes);
                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion

        #region Change Password

        /// <summary>
        /// This API will verify the old password first and if it is correct it will update it with New Password.
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns>Returns Success Message of Change Password</returns>
        [HttpPost("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            Message msg = new Message();
            ChangePasswordRes changePassRes = new ChangePasswordRes();
            try
            {
                changePassword.OldPassword = EncryptDecryptPassword.EncryptQueryString(changePassword.LoginName + "|" + changePassword.OldPassword);
                changePassword.NewPassword = EncryptDecryptPassword.EncryptQueryString(changePassword.LoginName + "|" + changePassword.NewPassword);
                DataTable dt = DataLogic.VerifyOldPassword(changePassword, SP_VerifyOldPassword);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("User", "Updated", userReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("User", 1, "Updated", changePassword.TransactionUser, "dbo.AppUsers");
                        }
                        changePassRes.Message = dt.Rows[0]["Message"].ToString();
                        changePassRes.Status = dt.Rows[0]["Status"].ToString();
                        return Ok(changePassRes);
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

        #region Forget Password
        /// <summary>
        /// Reset Password API
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns>Will check if user exists? if yes then will send an email to admin regarding password change</returns>
        [HttpPost("ForgetPassword")]
        [Consumes("application/json")]
        public IActionResult ForgetPassword([FromBody] ForgotPassword forgotPassword)
        {
            Message msg = new Message();
            ForgotPasswordRes forPassRes = new ForgotPasswordRes();
            try
            {
                //Verify that User is exist or not
                DataTable dt = DataLogic.ForgotPass(forgotPassword, SP_VerifyUser);
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
                        if (dt.Rows[0]["Message"].ToString().Contains("not"))
                        {
                            forPassRes.Message = dt.Rows[0]["Message"].ToString();
                            forPassRes.Status = dt.Rows[0]["Status"].ToString();    
                            return Ok(forPassRes);
                        }
                        else
                        {
                            DataTable GetPasswordResetTokenDateDT = DataLogic.GetPasswordResetTokenDate(forgotPassword, SP_GetPasswordResetTokenDate);
                            DateTime d1 = Convert.ToDateTime(GetPasswordResetTokenDateDT.Rows[0]["PasswordResetTokenDate"]);
                            DateTime d2 = DateTime.Now;

                            var diffOfDates = (d2 - d1);
                            int diffOfDatesDays = diffOfDates.Days;

                            if (diffOfDatesDays > 1)
                            {
                                msg.message = "Token Expire hogaya hai";
                                msg.status = "401";
                                return Ok(msg);
                            }
                            else if (diffOfDatesDays < 0)
                            {
                                msg.message = "Update your system Date Time!";
                                msg.status = "401";
                                return Ok(msg);
                            }
                            else
                            {
                                forgotPassword.NewPassword = EncryptDecryptPassword.EncryptQueryString(forgotPassword.LoginName + "|" + forgotPassword.NewPassword);
                                DataTable dt3 = DataLogic.UpdateUserPassword(forgotPassword, SP_UpdatePassword);
                                //msg.status = "200";
                                msg.message = dt3.Rows[0]["Message"].ToString();
                                msg.status = dt3.Rows[0]["Status"].ToString();

                                return Ok(msg);
                            }                            
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

        #region Generate Password Token

        /// <summary>
        /// This API is used to Generate Password Reset Token, an email will be sent with a HEX String which will validate the Password change
        /// </summary>
        /// <param name="generatePasswordTokenParam"></param>
        /// <returns>It will sent an email which contains a random hex string also update the Password Reset Token Date</returns>
        /// 

        [HttpPost("GeneratePasswordToken")]
        [AllowAnonymous]
        public IActionResult GeneratePasswordToken([FromBody] GeneratePasswordTokenParam generatePasswordTokenParam)
        {
            Message msg = new Message();
            MailRequest mailRequest = new MailRequest();
            try
            {
                DataTable dt = DataLogic.GeneratePasswordResetToken(generatePasswordTokenParam, SP_ForgetPasswordTest);
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
                        mailRequest.PasswordResetToken = dt.Rows[0]["PasswordResetToken"].ToString();
                        mailRequest.EmailTo = dt.Rows[0]["EmailAddress"].ToString();
                        mailRequest.AppliedUserName = generatePasswordTokenParam.LoginName;
                        var _1 = new EmailController(_mailService).GeneratePasswordResetTokenEmail(mailRequest);
                        msg.message = "Password Reset Token Updated and Email also sent to you!";
                        msg.status = "200";
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

        #region Get All Users
        /// <summary>
        /// Get all users by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Users</returns>
        [HttpPost("GetAllUsers")]
        [Authorize]
        public IActionResult GetAllUsers([FromBody] User userReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllUsers(userReq, SP_GetInsertUpdateDeleteUser);
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

        #region Insert User

        /// <summary>
        /// Insert a User by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertUser")]
        [Authorize]
        public IActionResult InsertUser([FromBody] User userReq)
        {
            Message msg = new Message();
            try
            {
                userReq.Password = EncryptDecryptPassword.EncryptQueryString(userReq.LoginName + "|" + userReq.Password);
                DataTable dt = DataLogic.InsertUser(userReq, SP_GetInsertUpdateDeleteUser);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("User", "Added", userReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("User", 1, "Inserted", userReq.TransactionUser, "dbo.AppUsers");
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

        #region Update User

        /// <summary>
        /// Update a user by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateUser")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] User userReq)
        {
            Message msg = new Message();
            try
            {
                userReq.Password = EncryptDecryptPassword.EncryptQueryString(userReq.LoginName + "|" + userReq.Password);
                DataTable dt = DataLogic.UpdateUser(userReq, SP_GetInsertUpdateDeleteUser);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("User", "Updated", userReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("User", 1, "Updated", userReq.TransactionUser, "dbo.AppUsers");
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

        #region Delete User

        /// <summary>
        /// Delete a user by passing "Delete = 1" and LoginName as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteUser")]
        [Authorize]
        public IActionResult DeleteUser([FromBody] User userReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteUser(userReq, SP_GetInsertUpdateDeleteUser);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("User", "Deleted", userReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("User", 1, "Deleted", userReq.TransactionUser, "dbo.AppUsers");
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

        #region Create Admin User & Role

        /// <summary>
        /// This API will check the Admin User & Role and if it is not available it will create that. No need to pass any parameter.
        /// </summary>
        /// <returns>This will return a message</returns>
        [HttpPost("CheckAdminUserRole")]
        [AllowAnonymous]
        public IActionResult CheckAdminUserRole()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.CheckAdminUserRole(SP_CreateAdminUserRole);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Admin User", 1, "Created", "System", "dbo.AppUsers");
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
