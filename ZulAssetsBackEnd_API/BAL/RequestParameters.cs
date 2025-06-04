using System.Data;
using System.Numerics;
using System.Text.Json.Nodes;

namespace ZulAssetsBackEnd_API.BAL
{
    public class RequestParameters
    {

        #region Pagination Parameters
        public class PaginationParam
        {
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 0;
        }
        #endregion

        #region DeviceRegistrationParam

        public class DeviceReq
        {
            public string DeviceID { get; set; }
        }

        public class DeviceReg
        {
            public int Get { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string Var { get; set; } = "";
            public int NewDeviceFlag { get; set; }
            public string DeviceDesc { get; set; } = "";
            public string DeviceSerialNo { get; set; } = "";
            public string DeviceID { get; set; } = "";
            public string LoginName { get; set; } = "";
            public string? LicKey { get; set; } = "";
            public string? DevicePhNo { get; set; }
            public string? DeviceSrNo { get; set; }
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #region Data Processing Get

        public class DataProcessingReqParam
        {
            public int AuditData { get; set; } = 0;
            public int Get { get; set; } = 0;
            public int Add { get; set; } = 0;
            public int Delete { get; set; } = 0;
            public string NonBCode { get; set; } = "";
            public string DeviceID { get; set; } = "";
            public string TransDate { get; set; } = "";
            public string InvSchCode { get; set; } = "";
            public PaginationParam paginationParam { get; set; }
            public List<AuditProcessingDataTree> auditProcessingDataTree { get; set; } = new List<AuditProcessingDataTree> { };
            public List<AnonymousProcessingDataTree> anonymousProcessingDataTree { get; set; } = new List<AnonymousProcessingDataTree> { };
            public List<ProcessAuditDataTree> processAuditDataTree { get; set; } = new List<ProcessAuditDataTree> { };

        }

        public class DeleteProcessingReqParam
        {
            public int AuditData { get; set; } = 0;
            public int Get { get; set; } = 0;
            public int Add { get; set; } = 0;
            public int Delete { get; set; } = 0;
            public string NonBCode { get; set; } = "";
            public string DeviceID { get; set; } = "";
            public string TransDate { get; set; } = "";
            public string InvSchCode { get; set; } = "";
            public List<AuditProcessingDataTree> auditProcessingDataTree { get; set; } = new List<AuditProcessingDataTree> { };
            public List<AnonymousProcessingDataTree> anonymousProcessingDataTree { get; set; } = new List<AnonymousProcessingDataTree> { };
            public List<ProcessAuditDataTree> processAuditDataTree { get; set; } = new List<ProcessAuditDataTree> { };

        }

        public class AuditProcessingDataTree
        {
            public string AstID { get; set; } = "";
            public string DeviceID { get; set; } = "";
            public string InvSchCode { get; set; } = "";
        }

        public class AnonymousProcessingDataTree
        {
            public string NonBCode { get; set; } = "";
            public string DeviceID { get; set; } = "";
            public string TransDate { get; set; } = "";
        }



        #endregion

        #region Process Audit Data

        public class ProcessAuditData
        {
            //Kabeer new
            public int InvSchCode { get; set; } = 0;
            public string LoginName { get; set; } = "";
            public List<ProcessAuditDataTree> processAuditDataTree { get; set; } = new List<ProcessAuditDataTree> { };
            //public List<LocTransferTree> Ast_HistoryTable { get; set; } = new List<LocTransferTree> { };

        }

        public class ProcessAuditDataTree
        {
            public string AstID { get; set; } = "";
            public string AstDesc { get; set; } = "";
            public int Status { get; set; } = 0;
            public string LocID { get; set; } = "";
            public string AssetStatus { get; set; } = "";
            public int NoPiece { get; set; } = 0;
            public int AstTransID { get; set; } = 0;
            public string DeviceID { get; set; } = "";
            public string CurrLoc { get; set; } = "";
        }

        #endregion

        #endregion

        #region User Parameters

        #region Loginparam
        public class Loginparam
        {
            public string LoginName { get; set; }
            public string Password { get; set; }

        }
        #endregion

        #region GeneratePasswordTokenParameters

        public class GeneratePasswordTokenParam
        {
            public string LoginName { get; set; }

        }

        #endregion

        #region ChangePassword
        public class ChangePassword
        {
            public string LoginName { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string TransactionUser { get; set; }
        }
        #endregion

        #region ForgotPassword
        public class ForgotPassword
        {
            public string LoginName { get; set; }
            public string token { get; set; }
            public string NewPassword { get; set; }
        }

        #endregion

        #region RefreshTokenRequest

        public class RefreshTokenRequest
        {
            public string JWTToken { get; set; }
            public string RefreshToken { get; set; }
        }

        #endregion

        #region User

        public class User
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int Dropdown { get; set; }
            public string LoginName { get; set; } = "";
            public string UserName { get; set; } = "";
            public string Password { get; set; } = "";
            public string UserAccess { get; set; } = "";
            public string RoleID { get; set; } = "";
            public string Var { get; set; } = "";
            public string TransactionUser { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #endregion

        #region Assets Parameters

        #region Asset Tracking

        public class AssetTrackingRequest
        {
            public string Barcode { get; set; }
        }

        #endregion

        #region Anonymous Assets

        public class AnonymousAssetsRequests
        {
            public int ID { get; set; }
            public string DeviceID { get; set; }
            public string LocID { get; set; }
            public string AssetDescription { get; set; }
            public string CatID { get; set; }
        }

        #endregion

        #region Assets Coding Definition

        public class AstCodingDefReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string AssetCodingID { get; set; } = "";
            public int CompanyID { get; set; } = 0;
            public int StartSerial { get; set; } = 0;
            public int EndSerial { get; set; } = 0;
            public bool Status { get; set; } = false;
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }
        #endregion

        #region Asset Item

        public class AssetItemReq
        {

            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string ItemCode { get; set; } = "";
            public string AstCatID { get; set; } = "";
            public string AstDesc { get; set; } = "";
            public int Warranty { get; set; } = 0;
            public string ImageBase64 { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #region Update Asset Location

        public class UpdateAssetLocation
        {
            public string Barcode { get; set; }
            public string LocID { get; set; }
            public string DeviceID { get; set; }
            public string InventoryDate { get; set; }
            public string LastEditDate { get; set; }
            public string LastEditBy { get; set; }
            public string Status { get; set; }
            public string AssetStatus { get; set; }
        }

        #endregion

        #region Asset Status Update

        public class UpdateAssetStatus
        {
            public string Barcode { get; set; }
            public string AssetStatus { get; set; }
        }
        #endregion

        #region Details & Maintenance

        public class AstInformationReqParam
        {
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public string AstID { get; set; } = "";
            public int DispCode { get; set; }
            public string ItemCode { get; set; } = "";
            public string SuppID { get; set; } = "";
            public long POCode { get; set; }
            public string InvNumber { get; set; } = "";
            public string CustodianID { get; set; } = "";
            public double BaseCost { get; set; } = 0.00;
            public double Tax { get; set; } = 0.00;
            public string AstNum { get; set; } = "";
            public string RefNum { get; set; } = "";
            public string SrvDate { get; set; } = "";
            public string PurDate { get; set; } = "";
            public bool Disposed { get; set; } = false;
            public string OldAssetID { get; set; } = "";
            public string DispDate { get; set; } = "";
            public long InvSchCode { get; set; }
            public string BookID { get; set; } = "";
            public int InsID { get; set; } = 0;
            public string LocID { get; set; } = "";
            public int InvStatus { get; set; } = 0;
            public bool IsSold { get; set; } = false;
            public string Sel_Date { get; set; } = "";
            public double Sel_Price { get; set; } = 0.00;
            public string SoldTo { get; set; } = "";
            public int AstBrandId { get; set; } = 0;
            public string AstDesc { get; set; } = "";
            public string AstModel { get; set; } = "";
            public int CompanyID { get; set; } = 0;
            public string TransRemarks { get; set; } = "";
            public int LabelCount { get; set; } = 0;
            public long Discount { get; set; }
            public int NoPiece { get; set; } = 0;
            public string BarCode { get; set; } = "";
            public string SerialNo { get; set; } = "";
            public string RefCode { get; set; } = "";
            public string Plate { get; set; } = "";
            public string POERP { get; set; } = "";
            public string Capex { get; set; } = "";
            public string Grn { get; set; } = "";
            public string GLCode { get; set; } = "";
            public string PONumber { get; set; } = "";
            public string AstDesc2 { get; set; } = "";
            public string CapitalizationDate { get; set; } = "";
            public string BussinessArea { get; set; } = "";
            public string InventoryNumber { get; set; } = "";
            public string CostCenterID { get; set; } = "";
            public bool InStockAsset { get; set; } = false;
            public string EvaluationGroup1 { get; set; } = "";
            public string EvaluationGroup2 { get; set; } = "";
            public string EvaluationGroup3 { get; set; } = "";
            public string EvaluationGroup4 { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public bool IsDataChanged { get; set; } = false;
            public string LastInventoryDate { get; set; } = "";
            public string LastEditDate { get; set; } = "";
            public string CreationDate { get; set; } = "";
            public string LastEditBy { get; set; } = "";
            public string CustomFld1 { get; set; } = "";
            public string CustomFld2 { get; set; } = "";
            public string CustomFld3 { get; set; } = "";
            public string CustomFld4 { get; set; } = "";
            public string CustomFld5 { get; set; } = "";
            public int Warranty { get; set; } = 0;
            public int StatusID { get; set; } = 0;
            public string DisposalComments { get; set; } = "";
            public string ImageBase64 { get; set; } = "";
            public string LoginName { get; set; } = "";

        }

        #endregion

        #region Assets Adminsitrator

        public class AssetAdministrator
        {
            public int Get { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string Var { get; set; } = "";
            public string LocID { get; set; } = "";
            public string AstCatID { get; set; } = "";
            public string CustodianID { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
            public string LoginName { get; set; } = "";
        }
        #endregion

        #region Search Assets Parameters 

        public class SearchAstsParams
        {
            public int Get { get; set; }
            public string AstID { get; set; } = "";
            public string AstNum { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string AstDesc { get; set; } = "";
            public string SerialNo { get; set; } = "";
            public string AstBrandID { get; set; } = "";
            public string OrgHierID { get; set; } = "";
            public string CustID { get; set; } = "";
            public string LocID { get; set; } = "";
            public string AstCatID { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #region Location/Custody Transfer Parameters

        public class LocCustTransferReqParams
        {
            public int Update { get; set; }
            public int LocationCheckbox { get; set; } = 0;
            public int CustodianCheckbox { get; set; } = 0;
            public int AssetStatusCheckbox { get; set; } = 0;
            public List<LocTransferTree> locTransferTree { get; set; } = new List<LocTransferTree> { };
            public List<CustTransferTree> custTransferTree { get; set; } = new List<CustTransferTree> { };
            public List<AssetStatusTransferTree> assetStatusTransferTree { get; set; } = new List<AssetStatusTransferTree> { };
            public string LoginName { get; set; } = "";
        }
        public class LocTransferTree
        {
            public int HistoryID { get; set; } = 0;
            public int InvSchCode { get; set; } = 0;
            public string AstID { get; set; } = "";
            public string Fr_Loc { get; set; } = "";
            public string To_Loc { get; set; } = "";
            public string HisDate { get; set; } = "";
            public int Status { get; set; } = 0;
            public string AssetStatus { get; set; } = "";
            public string LastEditBy { get; set; } = "";
            public string Barcode { get; set; } = "";
        }

        public class CustTransferTree
        {
            public int HistoryID { get; set; } = 0;
            public string AstID { get; set; } = "";
            public string ToCustodian { get; set; } = "";
            public string FromCustodian { get; set; } = "";
            public string HisDate { get; set; } = "";
            public string LastEditBy { get; set; } = "";
        }

        public class AssetStatusTransferTree
        {
            public string AstID { get; set; } = "";
            public int Status { get; set; } = 0;
            public string AssetStatus { get; set; } = "";
            public string LastEditBy { get; set; } = "";
        }

        public class ItemCategoryTransferReqParams
        {
            public List<ItemCategoryTransferTree> itemCategoryTransferTrees { get; set; } = new List<ItemCategoryTransferTree> { };
            public string LoginName { get; set; } = "";
        }

        public class ItemCategoryTransferTree
        {
            public string ItemCode { get; set; } = "";
            public string NewCatID { get; set; } = "";
        }

        #endregion

        #region Depreciation Policy Parameters

        public class UpdateDepreciationOnAsset
        {
            public string AstID { get; set; } = "";
            public double SalvageValue { get; set; } = 0.00;
            public string SalvageYear { get; set; } = "";
            public string SalvageYearPrevious { get; set; } = "";
            public string SalvageMonth { get; set; } = "";
            public string SalvageMonthPrevious { get; set; } = "";
            public string SalvagePercent { get; set; } = "";
            public double SalvageValuePercent { get; set; } = 0.00;
            public bool IsSalvageValuePercentage { get; set; } = false;
            public double TotalCost { get; set; } = 0.00;
            public string BVUpdate { get; set; } = "";
            public string BookDescription { get; set; } = "";
            public bool btnSaveDep { get; set; }
            public double CurrentBV { get; set; } = 0.00;
            public double CurrentBVPrevious { get; set; } = 0.00;
            public string DepText { get; set; } = "";
            public int BookID { get; set; }
            public string LoginName { get; set; } = "";

        }

        public class DepPolicy_History
        {
            public int BookID { get; set; }
            public string AstID { get; set; } = "";
            public int DepCode { get; set; } = 0;
            public double SalvageValue { get; set; } = 0;
            public double SalvageYear { get; set; } = 0;
            public double CurrentBV { get; set; } = 0;
            public DateTime BVUpdate { get; set; }
            public int SalvageMonth { get; set; } = 0;
            public int SalvagePercent { get; set; } = 0;
        }

        #endregion

        #endregion

        #region Location Parameters

        public class LocationRequest
        {
            public string LocID { get; set; }
            public int From { get; set; }
            public int To { get; set; }
        }

        public class LocationTree
        {
            //public List<tree> tree { get; set; } = new List<tree> { };
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Get { get; set; }
            public string LocCode { get; set; } = "";
            public string LocDesc { get; set; } = "";
            public string ParentId { get; set; } = "";
            public string ParentId2 { get; set; } = "";
            public int CompanyId { get; set; } = 0;
            public string LocId { get; set; } = "";
            public string LoginName { get; set; } = "";

        }

        #endregion

        #region Category Parameters

        public class CategoryTree
        {
            //public List<tree> tree { get; set; } = new List<tree> { };
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Get { get; set; }
            public string CatCode { get; set; } = "";
            public string CatDesc { get; set; } = "";
            public string ParentId { get; set; } = "";
            public string ParentId2 { get; set; } = "";
            public string CatId { get; set; } = "";
            public string LoginName { get; set; } = "";

        }

        #endregion

        #region Supplier Parameters

        public class SupplierRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int Dropdown { get; set; }
            public string SuppID { get; set; } = "";
            public string SuppName { get; set; } = "";
            public string SuppCell { get; set; } = "";
            public string SuppFax { get; set; } = "";
            public string SuppPhone { get; set; } = "";
            public string SuppEmail { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Designation Parameters

        public class DesignationRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public int DesignationID { get; set; } = 0;
            public string Description { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Cost Center Parameters

        public class CostCenterRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int DropDown { get; set; }
            public int Searching { get; set; }
            public int CostCenterID { get; set; } = 0;
            public string CostNumber { get; set; } = "";
            public string CostName { get; set; } = "";
            public int CompanyId { get; set; } = 0;
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Menu Parameters

        public class Menus
        {
            public string? RoleId { get; set; } = "";
        }

        public class MenuOptions
        {
            public string? RoleId { get; set; } = "";
            public string? MenuId { get; set; } = "";
        }

        #endregion

        #region Roles Parameters

        public class RoleRequest
        {
            public int Get { get; set; }
            public int GetByID { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string RoleID { get; set; } = "";
            public string Description { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";

            public List<RoleCompanies> roleCompanies_list { get; set; } = new List<RoleCompanies> { };
            public List<RoleAssignOptions> roleAssignOptions_list { get; set; } = new List<RoleAssignOptions> { };
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        public class RoleCompanies
        {
            public string Companies { get; set; } = "";
        }
        #endregion

        #region GLCode Parameters

        public class GLCodeRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string GLCode { get; set; } = "";
            public string GLDesc { get; set; } = "";
            public int CompanyId { get; set; } = 0;
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Disposal Methods Parameters

        public class DisposalMethodsRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string DispCode { get; set; } = "";
            public string DispDesc { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Brands Parameters

        public class BrandRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string AstBrandID { get; set; } = "";
            public string AstBrandName { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Depreciation Parameters

        #region Depreciation Method Parameters

        public class DepreciationMethodsRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int DropDown { get; set; }
            public int Searching { get; set; }
            public string DepCode { get; set; } = "";
            public string DepDesc { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Deprecattion Policy Parameters

        public class DepPolicyReqParams
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public string AstCatID { get; set; } = "";
            public string CatDepID { get; set; } = "";
            public double SalvageValue { get; set; } = 0.00;
            public string SalvageYear { get; set; } = "";
            public string SalvageMonth { get; set; } = "";
            public string SalvagePercent { get; set; } = "";
            public string SalvageValuePercent { get; set; } = "";
            public bool IsSalvageValuePercentage { get; set; } = false;
            public string LoginName { get; set; } = "";

        }

        #endregion

        #region Depreciation Engine Parameters

        public class DeprecitionEngineReqParams
        {
            public int Get { get; set; }
            public int Update { get; set; }
            public int Searching { get; set; }
            public string Var { get; set; } = "";
            public int CompanyID { get; set; } = 0;
            public string UpdateBookTillDate { get; set; } = "";
            public string LoginName { get; set; } = "";
            public List<DepEngTree> depEngTree { get; set; } = new List<DepEngTree> { };
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        public class DepEngTree
        {
            public string BookIDs { get; set; }
        }

        #endregion

        #endregion

        #region Company Parameters

        public class CompReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string CompanyId { get; set; } = "";
            public string CompanyCode { get; set; } = "";
            public string CompanyName { get; set; } = "";
            public int BarCodeStrucId { get; set; } = 0;
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        public class CompInfoReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string ID { get; set; } = "";
            public string Name { get; set; } = "";
            public string Address { get; set; } = "";
            public string State { get; set; } = "";
            public string PCode { get; set; } = "";
            public string City { get; set; } = "";
            public string Country { get; set; } = "";
            public string PhoneNo { get; set; } = "";
            public string Fax { get; set; } = "";
            public string Email { get; set; } = "";
            public string ImageToBase64 { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
        }

        public class InterCompanyTransferReqParams
        {
            public int Get { get; set; }
            public string AstID { get; set; } = "";
            public string NewCompanyID { get; set; } = "";
            public string OldCompanyID { get; set; } = "";
            public string GLCodes { get; set; } = "";
            public string RefNo { get; set; } = "";
            public double CurrentBV { get; set; } = 0.00;
            public int SalYr { get; set; } = 0;
            public float SalValue { get; set; } = 0;
            public string TransRemarks { get; set; } = "";
            public string ImageToBase64 { get; set; } = "";
        }

        #endregion

        #region Insurer Parameters

        public class InsurerReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public string InsCode { get; set; } = "";
            public string InsName { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Inventory Schedule Parameters

        public class InvSchReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int Dropdown { get; set; }
            public string InvSchCode { get; set; } = "";
            public string InvDesc { get; set; } = "";
            public DateTime InvStartDate { get; set; } = DateTime.Now;
            public DateTime InvEndDate { get; set; } = DateTime.Now;
            public bool Closed { get; set; } = false;
            public bool SchType { get; set; } = false;
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
            public List<LocTree> locTrees { get; set; } = new List<LocTree> { };
            public List<DeviceTree> deviceTrees { get; set; } = new List<DeviceTree> { };
            //public List<RoleAssignOptions> roleAssignOptions_list { get; set; } = new List<RoleAssignOptions> { };
        }

        public class LocTree
        {
            public string LocID { get; set; }
        }

        public class DeviceTree
        {
            public string DeviceHardwareID { get; set; }
        }

        #endregion

        #region Units Parameters

        public class UnitRequestParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int Dropdown { get; set; }
            public string UnitID { get; set; } = "";
            public string UnitDesc { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

        #region Adddress Templates Parameters

        public class AddTempReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string AddressID { get; set; } = "";
            public string AddressDesc { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #region TreeViewParam

        public class tree
        {
            public string id { get; set; }
            public string text { get; set; }
            public string parent { get; set; }
        }

        #endregion

        #region Role Assign Options Tree

        public class RoleAssignOptions
        {
            public int RoleId { get; set; }
            public int OptionId { get; set; }
            public int MenuId { get; set; }
            public int Value { get; set; }
        }

        #endregion

        #region RoleViewParam
        public class role
        {
            public string id { get; set; }
        }

        #endregion

        #region LevelsParams

        public class LevelsParams
        {

            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string LvlID { get; set; } = "";
            public string LvlDesc { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #region ServerSideSearchingParams

        public class ServerSideSearchingParams
        {
            public string TableName { get; set; }
            public string Var { get; set; }
        }

        #endregion

        #region OrganizationHierParams

        public class OrgHierReqParams
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public string OrgHierID { get; set; } = "";
            public string OrgHierName { get; set; } = "";
            public string LvlId { get; set; } = "";
            public string LvlCode { get; set; } = "";
            public string CompLvlCode { get; set; } = "";
            public string ParentId { get; set; } = "";
            public string LoginName { get; set; } = "";
        }

        #endregion

        #region Custodian Parameters

        public class CustodianReqParams
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int DropDown { get; set; }
            public int Searching { get; set; }
            public string CustodianID { get; set; } = "";
            public string CustodianName { get; set; } = "";
            public string CustodianCode { get; set; } = "";
            public string CustodianPhone { get; set; } = "";
            public string CustodianEmail { get; set; } = "";
            public string CustodianFax { get; set; } = "";
            public string CustodianCell { get; set; } = "";
            public string CustodianAddress { get; set; } = "";
            public string OrgHierID { get; set; } = "";
            public string DesignationID { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        #endregion

        #region AdditionalCostTypeParams

        public class AddCostTypeReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Searching { get; set; }
            public string Var { get; set; } = "";
            public string TypeID { get; set; } = "";
            public string TypeDesc { get; set; } = "";
            public string AstID { get; set; } = "";
            public float AddCost { get; set; }
            public string LoginName { get; set; } = "";

        }

        #endregion

        #region BooksParam

        public class BooksReqParam
        {
            public int Add { get; set; }
            public int Get { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int DropDown { get; set; }
            public int Searching { get; set; }
            public string Var { get; set; } = "";
            public string Description { get; set; } = "";
            public string BookID { get; set; } = "";
            public string DepCode { get; set; } = "";
            public string CompanyID { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();

        }

        public class AstBookReqParams
        {
            public string BookDescription { get; set; } = "";
            public int BookID { get; set; }
            public string AstID { get; set; } = "";
            public int DepCode { get; set; } = 0;
            public double SalvageValue { get; set; } = 0;
            public double SalvageYear { get; set; } = 0;
            public double LastBV { get; set; } = 0;
            public double CurrentBV { get; set; } = 0;
            public DateTime BVUpdate { get; set; }
            public int SalvageMonth { get; set; } = 0;
            public double SalvagePercent { get; set; } = 0;
        }

        #endregion

        #region Barcodeing Parameters

        #region Barcode Structure Columns Params

        public class BarcodeStructCol
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string Var { get; set; } = "";
            public int ID { get; set; } = 0;
            public string Name { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }
        #endregion

        #region Barcode Structures

        public class BarcodeStructureReqParam
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int BarStructID { get; set; } = 0;
            public string BarStructDesc { get; set; } = "";
            public int BarStructLength { get; set; } = 0;
            public string BarStructPrefix { get; set; } = "";
            public string ValueSep { get; set; } = "";
            public string Barcode { get; set; } = "";
            public string LoginName { get; set; } = "";
        }

        #endregion

        #region Applying Barcode Policy

        public class Barcode_AssignCompany
        {
            public int CompanyID { get; set; }
            public int BarcodeStructureID { get; set; }
            public string LoginName { get; set; }
        }

        #endregion

        #region Applying Barcode Policy on Asset Items

        public class BarcodePolicyOnAssetItems
        {
            public int BarcodeStructureID { get; set; }
            public string LoginName { get; set; }
            public List<ItemCodeTrees> itemCodeTree { get; set; } = new List<ItemCodeTrees>();

        }

        public class ItemCodeTrees
        {
            public string ItemCode { get; set; }
        }

        #endregion

        #endregion

        #region System Configuration Params

        public class SysConfigReqParams
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int ID { get; set; } = 0;
            public string FYDate { get; set; } = "";
            public int DepreciationRunType { get; set; } = 0;
            public int DeletePermanent { get; set; } = 0;
            public bool ExportToServer { get; set; } = false;
            public bool CodingMode { get; set; } = false;
            public string DateFormat { get; set; } = "";
            public string DescForReport { get; set; } = "";
            public string ImgStorageLoc { get; set; } = "";
            public string ImgType { get; set; } = "";
            public string ImgPath { get; set; } = "";
            public bool IsOfflineMachine { get; set; } = false;
            public bool ShowAlarmOnStartup { get; set; } = false;
            public int AlarmBeforeDays { get; set; } = 0;
            public string LoginName { get; set; } = "";
            public bool DescForLabelPrinting { get; set; } = false;

        }

        #endregion

        #region Purchase Orders & Details Parameters

        public class POReqParams
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int Dropdown { get; set; }
            public string Var { get; set; } = "";
            public int POCode { get; set; } = 0;
            public string SuppID { get; set; } = "";
            public string PODate { get; set; } = "";
            public string Quotation { get; set; } = "";
            public double Amount { get; set; } = 0.00;
            public double AddCharges { get; set; } = 0.00;
            public string ModeDelivery { get; set; } = "";
            public string PayTerm { get; set; } = "";
            public string Remarks { get; set; } = "";
            public string ApprovedBy { get; set; } = "";
            public string PreparedBy { get; set; } = "";
            public int POStatus { get; set; } = 0;
            public bool IsTrans { get; set; } = false;
            public string TermNCon { get; set; } = "";
            public string RequestedBy { get; set; } = "";
            public string CostID { get; set; } = "";
            public string RefNo { get; set; } = "";
            public int Discount { get; set; } = 0;
            public List<POItemDetailsList> poItemDetailsList { get; set; } = new List<POItemDetailsList> { };
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        public class PODetailsReqParams
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public string Var { get; set; } = "";
            public string POCode { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        public class POItemDetailsList
        {
            public int POItmID { get; set; } = 0;
            public int POCode { get; set; } = 0;
            public string ItemCode { get; set; } = "";
            public string POItmDesc { get; set; } = "";
            public double POItmBaseCode { get; set; } = 0.00;
            public double AddCharges { get; set; } = 0.00;
            public int POItmQty { get; set; } = 0;
            public bool IsTrans { get; set; } = false;
            public int UnitID { get; set; } = 0;
            public int PORecQty { get; set; } = 0;
        }

        public class AssetsInTransit
        {
            public int Add { get; set; }
            public string CompanyID { get; set; }
            public int ItemCode { get; set; }
            public string AstID { get; set; } = "";
            public string RefNo { get; set; } = "";
            public string Quantity { get; set; } = "";
            public double BaseCost { get; set; } = 0.00;
            public int CustodianID { get; set; } = 0;
            public string SerialNo { get; set; } = "";
            public string POCode { get; set; } = "";
            public string POItmID { get; set; } = "";
            public string PurchaseDate { get; set; } = "";
            public string SuppID { get; set; } = "";
            public double Tax { get; set; } = 0.00;
            public string GLCode { get; set; } = "";
            public string ServiceDate { get; set; } = "";
            public int AstBrandID { get; set; } = 0;
            public string AstDesc { get; set; } = "";
            public string AstDesc2 { get; set; } = "";
            public string AstModel { get; set; } = "";
            public string Discount { get; set; } = "";
            public string LocID { get; set; } = "";
            public string AstNum { get; set; } = "";
            public string InvNumber { get; set; } = "";
            public string LoginName { get; set; } = "";
        }

        #endregion

        #region Backend Inventory

        public class BackendInvReqParams
        {
            public string LocID { get; set; } = "";
            public string InvSchCode { get; set; } = "";
        }
        #endregion

        #region Warranty Req Parameters

        public class WarrantyReqParams
        {
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int WarrantyID { get; set; } = 0;
            public string AstID { get; set; } = "";
            public string WarrantyStart { get; set; } = "";
            public int WarrantyPeriodMonth { get; set; } = 0;
            //public int AlarmBeforeDays { get; set; } = 0;
            public bool AlarmActivate { get; set; } = true;
            public string LoginName { get; set; } = "";
        }

        #endregion

        #region Import Data Request Parameters
        public class ImportDataReqParams
        {
            public List<ImportParams> importData { get; set; } = new List<ImportParams> { };
        }

        public class ImportParams
        {
            //  public string ItemCode { get; set; } = "";

            public string CustodianName { get; set; } = "";
            public string CustodianCode { get; set; } = "";
            public string CustodianPosition { get; set; }
            public string Brand { get; set; }
            public string Department { get; set; }
            public string VendorAccountNumber { get; set; }
            public string VendorName { get; set; }
            public string CC { get; set; }
            public string CCDescriptionLocation { get; set; }
            public string MainLocationCode { get; set; }
            public string MainLocation { get; set; }
            public string LOCATIONCode { get; set; }
            public string LOCATION { get; set; }
            public string SublocationCode { get; set; }
            public string SublocationName { get; set; }
            public string Serial { get; set; }
            public string DescriptionEnglish { get; set; }
            public string BatchNo { get; set; }
            public string AstDesc { get; set; }
            public string PricePerPCs { get; set; }
            public string Category { get; set; }
            public string CategoryName { get; set; }
            public string SubCategory { get; set; }
            public string ServiceDate { get; set; }
            public string SalvageYear { get; set; }
            public string Quantity { get; set; }
            public string ElectronicSerialNumber { get; set; }
            public string CompanyName { get; set; }

        }

        #endregion

        #region Reports

        #region Standard Report

        public class ReportReqParams
        {
            public string PurchaseDate { get; set; } = "";
            public string LocID { get; set; } = "";
            public string AstBrandID { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string BaseCost { get; set; } = "";
            public string Tax { get; set; } = "";
            public string InvStatus { get; set; } = "";
            public string StatusID { get; set; } = "";
            public string AstCatID { get; set; } = "";
            public string CustodianID { get; set; } = "";
            public string SuppID { get; set; } = "";
            public string CompanyID { get; set; } = "";
            public string DeptID { get; set; } = "";
            public PaginationParam paginationParam { get; set; }

        }

        #endregion

        #region Disposed Assets Report

        public class DisposedAssetsReportReqParams
        {
            public string PurchaseDate { get; set; } = "";
            public string LocID { get; set; } = "";
            public string AstBrandID { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string BaseCost { get; set; } = "";
            public string Tax { get; set; } = "";
            public string InvStatus { get; set; } = "";
            public string StatusID { get; set; } = "";
            public string AstCatID { get; set; } = "";
            public string CustodianID { get; set; } = "";
            public string SuppID { get; set; } = "";
            public string CompanyID { get; set; } = "";
            public string DeptID { get; set; } = "";
            public string Disposed { get; set; } = "";
            public PaginationParam paginationParam { get; set; }

        }

        #endregion

        #region Audit Reports

        #region All Assets & Missing Report Params

        public class ReportAllAssetsReqParams
        {
            public List<ReportAllAssetsAuditTree> rptAllAstsAuditTree { get; set; } = new List<ReportAllAssetsAuditTree> { };
            public PaginationParam paginationParam { get; set; }
        }

        #endregion

        #region Report Found Misplaced Transferred

        public class ReportFoundMisplacedTransferredAssetsReqParams
        {
            public List<ReportAllAssetsAuditTree> rptAllAstsAuditTree { get; set; } = new List<ReportAllAssetsAuditTree> { };
            public bool Posted { get; set; }
            public PaginationParam paginationParam { get; set; }
        }

        #endregion

        #region General Parameters for Reports

        public class ReportAllAssetsAuditTree
        {
            public string InvSchCode { get; set; }
            public List<InvLoc> InvLoc { get; set; } = new List<InvLoc>();
        }

        public class InvLoc
        {
            public string LocID { get; set; }
        }

        #endregion

        #endregion

        #region Extended Reports

        #region General Parameters for Reports

        public class BulkSelectionParameters
        {
            public List<Custodian> Custodian { get; set; } = new List<Custodian>();
        }

        public class Custodian
        {
            public string CustodianID { get; set; }
        }

        #endregion

        #region Quarterly Report parameters

        public class QuarterlyReportRequestParams
        {
            public string? LoginName { get; set; } = "";
            public int? LocID { get; set; }
            public int? AssetsCountColumn { get; set; } = 0;
            public int? AssetFoundColumn { get; set; } = 0;
            public int? AssetTransferredColumn { get; set; } = 0;
            public int? AssetMissingColumn { get; set; } = 0;
            public int? AssetFoundPercentage { get; set; } = 0;
            public string? Year { get; set; } = "";
            public string? Quarterly { get; set; } = "";

        }
        #endregion

        #endregion

        #endregion

        #region Rights Params 

        public class RightsParams
        {
            public int? RoleID { get; set; }
            public int? MenuID { get; set; }
            public string? Flag { get; set; }
        }

        #endregion

        #region Barcode Labels Parameters

        public class BarcodeLabelsRequest
        {
            public int Get { get; set; }
            public int Add { get; set; }
            public int Update { get; set; }
            public int Delete { get; set; }
            public int Searching { get; set; }
            public int DropDown { get; set; }
            public int? LabelID { get; set; }
            public string LabelName { get; set; } = "";
            public string LabelDesign { get; set; } = "";
            public string Var { get; set; } = "";
            public string LoginName { get; set; } = "";
            public PaginationParam PaginationParam { get; set; } = new PaginationParam();
        }

        #endregion

    }
}
