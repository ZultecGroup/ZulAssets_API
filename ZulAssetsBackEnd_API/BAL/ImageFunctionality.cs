using System.IO;
using ZulAssetsBackEnd_API.DAL;
using static System.Net.Mime.MediaTypeNames;

namespace ZulAssetsBackEnd_API.BAL
{
    public class ImageFunctionality
    {

        #region Construction

        private Constants _constants;
        public ImageFunctionality()
        {
        }

        #endregion

        #region Convert Base64 to Image & Save to Server

        public Boolean ConvertBase64toImg_SavetoServer(string fileData, string imgName, string imageType)
        {
            try
            {
                GeneralFunctions GF = new GeneralFunctions();
                GF.CreateImgFolder(imageType);
                var base64string = fileData;
                var base64array = Convert.FromBase64String(base64string);
                var extesion = GeneralFunctions.GetFileExtension(base64string);
                var FileName = imgName + "." + extesion;
                var filePath = GeneralFunctions.GenerateFilePath() + "\\Img\\" + imageType + "\\" + FileName;
                System.IO.File.WriteAllBytes(filePath, base64array);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Get Image from Server and convert it to Base64

        public string ConvertImgFromServer_toBase64(string compName)
        {
            //string filePath = GenerateFilePath();

            //string filePath = GeneralFunctions.GenerateFilePath();

            //byte[] imageArray = System.IO.File.ReadAllBytes(filePath + "\\Img\\" + "\\CompanyLogo\\" + compName + ".png");
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            //return base64ImageRepresentation;

            string filePath = GeneralFunctions.GenerateFilePath();
            string imagePath = Path.Combine(filePath, "Img", "CompanyLogo", compName + ".png");

            string base64ImageRepresentation = string.Empty;
            try
            {
                if (File.Exists(imagePath))
                {
                    byte[] imageArray = File.ReadAllBytes(imagePath);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);
                }
                else
                {
                    // Handle the case when the image file does not exist
                    // Set a default or return an error message, as needed
                    base64ImageRepresentation = "";
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur during file read or conversion
                // Log the exception or return an error message, as needed
                base64ImageRepresentation = "Error: " + ex.Message;
            }

            return base64ImageRepresentation;

        }

        #endregion        

    }
}
