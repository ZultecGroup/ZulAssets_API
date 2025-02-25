using System.Data;

namespace ZulAssetsBackEnd_API.BAL
{
    public class ResponseParameters
    {

        #region ResponseMsg

        public class Message
        {
            public string message { get; set; }
            public string status { get; set; }
        }

        #endregion

        #region LoginResponse

        public class LoginRes
        {
            public string LoginName { get; set; }
            public string UserName { get; set; }
            public string RoleID { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
            public string ValidTill { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public string RefreshTokenNew { get; set; }

        }

        #endregion

        #region Change Password

        public class ChangePasswordRes
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }

        #endregion

        #region Assets Response

        #region Asset Tracking

        public class AssetTrackingResponse
        {
            public string Barcode { get; set; }
            public string AssestDescription { get; set; }
            public string ArDescription { get; set; }
            public string LocID { get; set; }
            public string LocCompCode { get; set; }
            public string CostCenter { get; set; }
            public string AsstStatus { get; set; }
            public string AssetLocationDescription { get; set; }
            public string CatID { get; set; }
            public string AssetCategoryDescription { get; set; }
            public string PurchaseDate { get; set; }
            public string AcquisitionPrice { get; set; }
            public string Custodian { get; set; }
            public string netBook { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }

        }

        #endregion
        
        #region Anonymous Asset

        public class AnonymousAssetResponse
        {
            public string Description { get; set; }
            public string HisDate { get; set; }
            public string Location { get; set; }
            public string Category { get; set; }
            public string DeviceName { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }
        }

        #endregion

        #region Update Asset Location

        public class UpdateAssetLocationResponse
        {
            public string Message { get; set; }
            public string Status { get; set; }
            public Object dt { get; set; }
        }

        #endregion

        #endregion

        #region Locations

        public class Locations
        {
            public string ID { get; set; }
            public string Values { get; set; }
            public string FullValues { get; set; }
        }

        #endregion

        #region EmailRes

        public class EmailRes
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }

        #endregion

        #region Forgot Password

        public class ForgotPasswordRes
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }

        #endregion

    }
}
