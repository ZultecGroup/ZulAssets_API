using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using ZulAssetsBackEnd_API.BAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;

namespace ZulAssetsBackEnd_API.DAL
{

    public class DataLogic
    {

        #region Declaration

        public static string Message = "ControllerName were Operation by UserName";

        #endregion

        #region Get Total Rows Count

        public static DataTable GetTotalRowsCount(string TableName, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@TableName", TableName)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Check RefID Logic

        public static DataTable CheckRefID(string RefID, string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@RefID", RefID),
                new SqlParameter ("@AstID", AstID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Dashboard Logics

        public static DataTable GetAllDashboardCounts(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Insert Audit Logs

        public static DataTable InsertAuditLogs(string controllerName, int add, string operationType, string loginName, string tableName)
        {
            string StoreProcedure = "[dbo].[SP_GetInsertAuditLogs]";
            string text = Message;
            GeneralFunctions GF = new GeneralFunctions();
            Dictionary<string, string> replacements = new Dictionary<string, string>()
                {
                    { "ControllerName", controllerName },
                    { "Operation", operationType },
                    { "UserName", loginName }
                };
            string operationMsg = GF.ReplaceMultipleWordsDB(text, replacements);

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", add),
                new SqlParameter ("@OperationType", operationType),
                new SqlParameter ("@LoginName", loginName),
                new SqlParameter ("@OperationMsg", operationMsg),
                new SqlParameter ("@EffectedTable", tableName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Device Logics

        #region DeviceRegistration

        public static DataTable InitializeDevice(DeviceReg deviceReg, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@NewDeviceFlag",deviceReg.NewDeviceFlag),
                    new SqlParameter("@HardwareID", deviceReg.DeviceSerialNo),
                    new SqlParameter("@DeviceDesc", deviceReg.DeviceDesc)
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
            }
            catch (Exception)
            {

                return new DataTable();
            }
        }

        #endregion

        #region VerifyDeviceLicKey

        public static DataTable VerifyDeviceLicKey(string HardwareID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@HardwareID", HardwareID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get All Devices

        public static DataSet GetAllDevices(DeviceReg deviceReg, string Storeprocedure)
        {
            try
            {
                if (deviceReg.Searching != 0)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] parameter =
                    {
                        new SqlParameter("@Searching",deviceReg.Searching),
                        new SqlParameter("@Var", deviceReg.Var),
                        new SqlParameter("@PageIndex", deviceReg.PaginationParam.PageIndex),
                        new SqlParameter("@PageSize", deviceReg.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(Storeprocedure, parameter, 1);
                    //return CGD.DSWithParam(Storeprocedure, parameter, 2);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] parameter =
                    {
                    new SqlParameter("@Get",deviceReg.Get),
                    new SqlParameter("@PageIndex", deviceReg.PaginationParam.PageIndex),
                    new SqlParameter("@PageSize", deviceReg.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(Storeprocedure, parameter, 1);
                    //return CGD.DSWithParam(Storeprocedure, parameter, 2);
                }
            }
            catch (Exception)
            {

                return new DataSet();
            }
        }

        #endregion

        #region Update Device

        public static DataTable UpdateDevice(DeviceReg deviceReg, string Storeprocedure)
        {
            try
            {

                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@Update",deviceReg.Update),
                    new SqlParameter("@DeviceID",deviceReg.DeviceID),
                    new SqlParameter("@DeviceDesc", deviceReg.DeviceDesc),
                    new SqlParameter("@LicKey", deviceReg.LicKey),
                    new SqlParameter("@DevicePhNo", deviceReg.DevicePhNo),
                    new SqlParameter("@DeviceSerialNo", deviceReg.DeviceSrNo),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
                //return CGD.DTWithParam(Storeprocedure, parameter, 2);
            }
            catch (Exception)
            {

                return new DataTable();
            }
        }

        #endregion

        #region Delete Device

        public static DataTable DeleteDevice(DeviceReg deviceReg, string Storeprocedure)
        {
            try
            {

                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                new SqlParameter("@Delete",deviceReg.Delete),
                new SqlParameter("@DeviceID",deviceReg.DeviceID),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
                //return CGD.DTWithParam(Storeprocedure, parameter, 2);
            }
            catch (Exception)
            {

                return new DataTable();
            }
        }

        #endregion

        #region Get Processing Data

        public static DataSet GetProcessingData(DataProcessingReqParam dataProcReqParam, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@Get",dataProcReqParam.Get),
                    new SqlParameter("@InvSchCode",dataProcReqParam.InvSchCode),
                    new SqlParameter("@AuditData",dataProcReqParam.AuditData),
                    new SqlParameter("@PageIndex",dataProcReqParam.paginationParam.PageIndex),
                    new SqlParameter("@PageSize",dataProcReqParam.paginationParam.PageSize),
                };
                return CGD.DSWithParam(Storeprocedure, parameter, 1);
                //return CGD.DSWithParamSecondDB(Storeprocedure, parameter, 2);
            }
            catch (Exception)
            {
                return new DataSet();
            }
        }

        #endregion

        #region Delete Processing Data

        public static DataTable DeleteProcessingData(DeleteProcessingReqParam deleteProcReqParams, DataTable dt, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                if (deleteProcReqParams.AuditData == 1)
                {
                    SqlParameter[] parameter =
                    {
                        new SqlParameter("@AuditData",deleteProcReqParams.AuditData),
                        new SqlParameter("@Delete",deleteProcReqParams.Delete),
                        new SqlParameter("@auditProcessingData",dt),
                    };
                    return CGD.DTWithParam(Storeprocedure, parameter, 1);
                    //return CGD.DTWithParamSecondDB(Storeprocedure, parameter, 2);
                }
                else
                {
                    SqlParameter[] parameter =
                    {
                        new SqlParameter("@AuditData",deleteProcReqParams.AuditData),
                        new SqlParameter("@Delete",deleteProcReqParams.Delete),
                        new SqlParameter("@anonymousProcessingData",dt),
                    };
                    return CGD.DTWithParam(Storeprocedure, parameter, 1);
                    //return CGD.DTWithParamSecondDB(Storeprocedure, parameter, 2);
                }
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        #endregion

        #region Get All Asset Transfer Temp

        public static DataTable GetAll_AssetTransferTemp(string AstID, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@AstID",AstID),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
                //return CGD.DTWithParamSecondDB(Storeprocedure, parameter, 2);

            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        #endregion

        #region Checking Child in BE Database of AstID

        public static DataTable CheckChild_AstHistory(string AstID, long invSchCode, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@AstID",AstID),
                    new SqlParameter("@InvSchCode",invSchCode),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);

            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        #endregion

        #region Delete Asset from Assets Temp Receiving

        public static DataTable DeleteFromTempDB(string AstID, string DeviceID, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@AstID",AstID),
                    new SqlParameter("@DeviceID",DeviceID),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
                //return CGD.DTWithParamSecondDB(Storeprocedure, parameter, 2);

            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        #endregion

        #region Update Asset Details for Inventory

        public static DataTable UpdateAssetDetailsForInventory(string LocID, string AstID, int status, string AssetStatus, string AstDesc, long InvSchCode, DateTime LastInventoryDate, string lastEditBy, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@LocID",LocID),
                    new SqlParameter("@AstID",AstID),
                    new SqlParameter("@status",status),
                    new SqlParameter("@AssetStatus",AssetStatus),
                    new SqlParameter("@AstDesc",AstDesc),
                    new SqlParameter("@InvSchCode",InvSchCode),
                    new SqlParameter("@LastInventoryDate",LastInventoryDate),
                    new SqlParameter("@LastEditBy",lastEditBy),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);

            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        #endregion

        #endregion

        #region Login & Password Related Logics

        #region LoginDetails

        public static DataTable LoginDetails(Loginparam loginParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", loginParam.LoginName),
                new SqlParameter ("@Pass", loginParam.Password)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region GeneratePasswordResetToken

        public static DataTable GeneratePasswordResetToken(GeneratePasswordTokenParam gptP, string StoreProcedure)
        {
            DbReports CGD = new DbReports();

            var passwordResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            var datetime = DateTime.Now;

            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@loginName", gptP.LoginName),
                new SqlParameter ("@PassowrdResetTokenDate", datetime),
                new SqlParameter ("@PasswordResetToken", passwordResetToken)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region ForGot Password

        public static DataTable ForgotPass(ForgotPassword forgotPassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", forgotPassword.LoginName)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable UpdateUserPassword(ForgotPassword forgotPassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", forgotPassword.LoginName),
                new SqlParameter ("@Password", forgotPassword.NewPassword)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Password Reset Token Date

        public static DataTable GetPasswordResetTokenDate(ForgotPassword forgotPassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", forgotPassword.LoginName)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region UpdateRefreshToken

        public static DataTable UpdateRefreshToken(string loginName, string refreshToken, string StoredProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@LoginName",loginName),
                    new SqlParameter ("@RefreshToken",refreshToken)
                };

                DataTable dt = CGD.DTWithParam(StoredProcedure, sqlParameters, 1);
                return dt;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataTable(errorMessage);
            }
        }

        #endregion

        #region Get User By ID

        public static DataTable GetUserByID(string loginName, string StoreProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();

                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@LoginName",loginName),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            catch (Exception ex)
            {
                return new DataTable(ex.Message);
            }
        }

        #endregion

        #region LoginDetails

        public static DataSet GetDetails(string loginName, string StoredProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@LoginName", loginName)
                };

                DataSet ds = CGD.DSWithParam(StoredProcedure, sqlParameters, 1);
                return ds;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataSet(errorMessage);
            }
        }

        #endregion

        #region VerifyOldPassword

        public static DataTable VerifyOldPassword(ChangePassword changePassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", changePassword.LoginName),
                new SqlParameter ("@OldPassword", changePassword.OldPassword),
                new SqlParameter ("@NewPassword", changePassword.NewPassword)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Assets Logics

        #region Asset Tracking

        public static DataTable AssetTracking(AssetTrackingRequest astTrkReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@Barcode", astTrkReq.Barcode)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Anonymous Assets

        public static DataTable AnonymousAssets(AnonymousAssetsRequests anonymousAstReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@ID", anonymousAstReq.ID),
                new SqlParameter ("@AstDesc", anonymousAstReq.AssetDescription),
                new SqlParameter ("@LocID", anonymousAstReq.LocID),
                new SqlParameter ("@AstCatID", anonymousAstReq.CatID),
                new SqlParameter ("@DeviceID", anonymousAstReq.DeviceID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable GetAllAnonymousAssets(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get All Assets

        public static DataTable GetAllAssets(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get Assets Details

        #region Get All Assets Details
        public static DataTable GetAllAssetsDetails(int CompanyID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@CompanyID", CompanyID),
            };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get Asset Details Against AstID

        public static DataTable GetAllAssetsDetailsAgainstAstID(string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@AstID", AstID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Asset Book Details Against AstID

        public static DataTable GetAstBookDetailsAgainstAstID(string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@AstID", AstID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Get All Assets Status

        public static DataTable GetAllAssetsStatus(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        public static DataSet GetAllAssetsStatusDataSet(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DSWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Update Asset Status By Barcode

        public static DataTable UpdateAssetStatusByBarcode(UpdateAssetStatus updAstStatus, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                 new SqlParameter ("@Barcode", updAstStatus.Barcode),
                 new SqlParameter ("@AssetStatus", updAstStatus.AssetStatus)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Asset Location

        public static DataTable UpdateAssetLocation(UpdateAssetLocation updAstLoc, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@Barcode", updAstLoc.Barcode),
                new SqlParameter ("@LocID", updAstLoc.LocID),
                new SqlParameter ("@DeviceID", updAstLoc.DeviceID),
                new SqlParameter ("@InventoryDate", updAstLoc.InventoryDate),
                new SqlParameter ("@LastEditDate", updAstLoc.LastEditDate),
                new SqlParameter ("@LastEditBy", updAstLoc.LastEditBy),
                new SqlParameter ("@Status", updAstLoc.Status),
                new SqlParameter ("@AssetStatus", updAstLoc.AssetStatus)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Transfer Assets From BE To Temp

        public static DataSet TransferAssetsFromBEToTemp(string deviceID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@DeviceID", deviceID),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Asset Coding Definition Logics

        #region Verify Asset Coding Def Range

        public static DataTable VerifyAssetCodingRange(int startSerial, int endSerial, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@startSerial", startSerial.ToString()),
                new SqlParameter ("@endSerial", endSerial.ToString()),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);


        }

        #endregion

        #region Verift Serial Exists for the Company

        public static DataTable VerifyCompanyExists(int companyID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", companyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);


        }

        #endregion

        #region Check Related Data

        public static DataTable CheckRelatedDataDT(int startSerial, int endSerial, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@startSerial", startSerial.ToString()),
                new SqlParameter ("@endSerial", endSerial.ToString()),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);


        }

        #endregion

        #region Get All Asset Coding Definitions

        public static DataSet GetAllAstCodingDef(AstCodingDefReqParam astCodingReq, string StoreProcedure)
        {
            if (astCodingReq.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", astCodingReq.Searching),
                new SqlParameter ("@Var", astCodingReq.Var),
                new SqlParameter ("@PageIndex", astCodingReq.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", astCodingReq.PaginationParam.PageSize),
            };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", astCodingReq.Get),
                    new SqlParameter ("@PageIndex", astCodingReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", astCodingReq.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        public static DataTable GetAstCodingDefAgainstCompanyID(string CompanyID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", CompanyID),
                new SqlParameter ("@GetByID", 1),
                new SqlParameter ("@Status", 1),
                //new SqlParameter ("@PageSize", astCodingReq.PaginationParam.PageSize),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);

        }

        public static DataTable CloseAstCodingRange(string CompanyID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", CompanyID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Insert Asset Coding Definition

        public static DataTable InsertAstCodingDef(AstCodingDefReqParam astCodingDefReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", astCodingDefReqParam.Add),
                new SqlParameter ("@CompanyID", astCodingDefReqParam.CompanyID),
                new SqlParameter ("@StartSerial", astCodingDefReqParam.StartSerial),
                new SqlParameter ("@EndSerial", astCodingDefReqParam.EndSerial),
                new SqlParameter ("@Status", astCodingDefReqParam.Status),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Asset Coding Definitions

        public static DataTable UpdateAstCodingDef(AstCodingDefReqParam astCodingDefReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", astCodingDefReqParam.Update),
                new SqlParameter ("@AstCodingID", astCodingDefReqParam.AssetCodingID),
                new SqlParameter ("@CompanyID", astCodingDefReqParam.CompanyID),
                new SqlParameter ("@StartSerial", astCodingDefReqParam.StartSerial),
                new SqlParameter ("@EndSerial", astCodingDefReqParam.EndSerial),
                new SqlParameter ("@Status", astCodingDefReqParam.Status),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Asset Coding Definitions

        public static DataTable DeleteAstCodingDef(AstCodingDefReqParam astCodingDefReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", astCodingDefReqParam.Delete),
                new SqlParameter ("@AstCodingID", astCodingDefReqParam.AssetCodingID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Asset Item

        #region Get Asset Items Against AstCatID

        public static DataSet GetAssetItemsAgainstCatID(AssetItemReq astItemReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstCatID", astItemReq.AstCatID),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Get All Asset Item

        public static DataSet GetAllAssetItems(AssetItemReq astItemReq, string StoreProcedure)
        {
            if (astItemReq.Searching != 0)
            {
                if (astItemReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", astItemReq.Searching),
                        new SqlParameter ("@DropDown", astItemReq.DropDown),
                        new SqlParameter ("@Var", astItemReq.Var),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", astItemReq.Searching),
                        new SqlParameter ("@Var", astItemReq.Var),
                        new SqlParameter ("@PageIndex", astItemReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", astItemReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (astItemReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", astItemReq.Get),
                    new SqlParameter ("@DropDown", astItemReq.DropDown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", astItemReq.Get),
                    new SqlParameter ("@PageIndex", astItemReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", astItemReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Asset Item

        public static DataTable InsertAssetItem(AssetItemReq astItemReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", astItemReq.Add),
                new SqlParameter ("@AstCatID", astItemReq.AstCatID),
                new SqlParameter ("@AstDesc", astItemReq.AstDesc),
                new SqlParameter ("@Warranty", astItemReq.Warranty),
                new SqlParameter ("@ImageBase64", astItemReq.ImageBase64),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Asset Item

        public static DataTable UpdateAssetItem(AssetItemReq astItemReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", astItemReq.Update),
                new SqlParameter ("@ItemCode", astItemReq.ItemCode),
                new SqlParameter ("@AstCatID", astItemReq.AstCatID),
                new SqlParameter ("@AstDesc", astItemReq.AstDesc),
                new SqlParameter ("@Warranty", astItemReq.Warranty),
                new SqlParameter ("@ImageBase64", astItemReq.ImageBase64),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Barcode Structure ID against Asset Item Code

        public static DataTable UpdateBarStructIDAgainstItemCode(int BarStructID, string ItemCode, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@BarStructID", BarStructID),
                new SqlParameter ("@ItemCode", ItemCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Asset Item

        public static DataTable DeleteAssetItem(AssetItemReq astItemReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", astItemReq.Delete),
                new SqlParameter ("@ItemCode", astItemReq.ItemCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Asset Administrator

        public static DataSet GetAllAssetsAdministrator(AssetAdministrator astAdmin, string StoreProcedure)
        {
            if (astAdmin.Searching != 0)
            {
                if (astAdmin.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", astAdmin.Searching),
                        new SqlParameter ("@DropDown", astAdmin.DropDown),
                        new SqlParameter ("@Var", astAdmin.Var),
                        new SqlParameter ("@LoginName", astAdmin.LoginName),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    if (astAdmin.LocID != "" && astAdmin.AstCatID == "" && astAdmin.CustodianID == "")
                    {
                        DbReports CGD = new DbReports();
                        SqlParameter[] sqlParameters = {
                            new SqlParameter ("@Searching", astAdmin.Searching),
                            new SqlParameter ("@Var", astAdmin.Var),
                            new SqlParameter ("@LocID", "'" + astAdmin.LocID + "'"),
                            new SqlParameter ("@AstCatID", "''"),
                            new SqlParameter ("@CustodianID", "''"),
                            new SqlParameter ("@PageIndex", astAdmin.PaginationParam.PageIndex),
                            new SqlParameter ("@PageSize", astAdmin.PaginationParam.PageSize),
                            new SqlParameter ("@LoginName", astAdmin.LoginName),
                        };
                        return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                    }
                    else if (astAdmin.LocID == "" && astAdmin.AstCatID != "" && astAdmin.CustodianID == "")
                    {
                        DbReports CGD = new DbReports();
                        SqlParameter[] sqlParameters = {
                            new SqlParameter ("@Searching", astAdmin.Searching),
                            new SqlParameter ("@Var", astAdmin.Var),
                            new SqlParameter ("@LocID", "''"),
                            new SqlParameter ("@AstCatID", "'" + astAdmin.AstCatID + "'"),
                            new SqlParameter ("@CustodianID", "''"),
                            new SqlParameter ("@PageIndex", astAdmin.PaginationParam.PageIndex),
                            new SqlParameter ("@PageSize", astAdmin.PaginationParam.PageSize),
                            new SqlParameter ("@LoginName", astAdmin.LoginName),
                        };
                        return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                    }
                    else if (astAdmin.LocID == "" && astAdmin.AstCatID == "" && astAdmin.CustodianID != "")
                    {
                        DbReports CGD = new DbReports();
                        SqlParameter[] sqlParameters = {
                            new SqlParameter ("@Searching", astAdmin.Searching),
                            new SqlParameter ("@Var", astAdmin.Var),
                            new SqlParameter ("@LocID", "''"),
                            new SqlParameter ("@AstCatID", "''"),
                            new SqlParameter ("@CustodianID", astAdmin.CustodianID),
                            new SqlParameter ("@PageIndex", astAdmin.PaginationParam.PageIndex),
                            new SqlParameter ("@PageSize", astAdmin.PaginationParam.PageSize),
                            new SqlParameter ("@LoginName", astAdmin.LoginName),
                        };
                        return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                    }
                    else
                    {
                        DbReports CGD = new DbReports();
                        SqlParameter[] sqlParameters = {
                            new SqlParameter ("@Searching", astAdmin.Searching),
                            new SqlParameter ("@Var", astAdmin.Var),
                            new SqlParameter ("@LocID", "''"),
                            new SqlParameter ("@AstCatID", "''"),
                            new SqlParameter ("@CustodianID", "''"),
                            new SqlParameter ("@PageIndex", astAdmin.PaginationParam.PageIndex),
                            new SqlParameter ("@PageSize", astAdmin.PaginationParam.PageSize),
                            new SqlParameter ("@LoginName", astAdmin.LoginName),
                        };
                        return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                    }
                }
            }
            else
            {
                if (astAdmin.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter("@Get", astAdmin.Get),
                        new SqlParameter("@DropDown", astAdmin.DropDown),
                        new SqlParameter ("@LoginName", astAdmin.LoginName),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", astAdmin.Get),
                        new SqlParameter ("@LocID",astAdmin.LocID),
                        new SqlParameter ("@AstCatID", astAdmin.AstCatID),
                        new SqlParameter ("@CustodianID", astAdmin.CustodianID),
                        new SqlParameter ("@PageIndex", astAdmin.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", astAdmin.PaginationParam.PageSize),
                        new SqlParameter ("@LoginName", astAdmin.LoginName),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

                }

            }

        }

        #endregion

        #region Details & Maintenance

        #region Get Asset Information

        public static DataSet GetAstInfoByAstID(string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstID", AstID)
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #region Get Asset Information Against Item Code

        public static DataSet GetAstInfoByItemCode(string ItemCode, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@ItemCode", ItemCode)
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Insert Asset Details

        public static DataTable InsertAssetDetails(AstInformationReqParam astInfoReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", astInfoReqParam.Add),
                new SqlParameter ("@AstID", astInfoReqParam.AstID),
                new SqlParameter ("@DispCode", astInfoReqParam.DispCode),
                new SqlParameter ("@ItemCode", astInfoReqParam.ItemCode),
                new SqlParameter ("@SuppID", astInfoReqParam.SuppID),
                new SqlParameter ("@POCode", astInfoReqParam.POCode),
                new SqlParameter ("@InvNumber", astInfoReqParam.InvNumber),
                new SqlParameter ("@CustodianID", astInfoReqParam.CustodianID),
                new SqlParameter ("@BaseCost", astInfoReqParam.BaseCost),
                new SqlParameter ("@Tax", astInfoReqParam.Tax),
                new SqlParameter ("@SrvDate", astInfoReqParam.SrvDate),
                new SqlParameter ("@PurDate", astInfoReqParam.PurDate),
                new SqlParameter ("@Disposed", astInfoReqParam.Disposed),
                new SqlParameter ("@DispDate", astInfoReqParam.DispDate),
                new SqlParameter ("@InvSchCode", astInfoReqParam.InvSchCode),
                new SqlParameter ("@BookID", astInfoReqParam.BookID),
                new SqlParameter ("@InsID", astInfoReqParam.InsID),
                new SqlParameter ("@LocID", astInfoReqParam.LocID),
                new SqlParameter ("@InvStatus", astInfoReqParam.InvStatus),
                new SqlParameter ("@AstBrandId", astInfoReqParam.AstBrandId),
                new SqlParameter ("@AstDesc", astInfoReqParam.AstDesc),
                new SqlParameter ("@AstModel", astInfoReqParam.AstModel),
                new SqlParameter ("@CompanyID", astInfoReqParam.CompanyID),
                new SqlParameter ("@TransRemarks", astInfoReqParam.TransRemarks),
                new SqlParameter ("@Barcode", astInfoReqParam.BarCode),
                new SqlParameter ("@SerialNo", astInfoReqParam.SerialNo),
                new SqlParameter ("@RefCode", astInfoReqParam.RefCode),
                new SqlParameter ("@Plate", astInfoReqParam.Plate),
                new SqlParameter ("@POERP", astInfoReqParam.POERP),
                new SqlParameter ("@Capex", astInfoReqParam.Capex),
                new SqlParameter ("@Grn", astInfoReqParam.Grn),
                new SqlParameter ("@GLCode", astInfoReqParam.GLCode),
                new SqlParameter ("@PONumber", astInfoReqParam.PONumber),
                new SqlParameter ("@AstDesc2", astInfoReqParam.AstDesc2),
                new SqlParameter ("@BussinessArea", astInfoReqParam.BussinessArea),
                new SqlParameter ("@InventoryNumber", astInfoReqParam.InventoryNumber),
                new SqlParameter ("@CostCenterID", astInfoReqParam.CostCenterID),
                new SqlParameter ("@InStockAsset", astInfoReqParam.InStockAsset),
                new SqlParameter ("@EvaluationGroup1", astInfoReqParam.EvaluationGroup1),
                new SqlParameter ("@EvaluationGroup2", astInfoReqParam.EvaluationGroup2),
                new SqlParameter ("@EvaluationGroup3", astInfoReqParam.EvaluationGroup3),
                new SqlParameter ("@EvaluationGroup4", astInfoReqParam.EvaluationGroup4),
                new SqlParameter ("@CreatedBy", astInfoReqParam.LoginName),
                new SqlParameter ("@CustomFld1", astInfoReqParam.CustomFld1),
                new SqlParameter ("@CustomFld2", astInfoReqParam.CustomFld2),
                new SqlParameter ("@CustomFld3", astInfoReqParam.CustomFld3),
                new SqlParameter ("@CustomFld4", astInfoReqParam.CustomFld4),
                new SqlParameter ("@CustomFld5", astInfoReqParam.CustomFld5),
                new SqlParameter ("@Warranty", astInfoReqParam.Warranty),
                new SqlParameter ("@StatusID", astInfoReqParam.StatusID),
                new SqlParameter ("@OldAssetID", astInfoReqParam.OldAssetID),
                new SqlParameter ("@DisposalComments", astInfoReqParam.DisposalComments),
                new SqlParameter ("@ImageBase64", astInfoReqParam.ImageBase64)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Asset Details

        public static DataTable UpdateAssetDetails(AstInformationReqParam astInfoReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", astInfoReqParam.Update),
                new SqlParameter ("@AstID", astInfoReqParam.AstID),
                new SqlParameter ("@DispCode", astInfoReqParam.DispCode),
                new SqlParameter ("@ItemCode", astInfoReqParam.ItemCode),
                new SqlParameter ("@SuppID", astInfoReqParam.SuppID),
                new SqlParameter ("@POCode", astInfoReqParam.POCode),
                new SqlParameter ("@InvNumber", astInfoReqParam.InvNumber),
                new SqlParameter ("@CustodianID", astInfoReqParam.CustodianID),
                new SqlParameter ("@BaseCost", astInfoReqParam.BaseCost),
                new SqlParameter ("@Tax", astInfoReqParam.Tax),
                new SqlParameter ("@SrvDate", astInfoReqParam.SrvDate),
                new SqlParameter ("@PurDate", astInfoReqParam.PurDate),
                new SqlParameter ("@Disposed", astInfoReqParam.Disposed),
                new SqlParameter ("@DispDate", astInfoReqParam.DispDate),
                new SqlParameter ("@InvSchCode", astInfoReqParam.InvSchCode),
                new SqlParameter ("@BookID", astInfoReqParam.BookID),
                new SqlParameter ("@InsID", astInfoReqParam.InsID),
                new SqlParameter ("@LocID", astInfoReqParam.LocID),
                new SqlParameter ("@InvStatus", astInfoReqParam.InvStatus),
                new SqlParameter ("@AstBrandId", astInfoReqParam.AstBrandId),
                new SqlParameter ("@AstDesc", astInfoReqParam.AstDesc),
                new SqlParameter ("@AstModel", astInfoReqParam.AstModel),
                new SqlParameter ("@CompanyID", astInfoReqParam.CompanyID),
                new SqlParameter ("@TransRemarks", astInfoReqParam.TransRemarks),
                new SqlParameter ("@Barcode", astInfoReqParam.BarCode),
                new SqlParameter ("@SerialNo", astInfoReqParam.SerialNo),
                new SqlParameter ("@RefCode", astInfoReqParam.RefCode),
                new SqlParameter ("@Plate", astInfoReqParam.Plate),
                new SqlParameter ("@POERP", astInfoReqParam.POERP),
                new SqlParameter ("@Capex", astInfoReqParam.Capex),
                new SqlParameter ("@Grn", astInfoReqParam.Grn),
                new SqlParameter ("@GLCode", astInfoReqParam.GLCode),
                new SqlParameter ("@PONumber", astInfoReqParam.PONumber),
                new SqlParameter ("@AstDesc2", astInfoReqParam.AstDesc2),
                new SqlParameter ("@BussinessArea", astInfoReqParam.BussinessArea),
                new SqlParameter ("@InventoryNumber", astInfoReqParam.InventoryNumber),
                new SqlParameter ("@CostCenterID", astInfoReqParam.CostCenterID),
                new SqlParameter ("@InStockAsset", astInfoReqParam.InStockAsset),
                new SqlParameter ("@EvaluationGroup1", astInfoReqParam.EvaluationGroup1),
                new SqlParameter ("@EvaluationGroup2", astInfoReqParam.EvaluationGroup2),
                new SqlParameter ("@EvaluationGroup3", astInfoReqParam.EvaluationGroup3),
                new SqlParameter ("@EvaluationGroup4", astInfoReqParam.EvaluationGroup4),
                new SqlParameter ("@CreatedBy", astInfoReqParam.CreatedBy),
                new SqlParameter ("@CustomFld1", astInfoReqParam.CustomFld1),
                new SqlParameter ("@CustomFld2", astInfoReqParam.CustomFld2),
                new SqlParameter ("@CustomFld3", astInfoReqParam.CustomFld3),
                new SqlParameter ("@CustomFld4", astInfoReqParam.CustomFld4),
                new SqlParameter ("@CustomFld5", astInfoReqParam.CustomFld5),
                new SqlParameter ("@Warranty", astInfoReqParam.Warranty),
                new SqlParameter ("@StatusID", astInfoReqParam.StatusID),
                new SqlParameter ("@DisposalComments", astInfoReqParam.DisposalComments),
                new SqlParameter ("@ImageBase64", astInfoReqParam.ImageBase64)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Asset Details

        public static DataTable DeleteAssetDetails(AstInformationReqParam astInfoReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", astInfoReqParam.Delete),
                new SqlParameter ("@AstID", astInfoReqParam.AstID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Search Assets

        public static DataSet SearchAssets(SearchAstsParams searchAstsParams, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", searchAstsParams.Get),
                new SqlParameter ("@AstID", searchAstsParams.AstID),
                new SqlParameter ("@AstNum", searchAstsParams.AstNum),
                new SqlParameter ("@ItemCode", searchAstsParams.ItemCode),
                new SqlParameter ("@AstDesc", searchAstsParams.AstDesc),
                new SqlParameter ("@SerialNo", searchAstsParams.SerialNo),
                new SqlParameter ("@AstBrandID", searchAstsParams.AstBrandID),
                new SqlParameter ("@OrgHierID", searchAstsParams.OrgHierID),
                new SqlParameter ("@CustID", searchAstsParams.CustID),
                new SqlParameter ("@LocID", searchAstsParams.LocID),
                new SqlParameter ("@AstCatID", searchAstsParams.AstCatID),
                new SqlParameter ("@PageIndex", searchAstsParams.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", searchAstsParams.PaginationParam.PageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }


        #endregion

        #endregion

        #region Locations Logics

        #region Get All Locations

        public static DataTable GetAllLocations(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get All Inventory Location Against Device Serial No

        public static DataTable GetInvLocAgainstDevSerialNo(string devSerialNo, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@DeviceSerialNo", devSerialNo),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Location By ID

        public static DataSet GetAssetsByLocationID(LocationRequest locReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                 new SqlParameter ("@LocID", locReq.LocID),
                 new SqlParameter ("@From", locReq.From),
                 new SqlParameter ("@To", locReq.To)
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get All Locations Tree

        public static DataTable GetAllLocationsTree(LocationTree locTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", locTree.Get),
                    new SqlParameter ("@LoginName", locTree.LoginName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Location

        public static DataTable InsertLocation(LocationTree locTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", locTree.Add),
                new SqlParameter ("@LocCode", locTree.LocCode),
                new SqlParameter ("@LocDesc", locTree.LocDesc),
                new SqlParameter ("@CompanyId", locTree.CompanyId),
                new SqlParameter ("@ParentId", locTree.ParentId),
                new SqlParameter ("@ParentId2", locTree.ParentId2),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Location

        public static DataTable UpdateLocation(LocationTree locTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", locTree.Update),
                new SqlParameter ("@LocCode", locTree.LocCode),
                new SqlParameter ("@LocDesc", locTree.LocDesc),
                new SqlParameter ("@CompanyId", locTree.CompanyId),
                new SqlParameter ("@LocID", locTree.LocId),
                new SqlParameter ("@ParentId", locTree.ParentId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Location

        public static DataTable DeleteLocation(LocationTree locTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", locTree.Delete),
                new SqlParameter ("@LocID", locTree.LocId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check Child Count Against LocID Location

        public static DataTable CheckChildForLocation(LocationTree locTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@LocID", locTree.LocId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check Assets Count Against LocID Location

        public static DataTable CheckAssetCountAgainstLocID(LocationTree locTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@LocID", locTree.LocId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Category Logics

        #region Get All Category

        public static DataTable GetAllCategory(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get All Categories Tree

        public static DataTable GetAllCategoriesTree(CategoryTree catTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", catTree.Get)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Category

        public static DataTable InsertCategory(CategoryTree catTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", catTree.Add),
                new SqlParameter ("@AstCatCode", catTree.CatCode),
                new SqlParameter ("@AstCatDesc", catTree.CatDesc),
                new SqlParameter ("@ParentId", catTree.ParentId),
                new SqlParameter ("@ParentId2", catTree.ParentId2),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Category

        public static DataTable UpdateCategory(CategoryTree catTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", catTree.Update),
                new SqlParameter ("@AstCatCode", catTree.CatCode),
                new SqlParameter ("@AstCatDesc", catTree.CatDesc),
                new SqlParameter ("@ParentId", catTree.ParentId),
                new SqlParameter ("@AstCatId", catTree.CatId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Category

        public static DataTable DeleteCategory(CategoryTree catTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", catTree.Delete),
                new SqlParameter ("@AstCatId", catTree.CatId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Suppliers Logics

        #region Get All Suppliers

        public static DataSet GetAllSuppliers(SupplierRequest suppReq, string StoreProcedure)
        {
            if (suppReq.Searching != 0)
            {
                if (suppReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", suppReq.Searching),
                    new SqlParameter ("@Var", suppReq.Var),
                    new SqlParameter ("@Dropdown", suppReq.Dropdown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", suppReq.Searching),
                    new SqlParameter ("@Var", suppReq.Var),
                    new SqlParameter ("@PageIndex", suppReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", suppReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (suppReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", suppReq.Get),
                    new SqlParameter ("@Dropdown", suppReq.Dropdown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", suppReq.Get),
                    new SqlParameter ("@PageIndex", suppReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", suppReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Supplier

        public static DataTable InsertSupplier(SupplierRequest suppReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", suppReq.Add),
                new SqlParameter ("@SuppID", suppReq.SuppID),
                new SqlParameter ("@SuppName", suppReq.SuppName),
                new SqlParameter ("@SuppCell", suppReq.SuppCell),
                new SqlParameter ("@SuppFax", suppReq.SuppFax),
                new SqlParameter ("@SuppPhone", suppReq.SuppPhone),
                new SqlParameter ("@SuppEmail", suppReq.SuppEmail),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Supplier

        public static DataTable UpdateSupplier(SupplierRequest suppReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", suppReq.Update),
                new SqlParameter ("@SuppID", suppReq.SuppID),
                new SqlParameter ("@SuppName", suppReq.SuppName),
                new SqlParameter ("@SuppCell", suppReq.SuppCell),
                new SqlParameter ("@SuppFax", suppReq.SuppFax),
                new SqlParameter ("@SuppPhone", suppReq.SuppPhone),
                new SqlParameter ("@SuppEmail", suppReq.SuppEmail),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Supplier

        public static DataTable DeleteSupplier(SupplierRequest suppReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", suppReq.Delete),
                new SqlParameter ("@SuppID", suppReq.SuppID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Designation Logics

        #region Get All Designations

        public static DataSet GetAllDesignations(DesignationRequest designReq, string StoreProcedure)
        {
            if (designReq.Searching != 0)
            {
                if (designReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", designReq.Searching),
                        new SqlParameter ("@DropDown", designReq.DropDown),
                        new SqlParameter ("@Var", designReq.Var),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", designReq.Searching),
                        new SqlParameter ("@Var", designReq.Var),
                        new SqlParameter ("@PageIndex", designReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", designReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
            }
            else
            {
                if (designReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", designReq.Get),
                        new SqlParameter ("@DropDown", designReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", designReq.Get),
                        new SqlParameter ("@PageIndex", designReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", designReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Designation

        public static DataTable InsertDesignation(DesignationRequest designationReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", designationReq.Add),
                new SqlParameter ("@Description", designationReq.Description)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Designation

        public static DataTable UpdateDesignation(DesignationRequest designationReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", designationReq.Update),
                new SqlParameter ("@DesignationID", designationReq.DesignationID),
                new SqlParameter ("@Description", designationReq.Description)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Designation

        public static DataTable DeleteDesignation(DesignationRequest designationReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", designationReq.Delete),
                new SqlParameter ("@DesignationID", designationReq.DesignationID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Cost Center Logics

        #region Get All Cost Centers

        public static DataSet GetAllCostCenters(CostCenterRequest costCenterReq, string StoreProcedure)
        {
            if (costCenterReq.Searching != 0)
            {
                if (costCenterReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", costCenterReq.Searching),
                        new SqlParameter ("@DropDown", costCenterReq.DropDown),
                        new SqlParameter ("@Var", costCenterReq.Var),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", costCenterReq.Searching),
                        new SqlParameter ("@Var", costCenterReq.Var),
                        new SqlParameter ("@PageIndex", costCenterReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", costCenterReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (costCenterReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", costCenterReq.Get),
                        new SqlParameter ("@DropDown", costCenterReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", costCenterReq.Get),
                        new SqlParameter ("@PageIndex", costCenterReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", costCenterReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert Cost Center

        public static DataTable InsertCostCenter(CostCenterRequest costCenterReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", costCenterReq.Add),
                new SqlParameter ("@CostNumber", costCenterReq.CostNumber),
                new SqlParameter ("@CostName", costCenterReq.CostName),
                new SqlParameter ("@CompanyId", costCenterReq.CompanyId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Cost Center

        public static DataTable UpdateCostCenter(CostCenterRequest costCenterReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", costCenterReq.Update),
                new SqlParameter ("@CostID", costCenterReq.CostCenterID),
                new SqlParameter ("@CostNumber", costCenterReq.CostNumber),
                new SqlParameter ("@CostName", costCenterReq.CostName),
                new SqlParameter ("@CompanyId", costCenterReq.CompanyId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Cost Center

        public static DataTable DeleteCostCenter(CostCenterRequest costCenterReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", costCenterReq.Delete),
                new SqlParameter ("@CostID", costCenterReq.CostCenterID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region User Logics

        #region Get All Users

        public static DataSet GetAllUsers(User userReq, string StoreProcedure)
        {
            if (userReq.Searching != 0)
            {
                if (userReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", userReq.Searching),
                    new SqlParameter ("@Var", userReq.Var),
                    new SqlParameter ("@Dropdown", userReq.Dropdown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", userReq.Searching),
                    new SqlParameter ("@Var", userReq.Var),
                    new SqlParameter ("@PageIndex", userReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", userReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (userReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", userReq.Get),
                    new SqlParameter ("@Dropdown", userReq.Dropdown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", userReq.Get),
                    new SqlParameter ("@PageIndex", userReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", userReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert User

        public static DataTable InsertUser(User userReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", userReq.Add),
                new SqlParameter ("@LoginName", userReq.LoginName),
                new SqlParameter ("@UserName", userReq.UserName),
                new SqlParameter ("@Password", userReq.Password),
                new SqlParameter ("@UserAccess", userReq.UserAccess),
                new SqlParameter ("@RoleID", userReq.RoleID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update User

        public static DataTable UpdateUser(User userReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", userReq.Update),
                new SqlParameter ("@LoginName", userReq.LoginName),
                new SqlParameter ("@UserName", userReq.UserName),
                new SqlParameter ("@UserAccess", userReq.UserAccess),
                new SqlParameter ("@Password", userReq.Password),
                new SqlParameter ("@RoleID", userReq.RoleID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete User

        public static DataTable DeleteUser(User userReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", userReq.Delete),
                new SqlParameter ("@LoginName", userReq.LoginName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check Admin User & Role

        public static DataTable CheckAdminUserRole(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #endregion

        #region Menu Logics

        #region Get Menus

        public static DataTable GetMenus(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get All Menus

        public static DataTable GetAllMenus(string RoleId, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@RoleId", RoleId)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get All Menus With No Rights

        public static DataTable GetAllMenusWithNoRights(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get Menu Options With Rights

        public static DataTable GetMenuOptionsWithRights(MenuOptions menuOptions, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@MenuID", menuOptions.MenuId),
                new SqlParameter ("@RoleID", menuOptions.RoleId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Roles Logics

        #region Get All Roles

        public static DataSet GetAllRoles(RoleRequest roleReq, string StoreProcedure)
        {
            if (roleReq.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", roleReq.Searching),
                    new SqlParameter ("@Var", roleReq.Var),
                    new SqlParameter ("@PageIndex", roleReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", roleReq.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", roleReq.Get),
                    new SqlParameter ("@PageIndex", roleReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", roleReq.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
        }

        #endregion

        #region Get Role By ID

        public static DataSet GetRoleByID(RoleRequest roleReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@GetByID", roleReq.GetByID),
                new SqlParameter ("@RoleID", roleReq.RoleID),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Insert Role

        public static DataTable InsertRole(RoleRequest roleReq, string companies, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter("@Description",roleReq.Description),
                new SqlParameter("@add",roleReq.Add),
                new SqlParameter("@Companies", companies),
                new SqlParameter("@LoginName",roleReq.LoginName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Role

        public static DataTable UpdateRole(RoleRequest roleReq, string companies, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter("@RoleId",roleReq.RoleID),
                new SqlParameter("@Description",roleReq.Description),
                new SqlParameter("@Companies", companies),
                new SqlParameter("@Update",roleReq.Update),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Role Rights

        public static DataTable UpdateRoleRights(RoleRequest roleReq, DataTable dtMainTree, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter("@RoleId",roleReq.RoleID),
                new SqlParameter("@LoginName",roleReq.LoginName),
                new SqlParameter("@Description",roleReq.Description),
                new SqlParameter("@Update",roleReq.Update),
                new SqlParameter("@RoleAssignOptions_DTable",dtMainTree),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Role

        public static DataTable DeleteRole(RoleRequest roleReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", roleReq.Delete),
                new SqlParameter ("@RoleID", roleReq.RoleID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Assigned Menus

        public static DataSet GetAssignedMenus(RightsParams rightParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@RoleID", rightParams.RoleID),
                new SqlParameter ("@MenuID", rightParams.MenuID),
                new SqlParameter ("@Flag", rightParams.Flag),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Get Assigned Menu Options

        public static DataSet GetAssignedMenuOptions(RightsParams rightParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@RoleID", rightParams.RoleID),
                new SqlParameter ("@MenuID", rightParams.MenuID),
                new SqlParameter ("@Flag", rightParams.Flag),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #endregion

        #region GLCodes Logics

        #region Get All GLCodes

        public static DataSet GetAllGLCodes(GLCodeRequest glCodeReq, string StoreProcedure)
        {
            if (glCodeReq.Searching != 0)
            {
                if (glCodeReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", glCodeReq.Searching),
                        new SqlParameter ("@Var", glCodeReq.Var),
                        new SqlParameter ("@DropDown", glCodeReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", glCodeReq.Searching),
                        new SqlParameter ("@Var", glCodeReq.Var),
                        new SqlParameter ("@PageIndex", glCodeReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", glCodeReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (glCodeReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", glCodeReq.Get),
                        new SqlParameter ("@DropDown", glCodeReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", glCodeReq.Get),
                        new SqlParameter ("@PageIndex", glCodeReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", glCodeReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert GLCode

        public static DataTable InsertGLCode(GLCodeRequest glcodeReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", glcodeReq.Add),
                new SqlParameter ("@GLCode", glcodeReq.GLCode),
                new SqlParameter ("@GLDesc", glcodeReq.GLDesc),
                new SqlParameter ("@CompanyId", glcodeReq.CompanyId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update GLCode

        public static DataTable UpdateGLCode(GLCodeRequest glcodeReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", glcodeReq.Update),
                new SqlParameter ("@GLCode", glcodeReq.GLCode),
                new SqlParameter ("@GLDesc", glcodeReq.GLDesc),
                new SqlParameter ("@CompanyId", glcodeReq.CompanyId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete GLCode

        public static DataTable DeleteGLCode(GLCodeRequest glcodeReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", glcodeReq.Delete),
                new SqlParameter ("@GLCode", glcodeReq.GLCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Units Logics

        #region Get All Units

        public static DataSet GetAllUnits(UnitRequestParam unitReq, string StoreProcedure)
        {
            if (unitReq.Searching != 0)
            {
                if (unitReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", unitReq.Searching),
                        new SqlParameter ("@Var", unitReq.Var),
                        new SqlParameter ("@Dropdown", unitReq.Dropdown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", unitReq.Searching),
                        new SqlParameter ("@Var", unitReq.Var),
                        new SqlParameter ("@PageIndex", unitReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", unitReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (unitReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", unitReq.Get),
                        new SqlParameter ("@Dropdown", unitReq.Dropdown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", unitReq.Get),
                        new SqlParameter ("@PageIndex", unitReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", unitReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert Unit

        public static DataTable InsertUnit(UnitRequestParam unitReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", unitReqParam.Add),
                new SqlParameter ("@UnitID", unitReqParam.UnitID),
                new SqlParameter ("@UnitDesc", unitReqParam.UnitDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Unit

        public static DataTable UpdateUnit(UnitRequestParam unitReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", unitReqParam.Update),
                new SqlParameter ("@UnitID", unitReqParam.UnitID),
                new SqlParameter ("@UnitDesc", unitReqParam.UnitDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Unit

        public static DataTable DeleteUnit(UnitRequestParam unitReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", unitReqParam.Delete),
                new SqlParameter ("@UnitID", unitReqParam.UnitID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Brands Logics

        #region Get All Brands

        public static DataSet GetAllBrands(BrandRequest brandReq, string StoreProcedure)
        {
            if (brandReq.Searching != 0)
            {
                if (brandReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", brandReq.Searching),
                        new SqlParameter ("@DropDown", brandReq.DropDown),
                        new SqlParameter ("@Var", brandReq.Var),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", brandReq.Searching),
                        new SqlParameter ("@Var", brandReq.Var),
                        new SqlParameter ("@PageIndex", brandReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", brandReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
            }
            else
            {
                if (brandReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", brandReq.Get),
                        new SqlParameter ("@DropDown", brandReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", brandReq.Get),
                        new SqlParameter ("@PageIndex", brandReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", brandReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert Brand

        public static DataTable InsertBrand(BrandRequest brandReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", brandReq.Add),
                new SqlParameter ("@AstBrandID", brandReq.AstBrandID),
                new SqlParameter ("@AstBrandName", brandReq.AstBrandName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Brand

        public static DataTable UpdateBrand(BrandRequest brandReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", brandReq.Update),
                new SqlParameter ("@AstBrandID", brandReq.AstBrandID),
                new SqlParameter ("@AstBrandName", brandReq.AstBrandName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Brand

        public static DataTable DeleteBrand(BrandRequest brandReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", brandReq.Delete),
                new SqlParameter ("@AstBrandID", brandReq.AstBrandID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Disposal Methods Logics

        #region Get All Disposal Methods

        public static DataSet GetAllDisposalMethods(DisposalMethodsRequest disposalMethodReq, string StoreProcedure)
        {
            if (disposalMethodReq.Searching != 0)
            {
                if (disposalMethodReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", disposalMethodReq.Searching),
                        new SqlParameter ("@Var", disposalMethodReq.Var),
                        new SqlParameter ("@DropDown", disposalMethodReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", disposalMethodReq.Searching),
                        new SqlParameter ("@Var", disposalMethodReq.Var),
                        new SqlParameter ("@PageIndex", disposalMethodReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", disposalMethodReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (disposalMethodReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", disposalMethodReq.Get),
                        new SqlParameter ("@DropDown", disposalMethodReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", disposalMethodReq.Get),
                        new SqlParameter ("@PageIndex", disposalMethodReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", disposalMethodReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Disposal Method

        public static DataTable InsertDisposalMethod(DisposalMethodsRequest dispMethodsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", dispMethodsReq.Add),
                new SqlParameter ("@DispCode", dispMethodsReq.DispCode),
                new SqlParameter ("@DispDesc", dispMethodsReq.DispDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Disposal Method

        public static DataTable UpdateDisposalMethod(DisposalMethodsRequest dispMethodsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", dispMethodsReq.Update),
                new SqlParameter ("@DispCode", dispMethodsReq.DispCode),
                new SqlParameter ("@DispDesc", dispMethodsReq.DispDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Disposal Method

        public static DataTable DeleteDisposalMethod(DisposalMethodsRequest dispMethodsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", dispMethodsReq.Delete),
                new SqlParameter ("@DispCode", dispMethodsReq.DispCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Depreciation Logics

        #region Depreciation Methods Logics

        #region Get All Depreciation Methods

        public static DataSet GetAllDepreciationMethods(DepreciationMethodsRequest depMethodReq, string StoreProcedure)
        {
            if (depMethodReq.Searching != 0)
            {
                if (depMethodReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", depMethodReq.Searching),
                        new SqlParameter ("@Var", depMethodReq.Var),
                        new SqlParameter ("@DropDown", depMethodReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", depMethodReq.Searching),
                        new SqlParameter ("@Var", depMethodReq.Var),
                        new SqlParameter ("@PageIndex", depMethodReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", depMethodReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (depMethodReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", depMethodReq.Get),
                        new SqlParameter ("@DropDown", depMethodReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", depMethodReq.Get),
                        new SqlParameter ("@PageIndex", depMethodReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", depMethodReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Depreciation Method

        public static DataTable InsertDepreciationMethod(DepreciationMethodsRequest depMethodReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", depMethodReq.Add),
                new SqlParameter ("@DepCode", depMethodReq.DepCode),
                new SqlParameter ("@DepDesc", depMethodReq.DepDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Depreciation Method

        public static DataTable UpdateDepreciationMethod(DepreciationMethodsRequest depMethodReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", depMethodReq.Update),
                new SqlParameter ("@DepCode", depMethodReq.DepCode),
                new SqlParameter ("@DepDesc", depMethodReq.DepDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Depreciation Method

        public static DataTable DeleteDepreciationMethod(DepreciationMethodsRequest depMethodReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", depMethodReq.Delete),
                new SqlParameter ("@DepCode", depMethodReq.DepCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Depreciation Policy Logics

        #region Get Depreciation Policy Against AstCatID

        public static DataTable GetDepPolicyAgainstAstCatID(DepPolicyReqParams depPolicyReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", depPolicyReqParams.Get),
                new SqlParameter ("@AstCatID", depPolicyReqParams.AstCatID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Depreciation Policy

        public static DataTable InsertDepPolicy(DepPolicyReqParams depPolicyReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", depPolicyReqParams.Add),
                new SqlParameter ("@CatDepID", depPolicyReqParams.AstCatID),
                new SqlParameter ("@AstCatID", depPolicyReqParams.AstCatID),
                new SqlParameter ("@DepCode", 1),
                new SqlParameter ("@SalvageValue", depPolicyReqParams.SalvageValue),
                new SqlParameter ("@SalvageYear", Convert.ToInt32(depPolicyReqParams.SalvageYear)),
                new SqlParameter ("@SalvageMonth", Convert.ToInt32(depPolicyReqParams.SalvageMonth)),
                new SqlParameter ("@SalvagePercent", Convert.ToDouble(depPolicyReqParams.SalvagePercent)),
                new SqlParameter ("@IsSalvageValuePercentage", depPolicyReqParams.IsSalvageValuePercentage),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Depreciation Policy History

        public static DataTable InsertDepPolicy_History(DepPolicy_History depPolicy_History, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@DepCode", depPolicy_History.DepCode),
                new SqlParameter ("@SalvageValue", depPolicy_History.SalvageValue),
                new SqlParameter ("@SalvageYear", depPolicy_History.SalvageYear),
                new SqlParameter ("@SalvageMonth", depPolicy_History.SalvageMonth),
                new SqlParameter ("@BookID", depPolicy_History.BookID),
                new SqlParameter ("@AstID", depPolicy_History.AstID),
                new SqlParameter ("@CurrentBV", depPolicy_History.CurrentBV),
                new SqlParameter ("@BVUpdate", depPolicy_History.BVUpdate),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Depreciation Policy

        public static DataTable UpdateDepPolicy(DepPolicyReqParams depPolicyReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", depPolicyReqParams.Update),
                new SqlParameter ("@CatDepID", depPolicyReqParams.CatDepID),
                new SqlParameter ("@SalvageValue", depPolicyReqParams.SalvageValue),
                new SqlParameter ("@SalvageYear", Convert.ToInt32(depPolicyReqParams.SalvageYear)),
                new SqlParameter ("@SalvageMonth", Convert.ToInt32(depPolicyReqParams.SalvageMonth)),
                new SqlParameter ("@SalvagePercent", Convert.ToDouble(depPolicyReqParams.SalvagePercent)),
                new SqlParameter ("@IsSalvageValuePercentage", depPolicyReqParams.IsSalvageValuePercentage),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Depreciation Engine Logics

        #region Get All Depreciation Methods

        public static DataSet GetAstBookAgainstCompanyIDForDeprecation(DeprecitionEngineReqParams depEngineReqParams, string StoreProcedure)
        {
            if (depEngineReqParams.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", depEngineReqParams.Searching),
                new SqlParameter ("@Var", depEngineReqParams.Var),
                new SqlParameter ("@CompanyID", depEngineReqParams.CompanyID),
                new SqlParameter ("@PageIndex", depEngineReqParams.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", depEngineReqParams.PaginationParam.PageSize),
            };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", depEngineReqParams.Get),
                    new SqlParameter ("@CompanyID", depEngineReqParams.CompanyID),
                    new SqlParameter ("@PageIndex", depEngineReqParams.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", depEngineReqParams.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        public static DataTable GetAstBookAgainstCompanyIDForDeprecationNewAsset(DeprecitionEngineReqParams depEngineReqParams, string StoreProcedure)
        {
            if (depEngineReqParams.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", depEngineReqParams.Searching),
                new SqlParameter ("@Var", depEngineReqParams.Var),
                new SqlParameter ("@CompanyID", depEngineReqParams.CompanyID),
                new SqlParameter ("@PageIndex", depEngineReqParams.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", depEngineReqParams.PaginationParam.PageSize),
            };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", depEngineReqParams.Get),
                    new SqlParameter ("@CompanyID", depEngineReqParams.CompanyID),
                    new SqlParameter ("@PageIndex", depEngineReqParams.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", depEngineReqParams.PaginationParam.PageSize),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        public static DataTable DisposeAsset(string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@AstID", AstID),
                };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable InsertInAstBooks(DataTable AstBooksUpdateDT, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@updateAstBooks", AstBooksUpdateDT),
                };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Assets Count Against BookID

        public static DataTable GetAssetsCountAgainstBookID(string selectedBookID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@BookID", Convert.ToInt32(selectedBookID)),
                };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check DepLogs

        public static DataTable GetAll_DepLogs_Date(string date, int bookID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@BookID", Convert.ToInt32(bookID)),
                    new SqlParameter ("@UpdateTillDate", date),
                };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Asset Details Against Book ID

        public static DataTable GetAssetDetails_ByBookID(int bookID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@BooksID", Convert.ToInt32(bookID))
                };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check Entries in DepHistory

        public static DataTable Check_DepHistory(string BookID, string AstID, DateTime BVUpdateDate, DateTime dtFiscal, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@BVUpdateDate", BVUpdateDate),
                    new SqlParameter ("@dtFiscal", dtFiscal),
                    new SqlParameter ("@BookID", BookID),
                    new SqlParameter ("@AstID", AstID),
                };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert In Book History

        public static DataTable InsertInBookHistory(int bookHistoryFlag, DataTable dt, string StoreProcedure)
        {
            if (bookHistoryFlag == 1)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@bookHistoryFlag", bookHistoryFlag),
                    new SqlParameter ("@bookHistory", dt),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@bookHistoryFlag", bookHistoryFlag),
                    new SqlParameter ("@updateAstBooks", dt),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        #endregion

        #region Insert In Dep Logs

        public static DataTable EndDepProcess(DateTime dtFiscal, double totDepValue, int totAstCount, double totAstValue, int month, int year, int bookID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@dtFiscal", dtFiscal),
                new SqlParameter ("@depPrdType", 0),
                new SqlParameter ("@totDepValue", totDepValue),
                new SqlParameter ("@totAstCount", totAstCount),
                new SqlParameter ("@totAstValue", totAstValue),
                new SqlParameter ("@month", month),
                new SqlParameter ("@year", year),
                new SqlParameter ("@bookID", bookID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #endregion

        #region Update Ast Book against AstID

        public static DataTable UpdateAstBook(AstBookReqParams astBookReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@BookID", astBookReqParams.BookID),
                new SqlParameter ("@AstID", astBookReqParams.AstID),
                new SqlParameter ("@DepCode", astBookReqParams.DepCode),
                new SqlParameter ("@SalvageValue", astBookReqParams.SalvageValue),
                new SqlParameter ("@SalvageYear", astBookReqParams.SalvageYear),
                new SqlParameter ("@LastBV", astBookReqParams.LastBV),
                new SqlParameter ("@CurrentBV", astBookReqParams.CurrentBV),
                new SqlParameter ("@BVUpdate", astBookReqParams.BVUpdate),
                new SqlParameter ("@SalvageValuePercent", astBookReqParams.SalvagePercent),
                new SqlParameter ("@SalvageMonth", astBookReqParams.SalvageMonth),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Company Logics

        #region Get All Companies

        public static DataSet GetAllCompanies(CompReqParam compReq, string StoreProcedure)
        {
            if (compReq.Searching != 0)
            {
                if (compReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", compReq.Searching),
                        new SqlParameter ("@DropDown", compReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", compReq.Searching),
                        new SqlParameter ("@Var", compReq.Var),
                        new SqlParameter ("@PageIndex", compReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", compReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (compReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", compReq.Get),
                        new SqlParameter ("@DropDown", compReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", compReq.Get),
                        new SqlParameter ("@PageIndex", compReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", compReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
            }

        }

        #endregion

        #region Insert Company

        public static DataTable InsertCompany(CompReqParam compReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", compReqParam.Add),
                new SqlParameter ("@CompanyCode", compReqParam.CompanyCode),
                new SqlParameter ("@CompanyName", compReqParam.CompanyName),
                new SqlParameter ("@BarCodeStrucID", compReqParam.BarCodeStrucId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Company

        public static DataTable UpdateCompany(CompReqParam compReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", compReqParam.Update),
                new SqlParameter ("@CompanyId", compReqParam.CompanyId),
                new SqlParameter ("@CompanyCode", compReqParam.CompanyCode),
                new SqlParameter ("@CompanyName", compReqParam.CompanyName),
                new SqlParameter ("@BarCodeStrucID", compReqParam.BarCodeStrucId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Company

        public static DataTable DeleteCompany(CompReqParam compReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", compReqParam.Delete),
                new SqlParameter ("@CompanyId", compReqParam.CompanyId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Company Info

        public static DataSet GetCompanyInfo(CompInfoReqParam compInfoReq, string StoreProcedure)
        {
            if (compInfoReq.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", compInfoReq.Searching),
                    new SqlParameter ("@Var", compInfoReq.Var),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", compInfoReq.Get),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        #endregion

        #region Insert Company Info

        public static DataTable InsertCompanyInfo(CompInfoReqParam compInfoReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", compInfoReqParam.Add),
                new SqlParameter ("@Name", compInfoReqParam.Name),
                new SqlParameter ("@Address", compInfoReqParam.Address),
                new SqlParameter ("@State", compInfoReqParam.State),
                new SqlParameter ("@PCode", compInfoReqParam.PCode),
                new SqlParameter ("@City", compInfoReqParam.City),
                new SqlParameter ("@Country", compInfoReqParam.Country),
                new SqlParameter ("@PhoneNo", compInfoReqParam.PhoneNo),
                new SqlParameter ("@Fax", compInfoReqParam.Fax),
                new SqlParameter ("@Email", compInfoReqParam.Email),
                new SqlParameter ("@ImageToBase64", compInfoReqParam.ImageToBase64),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Company Info

        public static DataTable UpdateCompanyInfo(CompInfoReqParam compInfoReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", compInfoReqParam.Update),
                new SqlParameter ("@ID", compInfoReqParam.ID),
                new SqlParameter ("@Name", compInfoReqParam.Name),
                new SqlParameter ("@Address", compInfoReqParam.Address),
                new SqlParameter ("@State", compInfoReqParam.State),
                new SqlParameter ("@PCode", compInfoReqParam.PCode),
                new SqlParameter ("@City", compInfoReqParam.City),
                new SqlParameter ("@Country", compInfoReqParam.Country),
                new SqlParameter ("@PhoneNo", compInfoReqParam.PhoneNo),
                new SqlParameter ("@Fax", compInfoReqParam.Fax),
                new SqlParameter ("@Email", compInfoReqParam.Email),
                new SqlParameter ("@ImageToBase64", compInfoReqParam.ImageToBase64),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Company Info Logo

        public static DataTable UpdateCompanyInfoLogo(string imgBase64, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@ImageToBase64", imgBase64)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Inter-Compayn Transfer Logics

        #region Get Ast Info For Inter-Company Transfer

        public static DataSet GetAstInfo(InterCompanyTransferReqParams interCompanyTransferReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", interCompanyTransferReqParams.Get),
                new SqlParameter ("@AstID", interCompanyTransferReqParams.AstID),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get GLCodes Against CompanyID

        public static DataTable GetGLCodesAgainstCompanyID(string CompanyID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", CompanyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Ast Info For Inter-Company Transfer

        public static DataTable UpdateAstInfo(InterCompanyTransferReqParams interCompanyTransferReqParams, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", interCompanyTransferReqParams.Get),
                new SqlParameter ("@AstID", interCompanyTransferReqParams.AstID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Is Asset Disposed

        public static DataTable IsAssetDisposed(string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstID", AstID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        public static DataTable GetAstInfoByAstID2(string AstID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstID", AstID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable CheckBookExistsAgainstCompanyID(string companyID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", companyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Insurer Logics

        #region Get All Insurers

        public static DataSet GetAllInsurers(InsurerReqParam insReq, string StoreProcedure)
        {
            if (insReq.Searching != 0)
            {
                if (insReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", insReq.Searching),
                        new SqlParameter ("@Var", insReq.Var),
                        new SqlParameter ("@DropDown", insReq.DropDown)
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", insReq.Searching),
                        new SqlParameter ("@Var", insReq.Var),
                        new SqlParameter ("@PageIndex", insReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", insReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (insReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", insReq.Get),
                        new SqlParameter ("@DropDown", insReq.DropDown)
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", insReq.Get),
                        new SqlParameter ("@PageIndex", insReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", insReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert Insurer

        public static DataTable InsertInsurer(InsurerReqParam insReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", insReqParam.Add),
                new SqlParameter ("@InsCode", insReqParam.InsCode),
                new SqlParameter ("@InsName", insReqParam.InsName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Insurer

        public static DataTable UpdateInsurer(InsurerReqParam insReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", insReqParam.Update),
                new SqlParameter ("@InsCode", insReqParam.InsCode),
                new SqlParameter ("@InsName", insReqParam.InsName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Insurer

        public static DataTable DeleteInsurer(InsurerReqParam insReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", insReqParam.Delete),
                new SqlParameter ("@InsCode", insReqParam.InsCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Inventory Schedules Logics

        #region Get All Inventory Schedules

        public static DataSet GetAllInvSchs(InvSchReqParam invSchReq, string StoreProcedure)
        {
            if (invSchReq.Searching != 0)
            {
                if (invSchReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", invSchReq.Searching),
                    new SqlParameter ("@Var", invSchReq.Var),
                    new SqlParameter ("@Dropdown", invSchReq.Dropdown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", invSchReq.Searching),
                    new SqlParameter ("@Var", invSchReq.Var),
                    new SqlParameter ("@PageIndex", invSchReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", invSchReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
            }
            else
            {
                if (invSchReq.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", invSchReq.Get),
                    new SqlParameter ("@Dropdown", invSchReq.Dropdown),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", invSchReq.Get),
                    new SqlParameter ("@PageIndex", invSchReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", invSchReq.PaginationParam.PageSize),
                };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Inventory Schedules

        public static DataTable InsertInvSch(InvSchReqParam invSchReqParam, string locIDs, string deviceHardwareIDs, string formattedLocIDs, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", invSchReqParam.Add),
                new SqlParameter ("@InvSchCode", invSchReqParam.InvSchCode),
                new SqlParameter ("@InvDesc", invSchReqParam.InvDesc),
                new SqlParameter ("@InvStartDate", invSchReqParam.InvStartDate),
                new SqlParameter ("@InvEndDate", invSchReqParam.InvEndDate),
                new SqlParameter ("@Closed", invSchReqParam.Closed),
                new SqlParameter ("@SchType", invSchReqParam.SchType),
                new SqlParameter ("@InvLoc", locIDs),
                new SqlParameter ("@InvDev", deviceHardwareIDs),
                new SqlParameter ("@FormattedLocIDs", formattedLocIDs),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check Count Of Ast_History

        public static DataTable CheckCountOfAstHistory(InvSchReqParam invSchReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCode", invSchReqParam.InvSchCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Ast_History Against Inv Sch Code

        public static DataTable DeleteAstHistoryAgainstInvSchCode(InvSchReqParam invSchReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCode", invSchReqParam.InvSchCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Inventory Schedules

        public static DataTable UpdateInvSch(InvSchReqParam invSchReqParam, string locIDs, string deviceHardwareIDs, string formattedLocIDs, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", invSchReqParam.Update),
                new SqlParameter ("@InvSchCode", invSchReqParam.InvSchCode),
                new SqlParameter ("@InvDesc", invSchReqParam.InvDesc),
                new SqlParameter ("@InvStartDate", invSchReqParam.InvStartDate),
                new SqlParameter ("@InvEndDate", invSchReqParam.InvEndDate),
                new SqlParameter ("@Closed", invSchReqParam.Closed),
                new SqlParameter ("@SchType", invSchReqParam.SchType),
                new SqlParameter ("@InvLoc", locIDs),
                new SqlParameter ("@InvDev", deviceHardwareIDs),
                new SqlParameter ("@FormattedLocIDs", formattedLocIDs),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Inventory Schedules

        public static DataTable DeleteInvSch(InvSchReqParam invSchReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", invSchReqParam.Delete),
                new SqlParameter ("@InvSchCode", invSchReqParam.InvSchCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Address Templates Logics

        #region Get All Address Templates

        public static DataSet GetAllAddTemp(AddTempReqParam addTempReq, string StoreProcedure)
        {
            if (addTempReq.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", addTempReq.Searching),
                new SqlParameter ("@Var", addTempReq.Var),
                new SqlParameter ("@PageIndex", addTempReq.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", addTempReq.PaginationParam.PageSize),
            };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", addTempReq.Get),
                new SqlParameter ("@PageIndex", addTempReq.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", addTempReq.PaginationParam.PageSize),
            };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        #endregion

        #region Insert Address Template

        public static DataTable InsertAddTemp(AddTempReqParam addTempReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", addTempReqParam.Add),
                new SqlParameter ("@AddressDesc", addTempReqParam.AddressDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Address Template

        public static DataTable UpdateAddTemp(AddTempReqParam addTempReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", addTempReqParam.Update),
                new SqlParameter ("@AddressID", addTempReqParam.AddressID),
                new SqlParameter ("@AddressDesc", addTempReqParam.AddressDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Address Template

        public static DataTable DeleteAddTemp(AddTempReqParam addTempReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", addTempReqParam.Delete),
                new SqlParameter ("@AddressID", addTempReqParam.AddressID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Company Levels Logics

        #region Get All Company Levels

        public static DataSet GetAllLvls(LevelsParams lvlReq, string StoreProcedure)
        {
            if (lvlReq.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", lvlReq.Searching),
                    new SqlParameter ("@Var", lvlReq.Var),
                    new SqlParameter ("@PageIndex", lvlReq.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", lvlReq.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", lvlReq.Get),
                new SqlParameter ("@PageIndex", lvlReq.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", lvlReq.PaginationParam.PageSize),
            };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }

        }

        #endregion

        #region Insert Company Level

        public static DataTable InsertLvl(LevelsParams lvlParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", lvlParams.Add),
                new SqlParameter ("@LvlDesc", lvlParams.LvlDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Company Level

        public static DataTable UpdateLvl(LevelsParams lvlParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", lvlParams.Update),
                new SqlParameter ("@LvlID", lvlParams.LvlID),
                new SqlParameter ("@LvlDesc", lvlParams.LvlDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Company Level

        public static DataTable DeleteLvl(LevelsParams lvlParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", lvlParams.Delete),
                new SqlParameter ("@LvlID", lvlParams.LvlID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Server-Side Searching Logics

        #region Server-Side Searching

        public static DataTable ServerSideSearching(ServerSideSearchingParams serverSearchingParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@TableName", serverSearchingParams.TableName),
                new SqlParameter ("@FindingVar", serverSearchingParams.Var),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Organization Hierarchy Logics

        #region Get All OrganizationHierarchy

        public static DataTable GetAllOrgHierTreeView(OrgHierReqParams orgHierReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", orgHierReqParam.Get),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Organization Hierarchy

        public static DataTable InsertOrgHier(OrgHierReqParams orgHierReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", orgHierReqParam.Add),
                new SqlParameter ("@LvlId", orgHierReqParam.LvlId),
                new SqlParameter ("@OrgHierName", orgHierReqParam.OrgHierName),
                new SqlParameter ("@ParentId", orgHierReqParam.ParentId),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Organization Hierarchy

        public static DataTable UpdateOrgHier(OrgHierReqParams orgHierReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", orgHierReqParam.Update),
                new SqlParameter ("@LvlCode", orgHierReqParam.LvlCode),
                new SqlParameter ("@OrgHierName", orgHierReqParam.OrgHierName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Organization Hierarchy

        public static DataTable DeleteOrgHier(OrgHierReqParams orgHierReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", orgHierReqParam.Delete),
                new SqlParameter ("@OrgHierID", orgHierReqParam.OrgHierID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Custodian Logics

        #region Get All Custodians

        public static DataSet GetAllCustodians(CustodianReqParams custReqParams, string StoreProcedure)
        {
            if (custReqParams.Searching != 0)
            {
                if (custReqParams.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", custReqParams.Searching),
                        new SqlParameter ("@Var", custReqParams.Var),
                        new SqlParameter ("@DropDown", custReqParams.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", custReqParams.Searching),
                        new SqlParameter ("@Var", custReqParams.Var),
                        new SqlParameter ("@PageIndex", custReqParams.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", custReqParams.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
            }
            else
            {
                if (custReqParams.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", custReqParams.Get),
                        new SqlParameter ("@DropDown", custReqParams.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", custReqParams.Get),
                        new SqlParameter ("@PageIndex", custReqParams.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", custReqParams.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }

        }

        #endregion

        #region Insert Custodian

        public static DataTable InsertCustodian(CustodianReqParams custReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", custReqParams.Add),
                new SqlParameter ("@CustodianName", custReqParams.CustodianName),
                new SqlParameter ("@CustodianCode", custReqParams.CustodianCode),
                new SqlParameter ("@CustodianPhone", custReqParams.CustodianPhone),
                new SqlParameter ("@CustodianEmail", custReqParams.CustodianEmail),
                new SqlParameter ("@CustodianFax", custReqParams.CustodianFax),
                new SqlParameter ("@CustodianCell", custReqParams.CustodianCell),
                new SqlParameter ("@CustodianAddress", custReqParams.CustodianAddress),
                new SqlParameter ("@OrgHierID", custReqParams.OrgHierID),
                new SqlParameter ("@DesignationID", custReqParams.DesignationID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Custodian

        public static DataTable UpdateCustodian(CustodianReqParams custReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", custReqParams.Update),
                new SqlParameter ("@CustodianID", custReqParams.CustodianID),
                new SqlParameter ("@CustodianName", custReqParams.CustodianName),
                new SqlParameter ("@CustodianCode", custReqParams.CustodianCode),
                new SqlParameter ("@CustodianPhone", custReqParams.CustodianPhone),
                new SqlParameter ("@CustodianEmail", custReqParams.CustodianEmail),
                new SqlParameter ("@CustodianFax", custReqParams.CustodianFax),
                new SqlParameter ("@CustodianCell", custReqParams.CustodianCell),
                new SqlParameter ("@CustodianAddress", custReqParams.CustodianAddress),
                new SqlParameter ("@OrgHierID", custReqParams.OrgHierID),
                new SqlParameter ("@DesignationID", custReqParams.DesignationID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Custodian

        public static DataTable DeleteCustodian(CustodianReqParams custReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", custReqParams.Delete),
                new SqlParameter ("@CustodianID", custReqParams.CustodianID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Additional Cost Type Logic

        #region Get All Additional Cost Types

        public static DataTable GetAllAddCostType(AddCostTypeReqParam addCostTypeReqParam, string StoreProcedure)
        {
            if (addCostTypeReqParam.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", addCostTypeReqParam.Searching),
                new SqlParameter ("@Var", addCostTypeReqParam.Var)
            };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", addCostTypeReqParam.Get),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
        }

        #endregion

        #region Insert Additional Cost Type

        public static DataTable InsertAddCostType(AddCostTypeReqParam addCostTypeReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", addCostTypeReqParam.Add),
                new SqlParameter ("@AddCost", addCostTypeReqParam.AddCost),
                new SqlParameter ("@AstID", addCostTypeReqParam.AstID),
                new SqlParameter ("@LoginName", addCostTypeReqParam.LoginName),
                new SqlParameter ("@TypeDesc", addCostTypeReqParam.TypeDesc),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        //#region Update Insurer

        //public static DataTable UpdateInsurer(InsurerReqParam insReqParam, string StoreProcedure)
        //{
        //    DbReports CGD = new DbReports();
        //    SqlParameter[] sqlParameters = {
        //        new SqlParameter ("@Update", insReqParam.Update),
        //        new SqlParameter ("@InsCode", insReqParam.InsCode),
        //        new SqlParameter ("@InsName", insReqParam.InsName),
        //    };
        //    return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        //}

        //#endregion

        //#region Delete Insurer

        //public static DataTable DeleteInsurer(InsurerReqParam insReqParam, string StoreProcedure)
        //{
        //    DbReports CGD = new DbReports();
        //    SqlParameter[] sqlParameters = {
        //        new SqlParameter ("@Delete", insReqParam.Delete),
        //        new SqlParameter ("@InsCode", insReqParam.InsCode),
        //    };
        //    return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        //}

        //#endregion

        #endregion

        #region Books Logics

        #region Get All Books

        public static DataSet GetAllBooks(BooksReqParam booksReq, string StoreProcedure)
        {
            if (booksReq.Searching != 0)
            {
                if (booksReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", booksReq.Searching),
                        new SqlParameter ("@Var", booksReq.Var),
                        new SqlParameter ("@DropDown", booksReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", booksReq.Searching),
                        new SqlParameter ("@Var", booksReq.Var),
                        new SqlParameter ("@PageIndex", booksReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", booksReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (booksReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", booksReq.Get),
                        new SqlParameter ("@DropDown", booksReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", booksReq.Get),
                        new SqlParameter ("@PageIndex", booksReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", booksReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Get Book Against CompanyID

        public static DataTable GetBookAgainstCompanyID(string CompanyID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", CompanyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert Book

        public static DataTable InsertBook(BooksReqParam booksReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", booksReqParam.Add),
                new SqlParameter ("@DepCode", booksReqParam.DepCode),
                new SqlParameter ("@Description", booksReqParam.Description),
                new SqlParameter ("@CompanyID", booksReqParam.CompanyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Book

        public static DataTable UpdateBook(BooksReqParam booksReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", booksReqParam.Update),
                new SqlParameter ("@DepCode", booksReqParam.DepCode),
                new SqlParameter ("@Description", booksReqParam.Description),
                new SqlParameter ("@CompanyID", booksReqParam.CompanyID),
                new SqlParameter ("@BookID", booksReqParam.BookID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Book

        public static DataTable DeleteBook(BooksReqParam booksReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", booksReqParam.Delete),
                new SqlParameter ("@BookID", booksReqParam.BookID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Get All Barcoding Policy

        public static DataSet GetAllBarcodingPolicy(string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };

            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);

        }

        #endregion

        #region Barcode Structure Columns Logics

        #region Get All Barcode Structure Columns

        public static DataTable GetAllBarcodeStructureCol(BarcodeStructCol barcodeStructColReqParam, string StoreProcedure)
        {
            if (barcodeStructColReqParam.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", barcodeStructColReqParam.Searching),
                new SqlParameter ("@Var", barcodeStructColReqParam.Var),
                new SqlParameter ("@PageIndex", barcodeStructColReqParam.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", barcodeStructColReqParam.PaginationParam.PageSize),
            };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", barcodeStructColReqParam.Get),
                    new SqlParameter ("@PageIndex", barcodeStructColReqParam.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", barcodeStructColReqParam.PaginationParam.PageSize),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
        }

        #endregion

        #region Insert Barcode Structure Column

        public static DataTable InsertBarcodeStructureCol(BarcodeStructCol barcodeStructColReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", barcodeStructColReqParam.Add),
                new SqlParameter ("@Name", barcodeStructColReqParam.Name),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Barcode Structure Column

        public static DataTable UpdateBarcodeStructureCol(BarcodeStructCol barcodeStructColReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", barcodeStructColReqParam.Update),
                new SqlParameter ("@Name", barcodeStructColReqParam.Name),
                new SqlParameter ("@ID", barcodeStructColReqParam.ID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Barcode Structure Column

        public static DataTable DeleteBarcodeStructureCol(BarcodeStructCol barcodeStructColReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", barcodeStructColReqParam.Delete),
                new SqlParameter ("@ID", barcodeStructColReqParam.ID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Barcode Structure Logics

        #region Get All Barcode Structure

        public static DataSet GetAllBarcodeStructures(BarcodeStructureReqParam barcodeStructureReqParam, string StoreProcedure)
        {
            //if (barcodeStructColReqParam.Searching != 0)
            //{
            //    DbReports CGD = new DbReports();
            //    SqlParameter[] sqlParameters = {
            //    new SqlParameter ("@Searching", barcodeStructColReqParam.Searching),
            //    new SqlParameter ("@Var", barcodeStructColReqParam.Var),
            //    new SqlParameter ("@PageIndex", barcodeStructColReqParam.PaginationParam.PageIndex),
            //    new SqlParameter ("@PageSize", barcodeStructColReqParam.PaginationParam.PageSize),
            //};
            //    return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            //}
            //else
            //{
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", barcodeStructureReqParam.Get),
                    //new SqlParameter ("@PageIndex", barcodeStructureReqParam.PaginationParam.PageIndex),
                    //new SqlParameter ("@PageSize", barcodeStructureReqParam.PaginationParam.PageSize),
                };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            //}
        }

        #endregion

        #region Insert Barcode Structure

        public static DataTable InsertBarcodeStructure(BarcodeStructureReqParam barcodeStructureReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", barcodeStructureReqParam.Add),
                new SqlParameter ("@BarStructDesc", barcodeStructureReqParam.BarStructDesc),
                new SqlParameter ("@BarStructLength", barcodeStructureReqParam.BarStructLength),
                new SqlParameter ("@BarStructPrefix", barcodeStructureReqParam.BarStructPrefix),
                new SqlParameter ("@ValueSep", barcodeStructureReqParam.ValueSep),
                new SqlParameter ("@Barcode", barcodeStructureReqParam.Barcode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Barcode Structure

        public static DataTable UpdateBarcodeStructure(BarcodeStructureReqParam barcodeStructureReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", barcodeStructureReqParam.Update),
                new SqlParameter ("@BarStructID", barcodeStructureReqParam.BarStructID),
                new SqlParameter ("@BarStructDesc", barcodeStructureReqParam.BarStructDesc),
                new SqlParameter ("@BarStructLength", barcodeStructureReqParam.BarStructLength),
                new SqlParameter ("@BarStructPrefix", barcodeStructureReqParam.BarStructPrefix),
                new SqlParameter ("@ValueSep", barcodeStructureReqParam.ValueSep),
                new SqlParameter ("@Barcode", barcodeStructureReqParam.Barcode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Barcode Structure

        public static DataTable DeleteBarcodeStructure(BarcodeStructureReqParam barcodeStructureReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", barcodeStructureReqParam.Delete),
                new SqlParameter ("@BarStructID", barcodeStructureReqParam.BarStructID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Apply Barcode Policy

        public static DataTable Barcode_AssignToSelectedCompany(string CompanyID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", CompanyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable BarcodeStructureAgainstBarStructID(int BarStructID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@BarStructID", BarStructID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable UpdateCompanyBarcodeStructure(Barcode_AssignCompany barcode_AssignCompanyReqParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyId", barcode_AssignCompanyReqParam.CompanyID),
                new SqlParameter ("@BarcodeStructID", barcode_AssignCompanyReqParam.BarcodeStructureID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable GetLocCompCode(string LocID)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@LocID", LocID),
            };
            return CGD.DTWithParam("[dbo].[SP_GetLocCompCodeAgainstLocID]", sqlParameters, 1);
        }

        public static DataTable GetCatCompCode(string astCatID)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstCatID", astCatID),
            };
            return CGD.DTWithParam("[dbo].[SP_GetCatCompCodeAgainstAstCatID]", sqlParameters, 1);
        }

        public static DataTable ApplyBarcodePolicy(DataTable ApplyBarCodePolicy, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@applyBarCodePolicy", ApplyBarCodePolicy),
                new SqlParameter ("@add", 1),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable GetAllAssetsAgainstItemCode(string ItemCode, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@ItemCode", ItemCode)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region System Configuration Logics

        #region Get System Configuration Information

        public static DataTable GetSysConfigInfo(SysConfigReqParams sysConfigReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", sysConfigReqParams.Get),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Insert System Configuration Information

        public static DataTable InsertSysConfigInfo(SysConfigReqParams sysConfigReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", sysConfigReqParams.Add),
                new SqlParameter ("@FYDate", 1),
                new SqlParameter ("@DepreciationRunType", sysConfigReqParams.DepreciationRunType),
                new SqlParameter ("@DeletePermanent", sysConfigReqParams.DeletePermanent),
                new SqlParameter ("@ExportToServer", sysConfigReqParams.ExportToServer),
                new SqlParameter ("@CodingMode", sysConfigReqParams.CodingMode),
                new SqlParameter ("@DateFormat", sysConfigReqParams.DateFormat),
                new SqlParameter ("@DescForReport", sysConfigReqParams.DescForReport),
                new SqlParameter ("@ImgStorageLoc", sysConfigReqParams.ImgStorageLoc),
                new SqlParameter ("@ImgType", sysConfigReqParams.ImgType),
                new SqlParameter ("@ImgPath", sysConfigReqParams.ImgPath),
                new SqlParameter ("@IsOfflineMachine", sysConfigReqParams.IsOfflineMachine),
                new SqlParameter ("@ShowAlarmOnStartup", sysConfigReqParams.ShowAlarmOnStartup),
                new SqlParameter ("@AlarmBeforeDays", sysConfigReqParams.AlarmBeforeDays),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update System Configuration Information

        public static DataTable UpdateSysConfigInfo(SysConfigReqParams sysConfigReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", sysConfigReqParams.Update),
                new SqlParameter ("@ID", sysConfigReqParams.ID),
                new SqlParameter ("@FYDate", 1),
                new SqlParameter ("@DepreciationRunType", sysConfigReqParams.DepreciationRunType),
                new SqlParameter ("@DeletePermanent", sysConfigReqParams.DeletePermanent),
                new SqlParameter ("@ExportToServer", sysConfigReqParams.ExportToServer),
                new SqlParameter ("@CodingMode", sysConfigReqParams.CodingMode),
                new SqlParameter ("@DateFormat", sysConfigReqParams.DateFormat),
                new SqlParameter ("@DescForReport", sysConfigReqParams.DescForReport),
                new SqlParameter ("@ImgStorageLoc", sysConfigReqParams.ImgStorageLoc),
                new SqlParameter ("@ImgType", sysConfigReqParams.ImgType),
                new SqlParameter ("@ImgPath", sysConfigReqParams.ImgPath),
                new SqlParameter ("@IsOfflineMachine", sysConfigReqParams.IsOfflineMachine),
                new SqlParameter ("@ShowAlarmOnStartup", sysConfigReqParams.ShowAlarmOnStartup),
                new SqlParameter ("@AlarmBeforeDays", sysConfigReqParams.AlarmBeforeDays),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Purchase Order Logics

        #region PODetails Count

        public static DataTable Count(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region POItemID Result Against POCode

        public static DataTable GetPOItmID(int POCode, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@POCode", POCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Purchase Order & Items

        #region Get Purchase Orders

        public static DataSet GetAllPurchaseOrders(POReqParams POReqParams, string StoreProcedure)
        {
            if (POReqParams.Searching != 0)
            {
                if (POReqParams.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", POReqParams.Searching),
                        new SqlParameter ("@Var", POReqParams.Var),
                        new SqlParameter ("@Dropdown", POReqParams.Dropdown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", POReqParams.Searching),
                        new SqlParameter ("@Var", POReqParams.Var),
                        new SqlParameter ("@PageIndex", POReqParams.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", POReqParams.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
            else
            {
                if (POReqParams.Dropdown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", POReqParams.Get),
                        new SqlParameter ("@Dropdown", POReqParams.Dropdown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", POReqParams.Get),
                        new SqlParameter ("@PageIndex", POReqParams.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", POReqParams.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Get Purchase Order Items Against POCode

        public static DataSet GetAllPOItems(PODetailsReqParams PODetailsReqParams, string StoreProcedure)
        {
            if (PODetailsReqParams.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Searching", PODetailsReqParams.Searching),
                    new SqlParameter ("@Var", PODetailsReqParams.Var),
                    new SqlParameter ("@POCode", PODetailsReqParams.POCode),
                    new SqlParameter ("@PageIndex", PODetailsReqParams.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", PODetailsReqParams.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", PODetailsReqParams.Get),
                    new SqlParameter ("@POCode", PODetailsReqParams.POCode),
                    new SqlParameter ("@PageIndex", PODetailsReqParams.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", PODetailsReqParams.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
        }

        #endregion

        #endregion

        #region Insert Purchase Order & Items

        public static DataTable InsertPOWithItems(POReqParams POReqParams, DataTable POItemDT, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", POReqParams.Add),
                new SqlParameter ("@SuppID", POReqParams.SuppID),
                new SqlParameter ("@PODate", POReqParams.PODate),
                new SqlParameter ("@Quotation", POReqParams.Quotation),
                new SqlParameter ("@Amount", POReqParams.Amount),
                new SqlParameter ("@AddCharges", POReqParams.AddCharges),
                new SqlParameter ("@ModeDelivery", POReqParams.ModeDelivery),
                new SqlParameter ("@PayTerm", POReqParams.PayTerm),
                new SqlParameter ("@Remarks", POReqParams.Remarks),
                new SqlParameter ("@ApprovedBy", POReqParams.ApprovedBy),
                new SqlParameter ("@PreparedBy", POReqParams.PreparedBy),
                new SqlParameter ("@POStatus", POReqParams.POStatus),
                new SqlParameter ("@IsTrans", POReqParams.IsTrans),
                new SqlParameter ("@TermNCon", POReqParams.TermNCon),
                new SqlParameter ("@RequestedBy", POReqParams.RequestedBy),
                new SqlParameter ("@CostID", POReqParams.CostID),
                new SqlParameter ("@RefNo", POReqParams.RefNo),
                new SqlParameter ("@Discount", POReqParams.Discount),
                new SqlParameter ("@poDetailsListToData", POItemDT),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Purchase Order & Items

        public static DataTable UpdatePOWithItems(POReqParams POReqParams, DataTable POItemDT, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", POReqParams.Update),
                new SqlParameter ("@POCode", POReqParams.POCode),
                new SqlParameter ("@SuppID", POReqParams.SuppID),
                new SqlParameter ("@PODate", POReqParams.PODate),
                new SqlParameter ("@Quotation", POReqParams.Quotation),
                new SqlParameter ("@Amount", POReqParams.Amount),
                new SqlParameter ("@AddCharges", POReqParams.AddCharges),
                new SqlParameter ("@ModeDelivery", POReqParams.ModeDelivery),
                new SqlParameter ("@PayTerm", POReqParams.PayTerm),
                new SqlParameter ("@Remarks", POReqParams.Remarks),
                new SqlParameter ("@ApprovedBy", POReqParams.ApprovedBy),
                new SqlParameter ("@PreparedBy", POReqParams.PreparedBy),
                new SqlParameter ("@POStatus", POReqParams.POStatus),
                new SqlParameter ("@IsTrans", POReqParams.IsTrans),
                new SqlParameter ("@TermNCon", POReqParams.TermNCon),
                new SqlParameter ("@RequestedBy", POReqParams.RequestedBy),
                new SqlParameter ("@CostID", POReqParams.CostID),
                new SqlParameter ("@RefNo", POReqParams.RefNo),
                new SqlParameter ("@Discount", POReqParams.Discount),
                new SqlParameter ("@poDetailsListToData", POItemDT),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Purchase Order & Items

        public static DataTable DeletePOWithItems(POReqParams POReqParams, DataTable POItemDT, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", POReqParams.Delete),
                new SqlParameter ("@POCode", POReqParams.POCode),
                new SqlParameter ("@poDetailsListToData", POItemDT),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Pending POs For Approval

        public static DataSet GetPendingPOsForApproval(POReqParams POReqParams, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Get", POReqParams.Get),
            };

            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Purchase Order Approval Cases

        public static DataTable ApprovePOWithItems(POReqParams POReqParams, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", POReqParams.Update),
                new SqlParameter ("@POCode", POReqParams.POCode),
                new SqlParameter ("@ApprovedBy", POReqParams.ApprovedBy),
                new SqlParameter ("@POStatus", POReqParams.POStatus),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Approved POs For Transit

        public static DataSet GetApprovedPOsForTransit(POReqParams POReqParams, string StoreProcedure)
        {

            if (POReqParams.Searching != 0)
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                new SqlParameter ("@Searching", POReqParams.Searching),
                new SqlParameter ("@Var", POReqParams.Var),
                new SqlParameter ("@PageIndex", POReqParams.PaginationParam.PageIndex),
                new SqlParameter ("@PageSize", POReqParams.PaginationParam.PageSize),
            };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
            else
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@Get", POReqParams.Get),
                    new SqlParameter ("@PageIndex", POReqParams.PaginationParam.PageIndex),
                    new SqlParameter ("@PageSize", POReqParams.PaginationParam.PageSize),
                };
                return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
            }
        }

        #endregion

        #region InsertAssetDetailsOfPO

        public static DataTable InsertAssetDetailsOfPO(AssetsInTransit assetsInTransit, string Barcode, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", assetsInTransit.Add),
                new SqlParameter ("@CompanyID", assetsInTransit.CompanyID),
                new SqlParameter ("@ItemCode", assetsInTransit.ItemCode),
                new SqlParameter ("@AstID", assetsInTransit.AstID),
                new SqlParameter ("@RefNo", assetsInTransit.RefNo),
                new SqlParameter ("@AstNum", assetsInTransit.AstNum),
                new SqlParameter ("@BaseCost", assetsInTransit.BaseCost),
                new SqlParameter ("@CustodianID", assetsInTransit.CustodianID),
                new SqlParameter ("@SerialNo", assetsInTransit.SerialNo),
                new SqlParameter ("@POCode", assetsInTransit.POCode),
                new SqlParameter ("@PurDate", assetsInTransit.PurchaseDate),
                new SqlParameter ("@SuppID", assetsInTransit.SuppID),
                new SqlParameter ("@Tax", assetsInTransit.Tax),
                new SqlParameter ("@SrvDate", assetsInTransit.ServiceDate),
                new SqlParameter ("@AstBrandID", assetsInTransit.AstBrandID),
                new SqlParameter ("@AstDesc", assetsInTransit.AstDesc),
                new SqlParameter ("@AstDesc2", assetsInTransit.AstDesc2),
                new SqlParameter ("@AstModel", assetsInTransit.AstModel),
                new SqlParameter ("@Discount", assetsInTransit.Discount),
                new SqlParameter ("@LocID", assetsInTransit.LocID),
                new SqlParameter ("@InvNumber", assetsInTransit.InvNumber),
                new SqlParameter ("@GLCode", assetsInTransit.GLCode),
                new SqlParameter ("@CreatedBy", assetsInTransit.LoginName),
                new SqlParameter ("@LastEditBy", assetsInTransit.LoginName),
                new SqlParameter ("@Barcode", Barcode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Last Asset Number of CompanyID

        public static DataTable UpdateCompanyLastAssetNumber(string CompanyID, string AstNum, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstNum", AstNum),
                new SqlParameter ("@CompanyID", CompanyID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Last Asset Number for Company

        public static DataTable GetCompanyLastAssetNumber(string CompanyID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@CompanyID", CompanyID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Ast Inv Sch Non Closed

        public static DataTable GetAllInvSchsNonClosed(string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        //Kabeer new
        #region Insert In Ast_History

        public static DataTable Insert_Ast_History(string AstID, int status, long invSchCode, string HisDate, string fromLoc, string toLoc, int NoPiece, string AssetStatus, string DeviceID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstID", AstID),
                new SqlParameter ("@Status", status),
                new SqlParameter ("@InvSchCode", invSchCode),
                new SqlParameter ("@HisDate", HisDate),
                new SqlParameter ("@fromLoc", fromLoc),
                new SqlParameter ("@toLoc", toLoc),
                new SqlParameter ("@NoPiece", NoPiece),
                new SqlParameter ("@AssetStatus", AssetStatus),
                new SqlParameter ("@DeviceID", DeviceID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update In Ast_History

        public static DataTable Update_Ast_History(string AstID, int status, long invSchCode, string HisDate, string fromLoc, string toLoc, int NoPiece, string AssetStatus, string DeviceID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstID", AstID),
                new SqlParameter ("@Status", status),
                new SqlParameter ("@InvSchCode", invSchCode),
                new SqlParameter ("@HisDate", HisDate),
                new SqlParameter ("@fromLoc", fromLoc),
                new SqlParameter ("@toLoc", toLoc),
                new SqlParameter ("@NoPiece", NoPiece),
                new SqlParameter ("@AssetStatus", AssetStatus),
                new SqlParameter ("@DeviceID", DeviceID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Depreciation Policy Against Item Code

        public static DataTable GetDepPolicyAgainstItemCode(string ItemCode, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@ItemCode", ItemCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Check Book For AstID

        public static DataTable CheckBookForAstID(int bookid, string astID, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@BookID", bookid),
                new SqlParameter ("@AstID", astID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Add Book For PO Asset

        public static DataTable AddBookForPOAsset(int BookID, string AstID, int DepCode, double salvageValue2, int salvageYear, double LastBookValue, double CurrentBookValue, string BVUpdate, int salvageMonth, double salvageValuePercentage, string StoreProcedure)
        {

            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@BookID", BookID),
                new SqlParameter ("@AstID", AstID),
                new SqlParameter ("@DepCode", DepCode),
                new SqlParameter ("@SalvageValue", salvageValue2),
                new SqlParameter ("@SalvageYear", salvageYear),
                new SqlParameter ("@LastBookValue", LastBookValue),
                new SqlParameter ("@CurrentBookValue", CurrentBookValue),
                new SqlParameter ("@BVUpdate", BVUpdate),
                new SqlParameter ("@SalvageMonth", salvageMonth),
                new SqlParameter ("@SalvageValuePercentage", salvageValuePercentage),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Generate Ast Num

        public static DataTable GenerateAstNum(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Transfer PO Item

        public static DataTable TransferPOItm(string pOItmID, int quantity, string pOCode, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@POItmID", pOItmID),
                new SqlParameter ("@POCode", pOCode),
                new SqlParameter ("@PORecQty", quantity),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Highest RefNO

        public static DataTable GetHighestRefNo(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #endregion

        #region Backend Inventory Logics

        #region Get All Assets Against LocID & InvSchCode

        public static DataTable GetAllAssetsAgainstLocID_InvSchCode(BackendInvReqParams backendInvReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@LocID", backendInvReqParams.LocID),
                new SqlParameter ("@InvSchCode", backendInvReqParams.InvSchCode),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Location_Custody_Transfer Logic 

        //Kabeer new
        public static DataTable GetHighestHistoryID(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        public static DataTable GetBarcodeStructureAgainstAstID(string astid, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@AstID", astid),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable Location_Custody_Transfer(LocCustTransferReqParams locCustTransferReqParams, DataTable dt, DataTable dt2, DataTable dt3, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@LocationCheckbox", locCustTransferReqParams.LocationCheckbox),
                new SqlParameter ("@CustodianCheckbox", locCustTransferReqParams.CustodianCheckbox),
                new SqlParameter ("@AssetStatusCheckbox", locCustTransferReqParams.AssetStatusCheckbox),
                new SqlParameter ("@DataTable", dt),
                new SqlParameter ("@Ast_Cust_His_DataTable", dt2),
                new SqlParameter ("@Ast_AssetStatus_DataTable", dt3),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable ItemCategoryTransfer(DataTable dt, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@ItemCategoryDT", dt),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Warranty Information Logics

        #region Insert Warranty Information

        public static DataTable InsertWarranty(WarrantyReqParams warrantyReqparams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", warrantyReqparams.Add),
                new SqlParameter ("@AstID", warrantyReqparams.AstID),
                new SqlParameter ("@WarrantyStart", warrantyReqparams.WarrantyStart),
                new SqlParameter ("@WarrantyPeriodMonth", warrantyReqparams.WarrantyPeriodMonth),
                new SqlParameter ("@AlarmActivate", warrantyReqparams.AlarmActivate)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update Warranty Information

        public static DataTable UpdateWarranty(WarrantyReqParams warrantyReqparams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", warrantyReqparams.Update),
                new SqlParameter ("@WarrantyID", warrantyReqparams.WarrantyID),
                new SqlParameter ("@WarrantyStart", warrantyReqparams.WarrantyStart),
                new SqlParameter ("@WarrantyPeriodMonth", warrantyReqparams.WarrantyPeriodMonth),
                new SqlParameter ("@AlarmActivate", warrantyReqparams.AlarmActivate)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete Warranty Information

        public static DataTable DeleteWarranty(WarrantyReqParams warrantyReqparams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", warrantyReqparams.Delete),
                new SqlParameter ("@WarrantyID", warrantyReqparams.WarrantyID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Alarm Before Days Value

        public static DataTable GetAlarmBeforeDaysValue(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get All Warranty Assets

        public static DataTable GetAllWarrantyAssets(int AlarmBeforeDays, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@AlarmBeforeDays", AlarmBeforeDays),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region ImportDataWork for all

        public static DataSet GetCustodianData(string Storeprocedure)
        {
            try
            {

                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                { };
                return CGD.DSWithOutParam(Storeprocedure, 1);
            }
            catch (Exception)
            {

                return new DataSet();
            }
        }

        #endregion

        #region Insert Items In Bulk

        public static DataTable InsertItemsInBulk(string TableFlag, int Add, DataTable dt, string Storeprocedure)
        {
            try
            {

                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@TableFlag", TableFlag),
                    new SqlParameter("@Add", Add),
                    new SqlParameter("@UT_"+TableFlag, dt),
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
            }
            catch (Exception)
            {

                return new DataTable();
            }
        }

        #endregion

        #region Standard Report

        public static DataSet StandardReport(ReportReqParams reportReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@PurDate", reportReqParams.PurchaseDate),
                new SqlParameter ("@LocID", reportReqParams.LocID),
                new SqlParameter ("@AstBrandID", reportReqParams.AstBrandID),
                new SqlParameter ("@ItemCode", reportReqParams.ItemCode),
                new SqlParameter ("@BaseCost", reportReqParams.BaseCost),
                new SqlParameter ("@Tax", reportReqParams.Tax),
                new SqlParameter ("@InvStatus", reportReqParams.InvStatus),
                new SqlParameter ("@StatusID", reportReqParams.StatusID),
                new SqlParameter ("@AstCatID", reportReqParams.AstCatID),
                new SqlParameter ("@CustodianID", reportReqParams.CustodianID),
                new SqlParameter ("@SuppID", reportReqParams.SuppID),
                new SqlParameter ("@CompanyID", reportReqParams.CompanyID),
                new SqlParameter ("@DeptID", reportReqParams.DeptID),
                new SqlParameter ("@PageIndex", reportReqParams.paginationParam.PageIndex),
                new SqlParameter ("@PageSize", reportReqParams.paginationParam.PageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataSet DisposedAssetsReport(DisposedAssetsReportReqParams disposedAssetsReportReqParams, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@PurDate", disposedAssetsReportReqParams.PurchaseDate),
                new SqlParameter ("@LocID", disposedAssetsReportReqParams.LocID),
                new SqlParameter ("@AstBrandID", disposedAssetsReportReqParams.AstBrandID),
                new SqlParameter ("@ItemCode", disposedAssetsReportReqParams.ItemCode),
                new SqlParameter ("@BaseCost", disposedAssetsReportReqParams.BaseCost),
                new SqlParameter ("@Tax", disposedAssetsReportReqParams.Tax),
                new SqlParameter ("@InvStatus", disposedAssetsReportReqParams.InvStatus),
                new SqlParameter ("@StatusID", disposedAssetsReportReqParams.StatusID),
                new SqlParameter ("@AstCatID", disposedAssetsReportReqParams.AstCatID),
                new SqlParameter ("@CustodianID", disposedAssetsReportReqParams.CustodianID),
                new SqlParameter ("@SuppID", disposedAssetsReportReqParams.SuppID),
                new SqlParameter ("@CompanyID", disposedAssetsReportReqParams.CompanyID),
                new SqlParameter ("@DeptID", disposedAssetsReportReqParams.DeptID),
                new SqlParameter ("@Disposed", disposedAssetsReportReqParams.Disposed),
                new SqlParameter ("@PageIndex", disposedAssetsReportReqParams.paginationParam.PageIndex),
                new SqlParameter ("@PageSize", disposedAssetsReportReqParams.paginationParam.PageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Audit Report

        #region Get All Audit Reports

        public static DataTable GetAllAuditReportsDD(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Missing Audit Report

        public static DataSet MissingAuditReport(string invSchCodes, string invLocs, int pageIndex, int pageSize, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCodes", invSchCodes),
                new SqlParameter ("@InvLocs", invLocs),
                new SqlParameter ("@PageIndex", pageIndex),
                new SqlParameter ("@PageSize", pageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Found Audit Report

        public static DataSet FoundAuditReport(string invSchCodes, string invLocs, int pageIndex, int pageSize, bool posted, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCodes", invSchCodes),
                new SqlParameter ("@InvLocs", invLocs),
                new SqlParameter ("@Posted", posted),
                new SqlParameter ("@Status", 1),
                new SqlParameter ("@PageIndex", pageIndex),
                new SqlParameter ("@PageSize", pageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Misplaced Audit Report

        public static DataSet MisplacedAuditReport(string invSchCodes, string invLocs, int pageIndex, int pageSize, bool posted, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCodes", invSchCodes),
                new SqlParameter ("@InvLocs", invLocs),
                new SqlParameter ("@Posted", posted),
                new SqlParameter ("@Status", 2),
                new SqlParameter ("@PageIndex", pageIndex),
                new SqlParameter ("@PageSize", pageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Transferred Audit Report

        public static DataSet TransferredAuditReport(string invSchCodes, string invLocs, int pageIndex, int pageSize, bool posted, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCodes", invSchCodes),
                new SqlParameter ("@InvLocs", invLocs),
                new SqlParameter ("@Posted", posted),
                new SqlParameter ("@Status", 3),
                new SqlParameter ("@PageIndex", pageIndex),
                new SqlParameter ("@PageSize", pageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region All Assets Audit Report

        public static DataSet AllAssetsAuditReport(string invSchCodes, string invLocs, int pageIndex, int pageSize, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@InvSchCodes", invSchCodes),
                new SqlParameter ("@InvLocs", invLocs),
                new SqlParameter ("@PageIndex", pageIndex),
                new SqlParameter ("@PageSize", pageSize),
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Barcode Labels Logics

        #region Get All Barcode Labels

        public static DataSet GetAllBarcodeLabels(BarcodeLabelsRequest barcodeLabelsReq, string StoreProcedure)
        {
            if (barcodeLabelsReq.Searching != 0)
            {
                if (barcodeLabelsReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", barcodeLabelsReq.Searching),
                        new SqlParameter ("@DropDown", barcodeLabelsReq.DropDown),
                        new SqlParameter ("@Var", barcodeLabelsReq.Var),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Searching", barcodeLabelsReq.Searching),
                        new SqlParameter ("@Var", barcodeLabelsReq.Var),
                        new SqlParameter ("@PageIndex", barcodeLabelsReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", barcodeLabelsReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
            }
            else
            {
                if (barcodeLabelsReq.DropDown == 1)
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", barcodeLabelsReq.Get),
                        new SqlParameter ("@DropDown", barcodeLabelsReq.DropDown),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }
                else
                {
                    DbReports CGD = new DbReports();
                    SqlParameter[] sqlParameters = {
                        new SqlParameter ("@Get", barcodeLabelsReq.Get),
                        new SqlParameter ("@PageIndex", barcodeLabelsReq.PaginationParam.PageIndex),
                        new SqlParameter ("@PageSize", barcodeLabelsReq.PaginationParam.PageSize),
                    };
                    return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
                }

            }
        }

        #endregion

        #region Insert BarcodeLabels

        public static DataTable InsertBarcodeLabels(BarcodeLabelsRequest barcodeLabelsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Add", barcodeLabelsReq.Add),
                new SqlParameter ("@LabelID", barcodeLabelsReq.LabelID),
                new SqlParameter ("@LabelName", barcodeLabelsReq.LabelName),
                new SqlParameter ("@LabelDesign", barcodeLabelsReq.LabelDesign),
                new SqlParameter ("@CreatedBy", barcodeLabelsReq.LoginName),
                new SqlParameter ("@UpdatedBy", barcodeLabelsReq.LoginName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Update BarcodeLabels

        public static DataTable UpdateBarcodeLabels(BarcodeLabelsRequest barcodeLabelsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Update", barcodeLabelsReq.Update),
                new SqlParameter ("@LabelID", barcodeLabelsReq.LabelID),
                new SqlParameter ("@LabelName", barcodeLabelsReq.LabelName),
                new SqlParameter ("@LabelDesign", barcodeLabelsReq.LabelDesign),
                new SqlParameter ("@UpdatedBy", barcodeLabelsReq.LoginName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Delete BarcodeLabels

        public static DataTable DeleteBarcodeLabels(BarcodeLabelsRequest barcodeLabelsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@Delete", barcodeLabelsReq.Delete),
                new SqlParameter ("@LabelID", barcodeLabelsReq.LabelID),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get Label Design For Printing

        public static DataTable GetLabelDesignForPrinting(BarcodeLabelsRequest barcodeLabelsReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                new SqlParameter ("@LabelName", barcodeLabelsReq.LabelName),
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

    }

}