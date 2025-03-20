using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using ZulAssetsBackEnd_API.BAL;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System;
using System.Security.Cryptography;
using System.Numerics;
using System.Globalization;
using Org.BouncyCastle.Asn1;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportDataController : ControllerBase
    {

        #region Declaration

        private static string SP_GetAllData = "[dbo].[SP_GetAllTableForImport]";
        private static string SP_ImportAssets = "[dbo].[SP_ImportAssets]";

        #endregion

        #region Import Excel File Data


        /// <param name="importDataReqParams"></param>
        [HttpPost("ImportData")]
        [Authorize]
        public IActionResult ImportData([FromBody] ImportDataReqParams importDataReqParams)
        {
            Message msg = new Message();
            try
            {
                // Get the start time
                DateTime startTime = DateTime.Now;

                #region Declarations

                DataTable importDataforCustomer = new DataTable();

                importDataforCustomer.Columns.Add("CustodianID");
                importDataforCustomer.Columns.Add("CustodianName");
                importDataforCustomer.Columns.Add("CustodianCode");
                importDataforCustomer.Columns.Add("DesignationID");

                DataTable importDataforSupplier = new DataTable();

                importDataforSupplier.Columns.Add("SuppID");
                importDataforSupplier.Columns.Add("SuppName");

                DataTable importDataforDesignation = new DataTable();

                importDataforDesignation.Columns.Add("DesignationID");
                importDataforDesignation.Columns.Add("Description");

                DataTable importDataforBrand = new DataTable();

                importDataforBrand.Columns.Add("AstBrandID");
                importDataforBrand.Columns.Add("AstBrandName");

                DataTable importDataforDepartment = new DataTable();

                importDataforDepartment.Columns.Add("DeptID");
                importDataforDepartment.Columns.Add("DeptName");

                DataTable importDataforCostCenter = new DataTable();

                importDataforCostCenter.Columns.Add("CostID");
                importDataforCostCenter.Columns.Add("CostNumber");
                importDataforCostCenter.Columns.Add("CostName");
                importDataforCostCenter.Columns.Add("CompanyID");

                DataTable importDataforLocation = new DataTable();

                importDataforLocation.Columns.Add("LocID");
                importDataforLocation.Columns.Add("LocDesc");
                importDataforLocation.Columns.Add("ID1");
                importDataforLocation.Columns.Add("Code");
                importDataforLocation.Columns.Add("CompCode");
                importDataforLocation.Columns.Add("LocLevel");
                importDataforLocation.Columns.Add("LocFullPath");
                importDataforLocation.Columns.Add("ParentID");
                importDataforLocation.Columns.Add("CompanyID");
                importDataforLocation.Columns.Add("isDeleted");

                DataTable importDataforCategory = new DataTable();

                importDataforCategory.Columns.Add("AstCatID");
                importDataforCategory.Columns.Add("CatDesc");
                importDataforCategory.Columns.Add("isDeleted");
                importDataforCategory.Columns.Add("ID1");
                importDataforCategory.Columns.Add("Code");
                importDataforCategory.Columns.Add("CompCode");
                importDataforCategory.Columns.Add("catLevel");
                importDataforCategory.Columns.Add("CatFullPath");
                importDataforCategory.Columns.Add("ParentID");

                DataTable importDataforAssets = new DataTable();

                importDataforAssets.Columns.Add("ItemCode");
                importDataforAssets.Columns.Add("AstBrandId");
                importDataforAssets.Columns.Add("AstCatID");
                importDataforAssets.Columns.Add("AstDesc");
                importDataforAssets.Columns.Add("IsDeleted");
                importDataforAssets.Columns.Add("AstQty");
                importDataforAssets.Columns.Add("Warranty");

                DataTable importDataforAssetDetails = new DataTable();

                importDataforAssetDetails.Columns.Add("AstID");
                importDataforAssetDetails.Columns.Add("ItemCode");
                importDataforAssetDetails.Columns.Add("SuppID");
                importDataforAssetDetails.Columns.Add("CustodianID");
                importDataforAssetDetails.Columns.Add("BaseCost");
                importDataforAssetDetails.Columns.Add("SrvDate");
                importDataforAssetDetails.Columns.Add("PurDate");
                importDataforAssetDetails.Columns.Add("InvSchCode");
                importDataforAssetDetails.Columns.Add("InvStatus");
                importDataforAssetDetails.Columns.Add("LocID");
                importDataforAssetDetails.Columns.Add("IsDeleted");
                importDataforAssetDetails.Columns.Add("RefNo");
                importDataforAssetDetails.Columns.Add("AstNum");
                importDataforAssetDetails.Columns.Add("AstBrandId");
                importDataforAssetDetails.Columns.Add("AstDesc");
                importDataforAssetDetails.Columns.Add("CompanyID");
                importDataforAssetDetails.Columns.Add("BarCode");
                importDataforAssetDetails.Columns.Add("SerialNo");
                importDataforAssetDetails.Columns.Add("AstDesc2");
                importDataforAssetDetails.Columns.Add("CostCenterID");
                importDataforAssetDetails.Columns.Add("CreatedBy");

                DataSet completeData = DataLogic.GetCustodianData(SP_GetAllData);

                DataTable CustodianFulldt = completeData.Tables["Table"];
                DataTable DesignationFulldt = completeData.Tables["Table1"];
                DataTable BrandFulldt = completeData.Tables["Table2"];
                DataTable DepartmentFulldt = completeData.Tables["Table3"];
                DataTable SupplierFulldt = completeData.Tables["Table4"];
                DataTable CostCenterFulldt = completeData.Tables["Table5"];
                DataTable LocationFulldt = completeData.Tables["Table6"];
                DataTable AssetsFulldt = completeData.Tables["Table7"];
                DataTable AssetDetailsFulldt = completeData.Tables["Table8"];
                DataTable CategoryFulldt = completeData.Tables["Table9"];

                CustodianFulldt.TableName = "CustodianTable";
                DesignationFulldt.TableName = "DesignationTable";
                BrandFulldt.TableName = "BrandTable";
                DepartmentFulldt.TableName = "DepartmentTable";
                SupplierFulldt.TableName = "SupplierTable";
                CostCenterFulldt.TableName = "CostCenterTable";
                LocationFulldt.TableName = "LocationTable";
                AssetsFulldt.TableName = "AssetsTable";
                AssetDetailsFulldt.TableName = "AssetDetailsTable";
                CategoryFulldt.TableName = "CategoryTable";


                DataTable insertCustodian = new DataTable();

                insertCustodian.Columns.Add("CustodianID");
                insertCustodian.Columns.Add("CustodianName");
                insertCustodian.Columns.Add("CustodianCode");
                insertCustodian.Columns.Add("DesignationID");
                insertCustodian.Columns.Add("isDeleted");


                DataTable insertDesignation = new DataTable();

                insertDesignation.Columns.Add("DesignationID");
                insertDesignation.Columns.Add("Description");
                insertDesignation.Columns.Add("isDeleted");

                DataTable insertBrand = new DataTable();
                insertBrand.Columns.Add("AstBrandID");
                insertBrand.Columns.Add("AstBrandName");
                insertBrand.Columns.Add("isDeleted");


                DataTable insertDepartment = new DataTable();
                insertDepartment.Columns.Add("DeptID");
                insertDepartment.Columns.Add("DeptName");
                insertDepartment.Columns.Add("isDeleted");


                DataTable insertSupplier = new DataTable();
                insertSupplier.Columns.Add("SuppID");
                insertSupplier.Columns.Add("SuppName");
                insertSupplier.Columns.Add("isDeleted");

                DataTable insertCostCenter = new DataTable();

                insertCostCenter.Columns.Add("CostID");
                insertCostCenter.Columns.Add("CostNumber");
                insertCostCenter.Columns.Add("CostName");
                insertCostCenter.Columns.Add("CompanyID");
                insertCostCenter.Columns.Add("isDeleted");


                DataTable insertLocation = new DataTable();

                insertLocation.Columns.Add("LocID");
                insertLocation.Columns.Add("LocDesc");
                insertLocation.Columns.Add("isDeleted");
                insertLocation.Columns.Add("ID1");
                insertLocation.Columns.Add("Code");
                insertLocation.Columns.Add("CompCode");
                insertLocation.Columns.Add("LocLevel");
                insertLocation.Columns.Add("LocationFullPath");

                insertLocation.Columns.Add("CompanyID");
                insertLocation.Columns.Add("ParentID");

                DataTable insertCategory = new DataTable();

                insertCategory.Columns.Add("AstCatID");
                insertCategory.Columns.Add("AstCatDesc");
                insertCategory.Columns.Add("isDeleted");
                insertCategory.Columns.Add("ID1");
                insertCategory.Columns.Add("Code");
                insertCategory.Columns.Add("CompCode");
                insertCategory.Columns.Add("catLevel");
                insertCategory.Columns.Add("CatFullPath");
                insertCategory.Columns.Add("ParentID");


                DataTable insertAssets = new DataTable();

                insertAssets.Columns.Add("ItemCode");
                insertAssets.Columns.Add("AstBrandId");
                insertAssets.Columns.Add("AstCatID");
                insertAssets.Columns.Add("AstDesc");
                insertAssets.Columns.Add("IsDeleted");
                insertAssets.Columns.Add("AstQty");
                insertAssets.Columns.Add("Warranty");


                DataTable insertAssetDetails = new DataTable();

                insertAssetDetails.Columns.Add("AstID");
                insertAssetDetails.Columns.Add("ItemCode");
                insertAssetDetails.Columns.Add("SuppID");
                insertAssetDetails.Columns.Add("CustodianID");
                insertAssetDetails.Columns.Add("BaseCost");
                insertAssetDetails.Columns.Add("SrvDate", typeof(DateTime));
                insertAssetDetails.Columns.Add("PurDate", typeof(DateTime));
                insertAssetDetails.Columns.Add("InvSchCode");
                insertAssetDetails.Columns.Add("InvStatus");
                insertAssetDetails.Columns.Add("LocID");
                insertAssetDetails.Columns.Add("IsDeleted");
                insertAssetDetails.Columns.Add("RefNo");
                insertAssetDetails.Columns.Add("AstNum");
                insertAssetDetails.Columns.Add("AstBrandId");
                insertAssetDetails.Columns.Add("AstDesc");
                insertAssetDetails.Columns.Add("CompanyID");
                insertAssetDetails.Columns.Add("BarCode");
                insertAssetDetails.Columns.Add("SerailNo");
                insertAssetDetails.Columns.Add("AstDesc2");
                insertAssetDetails.Columns.Add("CostCenterID");
                insertAssetDetails.Columns.Add("CreatedBY");

                DataTable updateAssetDetails = new DataTable();

                updateAssetDetails.Columns.Add("AstID");
                updateAssetDetails.Columns.Add("ItemCode");
                updateAssetDetails.Columns.Add("SuppID");
                updateAssetDetails.Columns.Add("CustodianID");
                updateAssetDetails.Columns.Add("BaseCost");
                updateAssetDetails.Columns.Add("SrvDate", typeof(DateTime));
                updateAssetDetails.Columns.Add("PurDate", typeof(DateTime));
                updateAssetDetails.Columns.Add("InvSchCode");
                updateAssetDetails.Columns.Add("InvStatus");
                updateAssetDetails.Columns.Add("LocID");
                updateAssetDetails.Columns.Add("IsDeleted");
                updateAssetDetails.Columns.Add("RefNo");
                updateAssetDetails.Columns.Add("AstNum");
                updateAssetDetails.Columns.Add("AstBrandId");
                updateAssetDetails.Columns.Add("AstDesc");
                updateAssetDetails.Columns.Add("CompanyID");
                updateAssetDetails.Columns.Add("BarCode");
                updateAssetDetails.Columns.Add("SerailNo");
                updateAssetDetails.Columns.Add("AstDesc2");
                updateAssetDetails.Columns.Add("CostCenterID");
                updateAssetDetails.Columns.Add("CreatedBY");


                string checkInItem = "Custodiann";

                //string addedBrandRows, updatedBrandRows, addedStoreRows, updatedStoreRows;
                string addedItemRows = string.Empty;
                string updatedItemRows = string.Empty;

                #endregion

                #region Conversion of Data from List to DataTable

                if (importDataReqParams.importData != null)
                {
                    importDataforCustomer = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforDesignation = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforBrand = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforDepartment = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforSupplier = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforCostCenter = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforLocation = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforAssets = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforAssetDetails = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }
                if (importDataReqParams.importData != null)
                {
                    importDataforCategory = ListintoDataTable.ToDataTable(importDataReqParams.importData);
                }

                #endregion

                #region Assets Import 

                int custodianCountt = CustodianFulldt.Rows.Count;
                int designationCountt = DesignationFulldt.Rows.Count;
                int brandCountt = BrandFulldt.Rows.Count;
                int deptCountt = DepartmentFulldt.Rows.Count;
                int costcenterCountt = CostCenterFulldt.Rows.Count;
                int assetsCountt = AssetsFulldt.Rows.Count;

                string designationCount = DesignationFulldt.Rows[designationCountt - 1]["DesignationID"].ToString();
                string custodianCount = CustodianFulldt.Rows[custodianCountt - 1]["CustodianID"].ToString();
                string brandCount = BrandFulldt.Rows[brandCountt - 1]["AstBrandID"].ToString();
                string departmentCount = DepartmentFulldt.Rows[deptCountt - 1]["DeptID"].ToString();
                string costcenterCount = CostCenterFulldt.Rows[costcenterCountt - 1]["CostID"].ToString();
                string assetsCount = AssetsFulldt.Rows[assetsCountt - 1]["ItemCode"].ToString();

                string astid = "";


                for (int k = 0; k < importDataforCustomer.Rows.Count; k++)
                {
                    int DesignationID = 1;
                    int CustodianID = 1;
                    int AstBrandID = 1;
                    int DeptID = 1;
                    string SuppID = "Unknown";
                    int CostID = 1;
                    string thirdlocation = "";
                    string finalcat = "";
                    string refno = importDataforCustomer.Rows[k]["BatchNo"].ToString();
                    string astnum = importDataforCustomer.Rows[k]["BatchNo"].ToString();
                    string serial = importDataforCustomer.Rows[k]["Serial"].ToString();
                    string itemDesc = "";
                    string astDesc2 = "";
                    int ItemCode = 0;
                    if (importDataforCustomer.Rows[k]["DescriptionEnglish"] == "")
                    {
                        itemDesc = "N/A";
                    }
                    else
                    {
                        itemDesc = importDataforCustomer.Rows[k]["DescriptionEnglish"].ToString();
                    }
                    if (importDataforCustomer.Rows[k]["AstDesc"] == "")
                    {
                        astDesc2 = "N/A";
                    }
                    else
                    {
                        astDesc2 = importDataforCustomer.Rows[k]["AstDesc"].ToString();
                    }
                    string baseCost;
                    if (importDataforCustomer.Rows[k]["PricePerPCs"] == "")
                    {
                        baseCost = "0";
                    }
                    else
                    {
                        baseCost = importDataforCustomer.Rows[k]["PricePerPCs"].ToString();
                    }

                    DateTime PDate;
                    string formattedDate = "";
                    if (importDataforCustomer.Rows[k]["ServiceDate"].ToString() != "")
                    {

                        if (DateTime.TryParse(importDataforCustomer.Rows[k]["ServiceDate"].ToString(), out PDate))
                        {
                            formattedDate = PDate.ToString("dd-MM-yyyy HH:mm:ss.fff");
                            // MessageBox.Show(formattedDate);
                        }
                        else
                        {
                            formattedDate = DateTime.ParseExact(importDataforCustomer.Rows[k]["ServiceDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
                            //formattedDate = PDate.ToString("dd-MM-yyyy HH:mm:ss.fff");
                        }
                    }
                    else
                    {
                        if (DateTime.TryParse(DateTime.Now.ToString(), out PDate))
                        {
                            formattedDate = PDate.ToString("dd-MM-yyyy HH:mm:ss.fff");
                            // MessageBox.Show(formattedDate);
                        }
                    }

                    #region For Category

                    string searchElementforCategory = importDataforCategory.Rows[k]["CategoryName"].ToString();
                    DataRow[] categoryrows = completeData.Tables["CategoryTable"].Select("AstCatDesc ='" + searchElementforCategory + "' AND catLevel = 0");
                    if (categoryrows.Length > 0)
                    {
                        foreach (DataRow row in categoryrows)
                        {
                            string astCatId = row["AstCatID"].ToString();
                            string astCatdesc = row["AstCatDesc"].ToString();
                            string code = row["Code"].ToString();
                            string ID1 = row["ID1"].ToString();
                            string comcode = row["CompCode"].ToString();
                            string categoryfullpath = row["catFullPath"].ToString();
                            string subastCatId = "";
                            string subastCatdesc = "";
                            string subcode = "";
                            string subID1 = "";
                            string subcomcode = "";
                            string subcategoryfullpath = "";

                            string searchElementforSubCategory = importDataforLocation.Rows[k]["SubCategory"].ToString();
                            DataRow[] subcategoryrows = completeData.Tables["CategoryTable"].Select("catFullPath ='" + categoryfullpath + " \\ " + searchElementforSubCategory + "'");

                            if (subcategoryrows.Length > 0)
                            { //for second location if exist
                                foreach (DataRow r in subcategoryrows)
                                {

                                    subastCatId = r["AstCatID"].ToString();
                                    subastCatdesc = r["AstCatDesc"].ToString();
                                    subcode = r["Code"].ToString();
                                    subID1 = r["ID1"].ToString();
                                    subcomcode = r["CompCode"].ToString();
                                    subcategoryfullpath = r["catFullPath"].ToString();
                                    finalcat = r["AstCatID"].ToString();
                                    //finalcat = subcomcode;    //Verify with Shahroz
                                }
                            }
                            else
                            {
                                DataRow insertItemRowd = CategoryFulldt.NewRow();
                                DataRow insertItemRowforCategory = insertCategory.NewRow();
                                int countForlocID2 = 0;
                                int countForlocID2_1 = 0;
                                int counted = CategoryFulldt.AsEnumerable().Count(row =>
                                {
                                    string cellValue = row.Field<string>("AstCatID");
                                    return !string.IsNullOrEmpty(cellValue) && Regex.IsMatch(cellValue, @"^" + astCatId + "-\\d+$");
                                });

                                string subcatIDloc = astCatId.ToString() + "-" + (counted + 1).ToString();
                                string subcatfullPath = categoryfullpath + " \\ " + importDataforCategory.Rows[k]["SubCategory"].ToString();
                                int co = CategoryFulldt.Rows.Count;
                                string subcatID1 = CategoryFulldt.Rows[co - 1]["ID1"].ToString();
                                int a = Convert.ToInt16(subcatID1) + 1;
                                subcatID1 = a.ToString();
                                string subCode = astCatId.ToString() + "-" + (counted + 1).ToString();
                                string subloccompCodeloc = comcode + " \\ " + subCode;

                                string subcatParent = ID1;
                                string subcatlevelloc = "1";
                                thirdlocation = subloccompCodeloc;

                                insertItemRowd["AstCatID"] = subcatIDloc;
                                insertItemRowd["AstCatDesc"] = importDataforCategory.Rows[k]["SubCategory"].ToString();
                                insertItemRowd["ID1"] = subcatID1;
                                insertItemRowd["Code"] = subCode;
                                insertItemRowd["CompCode"] = subloccompCodeloc;
                                insertItemRowd["CatLevel"] = 1;
                                insertItemRowd["catFullPath"] = subcatfullPath;
                                insertItemRowd["ParentID"] = ID1;
                                insertItemRowd["isDeleted"] = 0;

                                CategoryFulldt.Rows.Add(insertItemRowd);

                                insertItemRowforCategory["AstCatID"] = subcatIDloc;
                                insertItemRowforCategory["AstCatDesc"] = importDataforCategory.Rows[k]["SubCategory"].ToString();
                                insertItemRowforCategory["ID1"] = subcatID1;
                                insertItemRowforCategory["Code"] = subCode;
                                insertItemRowforCategory["CompCode"] = subloccompCodeloc;
                                insertItemRowforCategory["CatLevel"] = 1;
                                insertItemRowforCategory["catFullPath"] = subcatfullPath;
                                insertItemRowforCategory["ParentID"] = ID1;
                                insertItemRowforCategory["isDeleted"] = 0;

                                insertCategory.Rows.Add(insertItemRowforCategory);
                            }
                        }
                    }
                    else
                    {
                        int countforcatID = 0;
                        int countforID1 = 0;
                        int countforcatID2 = 0;
                        int countforID1_2 = 0;

                        for (int i = 0; i < CategoryFulldt.Rows.Count; i++)
                        {


                            // Check if the column value does not contain a dash
                            if (!CategoryFulldt.Rows[i]["AstCatID"].ToString().Contains("-"))
                            {
                                countforcatID = countforcatID + 1;
                            }
                            countforID1 = countforID1 + 1;
                        }
                        DataRow insertItemRow = insertCategory.NewRow();
                        insertItemRow["AstCatID"] = (countforcatID + 1).ToString(); //Verify this with Shahroz
                        //insertItemRow["AstCatID"] = (countforcatID + 2).ToString(); //yet to be done
                        insertItemRow["AstCatDesc"] = importDataforLocation.Rows[k]["CategoryName"].ToString();
                        int co = CategoryFulldt.Rows.Count;
                        string sublocID1loc = CategoryFulldt.Rows[co - 1]["ID1"].ToString();
                        int a = Convert.ToInt16(sublocID1loc) + 1;
                        countforID1 = a;
                        insertItemRow["ID1"] = a.ToString();

                        insertItemRow["Code"] = importDataforCategory.Rows[k]["Category"].ToString();
                        insertItemRow["CompCode"] = importDataforCategory.Rows[k]["Category"].ToString();
                        insertItemRow["CatLevel"] = 0;
                        insertItemRow["catFullPath"] = importDataforCategory.Rows[k]["CategoryName"].ToString();
                        insertItemRow["ParentID"] = "";
                        insertItemRow["isDeleted"] = 0;
                        insertCategory.Rows.Add(insertItemRow);
                        //CostID = int.Parse(costcenterCount) + 1;

                        DataRow insertItemRowd = CategoryFulldt.NewRow();
                        insertItemRowd["AstCatID"] = (countforcatID + 1).ToString(); //Verify this with Shahroz
                        //insertItemRowd["AstCatID"] = (countforcatID + 2).ToString(); //yet to be done
                        insertItemRowd["AstCatDesc"] = importDataforCategory.Rows[k]["CategoryName"].ToString();
                        insertItemRowd["ID1"] = countforID1;
                        //insertItemRowd["ID1"] = countforID1 + 1;  //Verify with Shahroz
                        insertItemRowd["Code"] = importDataforCategory.Rows[k]["Category"].ToString();
                        insertItemRowd["CompCode"] = importDataforCategory.Rows[k]["Category"].ToString();
                        insertItemRowd["CatLevel"] = 0;
                        insertItemRowd["catFullPath"] = importDataforCategory.Rows[k]["CategoryName"].ToString();
                        insertItemRowd["ParentID"] = "";
                        insertItemRowd["isDeleted"] = 0;

                        CategoryFulldt.Rows.Add(insertItemRowd);

                        //for second category
                        DataRow insertItemRow2 = insertCategory.NewRow();

                        insertItemRow2["AstCatID"] = (countforcatID + 1) + "-1".ToString(); //yet to be done. But Verify with Shahroz
                        //insertItemRow2["AstCatID"] = (countforcatID + 2) + "-1".ToString(); //yet to be done. But Verify with Shahroz
                        insertItemRow2["AstCatDesc"] = importDataforCategory.Rows[k]["SubCategory"].ToString();
                        insertItemRow2["ID1"] = countforID1 + 1;
                        insertItemRow2["Code"] = (countforcatID + 1) + "-1".ToString();//Verify with Shahroz
                        //insertItemRow2["Code"] = (countforcatID + 2) + "-1".ToString();//Verify with Shahroz
                        insertItemRow2["CompCode"] = importDataforCategory.Rows[k]["Category"].ToString() + " \\ " + (countforcatID + 1) + "-1".ToString(); ;
                        insertItemRow2["CatLevel"] = 1;
                        insertItemRow2["catFullPath"] = importDataforCategory.Rows[k]["CategoryName"].ToString() + " \\ " + importDataforCategory.Rows[k]["SubCategory"].ToString();
                        insertItemRow2["ParentID"] = countforID1;
                        insertItemRow2["isDeleted"] = 0;
                        insertCategory.Rows.Add(insertItemRow2);
                        //CostID = int.Parse(costcenterCount) + 1;

                        DataRow insertItemRowd2 = CategoryFulldt.NewRow();
                        insertItemRowd2["AstCatID"] = (countforcatID + 1) + "-1"; //Verify with Shahroz
                        //insertItemRowd2["AstCatID"] = (countforcatID + 2) + "-1"; //yet to be done
                        insertItemRowd2["AstCatDesc"] = importDataforCategory.Rows[k]["SubCategory"].ToString();
                        insertItemRowd2["ID1"] = countforID1 + 1;
                        insertItemRowd2["Code"] = (countforcatID + 1) + "-1".ToString();
                        //insertItemRowd2["Code"] = (countforcatID + 2) + "-1".ToString();  //Verify with Shahroz
                        insertItemRowd2["CompCode"] = importDataforCategory.Rows[k]["Category"].ToString() + " \\ " + (countforcatID + 1) + "-1".ToString(); ;
                        insertItemRowd2["CatLevel"] = 1;
                        insertItemRowd2["catFullPath"] = importDataforLocation.Rows[k]["CategoryName"].ToString() + " \\ " + importDataforLocation.Rows[k]["SubCategory"].ToString();
                        insertItemRowd2["ParentID"] = countforID1;
                        insertItemRowd2["isDeleted"] = 0;
                        finalcat = (countforcatID + 1) + "-1";
                        //finalcat = importDataforCategory.Rows[k]["Category"].ToString() + " \\ " + (countforcatID + 1) + "-1".ToString(); //Verify with Shahroz
                        CategoryFulldt.Rows.Add(insertItemRowd2);
                    }

                    #endregion

                    #region For Location

                    string searchElementforMainLocation = importDataforLocation.Rows[k]["MainLocation"].ToString();
                    DataRow[] mainlocationrows = completeData.Tables["LocationTable"].Select("LocDesc ='" + searchElementforMainLocation + "' AND LocLevel = 0");

                    if (mainlocationrows.Length > 0)
                    {
                        //first location

                        foreach (DataRow row in mainlocationrows)
                        {
                            int countforlocID = 0;
                            int countforID1 = 0;
                            int countforlocID2 = 0;
                            int countforID1_2 = 0;
                            int countforlocID3 = 0;
                            int countforID1_3 = 0;
                            DataRow insertItemRow = insertLocation.NewRow();


                            string locID = row["LocID"].ToString();
                            string locfullPath = row["LocationFullPath"].ToString();
                            string locID1 = row["ID1"].ToString();
                            string loccompCode = row["CompCode"].ToString();
                            string locParent = row["ParentID"].ToString();
                            string loclevel = row["LocLevel"].ToString();
                            string Code = row["Code"].ToString();
                            string locIDloc = "";
                            string locfullPathloc = "";
                            string locID1loc = "";
                            string loccompCodeloc = "";
                            string locParentloc = "";
                            string loclevelloc = "";
                            string Codeloc = "";

                            string searchElementforLocation = importDataforLocation.Rows[k]["Location"].ToString();
                            DataRow[] locationrows = completeData.Tables["LocationTable"].Select("LocationFullPath ='" + locfullPath + " \\ " + searchElementforLocation + "'");

                            if (locationrows.Length > 0)
                            {
                                //for second location if exist
                                foreach (DataRow r in locationrows)
                                {
                                    locIDloc = r["LocID"].ToString();
                                    locfullPathloc = r["LocationFullPath"].ToString();
                                    locID1loc = r["ID1"].ToString();
                                    loccompCodeloc = r["CompCode"].ToString();
                                    locParentloc = r["ParentID"].ToString();
                                    loclevelloc = r["LocLevel"].ToString();
                                    Codeloc = r["Code"].ToString();
                                }
                            }


                            //if second location is not available
                            else
                            {
                                DataRow insertItemRowd = LocationFulldt.NewRow();
                                DataRow insertItemRowforlocation = insertLocation.NewRow();
                                int countForlocID2 = 0;
                                int countForlocID2_1 = 0;

                                int counted = LocationFulldt.AsEnumerable().Count(row =>
                                {
                                    string cellValue = row.Field<string>("LocID");
                                    return !string.IsNullOrEmpty(cellValue) && Regex.IsMatch(cellValue, @"^" + locID + "-\\d+$");
                                });

                                locIDloc = locID.ToString() + "-" + (counted + 1).ToString();
                                locfullPathloc = locfullPath + " \\ " + importDataforLocation.Rows[k]["Location"].ToString();
                                locID1loc = LocationFulldt.Rows[LocationFulldt.Rows.Count - 1]["ID1"] + 1.ToString();
                                int co = LocationFulldt.Rows.Count;
                                locID1loc = LocationFulldt.Rows[co - 1]["ID1"].ToString();
                                int a = Convert.ToInt16(locID1loc) + 1;
                                locID1loc = a.ToString();
                                Codeloc = importDataforLocation.Rows[k]["LOCATIONCode"].ToString();
                                loccompCodeloc = loccompCode + " \\ " + Codeloc;
                                locParentloc = locID1;
                                loclevelloc = "1";

                                insertItemRowd["LocID"] = locIDloc;
                                insertItemRowd["LocDesc"] = importDataforLocation.Rows[k]["Location"].ToString();
                                insertItemRowd["ID1"] = locID1loc;
                                insertItemRowd["Code"] = Codeloc;
                                insertItemRowd["CompCode"] = loccompCodeloc;
                                insertItemRowd["LocLevel"] = 1;
                                insertItemRowd["LocationFullPath"] = locfullPathloc;
                                insertItemRowd["ParentID"] = locParentloc;
                                insertItemRowd["CompanyID"] = 1;
                                insertItemRowd["isDeleted"] = 0;

                                LocationFulldt.Rows.Add(insertItemRowd);

                                insertItemRowforlocation["LocID"] = locIDloc;
                                insertItemRowforlocation["LocDesc"] = importDataforLocation.Rows[k]["Location"].ToString();
                                insertItemRowforlocation["ID1"] = locID1loc;
                                insertItemRowforlocation["Code"] = Codeloc;
                                insertItemRowforlocation["CompCode"] = loccompCodeloc;
                                insertItemRowforlocation["LocLevel"] = 1;
                                insertItemRowforlocation["LocationFullPath"] = locfullPathloc;
                                insertItemRowforlocation["ParentID"] = locParentloc;
                                insertItemRowforlocation["CompanyID"] = 1;
                                insertItemRowforlocation["isDeleted"] = 0;

                                insertLocation.Rows.Add(insertItemRowforlocation);
                            }
                            //for third location
                            string searchElementforSubLocation = importDataforLocation.Rows[k]["SubLocationName"].ToString();
                            DataRow[] sublocationrows = completeData.Tables["LocationTable"].Select("LocationFullPath ='" + locfullPathloc + " \\ " + searchElementforSubLocation + "'");

                            if (sublocationrows.Length > 0)
                            {// if third location exist
                                foreach (DataRow ro in sublocationrows)
                                {
                                    string sublocIDloc = ro["LocID"].ToString();
                                    string sublocfullPathloc = ro["LocationFullPath"].ToString();
                                    string sublocID1loc = ro["ID1"].ToString();
                                    string subloccompCodeloc = ro["CompCode"].ToString();
                                    string sublocParentloc = locID1loc;
                                    string subloclevelloc = ro["LocLevel"].ToString();
                                    string subCodeloc = ro["Code"].ToString();
                                    thirdlocation = sublocIDloc;
                                }
                            }

                            else
                            {// if third location does not exist
                                DataRow insertItemRowdd = LocationFulldt.NewRow();
                                DataRow insertItemRowforlocationn = insertLocation.NewRow();
                                int countForlocID2 = 0;
                                int countForlocID2_1 = 0;

                                int counted = LocationFulldt.AsEnumerable().Count(row =>
                                {
                                    string cellValue = row.Field<string>("LocID");
                                    return !string.IsNullOrEmpty(cellValue) && Regex.IsMatch(cellValue, @"^1-1-\d+$");
                                });

                                string sublocIDloc = locIDloc.ToString() + "-" + (counted + 1).ToString();
                                string sublocfullPathloc = locfullPathloc + " \\ " + importDataforLocation.Rows[k]["SublocationName"].ToString();
                                int co = LocationFulldt.Rows.Count;
                                string sublocID1loc = LocationFulldt.Rows[co - 1]["ID1"].ToString();
                                int a = Convert.ToInt16(sublocID1loc) + 1;
                                sublocID1loc = a.ToString();
                                string subCodeloc = importDataforLocation.Rows[k]["SublocationCode"].ToString();
                                string subloccompCodeloc = loccompCodeloc + " \\ " + subCodeloc;
                                string sublocParentloc = locID1loc;
                                string subloclevelloc = "2";
                                thirdlocation = sublocIDloc;

                                insertItemRowdd["LocID"] = sublocIDloc;
                                insertItemRowdd["LocDesc"] = importDataforLocation.Rows[k]["SublocationName"].ToString();
                                insertItemRowdd["ID1"] = sublocID1loc;
                                insertItemRowdd["Code"] = subCodeloc;
                                insertItemRowdd["CompCode"] = subloccompCodeloc;
                                insertItemRowdd["LocLevel"] = 1;
                                insertItemRowdd["LocationFullPath"] = sublocfullPathloc;
                                insertItemRowdd["ParentID"] = locID1loc;
                                insertItemRowdd["CompanyID"] = 1;
                                insertItemRowdd["isDeleted"] = 0;

                                LocationFulldt.Rows.Add(insertItemRowdd);

                                insertItemRowforlocationn["LocID"] = sublocIDloc;
                                insertItemRowforlocationn["LocDesc"] = importDataforLocation.Rows[k]["SublocationName"].ToString();
                                insertItemRowforlocationn["ID1"] = sublocID1loc;
                                insertItemRowforlocationn["Code"] = subCodeloc;
                                insertItemRowforlocationn["CompCode"] = subloccompCodeloc;
                                insertItemRowforlocationn["LocLevel"] = 1;
                                insertItemRowforlocationn["LocationFullPath"] = sublocfullPathloc;
                                insertItemRowforlocationn["ParentID"] = locID1loc;
                                insertItemRowforlocationn["CompanyID"] = 1;
                                insertItemRowforlocationn["isDeleted"] = 0;

                                insertLocation.Rows.Add(insertItemRowforlocationn);
                            }

                        }

                    }
                    else // if the main location is not created
                    {
                        DataRow[] rows1 = insertLocation.Select("LocDesc ='" + searchElementforMainLocation + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {

                            int countforlocID = 0;
                            int countforID1 = 0;
                            int countforlocID2 = 0;
                            int countforID1_2 = 0;
                            int countforlocID3 = 0;
                            int countforID1_3 = 0;
                            for (int i = 0; i < LocationFulldt.Rows.Count; i++)
                            {
                                // Check if the column value does not contain a dash
                                if (!LocationFulldt.Rows[i]["LocID"].ToString().Contains("-"))
                                {
                                    countforlocID = countforlocID + 1;
                                }
                                countforID1 = countforID1 + 1;
                            }
                            DataRow insertItemRow = insertLocation.NewRow();
                            insertItemRow["LocID"] = (countforlocID + 1).ToString(); //yet to be done
                            insertItemRow["LocDesc"] = importDataforLocation.Rows[k]["MainLocation"].ToString();
                            int co = LocationFulldt.Rows.Count;
                            string sublocID1loc = LocationFulldt.Rows[co - 1]["ID1"].ToString();
                            int a = Convert.ToInt16(sublocID1loc) + 1;
                            countforID1 = a;
                            insertItemRow["ID1"] = a.ToString();

                            insertItemRow["Code"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString();
                            insertItemRow["CompCode"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString();
                            insertItemRow["LocLevel"] = 0;
                            insertItemRow["LocationFullPath"] = importDataforLocation.Rows[k]["MainLocation"].ToString();
                            insertItemRow["ParentID"] = "";
                            insertItemRow["CompanyID"] = 1;
                            insertItemRow["isDeleted"] = 0;
                            insertLocation.Rows.Add(insertItemRow);
                            //CostID = int.Parse(costcenterCount) + 1;

                            DataRow insertItemRowd = LocationFulldt.NewRow();
                            insertItemRowd["LocID"] = (countforlocID + 1).ToString(); //yet to be done
                            insertItemRowd["LocDesc"] = importDataforLocation.Rows[k]["MainLocation"].ToString();
                            insertItemRowd["ID1"] = countforID1;
                            //insertItemRowd["ID1"] = countforID1 + 1;  //Verify with Shahroz
                            insertItemRowd["Code"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString();
                            insertItemRowd["CompCode"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString();
                            insertItemRowd["LocLevel"] = 0;
                            insertItemRowd["LocationFullPath"] = importDataforLocation.Rows[k]["MainLocation"].ToString();
                            insertItemRowd["ParentID"] = "";
                            insertItemRowd["CompanyID"] = 1;
                            insertItemRowd["isDeleted"] = 0;

                            LocationFulldt.Rows.Add(insertItemRowd);

                            //for second location
                            DataRow insertItemRow2 = insertLocation.NewRow();

                            insertItemRow2["LocID"] = (countforlocID + 1) + "-1".ToString(); //yet to be done
                            insertItemRow2["LocDesc"] = importDataforLocation.Rows[k]["Location"].ToString();
                            insertItemRow2["ID1"] = countforID1 + 1;
                            insertItemRow2["Code"] = importDataforLocation.Rows[k]["LocationCode"].ToString();
                            insertItemRow2["CompCode"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString() + " \\ " + importDataforLocation.Rows[k]["LocationCode"].ToString();
                            insertItemRow2["LocLevel"] = 1;
                            insertItemRow2["LocationFullPath"] = importDataforLocation.Rows[k]["MainLocation"].ToString() + " \\ " + importDataforLocation.Rows[k]["Location"].ToString();
                            insertItemRow2["ParentID"] = countforID1;
                            insertItemRow2["CompanyID"] = 1;
                            insertItemRow2["isDeleted"] = 0;
                            insertLocation.Rows.Add(insertItemRow2);
                            //CostID = int.Parse(costcenterCount) + 1;

                            DataRow insertItemRowd2 = LocationFulldt.NewRow();
                            insertItemRowd2["LocID"] = (countforlocID + 1) + "-1"; //yet to be done
                            insertItemRowd2["LocDesc"] = importDataforLocation.Rows[k]["Location"].ToString();
                            insertItemRowd2["ID1"] = countforID1 + 1;
                            insertItemRowd2["Code"] = importDataforLocation.Rows[k]["LocationCode"].ToString();
                            insertItemRowd2["CompCode"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString() + " \\ " + importDataforLocation.Rows[k]["LocationCode"].ToString();
                            insertItemRowd2["LocLevel"] = 1;
                            insertItemRowd2["LocationFullPath"] = importDataforLocation.Rows[k]["MainLocation"].ToString() + " \\ " + importDataforLocation.Rows[k]["Location"].ToString();
                            insertItemRowd2["ParentID"] = countforID1;
                            insertItemRowd2["CompanyID"] = 1;
                            insertItemRowd2["isDeleted"] = 0;
                            thirdlocation = importDataforLocation.Rows[k]["MainLocation"].ToString() + " \\ " + importDataforLocation.Rows[k]["Location"].ToString();
                            LocationFulldt.Rows.Add(insertItemRowd2);

                            //third loc
                            DataRow insertItemRow3 = insertLocation.NewRow();

                            insertItemRow3["LocID"] = (countforlocID + 1) + "-1-1".ToString(); //yet to be done
                            insertItemRow3["LocDesc"] = importDataforLocation.Rows[k]["SubLocationName"].ToString();
                            insertItemRow3["ID1"] = countforID1 + 2;
                            insertItemRow3["Code"] = importDataforLocation.Rows[k]["SubLocationCode"].ToString();
                            insertItemRow3["CompCode"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString() + " \\ " + importDataforLocation.Rows[k]["LocationCode"].ToString() + " \\ " + importDataforLocation.Rows[k]["SubLocationCode"].ToString();
                            insertItemRow3["LocLevel"] = 2;
                            insertItemRow3["LocationFullPath"] = importDataforLocation.Rows[k]["MainLocation"].ToString() + " \\ " + importDataforLocation.Rows[k]["Location"].ToString() + " \\ " + importDataforLocation.Rows[k]["SublocationName"].ToString();
                            insertItemRow3["ParentID"] = countforID1 + 1;
                            insertItemRow3["CompanyID"] = 1;
                            insertItemRow3["isDeleted"] = 0;
                            insertLocation.Rows.Add(insertItemRow3);
                            //CostID = int.Parse(costcenterCount) + 1;

                            DataRow insertItemRowd3 = LocationFulldt.NewRow();
                            insertItemRowd3["LocID"] = (countforlocID + 1) + "-1-1"; //yet to be done
                            insertItemRowd3["LocDesc"] = importDataforLocation.Rows[k]["SubLocationName"].ToString();
                            insertItemRowd3["ID1"] = countforID1 + 2;
                            insertItemRowd3["Code"] = importDataforLocation.Rows[k]["SubLocationCode"].ToString();
                            insertItemRowd3["CompCode"] = importDataforLocation.Rows[k]["MainLocationCode"].ToString() + " \\ " + importDataforLocation.Rows[k]["LocationCode"].ToString() + " \\ " + importDataforLocation.Rows[k]["SubLocationCode"].ToString();
                            insertItemRowd3["LocLevel"] = 2;
                            insertItemRowd3["LocationFullPath"] = importDataforLocation.Rows[k]["MainLocation"].ToString() + " \\ " + importDataforLocation.Rows[k]["Location"].ToString() + " \\ " + importDataforLocation.Rows[k]["SublocationName"].ToString();
                            insertItemRowd3["ParentID"] = countforID1 + 1;
                            insertItemRowd3["CompanyID"] = 1;
                            insertItemRowd3["isDeleted"] = 0;

                            LocationFulldt.Rows.Add(insertItemRowd3);

                            thirdlocation = (countforlocID + 1) + "-1-1";
                        }

                    }

                    #endregion

                    #region For CostCenter

                    string searchElementforCostCenter = importDataforCostCenter.Rows[k]["CCDescriptionLocation"].ToString();
                    DataRow[] rowssssss = completeData.Tables["CostCenterTable"].Select("CostName ='" + searchElementforCostCenter + "'");

                    if (rowssssss.Length > 0)
                    {
                        foreach (DataRow row in rowssssss)
                        {
                            CostID = Convert.ToInt16(row["CostID"]);
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertCostCenter.Select("CostName ='" + searchElementforCostCenter + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertCostCenter.NewRow();
                            insertItemRow["CostID"] = int.Parse(costcenterCount) + 1;
                            insertItemRow["CostName"] = importDataforCostCenter.Rows[k]["CCDescriptionLocation"].ToString();
                            insertItemRow["CostNumber"] = importDataforCostCenter.Rows[k]["CC"].ToString();
                            insertItemRow["CompanyID"] = 1;
                            insertCostCenter.Rows.Add(insertItemRow);
                            CostID = int.Parse(costcenterCount) + 1;

                            DataRow insertItemRowd = CostCenterFulldt.NewRow();
                            insertItemRowd["CostID"] = int.Parse(costcenterCount) + 1;
                            insertItemRowd["CostName"] = importDataforCostCenter.Rows[k]["CCDescriptionLocation"].ToString();
                            insertItemRowd["CostNumber"] = importDataforCostCenter.Rows[k]["CC"].ToString();
                            insertItemRowd["CompanyID"] = 1;
                            insertItemRowd["isDeleted"] = 0;
                            CostCenterFulldt.Rows.Add(insertItemRowd);
                        }
                    }

                    #endregion

                    #region For Vendor

                    string searchElementforSupplier = importDataforBrand.Rows[k]["VendorName"].ToString();
                    DataRow[] rowsssss = completeData.Tables["SupplierTable"].Select("SuppName ='" + searchElementforSupplier + "'");

                    if (rowsssss.Length > 0)
                    {
                        foreach (DataRow row in rowsssss)
                        {
                            SuppID = row["SuppID"].ToString();
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertSupplier.Select("SuppName ='" + searchElementforSupplier + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertSupplier.NewRow();
                            insertItemRow["SuppID"] = importDataforSupplier.Rows[k]["VendorAccountNumber"].ToString();
                            insertItemRow["SuppName"] = importDataforSupplier.Rows[k]["VendorName"].ToString();
                            insertItemRow["isDeleted"] = 0;
                            insertSupplier.Rows.Add(insertItemRow);
                            SuppID = importDataforDepartment.Rows[k]["VendorAccountNumber"].ToString();

                            DataRow insertItemRowd = SupplierFulldt.NewRow();
                            insertItemRowd["SuppID"] = importDataforSupplier.Rows[k]["VendorAccountNumber"].ToString();
                            insertItemRowd["SuppName"] = importDataforSupplier.Rows[k]["VendorName"].ToString();
                            insertItemRowd["isDeleted"] = 0;
                            SupplierFulldt.Rows.Add(insertItemRowd);
                        }
                    }

                    #endregion

                    #region For Department

                    string searchElementforDepartment = importDataforBrand.Rows[k]["Department"].ToString();
                    DataRow[] rowssss = completeData.Tables["DepartmentTable"].Select("DeptName ='" + searchElementforDepartment + "'");

                    if (rowssss.Length > 0)
                    {
                        foreach (DataRow row in rowssss)
                        {
                            DeptID = DesignationID = Convert.ToInt16(row["DeptID"]);
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertDepartment.Select("DeptName ='" + searchElementforDepartment + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertDepartment.NewRow();
                            insertItemRow["DeptID"] = int.Parse(departmentCount) + 1;
                            insertItemRow["DeptName"] = importDataforDepartment.Rows[k]["Department"].ToString();
                            insertItemRow["isDeleted"] = 0;
                            insertDepartment.Rows.Add(insertItemRow);
                            DeptID = int.Parse(departmentCount) + 1;

                            DataRow insertItemRowd = DepartmentFulldt.NewRow();
                            insertItemRowd["DeptID"] = int.Parse(departmentCount) + 1;
                            insertItemRowd["DeptName"] = importDataforDepartment.Rows[k]["Department"].ToString();
                            insertItemRowd["isDeleted"] = 0;
                            DepartmentFulldt.Rows.Add(insertItemRowd);
                        }
                    }

                    #endregion

                    #region For Brand

                    string searchElementforBrand = importDataforBrand.Rows[k]["Brand"].ToString();
                    DataRow[] rowsss = completeData.Tables["BrandTable"].Select("AstBrandName ='" + searchElementforBrand + "'");

                    if (rowsss.Length > 0)
                    {
                        foreach (DataRow row in rowsss)
                        {
                            AstBrandID = Convert.ToInt16(row["AstBrandID"]);
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertBrand.Select("AstBrandName ='" + searchElementforBrand + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertBrand.NewRow();
                            insertItemRow["AstBrandID"] = int.Parse(brandCount) + 1;
                            insertItemRow["AstBrandName"] = importDataforBrand.Rows[k]["Brand"].ToString();
                            insertBrand.Rows.Add(insertItemRow);
                            AstBrandID = int.Parse(brandCount) + 1;

                            DataRow insertItemRowd = BrandFulldt.NewRow();
                            insertItemRowd["AstBrandID"] = int.Parse(brandCount) + 1;
                            insertItemRowd["AstBrandName"] = importDataforBrand.Rows[k]["Brand"].ToString();
                            insertItemRowd["isDeleted"] = 0;
                            BrandFulldt.Rows.Add(insertItemRowd);
                        }
                    }

                    #endregion

                    #region For Designation

                    string searchElementforDesignation = importDataforDesignation.Rows[k]["CustodianPosition"].ToString();
                    DataRow[] rowss = completeData.Tables["DesignationTable"].Select("Description ='" + searchElementforDesignation + "'");

                    if (rowss.Length > 0)
                    {
                        foreach (DataRow row in rowss)
                        {
                            DesignationID = Convert.ToInt16(row["DesignationID"]);
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertDesignation.Select("Description ='" + searchElementforDesignation + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertDesignation.NewRow();
                            insertItemRow["DesignationID"] = int.Parse(designationCount) + 1;
                            insertItemRow["Description"] = importDataforDesignation.Rows[k]["CustodianPosition"].ToString();
                            insertDesignation.Rows.Add(insertItemRow);
                            DesignationID = int.Parse(designationCount) + 1;

                            DataRow insertItemRowd = DesignationFulldt.NewRow();
                            insertItemRowd["DesignationID"] = int.Parse(designationCount) + 1;
                            insertItemRowd["Description"] = importDataforDesignation.Rows[k]["CustodianPosition"].ToString();
                            insertItemRowd["isDeleted"] = 0;
                            DesignationFulldt.Rows.Add(insertItemRowd);
                        }
                    }

                    #endregion

                    #region For Custodian

                    string searchElement = importDataforCustomer.Rows[k]["CustodianName"].ToString();
                    DataRow[] rows = completeData.Tables["CustodianTable"].Select("custodianName ='" + searchElement + "'");

                    if (rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            CustodianID = Convert.ToInt16(row["CustodianID"]);
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertCustodian.Select("CustodianName ='" + searchElement + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertCustodian.NewRow();
                            insertItemRow["CustodianID"] = int.Parse(custodianCount) + 1;
                            insertItemRow["CustodianName"] = importDataforCustomer.Rows[k]["CustodianName"].ToString();
                            insertItemRow["CustodianCode"] = importDataforCustomer.Rows[k]["CustodianCode"].ToString();
                            insertItemRow["DesignationID"] = DesignationID.ToString();
                            insertItemRow["isDeleted"] = 0;

                            insertCustodian.Rows.Add(insertItemRow);
                            CustodianID = int.Parse(custodianCount) + 1;

                            DataRow insertCustodianRowd = CustodianFulldt.NewRow();
                            insertCustodianRowd["CustodianID"] = int.Parse(custodianCount) + 1;
                            insertCustodianRowd["CustodianName"] = importDataforCustomer.Rows[k]["CustodianName"].ToString();
                            insertCustodianRowd["CustodianCode"] = importDataforCustomer.Rows[k]["CustodianCode"].ToString();
                            insertCustodianRowd["DesignationID"] = DesignationID.ToString();
                            insertCustodianRowd["isDeleted"] = 0;
                            CustodianFulldt.Rows.Add(insertCustodianRowd);
                        }
                    }

                    #endregion

                    #region For Assets

                    string searchElementforAssets = importDataforAssets.Rows[k]["AstDesc"].ToString();
                    DataRow[] rowsforasset = completeData.Tables["AssetsTable"].Select("AstDesc ='" + searchElementforAssets + "'");

                    if (rowsforasset.Length > 0)
                    {
                        foreach (DataRow row in rowsforasset)
                        {
                            ItemCode = Convert.ToInt16(row["ItemCode"]);
                            // Do something with the data
                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertAssets.Select("AstDesc ='" + searchElementforAssets + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            DataRow insertItemRow = insertAssets.NewRow();
                            insertItemRow["ItemCode"] = int.Parse(assetsCount) + 1;

                            assetsCount = (Convert.ToInt32(assetsCount) + 1).ToString();

                            insertItemRow["AstBrandID"] = AstBrandID;
                            insertItemRow["AstCatID"] = finalcat;
                            insertItemRow["AstDesc"] = importDataforAssets.Rows[k]["AstDesc"].ToString();
                            insertItemRow["isDeleted"] = 0;
                            insertItemRow["AstQty"] = 1;
                            insertItemRow["Warranty"] = 0;
                            insertAssets.Rows.Add(insertItemRow);
                            ItemCode = int.Parse(assetsCount) + 1;

                            DataRow insertItemRowd = AssetsFulldt.NewRow();
                            insertItemRowd["ItemCode"] = int.Parse(assetsCount) + 1;
                            insertItemRowd["AstBrandID"] = AstBrandID;
                            insertItemRowd["AstCatID"] = finalcat;
                            insertItemRowd["AstDesc"] = importDataforAssets.Rows[k]["AstDesc"].ToString();
                            insertItemRowd["isDeleted"] = 0;
                            insertItemRowd["AstQty"] = 1;
                            insertItemRowd["Warranty"] = 0;
                            AssetsFulldt.Rows.Add(insertItemRowd);
                        }
                    }

                    #endregion

                    #region For Assets Details

                    string searchElementforAssetDetails = importDataforAssetDetails.Rows[k]["BatchNo"].ToString();
                    DataRow[] rowsforassetdetails = completeData.Tables["AssetDetailsTable"].Select("RefNo ='" + searchElementforAssetDetails + "'");

                    if (rowsforassetdetails.Length > 0)
                    {
                        foreach (DataRow row in rowsforassetdetails)
                        {

                            DataRow updateAssetDetailsRow = updateAssetDetails.NewRow();

                            updateAssetDetailsRow["AstID"] = row["AstID"];
                            updateAssetDetailsRow["ItemCode"] = ItemCode;
                            updateAssetDetailsRow["SuppID"] = SuppID;
                            updateAssetDetailsRow["CustodianID"] = CustodianID;
                            updateAssetDetailsRow["BaseCost"] = baseCost;
                            updateAssetDetailsRow["SrvDate"] = formattedDate;
                            updateAssetDetailsRow["PurDate"] = formattedDate;
                            updateAssetDetailsRow["InvSchCode"] = row["InvSchCode"];
                            updateAssetDetailsRow["InvStatus"] = row["InvStatus"];
                            updateAssetDetailsRow["LocID"] = thirdlocation;
                            updateAssetDetailsRow["IsDeleted"] = 0;
                            updateAssetDetailsRow["RefNo"] = refno;
                            updateAssetDetailsRow["AstNum"] = astnum;
                            updateAssetDetailsRow["AstBrandId"] = AstBrandID;
                            updateAssetDetailsRow["AstDesc"] = itemDesc;
                            updateAssetDetailsRow["CompanyID"] = row["CompanyID"];
                            updateAssetDetailsRow["BarCode"] = astid;
                            updateAssetDetailsRow["SerailNo"] = serial;
                            updateAssetDetailsRow["AstDesc2"] = astDesc2;
                            updateAssetDetailsRow["CostCenterID"] = CostID;
                            updateAssetDetailsRow["CreatedBY"] = "Import Process";

                            updateAssetDetails.Rows.Add(updateAssetDetailsRow);

                        }

                    }
                    else
                    {
                        DataRow[] rows1 = insertAssetDetails.Select("RefNo ='" + searchElementforAssetDetails + "'");
                        if (rows1.Length > 0)
                        {
                            string msg123 = "do nothing";
                        }
                        else
                        {
                            int quan = Convert.ToInt16(importDataforAssetDetails.Rows[k]["Quantity"]);
                            for (int q = 0; q < quan; q++)
                            {
                                DataRow insertItemRow = insertAssetDetails.NewRow();
                                DateTime now = DateTime.Now;
                                long microseconds = (now.Ticks % TimeSpan.TicksPerSecond) / 10;
                                string formatted = now.ToString("yy")  // Year (2 digits)
                                + now.ToString("dd")  // Day (2 digits)
                                + now.ToString("MM")  // Month (2 digits)
                                + now.ToString("HH")  // Hour (2 digits, 24-hour format)
                                + now.ToString("mm")  // Minutes (2 digits)
                                + now.ToString("ss")  // Seconds (2 digits)
                                + microseconds.ToString("000000").Substring(0, 2);

                                if (astid.Trim() == formatted.Trim())
                                {
                                    Thread.Sleep(1000);
                                    microseconds = (now.Ticks % TimeSpan.TicksPerSecond) / 10;
                                    now = DateTime.Now;

                                    formatted = now.ToString("yy")  // Year (2 digits)
                                    + now.ToString("dd")  // Day (2 digits)
                                    + now.ToString("MM")  // Month (2 digits)
                                    + now.ToString("HH")  // Hour (2 digits, 24-hour format)
                                    + now.ToString("mm")  // Minutes (2 digits)
                                    + now.ToString("ss")  // Seconds (2 digits)
                                    + microseconds.ToString("000000").Substring(0, 2);
                                }

                                astid = formatted;
                                insertItemRow["AstID"] = astid;
                                insertItemRow["ItemCode"] = ItemCode;
                                insertItemRow["SuppID"] = SuppID;
                                insertItemRow["CustodianID"] = CustodianID;
                                insertItemRow["BaseCost"] = baseCost;
                                insertItemRow["SrvDate"] = formattedDate;
                                insertItemRow["PurDate"] = formattedDate;
                                insertItemRow["InvSchCode"] = "1";
                                insertItemRow["InvStatus"] = "0";
                                insertItemRow["LocID"] = thirdlocation;
                                insertItemRow["IsDeleted"] = 0;
                                insertItemRow["RefNo"] = refno;
                                insertItemRow["AstNum"] = astnum;
                                insertItemRow["AstBrandId"] = AstBrandID;
                                insertItemRow["AstDesc"] = itemDesc;
                                insertItemRow["CompanyID"] = "1";
                                insertItemRow["BarCode"] = astid;
                                insertItemRow["SerailNo"] = serial;
                                insertItemRow["AstDesc2"] = astDesc2;
                                insertItemRow["CostCenterID"] = CostID;
                                insertItemRow["CreatedBY"] = "Import Process";

                                insertAssetDetails.Rows.Add(insertItemRow);

                                DataRow insertItemRowd = AssetDetailsFulldt.NewRow();
                                insertItemRowd["AstID"] = astid;
                                insertItemRowd["ItemCode"] = ItemCode;
                                insertItemRowd["SuppID"] = SuppID;
                                insertItemRowd["CustodianID"] = CustodianID;
                                insertItemRowd["BaseCost"] = baseCost;
                                insertItemRowd["SrvDate"] = PDate.ToString();
                                insertItemRowd["PurDate"] = PDate.ToString();
                                insertItemRowd["InvSchCode"] = 1;
                                insertItemRowd["InvStatus"] = 0;
                                insertItemRowd["LocID"] = thirdlocation;
                                insertItemRowd["IsDeleted"] = 0;
                                insertItemRowd["RefNo"] = refno;
                                insertItemRowd["AstNum"] = astnum;
                                insertItemRowd["AstBrandId"] = AstBrandID;
                                insertItemRowd["AstDesc"] = itemDesc;
                                insertItemRowd["CompanyID"] = "1";
                                insertItemRowd["BarCode"] = astid;
                                insertItemRowd["SerailNo"] = serial;
                                insertItemRowd["AstDesc2"] = astDesc2;
                                insertItemRowd["CostCenterID"] = CostID;
                                insertItemRowd["CreatedBy"] = "Import Process";
                                AssetDetailsFulldt.Rows.Add(insertItemRowd);

                            }
                        }
                    }

                    #endregion

                }

                #region Data Insert Sections

                #region Cost Center Insert

                if (insertCostCenter.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("CostCenter", 1, insertCostCenter, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Supplier Insert

                if (insertSupplier.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Supplier", 1, insertSupplier, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Department Insert

                if (insertDepartment.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Department", 1, insertDepartment, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Brand Insert

                if (insertBrand.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Brand", 1, insertBrand, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Designation Insert

                if (insertDesignation.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Designation", 1, insertDesignation, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Custodian Insert

                if (insertCustodian.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Custodian", 1, insertCustodian, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Location Insert

                if (insertLocation.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Location", 1, insertLocation, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Category Insert

                if (insertCategory.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Category", 1, insertCategory, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Asset Insert

                if (insertAssets.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("Assets", 1, insertAssets, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();
                    }
                }

                #endregion

                #region Asset Details

                #region Asset Details Insert

                if (insertAssetDetails.Rows.Count > 0)
                {
                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("AssetDetails", 1, insertAssetDetails, SP_ImportAssets);
                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        addedItemRows = itemInsertResponse.Rows[0]["AddedRows"].ToString();

                        DataTable insertAstBooks = new DataTable();

                        insertAstBooks.Columns.Add("BookID");
                        insertAstBooks.Columns.Add("AstID");
                        insertAstBooks.Columns.Add("DepCode");
                        insertAstBooks.Columns.Add("SalvageValue");
                        insertAstBooks.Columns.Add("SalvageYear");
                        insertAstBooks.Columns.Add("LastBV");
                        insertAstBooks.Columns.Add("CurrentBV");
                        insertAstBooks.Columns.Add("BVUpdate");
                        insertAstBooks.Columns.Add("SalvageMonth");
                        insertAstBooks.Columns.Add("SalvageValuePercent");

                        GeneralFunctions GF = new GeneralFunctions();

                        DataTable dtBookInfoAgainstCompanyID = DataLogic.GetBookAgainstCompanyID("1", "[dbo].[SP_GetBooksAgainstCompanyID]");
                        for (int i = 0; i < insertAssetDetails.Rows.Count; i++)
                        {
                            DataTable dtGetDepPolicyAgainstItemCode = DataLogic.GetDepPolicyAgainstItemCode(insertAssetDetails.Rows[i]["ItemCode"].ToString(), "[dbo].[SP_GetDepPolicyAgainstItemCode]");
                            if (GF.Check_BookExists(Convert.ToInt32(dtBookInfoAgainstCompanyID.Rows[0]["BookID"]), insertAssetDetails.Rows[i]["AstID"].ToString()) == false)
                            {
                                DataRow AstBooksRow = insertAstBooks.NewRow();
                                AstBooksRow["BookID"] = dtBookInfoAgainstCompanyID.Rows[0]["BookID"].ToString();
                                AstBooksRow["AstID"] = insertAssetDetails.Rows[i]["AstID"].ToString();


                                AstBooksRow["DepCode"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["DepCode"]) : 1;
                                //AstBooksRow["DepCode"] = Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["DepCode"]); //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.

                                AstBooksRow["SalvageValue"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageValue"]) : 0.00;
                                //AstBooksRow["SalvageValue"] = Convert.ToDouble(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageValue"]);  //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.

                                AstBooksRow["SalvageYear"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageYear"]) : 0;
                                //AstBooksRow["SalvageYear"] = Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageYear"]); //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.  

                                AstBooksRow["LastBV"] = 0.00;

                                DateTime datetime = Convert.ToDateTime(insertAssetDetails.Rows[i]["SrvDate"].ToString());

                                string formattedDate = datetime.ToString("yyyy-MM-dd HH:mm:ss.fff"); // This will format the datetime correctly

                                double Tax = 0.00;

                                AstBooksRow["CurrentBV"] = Convert.ToDouble(importDataforCustomer.Rows[i]["PricePerPCs"]) + Tax;
                                //AstBooksRow["CurrentBV"] = Convert.ToDecimal(insertAssetDetails.Rows[i]["BaseCost"]);
                                AstBooksRow["BVUpdate"] = formattedDate;

                                AstBooksRow["SalvageMonth"] = Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageMonth"]);
                                AstBooksRow["SalvageValuePercent"] = 0.00;

                                insertAstBooks.Rows.Add(AstBooksRow);
                            }

                        }
                        DataTable dtAstBook = DataLogic.InsertInAstBooks(insertAstBooks, "[dbo].[SP_InsertAssetIntoAstBooks]");

                    }
                }

                #endregion

                #region Asset Details Update

                else
                {

                    DataTable itemInsertResponse = DataLogic.InsertItemsInBulk("AssetDetails", 0, updateAssetDetails, SP_ImportAssets);     //Abdul Kabeer Check this for update the asset details

                    if (itemInsertResponse.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = itemInsertResponse.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = itemInsertResponse.Rows[0]["Message"].ToString();
                        updatedItemRows = itemInsertResponse.Rows[0]["UpdatedRows"].ToString();
                        DataTable insertAstBooks = new DataTable();

                        insertAstBooks.Columns.Add("BookID");
                        insertAstBooks.Columns.Add("AstID");
                        insertAstBooks.Columns.Add("DepCode");
                        insertAstBooks.Columns.Add("SalvageValue");
                        insertAstBooks.Columns.Add("SalvageYear");
                        insertAstBooks.Columns.Add("LastBV");
                        insertAstBooks.Columns.Add("CurrentBV");
                        insertAstBooks.Columns.Add("BVUpdate");
                        insertAstBooks.Columns.Add("SalvageMonth");
                        insertAstBooks.Columns.Add("SalvageValuePercent");

                        GeneralFunctions GF = new GeneralFunctions();

                        DataTable dtBookInfoAgainstCompanyID = DataLogic.GetBookAgainstCompanyID("1", "[dbo].[SP_GetBooksAgainstCompanyID]");
                        for (int i = 0; i < updateAssetDetails.Rows.Count; i++)
                        {

                            DataTable dtGetDepPolicyAgainstItemCode = DataLogic.GetDepPolicyAgainstItemCode(updateAssetDetails.Rows[i]["ItemCode"].ToString(), "[dbo].[SP_GetDepPolicyAgainstItemCode]");

                            if (GF.Check_BookExists(Convert.ToInt32(dtBookInfoAgainstCompanyID.Rows[0]["BookID"]), updateAssetDetails.Rows[i]["AstID"].ToString()) == false)
                            {
                                DataRow AstBooksRow = insertAstBooks.NewRow();
                                AstBooksRow["BookID"] = dtBookInfoAgainstCompanyID.Rows[0]["BookID"].ToString();
                                AstBooksRow["AstID"] = updateAssetDetails.Rows[i]["AstID"].ToString();

                                AstBooksRow["DepCode"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["DepCode"]) : 1; //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.
                                //AstBooksRow["DepCode"] = Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["DepCode"]); //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.

                                AstBooksRow["SalvageValue"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageValue"]) : 0.00;  //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.
                                //AstBooksRow["SalvageValue"] = Convert.ToDouble(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageValue"]);  //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.

                                AstBooksRow["SalvageYear"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageYear"]) : 0; //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.     
                                //AstBooksRow["SalvageYear"] = Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageYear"]); //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.     

                                AstBooksRow["LastBV"] = 0.00;

                                DateTime datetime = Convert.ToDateTime(updateAssetDetails.Rows[i]["SrvDate"].ToString());
                                string formattedDate = datetime.ToString("yyyy-MM-dd HH:mm:ss.fff"); // This will format the datetime correctly

                                double Tax = 0.00;

                                AstBooksRow["CurrentBV"] = Convert.ToDouble(importDataforCustomer.Rows[i]["PricePerPCs"]) + Tax;
                                AstBooksRow["BVUpdate"] = formattedDate;

                                AstBooksRow["SalvageMonth"] = dtGetDepPolicyAgainstItemCode.Rows.Count > 0 ? Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageYear"]) : 0;   //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.
                                //AstBooksRow["SalvageMonth"] = Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageMonth"]);   //Replace them with some defualt value because new category inserted and Depreciation Policy is not applied on it.
                                AstBooksRow["SalvageValuePercent"] = 0.00;

                                insertAstBooks.Rows.Add(AstBooksRow);
                            }

                        }
                        DataTable dtAstBook = DataLogic.InsertInAstBooks(insertAstBooks, "[dbo].[SP_InsertAssetIntoAstBooks]");
                    }
                }

                #endregion

                #endregion

                #endregion

                #endregion

                #region Response Handling

                // Get the end time
                DateTime endTime = DateTime.Now;

                //Date difference
                var diffInSeconds = (endTime - startTime).TotalMinutes;

                //added or updated item count
                string addRowsCount = addedItemRows == "" ? "0" : addedItemRows;
                string updateRowsCount = updatedItemRows == "" ? "0" : updatedItemRows;

                //return message
                msg.message = "(" + addRowsCount.ToString() + ") items have been added and (" + updateRowsCount.ToString() + ") items " +
                    "have been updated in " + string.Format("{0:0.00}", diffInSeconds).ToString() + " Min(s)";
                msg.status = "200";

                #endregion


                return Ok(msg);
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
