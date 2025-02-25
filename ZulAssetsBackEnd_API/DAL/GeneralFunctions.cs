using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using ZulAssetsBackEnd_API.BAL;
using static System.Net.Mime.MediaTypeNames;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.DAL
{
    public class GeneralFunctions
    {

        #region Declaration

        public static string Message = "ControllerName were Operation by UserName";
        private static string SP_GetAllAssetsAgainstItemCode = "[dbo].[SP_GetAllAssetsAgainstItemCode]";

        #endregion

        #region Write or Append Text in Text File

        public static string CreateAndWriteToFile(string ControllerName, string Operation, string UserName)
        {
            try
            {
                string text = Message;
                Dictionary<string, string> replacements = new Dictionary<string, string>()
                {
                    { "ControllerName", ControllerName },
                    { "Operation", Operation },
                    { "UserName", UserName }
                };

                using (StreamWriter writer = File.AppendText(CreateTransactionsFolder()))
                {
                    writer.WriteLine(ReplaceMultipleWords(text, replacements));
                    writer.WriteLine("-----------------**************************------------------");
                }
                return "Text appended to the file successfully.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string ReplaceMultipleWords(string text, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }

        public string ReplaceMultipleWordsDB(string text, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }

        #endregion

        #region Company Folder Creation

        public Boolean CreateImgFolder(string imgType)
        {
            var folderDir = GenerateFilePath();
            // If directory does not exist, create it
            if (!Directory.Exists(folderDir + "\\Img\\" + imgType))
            {
                Directory.CreateDirectory(folderDir + "\\Img\\" + imgType);
            }
            return true;
        }

        #endregion

        #region Transaction Folder and Text File Creation

        public static string CreateTransactionsFolder()
        {
            var folderDir = GenerateFilePath();
            // If directory does not exist, create it
            if (!Directory.Exists(folderDir  + "\\TransactionLogs"))
            {
                Directory.CreateDirectory(folderDir + "\\TransactionLogs");
            }
            return folderDir + "\\TransactionLogs\\TransactionLogs.txt";
        }

        #endregion

        #region Get File Path

        public static string GenerateFilePath()
        {
            string folderDir = Directory.GetCurrentDirectory();
            return folderDir;
        }

        #endregion

        #region Get File Type & Extension

        public static string GetFileType(string newString)
        {
            var data = newString.Substring(0, 5);
            switch (data.ToUpper())
            {
                case "IVBOR":   //PNG File
                    return "image/png";
                case "/9J/4":   //JPG or JPEG
                    return "image/jpeg";
                case "JVBER":   //PDF File
                    return "application/pdf";
                case "U1PKC":   //Text File
                    return "text/plain";
                case "UESDB":   //Document File
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                default:
                    return string.Empty;
            }
        }

        public static string GetFileExtension(string string2)
        {
            var data = string2.Substring(0, 5);
            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "/9J/2":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "77U/M":
                    return "srt";
                case "UESDB":
                    return "docx";
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region Delete Asset Image from Directory

        public string DeleteImg(string imgName, string imageType)
        {
            string folderPath = GenerateFilePath() + "\\Img\\" + imageType;
            string[] files = Directory.GetFiles(folderPath, imgName + ".*");
            int a = files.Count();
            if (a > 0)
            {
                foreach (string f in files)
                {
                    System.IO.File.Delete(f);

                }
                return "Image deleted.";
            }
            else
            {
                return "Image not found!";
            }
        }

        #endregion

        #region Get All Files From Directory

        public DataTable GetAllFilesFromDirectory(DataTable dt, string imageType)
        {
            string folderPath = GenerateFilePath() + "\\Img\\" + imageType;
            DataTable imgFromDirDT = new DataTable();
            imgFromDirDT.Columns.Add("ImageBase64Dir");
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] files = Directory.GetFiles(folderPath, dt.Rows[i]["ItemCode"] + ".*");
                    DataRow dtRow = imgFromDirDT.NewRow();
                    if (files.Length > 0)
                    {
                        using (FileStream reader = new FileStream(files[0], FileMode.Open))
                        {
                            byte[] buffer = new byte[reader.Length];
                            reader.Read(buffer, 0, (int)reader.Length);
                            string base64String = Convert.ToBase64String(buffer);
                            dtRow["ImageBase64Dir"] = base64String;
                            imgFromDirDT.Rows.Add(dtRow);
                        }
                    }
                    else
                    {
                        dtRow["ImageBase64Dir"] = null;
                        imgFromDirDT.Rows.Add(dtRow);
                    }
                }
                return imgFromDirDT;
            }
            catch (Exception ex)
            {

                return new DataTable();
            }
        }
        #endregion

        #region Apply Barcode Policy

        public DataTable ApplyPolicySingleAst(string astID, string barcode_Structure_Assign, int CompanyID, string valueSep)
        {
            DataTable allAssetsDT = DataLogic.GetAllAssetsDetailsAgainstAstID(astID, "[dbo].[SP_GetAllAssetDetailsAgainstAstID]");

            string AssetID, Reference, AssetNum, Location, Category, CompCode, replaceLocCompCode, Barcode;
            //var barcode_Struct_Assign_Split = barcode_Structure_Assign.Split(',');

            DataTable exportDataTable = new DataTable();
            exportDataTable.Columns.Add("AstID"); exportDataTable.Columns.Add("BarCode");
            for (int i = 0; i < allAssetsDT.Rows.Count; i++)
            {
                AssetID = allAssetsDT.Rows[i]["AstID"].ToString();
                Reference = allAssetsDT.Rows[i]["RefNo"].ToString();
                AssetNum = allAssetsDT.Rows[i]["AstNum"].ToString();

                Location = DataLogic.GetLocCompCode(allAssetsDT.Rows[i]["LocID"].ToString()).Rows[0]["CompCode"].ToString();
                replaceLocCompCode = Location.Replace("\\", "-");
                //Category = DataLogic.GetCatCompCode(allAssetsDT.Rows[i]["AstCatID"].ToString()).Rows[0]["CompCode"].ToString();

                DataTable catDT = DataLogic.GetCatCompCode(allAssetsDT.Rows[i]["AstCatID"].ToString());

                Category = catDT.Rows.Count == 0 || catDT.Rows[0]["CompCode"].ToString() == "" ? "" : catDT.Rows[0]["CompCode"].ToString();

                Barcode = Generate_Barcode(CompanyID, AssetID, AssetNum, Reference, Category, Category, Location, Location, replaceLocCompCode, valueSep, barcode_Structure_Assign);

                DataRow newRow = exportDataTable.NewRow();

                newRow["AstID"] = allAssetsDT.Rows[i]["AstID"].ToString();
                newRow["BarCode"] = Barcode;

                exportDataTable.Rows.Add(newRow);

            }

            return exportDataTable;
        }

        public DataTable ApplyPolicy(string barcode_Structure_Assign, int CompanyID, string valueSep)
        {

            DataTable allAssetsDT = DataLogic.GetAllAssetsDetails(CompanyID, "[dbo].[SP_GetAllAssetDetails]");
            
            string AssetID, Reference, AssetNum, Location, Category, CompCode, replaceLocCompCode, Barcode;
            //var barcode_Struct_Assign_Split = barcode_Structure_Assign.Split(',');

            DataTable exportDataTable = new DataTable();
            exportDataTable.Columns.Add("AstID"); exportDataTable.Columns.Add("BarCode");
            for (int i = 0; i < allAssetsDT.Rows.Count; i++)
            {
                AssetID = allAssetsDT.Rows[i]["AstID"].ToString();
                Reference = allAssetsDT.Rows[i]["RefNo"].ToString();
                AssetNum = allAssetsDT.Rows[i]["AstNum"].ToString();

                Location = DataLogic.GetLocCompCode(allAssetsDT.Rows[i]["LocID"].ToString()).Rows[0]["CompCode"].ToString();
                replaceLocCompCode = Location.Replace("\\", "-");
                //Category = DataLogic.GetCatCompCode(allAssetsDT.Rows[i]["AstCatID"].ToString()).Rows[0]["CompCode"].ToString();

                DataTable catDT = DataLogic.GetCatCompCode(allAssetsDT.Rows[i]["AstCatID"].ToString());

                Category = catDT.Rows.Count == 0 || catDT.Rows[0]["CompCode"].ToString() == "" ? "" : catDT.Rows[0]["CompCode"].ToString();

                Barcode = Generate_Barcode(CompanyID, AssetID, AssetNum, Reference, Category, Category, Location, Location, replaceLocCompCode, valueSep, barcode_Structure_Assign);

                DataRow newRow = exportDataTable.NewRow();

                newRow["AstID"] = allAssetsDT.Rows[i]["AstID"].ToString();
                newRow["BarCode"] = Barcode;

                exportDataTable.Rows.Add(newRow);

            }

            return exportDataTable;
        }

        public DataTable ApplyPolicyAgainstItemCode(string barcode_Structure_Assign, int CompanyID, string valueSep, string astItems)
        {
            DataTable allAssetsDT = new DataTable();
            
            allAssetsDT = DataLogic.GetAllAssetsAgainstItemCode(astItems, SP_GetAllAssetsAgainstItemCode);
            
            string AssetID, Reference, AssetNum, Location, Category, CompCode, replaceLocCompCode, Barcode;
            //var barcode_Struct_Assign_Split = barcode_Structure_Assign.Split(',');

            DataTable exportDataTable = new DataTable();
            exportDataTable.Columns.Add("AstID"); exportDataTable.Columns.Add("BarCode");
            for (int i = 0; i < allAssetsDT.Rows.Count; i++)
            {
                AssetID = allAssetsDT.Rows[i]["AstID"].ToString();
                Reference = allAssetsDT.Rows[i]["RefNo"].ToString();
                AssetNum = allAssetsDT.Rows[i]["AstNum"].ToString();

                Location = DataLogic.GetLocCompCode(allAssetsDT.Rows[i]["LocID"].ToString()).Rows[0]["CompCode"].ToString();
                replaceLocCompCode = Location.Replace("\\", "-");
                //Category = DataLogic.GetCatCompCode(allAssetsDT.Rows[i]["AstCatID"].ToString()).Rows[0]["CompCode"].ToString();

                DataTable catDT = DataLogic.GetCatCompCode(allAssetsDT.Rows[i]["AstCatID"].ToString());

                Category = catDT.Rows.Count == 0 || catDT.Rows[0]["CompCode"].ToString() == "" ? "" : catDT.Rows[0]["CompCode"].ToString();

                Barcode = Generate_Barcode(CompanyID, AssetID, AssetNum, Reference, Category, Category, Location, Location, replaceLocCompCode, valueSep, barcode_Structure_Assign);

                DataRow newRow = exportDataTable.NewRow();

                newRow["AstID"] = allAssetsDT.Rows[i]["AstID"].ToString();
                newRow["BarCode"] = Barcode;

                exportDataTable.Rows.Add(newRow);

            }

            return exportDataTable;
        }

        public string Generate_Barcode(int CompanyID, string AstID, string AstNum, string RefNo, string CatCompCode1, string CatCompCode2, string LocCompCode1, string LocCompCode2, string MainLocation, string valueSep, string barcode_Structure_Assign)
        {
            string strBarCode = string.Empty;
            if (valueSep == "None")
            {
                valueSep = "";
            }

            var splittedBarcode_Structure1 = barcode_Structure_Assign.Split(",");
            if (splittedBarcode_Structure1.Length > 0)
            {
                for (int i = 0; i < splittedBarcode_Structure1.Length; i++)
                {
                    if (splittedBarcode_Structure1[i].StartsWith("AID"))    //ASTID
                    {
                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            if (AstID != "")
                            {
                                if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                                {
                                    if (strBarCode == "")
                                    {
                                        strBarCode += AstID.Trim();
                                    }
                                    else
                                    {
                                        strBarCode += valueSep + AstID.Trim();
                                    }
                                }
                                else
                                {
                                    int j;
                                    j = AstID.Length - Convert.ToInt32(splittedBarcode_Structure2[1]);
                                    if (j < 0)
                                    {
                                        j = 0;
                                    }
                                    if (strBarCode == "")
                                    {
                                        strBarCode += (AstID.Substring(j)).Trim();
                                    }
                                    else
                                    {
                                        strBarCode += valueSep + (AstID.Substring(j)).Trim();
                                    }
                                }
                            }
                        }
                    }

                    else if (splittedBarcode_Structure1[i].StartsWith("LOCM"))  //LOCM
                    {
                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            var splittedBarcode_Structure3 = MainLocation.Split("-");
                            if (splittedBarcode_Structure3.Length > 1)
                            {
                                if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, 4);
                                        if(strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, 4);
                                        }

                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, 4);
                                        }
                                    }
                                }
                                else
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 3].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? valueSep + strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("ANM"))  //ANM
                    {
                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                            {
                                //strBarCode += strBarCode == "" ? AstNum.Trim() : valueSep + AstNum.Trim();
                                if (strBarCode == "")
                                {
                                    strBarCode += AstNum.Trim();
                                }
                                else
                                {
                                    strBarCode += valueSep + AstNum.Trim();
                                }
                            }
                            else
                            {
                                int j;
                                j = AstNum.Length - Convert.ToInt32(splittedBarcode_Structure2[1]);
                                if (j < 0)
                                {
                                    j = 0;
                                }
                                //strBarCode += strBarCode == "" ? (AstNum.Substring(j).Trim()).PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0') : valueSep + (AstNum.Substring(j).Trim()).PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0');
                                if (strBarCode == "")
                                {
                                    strBarCode += AstNum.Substring(j).Trim().PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0');
                                }
                                else
                                {
                                    strBarCode += valueSep + AstNum.Substring(j).Trim().PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0');
                                }
                            }
                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("REF"))  //REF
                    {
                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                            {
                                //strBarCode += strBarCode == "" ? RefNo.Trim() : valueSep + RefNo.Trim();
                                if (strBarCode == "")
                                {
                                    strBarCode += RefNo.Trim();
                                }
                                else
                                {
                                    strBarCode += valueSep + RefNo.Trim();
                                }
                            }
                            else
                            {
                                int j;
                                j = AstID.Length - Convert.ToInt32(splittedBarcode_Structure2[1]);
                                if (j < 0)
                                {
                                    j = 0;
                                }
                                //strBarCode += strBarCode == "" ? (RefNo.Substring(i).Trim()).PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0') : valueSep + (RefNo.Substring(i).Trim()).PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0');
                                if (strBarCode == "")
                                {
                                    strBarCode += (RefNo.Substring(i).Trim()).PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0');
                                }
                                else
                                {
                                    strBarCode += valueSep + (RefNo.Substring(i).Trim()).PadLeft(Convert.ToInt32(splittedBarcode_Structure2[1]), '0');
                                }
                            }
                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("CAT1"))  //CAT1
                    {
                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            var splittedBarcode_Structure3 = CatCompCode1.Split("\\");
                            if (splittedBarcode_Structure3.Length >= 1) //CatCompCode >= 2
                            {
                                if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                                {
                                    string strCat = string.Empty;

                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();  //CatCompCode2 Length - 1
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0,4);
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();  //CatCompCode2 Length - 1
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, 4);
                                        }
                                    }
                                }
                                else
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();  //CatCompCode2 Length - 1
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();  //CatCompCode2 Length - 1
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? valueSep + strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("CAT2"))  //CAT2
                    {
                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            var splittedBarcode_Structure3 = CatCompCode1.Split("\\");
                            if (splittedBarcode_Structure3.Length >= 2)
                            {
                                if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                                {
                                    string strCat = string.Empty;

                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, 4);
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, 4);
                                        }
                                    }
                                }
                                else
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? valueSep + strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("LOC1"))  //LOC1
                    {

                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {
                            var splittedBarcode_Structure3 = MainLocation.Split("-");
                            if (splittedBarcode_Structure3.Length > 1)  // Length > 2
                            {
                                if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();  //Length - 1
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, 4);
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();  //Length - 1
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, 4);
                                        }
                                    }
                                }
                                else
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 2].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? valueSep + strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("LOC2"))  //LOC2
                    {

                        var splittedBarcode_Structure2 = splittedBarcode_Structure1[i].Split("-");
                        if (splittedBarcode_Structure2.Length > 1)
                        {

                            var splittedBarcode_Structure3 = MainLocation.Split("-");
                            if (splittedBarcode_Structure3.Length > 2)
                            {
                                if (Convert.ToInt32(splittedBarcode_Structure2[1]) == 0)
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode  += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, 4);
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < 4 ? strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, 4);
                                        if (strCat.Length < 4)
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, 4);
                                        }
                                    }
                                }
                                else
                                {
                                    string strCat = string.Empty;
                                    if (strBarCode == "")
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? strCat.Substring(0, strCat.Length) : strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                    else
                                    {
                                        strCat = splittedBarcode_Structure3[splittedBarcode_Structure3.Length - 1].Trim();
                                        //strBarCode += strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]) ? valueSep + strCat.Substring(0, strCat.Length) : valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        if (strCat.Length < Convert.ToInt32(splittedBarcode_Structure2[1]))
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, strCat.Length);
                                        }
                                        else
                                        {
                                            strBarCode += valueSep + strCat.Substring(0, Convert.ToInt32(splittedBarcode_Structure2[1]));
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                    
                    else if (splittedBarcode_Structure1[i].StartsWith("FIX"))  // Prefix
                    {
                        var strPrefix = splittedBarcode_Structure1[i].Split("-");
                        if (strPrefix.Length > 1)
                        {
                            strBarCode = strPrefix[1];
                        }
                    }

                }
            }

            return strBarCode;
        }

        #endregion

        #region Add New Asset For Purchase Order Items/Assets

        public string AddNew_AssetDetails(AssetsInTransit assetsInTransit, string valueSep, string barcode_Structure_Assign)
        {
            string Location, Barcode, Category, replaceLocCompCode, CreatedBy, LastEditBy;

            int refNoDB = (Convert.ToInt32(GenerateNewAstNum()) + 1);
            for (int i = 0; i < Convert.ToInt32(assetsInTransit.Quantity); i++)
                {
                assetsInTransit.AstNum = (Convert.ToInt32(GenerateNewAstNum()) + 1).ToString();
                assetsInTransit.AstID = Generate_AssetID();
                Location = DataLogic.GetLocCompCode(assetsInTransit.LocID).Rows[0]["CompCode"].ToString();
                replaceLocCompCode = Location.Replace("\\", "-");
                Category = DataLogic.GetCatCompCode(assetsInTransit.ItemCode.ToString()).Rows[0]["CompCode"].ToString();
                if (i > 0)
                {
                    assetsInTransit.RefNo = refNoDB.ToString() + "-" + i.ToString();
                }
                else
                {
                    assetsInTransit.RefNo = refNoDB.ToString();
                }
                Barcode = Generate_Barcode(Convert.ToInt32(assetsInTransit.CompanyID), assetsInTransit.AstID, assetsInTransit.AstNum, assetsInTransit.RefNo, Category, Category, Location, Location, replaceLocCompCode, valueSep, barcode_Structure_Assign);

                DataTable dt321 = DataLogic.InsertAssetDetailsOfPO(assetsInTransit, Barcode, "[dbo].[SP_InsertPOAsset]");
                bool trueTest = SetCompanyLastAssetNumber(assetsInTransit.CompanyID, assetsInTransit.AstNum);                
                
                //Create history for all inventory schedules
                DataTable dtAstInvHis = DataLogic.GetAllInvSchsNonClosed("[dbo].[SP_GetInvSchNonClosed]");

                for (int j = 0; j < dtAstInvHis.Rows.Count; j++)
                {
                    string AstID = assetsInTransit.AstID;
                    int status = 0;
                    long invSchCode = Convert.ToInt32(dtAstInvHis.Rows[j]["InvSchCode"]);
                    string HisDate = DateTime.Now.ToString();
                    string fromLoc = assetsInTransit.LocID;
                    string toLoc = assetsInTransit.LocID;
                    int NoPiece = 1;
                    string AssetStatus = "1";
                    DataTable teet = DataLogic.Insert_Ast_History(AstID, status, invSchCode, HisDate, fromLoc, toLoc, NoPiece, AssetStatus, "", "[dbo].[SP_InsertAstInAst_History]");
                    string msmg = teet.Rows[0]["Message"].ToString();
                }

                Decimal ESalValue = 0;
                Decimal ESalMonth = 0;
                Decimal ESalYear = 0;
                Decimal EDepValue = 0;

                DataTable dtDepPolicy = DataLogic.GetDepPolicyAgainstItemCode(assetsInTransit.ItemCode.ToString(), "[dbo].[SP_GetDepPolicyAgainstItemCode]");

                if (dtDepPolicy.Rows.Count > 0)
                {
                    Add_Ast_Books(assetsInTransit.AstID, assetsInTransit.BaseCost + assetsInTransit.Tax, assetsInTransit.CompanyID, Convert.ToDouble(dtDepPolicy.Rows[0]["SalvageValue"]), Convert.ToInt32(dtDepPolicy.Rows[0]["SalvageYear"]), Convert.ToInt32(dtDepPolicy.Rows[0]["SalvageMonth"]), assetsInTransit.ServiceDate, Convert.ToBoolean(dtDepPolicy.Rows[0]["IsSalvageValuePercent"]));
                    ESalValue = Convert.ToDecimal(dtDepPolicy.Rows[0]["SalvageValue"]);
                    ESalMonth = Convert.ToDecimal(dtDepPolicy.Rows[0]["SalvageMonth"]);
                    ESalYear = Convert.ToDecimal(dtDepPolicy.Rows[0]["SalvageYear"]);
                }

                assetsInTransit.AstID = Generate_AssetID();

                assetsInTransit.AstNum = GenerateNewAstNum();

                if (assetsInTransit.AstNum == "")
                {
                    assetsInTransit.Quantity = (i + 1).ToString();
                }

            }

            if (assetsInTransit.Quantity == assetsInTransit.Quantity)
            {
                string gggg = TransferPOItm(assetsInTransit.POItmID, assetsInTransit.Quantity, assetsInTransit.POCode);  //POCode should replace with POItmID
            }
            else
            {

            }
            return assetsInTransit.Quantity + " Assets transferred successfully and " + 
                (Convert.ToInt32(assetsInTransit.Quantity) - Convert.ToInt32(assetsInTransit.Quantity)) + " Assets still in this order.";
        }

        private string  TransferPOItm(string pOItmID, string quantity, string pOCode)
        {
            DataTable restInPeace = DataLogic.TransferPOItm(pOItmID, Convert.ToInt32(quantity), pOCode, "[dbo].[SP_UpdatePODetailsItem]");
            string you = restInPeace.Rows[0]["Message"].ToString();
            return you;
        }

        private string GenerateNewAstNum()
        {
            DataTable newDataTable = DataLogic.GenerateAstNum("[dbo].[SP_GetMaxAstNumInAssetDetails]");
            string aa = newDataTable.Rows[0]["LatestAstNum"].ToString();
            return aa;
        }

        #endregion

        #region Set Company Last Asset Number

        private bool SetCompanyLastAssetNumber(string CompanyID, string AstNum)
        {

            DataTable tt3 = DataLogic.UpdateCompanyLastAssetNumber(CompanyID, AstNum, "[dbo].[SP_UpdateLastAssetNumberOfCompany]");
            if (tt3.Rows[0]["Status"].ToString() == "200")
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        #endregion

        #region Add Asset Books

        private bool Add_Ast_Books(string AstID, double Cost, string CompanyID, double salvageValue, int salvageYear, int salvageMonth, string serviceDate, bool isSalvageValuePercentage)
        {
            DataTable newDTable = DataLogic.GetBookAgainstCompanyID(CompanyID, "[dbo].[SP_GetBooksAgainstCompanyID]");
            for (int i = 0; i < newDTable.Rows.Count; i++)
            {
                int BookID = Convert.ToInt32(newDTable.Rows[0]["BookID"]);
                int DepCode = Convert.ToInt32(newDTable.Rows[0]["DepCode"]);
                string Description = newDTable.Rows[0]["Description"].ToString();
                double LastBookValue = 0.00;
                double salvageValuePercentage;
                double salvageValue2;
                if (isSalvageValuePercentage)
                {
                    salvageValuePercentage = salvageValue;
                    salvageValue2 = (Convert.ToDouble(Cost) * Convert.ToDouble(salvageValue)) / 100;
                }
                else
                {
                    if (Cost > 0)
                    {
                        salvageValuePercentage = Math.Round((salvageValue / Cost) * 100, 2);
                    }
                    else
                    {
                        salvageValuePercentage = 0;
                    }
                    salvageValue2 = salvageValue;
                }

                double CurrentBookValue = Cost;
                string BVUpdate = serviceDate;

                if (!Check_BookExists(BookID, AstID))
                {
                    DataTable dt123 = DataLogic.AddBookForPOAsset(BookID, AstID, DepCode, salvageValue2, salvageYear, LastBookValue, CurrentBookValue, BVUpdate, salvageMonth, salvageValuePercentage, "[dbo].[SP_InsertAstIn_AstBook]");
                    string gh = dt123.Rows[0]["Message"].ToString();
                }
                
            }
            return true;
        }

        #region Check Book Exists

        public bool Check_BookExists(int bookID, string astID)
        {
            DataTable checkDT = DataLogic.CheckBookForAstID(bookID, astID, "[dbo].[SP_GetCheckBookExistsForAstID]");
            if (checkDT.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Generate AstID
        private string Generate_AssetID()
        {
            string str = "";
            try
            {
                do
                {
                    str = DateTime.Now.ToString("yyddMMHHmmssff");
                } while (Check_AstID(str, true));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        #endregion

        #region Check AstID exists or not

        private bool Check_AstID(string AssetID, bool IsInsertStatus)
        {
            return FieldValueExisted("AstID", AssetID, "AssetDetails", IsInsertStatus, "AstID", AssetID);
        }

        protected bool FieldValueExisted(string FieldName, string FieldValue, string TableName,
                                 bool IsInsertStatus, string PKField, string PKFieldVal)
        {
            SqlCommand objCommand = new SqlCommand();
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append("select " + PKField + " from " + TableName + " where " + FieldName);
            strQuery.Append(" = '" + FieldValue + "'");
            //objCommand.Parameters.Add(new SqlParameter("@FieldValue", FieldValue));
            objCommand.CommandText = strQuery.ToString();



            try
            {
                string str = General_Executer_Scalar(objCommand);
                if (str == "0")
                {
                    str = "";
                }

                if (!string.IsNullOrEmpty(str) && IsInsertStatus)
                {
                    return true;
                }
                else if (!string.IsNullOrEmpty(str) && !IsInsertStatus && str != PKFieldVal)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Execute Method

        public string General_Executer_Scalar(SqlCommand cmd)
        {
            SqlConnection Conn = null;
            string strKey = "";
            Connection connect = new Connection();
            try
            {
                Conn = connect.GetDataBaseConnection();

                cmd.Connection = Conn;
                object obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    strKey = Convert.ToString(obj);
                }

                if (strKey.Trim() == "")
                {
                    strKey = "0";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn != null)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            return strKey;
        }

        #endregion

        #region Calculate Percentage for Depreciation Policy

        public string CalculatePercent(string SalYearValue, string SalMonthValue)
        {
            if (!string.IsNullOrEmpty(SalYearValue) && !string.IsNullOrEmpty(SalMonthValue))
            {
                double percent;
                int SalMonth = int.Parse(SalMonthValue);
                int SalYear = int.Parse(SalYearValue);

                double perc;
                perc = SalYear + (SalMonth / 12.0);
                if (perc == 0)
                {
                    percent = 0;
                }
                else
                {
                    percent = Math.Round(100 / perc, 2);
                }

                return percent.ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region Depreciation Engine Logic

        public string ThrBreakMethod(DataTable selectedBooksDT, string updateTillDate, int dtCount)
        {
            int bookIDs, totalAssetsCount= 0;
            string loopSelectedBook = string.Empty;
            string msg = string.Empty;
            for (int i = 0; i < dtCount; i++)
            {
                bookIDs = Convert.ToInt32(selectedBooksDT.Rows[i]["BookIDs"].ToString());
                loopSelectedBook += bookIDs.ToString() + ", ";
                totalAssetsCount += Convert.ToInt32(DataLogic.GetAssetsCountAgainstBookID(bookIDs.ToString(), "[dbo].[SP_GetAssetsCountAgainstBookId]").Rows[0]["AstCount"]);
            }
            
            loopSelectedBook = loopSelectedBook.Substring(0, loopSelectedBook.Length - 2);
            
            try
            {
                for (int i = 0; i < dtCount; i++)
                {
                    bookIDs = Convert.ToInt32(selectedBooksDT.Rows[i]["BookIDs"].ToString());
            
                    if (Check_Get_DepLogs(updateTillDate, bookIDs))
                    {
                        msg = "Book " + bookIDs + " Already Closed";
                        return msg;
                    }
                    else
                    {
                        DateTime dateTimeValue = DateTime.Parse(updateTillDate);
                        msg = Run_Depreciation_ProcessNew(dateTimeValue.Day, dateTimeValue.Month, dateTimeValue.Year, bookIDs);
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private string Run_Depreciation_ProcessNew(int days, int month, int year, int bookIDs)
        {
            DataTable dtAstBooks = new DataTable();
            DataTable bookHistoryAssetsDT = new DataTable();
            DataTable AstBooksUpdateDT = new DataTable();
            DataTable dtbookHistoryMsg = new DataTable();
            DataTable dtLog = new DataTable();
            DateTime dtFiscal = new DateTime(year, month, days);

            dtAstBooks = DataLogic.GetAssetDetails_ByBookID(bookIDs, "[dbo].[SP_GetBooksAssetAgainstBookID]");

            string msg = string.Empty;
            double price, DeprValue;
            int salYr, salMonth;
            DateTime serviceDate;
            double totDepValue, totAstValue;
            totDepValue = 0;
            totAstValue = 0;

            bookHistoryAssetsDT.Columns.Add("BookID", typeof(string)); bookHistoryAssetsDT.Columns.Add("DepCode", typeof(int)); bookHistoryAssetsDT.Columns.Add("DepValue", typeof(float));
            bookHistoryAssetsDT.Columns.Add("AccDepValue", typeof(float)); bookHistoryAssetsDT.Columns.Add("DepDate", typeof(DateTime)); bookHistoryAssetsDT.Columns.Add("CurrentBV", typeof(float));
            bookHistoryAssetsDT.Columns.Add("AstID", typeof(string)); bookHistoryAssetsDT.Columns.Add("SalvageYear", typeof(int)); bookHistoryAssetsDT.Columns.Add("SalvageMonth", typeof(int));

            AstBooksUpdateDT.Columns.Add("BookID", typeof(string)); AstBooksUpdateDT.Columns.Add("AstID", typeof(string)); AstBooksUpdateDT.Columns.Add("DepCode", typeof(int));
            AstBooksUpdateDT.Columns.Add("SalvageValue", typeof(float)); AstBooksUpdateDT.Columns.Add("SalvageYear", typeof(int)); AstBooksUpdateDT.Columns.Add("LastBV", typeof(float));
            AstBooksUpdateDT.Columns.Add("CurrentBV", typeof(float)); AstBooksUpdateDT.Columns.Add("BVUpdate", typeof(DateTime)); AstBooksUpdateDT.Columns.Add("SalvageMonth", typeof(int));
            AstBooksUpdateDT.Columns.Add("SalvageValuePercent", typeof(float));

            try
            {
                for (int i = 0; i < dtAstBooks.Rows.Count; i++)
                {
                    price = Convert.ToDouble(dtAstBooks.Rows[i]["BaseCost"] )+ Convert.ToDouble(dtAstBooks.Rows[i]["Tax"]);
                    serviceDate = DateTime.Parse(dtAstBooks.Rows[i]["SrvDate"].ToString());
                    if (DateTime.Compare(serviceDate, dtFiscal) <= 0)
                    {
                        salYr = Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]);
                        salMonth = Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]);
                        salMonth = salMonth + (salYr * 12);
                        if (salMonth > 0)
                        {
                            if (salMonth - ((int)((dtFiscal - Convert.ToDateTime(dtAstBooks.Rows[i]["SrvDate"].ToString())).TotalDays / 30)) > 0)
                            {
                                double currentBV = Convert.ToDouble(dtAstBooks.Rows[i]["CurrentBV"]);
                                DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtFiscal, Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"]), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                int depHistCount = Convert.ToInt32(DataLogic.Check_DepHistory(dtAstBooks.Rows[i]["BookID"].ToString(), dtAstBooks.Rows[i]["AstID"].ToString(), Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString()), dtFiscal, "[dbo].[SP_CheckDepHistory]").Rows.Count);
                                bool result = depHistCount > 0 ? true : false;

                                if (result)
                                {
                                    dtLog = DataLogic.Check_DepHistory(dtAstBooks.Rows[i]["BookID"].ToString(), dtAstBooks.Rows[i]["AstID"].ToString(), Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString()), dtFiscal, "[dbo].[SP_CheckDepHistory]");
                                    if (dtLog != null && dtLog.Rows.Count > 0)
                                    {
                                        DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtFiscal, Convert.ToDateTime(dtLog.Rows[0]["BVUpdate"].ToString()), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                    }
                                    else
                                    {
                                        DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtFiscal, Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString()), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                    }
                                }
                                else
                                {
                                    DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtFiscal, Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"]), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                }

                                totDepValue += DeprValue;
                                totAstValue += (currentBV - DeprValue);

                                if (currentBV > Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()))
                                {
                                    if (currentBV - DeprValue <= Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"]))
                                    {
                                        DeprValue = currentBV - Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"]);
                                        currentBV = Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString());
                                    }
                                    else
                                    {
                                        currentBV -= DeprValue;
                                        //currentBV = currentBV - DeprValue;
                                    }
                                    double accDepValue = (price - currentBV);

                                    bookHistoryAssetsDT.Rows.Add(Convert.ToInt32(dtAstBooks.Rows[i]["BookID"].ToString()), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), DeprValue, accDepValue, dtFiscal, currentBV, dtAstBooks.Rows[i]["AstID"].ToString(), salYr, Convert.ToInt32(dtAstBooks.Rows[i]["SalvageMonth"]));
                                    
                                    AstBooksUpdateDT.Rows.Add(bookIDs, dtAstBooks.Rows[i]["AstID"].ToString(), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), Convert.ToDecimal(dtAstBooks.Rows[i]["SalvageValue"]), Convert.ToInt32(dtAstBooks.Rows[i]["SalvageYear"]), currentBV + DeprValue, (currentBV-DeprValue), dtFiscal, Convert.ToInt32(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToDecimal(dtAstBooks.Rows[i]["SalvageValuePercent"]));

                                    // using the for loop below when all assets bookhistoryDT and astBooksDT is filled
                                    // and then push that data to insert and update accordingly
                                    //if (i == (dtAstBooks.Rows.Count - 1))   
                                    //{
                                    //    ////Insert hoga Book History main
                                    //    //dtbookHistoryMsg = DataLogic.InsertInBookHistory(1, bookHistoryAssetsDT, "[dbo].[SP_InsertBookHistory]");

                                    //    //if (dtbookHistoryMsg.Columns.Contains("ErrorMessage"))
                                    //    //{
                                    //    //    msg = dtbookHistoryMsg.Rows[0]["ErrorMessage"].ToString();    //Whenever the error appears it comes in
                                    //    //    return msg;
                                    //    //}
                                    //    //else
                                    //    //{
                                    //    //    msg = dtbookHistoryMsg.Rows[0]["Message"].ToString();
                                    //    //    //Success ke case main yahan ayga
                                    //    //    dtbookHistoryMsg = new DataTable();
                                    //    //    dtbookHistoryMsg = DataLogic.InsertInBookHistory(0, AstBooksUpdateDT, "[dbo].[SP_InsertBookHistory]");
                                    //    //    if (dtbookHistoryMsg.Columns.Contains("ErrorMessage"))
                                    //    //    {
                                    //    //        msg = dtbookHistoryMsg.Rows[0]["ErrorMessage"].ToString();    //Whenever the error appears it comes in
                                    //    //        return msg;
                                    //    //    }
                                    //    //    else
                                    //    //    {
                                    //    //        msg += " &" + dtbookHistoryMsg.Rows[0]["Message"].ToString();
                                    //    //        return msg;
                                    //    //    }
                                    //    //}
                                    //}
                                }
                            }
                            else
                            {
                                DateTime dtSrvDispose;
                                dtSrvDispose = Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString());
                                dtSrvDispose = dtSrvDispose.AddMonths(Convert.ToInt32(dtAstBooks.Rows[i]["SalvageMonth"].ToString()));
                                dtSrvDispose = dtSrvDispose.AddYears(Convert.ToInt32(dtAstBooks.Rows[i]["SalvageYear"].ToString()));

                                double currentBV = Convert.ToDouble(dtAstBooks.Rows[i]["CurrentBV"].ToString());
                                DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"].ToString()), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"].ToString()), Convert.ToInt16(dtAstBooks.Rows[i]["DepCode"].ToString()), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtSrvDispose, Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"]), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                int depHistCount = Convert.ToInt32(DataLogic.Check_DepHistory(dtAstBooks.Rows[i]["BookID"].ToString(), dtAstBooks.Rows[i]["AstID"].ToString(), Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString()), dtFiscal, "[dbo].[SP_CheckDepHistory]").Rows.Count);
                                bool result = depHistCount > 0 ? true : false;

                                if (result)
                                {
                                    dtLog = DataLogic.Check_DepHistory(dtAstBooks.Rows[i]["BookID"].ToString(), dtAstBooks.Rows[i]["AstID"].ToString(), Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString()), dtSrvDispose, "[dbo].[SP_CheckDepHistory]");
                                    if (dtLog != null && dtLog.Rows.Count > 0)
                                    {
                                        DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtFiscal, Convert.ToDateTime(dtLog.Rows[0]["BVUpdate"].ToString()), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                    }
                                    else
                                    {
                                        DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtFiscal, Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"].ToString()), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                    }
                                }
                                else
                                {
                                    DeprValue = CalcDep(Convert.ToInt16(dtAstBooks.Rows[i]["SalvageYear"]), Convert.ToInt16(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), currentBV, Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()), dtSrvDispose, Convert.ToDateTime(dtAstBooks.Rows[i]["BVUpdate"]), Convert.ToDateTime(dtAstBooks.Rows[i]["srvdate"]), price);
                                }

                                totDepValue += DeprValue;
                                totAstValue += (currentBV - DeprValue);
                                if (currentBV > Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString()))
                                {
                                    if (currentBV - DeprValue <= Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"]))
                                    {
                                        DeprValue = currentBV - Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"]);
                                        currentBV = Convert.ToDouble(dtAstBooks.Rows[i]["SalvageValue"].ToString());
                                    }
                                    else
                                    {
                                        currentBV -= DeprValue;
                                        //currentBV = currentBV - DeprValue;
                                    }

                                    DeprValue = Round(DeprValue);
                                    double accDepValue = Round(price - currentBV);

                                    bookHistoryAssetsDT.Rows.Add(Convert.ToInt32(dtAstBooks.Rows[i]["BookID"].ToString()), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), DeprValue, accDepValue, dtSrvDispose, currentBV, dtAstBooks.Rows[i]["AstID"].ToString(), salYr, Convert.ToInt32(dtAstBooks.Rows[i]["SalvageMonth"]));

                                    AstBooksUpdateDT.Rows.Add(bookIDs, dtAstBooks.Rows[i]["AstID"].ToString(), Convert.ToInt32(dtAstBooks.Rows[i]["DepCode"]), Convert.ToDecimal(dtAstBooks.Rows[i]["SalvageValue"]), Convert.ToInt32(dtAstBooks.Rows[i]["SalvageYear"]), currentBV + DeprValue, (currentBV - DeprValue), dtSrvDispose, Convert.ToInt32(dtAstBooks.Rows[i]["SalvageMonth"]), Convert.ToDecimal(dtAstBooks.Rows[i]["SalvageValuePercent"]));

                                    //if (i == (dtAstBooks.Rows.Count - 1))
                                    //{
                                    //    //Insert hoga Book History main
                                    //    dtbookHistoryMsg = DataLogic.InsertInBookHistory(1, bookHistoryAssetsDT, "[dbo].[SP_InsertBookHistory]");

                                    //    if (dtbookHistoryMsg.Columns.Contains("ErrorMessage"))
                                    //    {
                                    //        msg = dtbookHistoryMsg.Rows[0]["ErrorMessage"].ToString();    //jb error hoga koi b tw isme ayga
                                    //        return msg;
                                    //    }
                                    //    else
                                    //    {
                                    //        msg = dtbookHistoryMsg.Rows[0]["Message"].ToString();
                                    //        //Success ke case main yahan ayga
                                    //        dtbookHistoryMsg = new DataTable();
                                    //        dtbookHistoryMsg = DataLogic.InsertInBookHistory(0, AstBooksUpdateDT, "[dbo].[SP_InsertBookHistory]");
                                    //        if (dtbookHistoryMsg.Columns.Contains("ErrorMessage"))
                                    //        {
                                    //            msg = dtbookHistoryMsg.Rows[0]["ErrorMessage"].ToString();    //jb error hoga koi b tw isme ayga
                                    //            return msg;
                                    //        }
                                    //        else
                                    //        {
                                    //            msg += " &" + dtbookHistoryMsg.Rows[0]["Message"].ToString();
                                    //            return msg;
                                    //        }
                                    //    }
                                    //}

                                }
                            }
                        }
                    }
                }

                if (bookHistoryAssetsDT.Rows.Count > 0 & AstBooksUpdateDT.Rows.Count > 0)
                {
                    //Insert hoga Book History main
                    dtbookHistoryMsg = DataLogic.InsertInBookHistory(1, bookHistoryAssetsDT, "[dbo].[SP_InsertBookHistory]");

                    if (dtbookHistoryMsg.Columns.Contains("ErrorMessage"))
                    {
                        msg = dtbookHistoryMsg.Rows[0]["ErrorMessage"].ToString();    //Whenever the error appears it comes in
                        //return msg;
                    }
                    else
                    {
                        msg = dtbookHistoryMsg.Rows[0]["Message"].ToString();
                        //Success ke case main yahan ayga
                        dtbookHistoryMsg = new DataTable();
                        dtbookHistoryMsg = DataLogic.InsertInBookHistory(0, AstBooksUpdateDT, "[dbo].[SP_InsertBookHistory]");
                        if (dtbookHistoryMsg.Columns.Contains("ErrorMessage"))
                        {
                            msg = dtbookHistoryMsg.Rows[0]["ErrorMessage"].ToString();    //Whenever the error appears it comes in
                            //return msg;
                        }
                        else
                        {
                            msg += " &" + dtbookHistoryMsg.Rows[0]["Message"].ToString();
                            EndDepProcess(dtFiscal, totDepValue, dtAstBooks.Rows.Count, totAstValue, month, year, bookIDs);
                            //return msg;
                        }
                    }
                    
                }

                return msg;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private static string EndDepProcess(DateTime dtFiscal, double totDepValue, int totAstCount, double totAstValue, int month, int year, int BookID)
        {
            DataTable dt = DataLogic.EndDepProcess(dtFiscal, totDepValue, totAstCount, totAstValue, month, year, BookID, "[dbo].[SP_InsertInDepLogs]");
            return "";
        }

        private double CalcDep(Int16 salYr, Int16 salMon, int intDepType, double lastBookValue, double salValue, DateTime fiscalYr, DateTime lastBookUpdateDate, DateTime serviceDate, double TotalCost)
        {
            //int DaysInMonth = DateTime.DaysInMonth(lastBookUpdateDate.Year, lastBookUpdateDate.Month);
            //DateOnly LastDayInMonthDate = new DateOnly(lastBookUpdateDate.Year, lastBookUpdateDate.Month, DaysInMonth);
            //DateOnly endOfMonth = LastDayInMonthDate.AddDays(1);
            //DateTime startOfMonth = lastBookUpdateDate;

            // Assuming fiscalYr is a DateTime
            DateTime startOfMonth = lastBookUpdateDate;
            DateTime lastDayOfMonth = GetLastDayInMonth(fiscalYr); // Get the last day as DateTime
            DateOnly lastDayOfMonthAsDateOnly = new DateOnly(lastDayOfMonth.Year, lastDayOfMonth.Month, lastDayOfMonth.Day); // Convert to DateOnly

            DateOnly endOfMonth = lastDayOfMonthAsDateOnly.AddDays(1); // Add one day to the last day

            if (lastBookValue != TotalCost)
            {
                startOfMonth = lastBookUpdateDate.AddDays(1);
                DateTime lastDayOfMonth2 = GetLastDayInMonth(fiscalYr); // Get the last day as DateTime
                DateOnly lastDayOfMonthAsDateOnly2 = new DateOnly(lastDayOfMonth2.Year, lastDayOfMonth2.Month, lastDayOfMonth2.Day); // Convert to DateOnly
                endOfMonth = lastDayOfMonthAsDateOnly2.AddDays(1);
                //endOfMonth = DepreciationAlgorithm.GetLastDayInMonth(LastDayInMonthDate.AddDays(1));
            }

            return DepreciationAlgorithm.CalcDepValue(salYr, salMon, intDepType, TotalCost, salValue, endOfMonth, DateOnly.FromDateTime(startOfMonth));
        }

        private DateTime GetLastDayInMonth(DateTime dt)
        {
            // Get the last day of the month as DateTime
            return dt.AddDays(DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day);
        }

        private bool Check_Get_DepLogs(string updateTillDate, int bookIDs)
        {
            DataTable checkDepLogsDT = new DataTable();
            int DepLogsExists = DataLogic.GetAll_DepLogs_Date(updateTillDate, bookIDs, "[dbo].[SP_CheckBookInDepLogs]").Rows.Count;
            return  DepLogsExists > 0 ? true : false;
        }

        public static double Round(double number, int numDigitsAfterDecimal = 0)
        {
            return Math.Round(number, numDigitsAfterDecimal);
        }

        #endregion

        #region Add New Asset Details For New Company ID

        public Tuple<string, string, string> AddNew_AssetDetails_ForNewCompanyID(string newCompanyID, string AstID, string refNo, double currentBV, string transRemarks,
            string GLCodes, int salYr, string ImageToBase64, float salValue, string StoredProcedure)
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(0);

            string astNum, Location, replaceLocCompCode, Category, newAstID;
            newAstID = Generate_AssetID();
            Message msg = new Message();
            DataTable AstBooksUpdateDT = new DataTable();
            AstInformationReqParam astInfoReqParam = new AstInformationReqParam();
            DataTable dt1 = DataLogic.GetAstInfoByAstID2(AstID, StoredProcedure);
            ImageFunctionality imgFun = new ImageFunctionality();

            AstBooksUpdateDT.Columns.Add("BookID", typeof(string)); AstBooksUpdateDT.Columns.Add("AstID", typeof(string)); AstBooksUpdateDT.Columns.Add("DepCode", typeof(int));
            AstBooksUpdateDT.Columns.Add("SalvageValue", typeof(float)); AstBooksUpdateDT.Columns.Add("SalvageYear", typeof(int)); AstBooksUpdateDT.Columns.Add("LastBV", typeof(float));
            AstBooksUpdateDT.Columns.Add("CurrentBV", typeof(float)); AstBooksUpdateDT.Columns.Add("BVUpdate", typeof(DateTime)); AstBooksUpdateDT.Columns.Add("SalvageMonth", typeof(int));
            AstBooksUpdateDT.Columns.Add("SalvageValuePercent", typeof(float));
            msg.message = dt1.Rows.Count.ToString();
            if (dt1.Rows.Count > 0)
            {
                astNum = (Convert.ToInt32(GenerateNewAstNum()) + 1).ToString();
                if (refNo == "")
                {
                    refNo = astNum;
                }
                else
                {
                    refNo = refNo;
                }
                string aa = string.Empty;
                if (Convert.ToInt32((DataLogic.CheckRefID(refNo, AstID, "[dbo].[SP_GetAstIDAgainstRefNo_AstID]")).Rows[0]["AstID"].ToString()) == 0)
                {
                    aa = "Not Exists";
                }
                else
                {
                    aa = "Exists";
                }

                if (aa == "Not Exists")
                {
                    DataTable barcode_Assign = DataLogic.Barcode_AssignToSelectedCompany(newCompanyID.ToString(), "[dbo].[SP_GetCompanyBarcode_Assign]");

                    string barcode_Structure_Assign = barcode_Assign.Rows[0]["BarCode"].ToString();
                    string valueSep = barcode_Assign.Rows[0]["ValueSep"].ToString();
                    int CompanyID = Convert.ToInt32(newCompanyID);
                    astInfoReqParam.AstID = newAstID;
                    astInfoReqParam.AstNum = astNum;
                    astInfoReqParam.RefNum = refNo;
                    astInfoReqParam.BaseCost = currentBV;
                    astInfoReqParam.TransRemarks = transRemarks;
                    astInfoReqParam.CustodianID = dt1.Rows[0]["CustodianID"].ToString();
                    astInfoReqParam.InsID = Convert.ToInt32(dt1.Rows[0]["InsID"].ToString());
                    astInfoReqParam.InvNumber = dt1.Rows[0]["InvNumber"].ToString();
                    astInfoReqParam.ItemCode = dt1.Rows[0]["ItemCode"].ToString();
                    astInfoReqParam.AstBrandId = Convert.ToInt32(dt1.Rows[0]["AstBrandId"].ToString());
                    astInfoReqParam.AstDesc = dt1.Rows[0]["AstDesc"].ToString();
                    Location = DataLogic.GetLocCompCode(dt1.Rows[0]["LocID"].ToString()).Rows[0]["CompCode"].ToString();
                    replaceLocCompCode = Location.Replace("\\", "-");
                    if (dt1.Rows[0]["AstCatID"].ToString() != "")
                    {
                        Category = DataLogic.GetCatCompCode(dt1.Rows[0]["AstCatID"].ToString()).Rows[0]["CompCode"].ToString();
                    }
                    else
                    {
                        Category = "";
                    }
                    astInfoReqParam.BarCode = Generate_Barcode(CompanyID, newAstID, astNum, refNo, Category, Category, Location, Location, replaceLocCompCode, valueSep, barcode_Structure_Assign);
                    astInfoReqParam.POCode = long.Parse(dt1.Rows[0]["POCode"].ToString());
                    DateTime purDate = Convert.ToDateTime(dt1.Rows[0]["PurDate"].ToString());
                    astInfoReqParam.PurDate = purDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    astInfoReqParam.SuppID = dt1.Rows[0]["SuppID"].ToString();
                    DateTime srvDate = Convert.ToDateTime(dt1.Rows[0]["SrvDate"].ToString());
                    astInfoReqParam.SrvDate = srvDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    astInfoReqParam.Tax = Convert.ToDouble(dt1.Rows[0]["Tax"].ToString());
                    astInfoReqParam.SuppID = dt1.Rows[0]["SuppID"].ToString();
                    astInfoReqParam.CompanyID = Convert.ToInt32(newCompanyID);
                    astInfoReqParam.IsDataChanged = true;
                    astInfoReqParam.GLCode = GLCodes;
                    astInfoReqParam.ImageBase64 = ImageToBase64;
                    if (dt1.Rows[0]["LocID"].ToString() != "")
                    {
                        astInfoReqParam.LocID = dt1.Rows[0]["LocID"].ToString();
                    }
                    if ((bool)dt1.Rows[0]["Disposed"])
                    {
                        astInfoReqParam.DispCode = Convert.ToInt32(dt1.Rows[0]["Dispcode"]);
                        if (!string.IsNullOrEmpty(dt1.Rows[0]["DispDate"].ToString()))
                        {
                            astInfoReqParam.DispDate = (dt1.Rows[0]["DispDate"]).ToString();
                        }
                        if (dt1.Rows[0]["Dispcode"].ToString() == "1")
                        {
                            if (!string.IsNullOrEmpty(dt1.Rows[0]["Sel_date"].ToString()))
                            {
                                astInfoReqParam.Sel_Date = (dt1.Rows[0]["Sel_date"]).ToString();
                            }

                            if (!string.IsNullOrEmpty(dt1.Rows[0]["Sel_Price"].ToString()))
                            {
                                astInfoReqParam.Sel_Price = Convert.ToDouble(string.Format("{0:###,###,###,###,###.00}", Convert.ToDouble(dt1.Rows[0]["Sel_Price"].ToString())));
                            }

                            if (!string.IsNullOrEmpty(dt1.Rows[0]["Soldto"].ToString()))
                            {
                                astInfoReqParam.SoldTo = dt1.Rows[0]["Soldto"].ToString();
                            }
                        }
                    }

                    astInfoReqParam.OldAssetID = AstID;
                    astInfoReqParam.CreatedBy = astInfoReqParam.LoginName;
                    astInfoReqParam.LastEditBy = astInfoReqParam.LoginName;
                    DateTime currentDateTime = DateTime.Now;
                    astInfoReqParam.LastEditDate = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    astInfoReqParam.CreationDate = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    astInfoReqParam.CostCenterID = dt1.Rows[0]["CostID"].ToString();
                    astInfoReqParam.BussinessArea = dt1.Rows[0]["BussinessArea"].ToString();
                    astInfoReqParam.InventoryNumber = dt1.Rows[0]["InventoryNumber"].ToString();

                    astInfoReqParam.CustomFld1 = dt1.Rows[0]["CustomFld1"].ToString();
                    astInfoReqParam.CustomFld2 = dt1.Rows[0]["CustomFld2"].ToString();
                    astInfoReqParam.CustomFld3 = dt1.Rows[0]["CustomFld3"].ToString();
                    astInfoReqParam.CustomFld4 = dt1.Rows[0]["CustomFld4"].ToString();
                    astInfoReqParam.CustomFld5 = dt1.Rows[0]["CustomFld5"].ToString();

                    astInfoReqParam.EvaluationGroup1 = dt1.Rows[0]["EvaluationGroup1"].ToString();
                    astInfoReqParam.EvaluationGroup2 = dt1.Rows[0]["EvaluationGroup2"].ToString();
                    astInfoReqParam.EvaluationGroup3 = dt1.Rows[0]["EvaluationGroup3"].ToString();
                    astInfoReqParam.EvaluationGroup4 = dt1.Rows[0]["EvaluationGroup4"].ToString();

                    astInfoReqParam.StatusID = 1;
                    try
                    {
                        astInfoReqParam.Add = 1;
                        astInfoReqParam.BookID = "";
                        int count123 = Convert.ToInt32(DataLogic.CheckBookExistsAgainstCompanyID(newCompanyID, "[dbo].[SP_CheckBookExistsAgainstCompanyID]").Rows[0]["Books"].ToString());
                        if (count123 == 0)
                        {
                            //return "Please Create Book First for Transferred Company";
                            return Tuple.Create("Please Create Book First for Transferred Company", newAstID, refNo);
                            
                        }
                        else
                        {
                            DataTable qwe123 = DataLogic.InsertAssetDetails(astInfoReqParam, "[dbo].[SP_InsertUpdateDeleteAssetDetails]");

                            bool takuma = SetCompanyLastAssetNumber(newCompanyID, astNum);

                            string msgFromDB = qwe123.Rows[0]["Message"].ToString();
                            msg.message = msgFromDB;
                            if (msgFromDB.Contains("successfully"))
                            {
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Item", "Added", astItemReq.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Asset Details", 1, "Inserted", astInfoReqParam.LoginName, "dbo.AssetDetails");
                                if (qwe123.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                                {
                                    bool resu = imgFun.ConvertBase64toImg_SavetoServer(astInfoReqParam.ImageBase64, astInfoReqParam.AstID, "AssetImg");
                                }

                                DataTable dta = DataLogic.GetDepPolicyAgainstItemCode(astInfoReqParam.ItemCode, "[dbo].[SP_GetDepPolicyAgainstItemCode]");
                                if (dta.Rows.Count > 0)
                                {
                                    DeprecitionEngineReqParams depEngineReqParams = new DeprecitionEngineReqParams();
                                    depEngineReqParams.Get = 1;
                                    depEngineReqParams.CompanyID = Convert.ToInt32(newCompanyID);
                                    depEngineReqParams.PaginationParam.PageSize = 1000;
                                    depEngineReqParams.PaginationParam.PageIndex = 1;

                                    DataTable newqwe = DataLogic.GetAstBookAgainstCompanyIDForDeprecationNewAsset(depEngineReqParams, "[dbo].[SP_GetUpdateDepreciationEngineNewAsset]");
                                    if (newqwe.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in newqwe.Rows)
                                        {
                                            double cost = astInfoReqParam.BaseCost + astInfoReqParam.Tax;
                                            int BookIDInt = Convert.ToInt32(dr["BookID"].ToString());
                                            AstBooksUpdateDT.Rows.Add(
                                                newqwe.Rows[0]["BookID"].ToString(),//BookID 
                                                newAstID,//AstID
                                                Convert.ToInt32(dr["DepCode"]), //DepCode
                                                Convert.ToDecimal(salValue), //salValue
                                                Convert.ToInt32(salYr), //salYear
                                                0.0, //LastBV
                                                cost, //CurrentBV
                                                DateTime.Today.ToString(), //BVUpdate
                                                Convert.ToInt32(0), //SalMonth
                                                Convert.ToDecimal(0)); //SalvageValuePercentage

                                            if (!Check_BookExists(BookIDInt, AstID))
                                            {
                                                //DataTable dtbookHistoryMsg = DataLogic.InsertInAstBooks(AstBooksUpdateDT, "[dbo].[SP_InsertAssetIntoAstBooks]");
                                                DataTable dtbookHistoryMsg = DataLogic.InsertInBookHistory(0, AstBooksUpdateDT, "[dbo].[SP_InsertBookHistory]");
                                                
                                            }
                                        }
                                    }

                                    DataTable tqwe = DataLogic.DisposeAsset(AstID, "[dbo].[SP_DisposedAssetAfterInterCompanyTransfer]");

                                    string msgFromDB2 = tqwe.Rows[0]["Message"].ToString();
                                    if (msgFromDB2.Contains("Successfully"))//Successfully
                                    {
                                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Item", "Added", astItemReq.LoginName);
                                        DataTable dt299 = DataLogic.InsertAuditLogs("Asset Details", 1, "Updated", astInfoReqParam.LoginName, "dbo.AssetDetails");
                                        msg.message = "Asset Transferred successfully";
                                    }

                                    //DataTable newdt = DataLogic.Add_AstBook(newAstID, astInfoReqParam.BaseCost + astInfoReqParam.Tax, newCompanyID, currentBV, salYr, 0, DateTime.Today, False);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        msg.message = ex.Message.ToString();
                        return Tuple.Create(msg.message, newAstID, refNo);
                    }                    
                }
                else
                {
                    msg.message = "ReferenceID already exists";
                    return Tuple.Create(msg.message, newAstID, refNo);
                }
            }
            return Tuple.Create(msg.message, newAstID, refNo);
        }

        #endregion

        #region Location Logic

        public static DataTable MergeRows(DataTable table)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("InvLoc", typeof(string));

            string concatenatedProducts = string.Join("|", table.AsEnumerable().Select(row => row.Field<string>("InvLoc")));

            resultTable.Rows.Add(concatenatedProducts);

            return resultTable;
        }

        public static string MergeAllRowsWithQuotes(DataTable table)
        {
            // Concatenate all values in the Data column with single quotes, separated by '|'
            return string.Join("|", table.AsEnumerable().Select(row => $"'{row.Field<string>("InvLoc")}'"));
        }

        #endregion

        #region Report Functions

        #region InvSchCodes & InvLocs

        public static (string SelInvCode, string SelLoc, int rowCount) ConvertInvSchCodesInvLocs(List<string> invSchCodes, List<string> invLocs)
        {
            string SelInvCode = string.Empty;
            string SelLoc = string.Empty;
            int rowCount = 0;

            for (int i = 0; i < invSchCodes.Count; i++)
            {
                rowCount = i + 1;
                // Building SelInvCode
                SelInvCode = SelInvCode.Trim().Length == 0 ? invSchCodes[i] : SelInvCode + "," + invSchCodes[i];

                // Building SelLoc, replacing "|" with "','"
                SelLoc = SelLoc.Trim().Length == 0 ? "'" + invLocs[i].ToString().Replace("|", "','") + "'" : SelLoc + "," + "'" + invLocs[i].ToString().Replace("|", "','") + "'";
            }

            // Wrap in parentheses if needed
            if (SelInvCode.Trim().Length > 0)
            {
                SelInvCode = "(" + SelInvCode + ")";
            }
            if (SelLoc.Trim().Length > 0)
            {
                SelLoc = "(" + SelLoc + ")";
            }

            return (SelInvCode, SelLoc, rowCount); // Return as tuple
        }

        #endregion

        #endregion

    }
}
