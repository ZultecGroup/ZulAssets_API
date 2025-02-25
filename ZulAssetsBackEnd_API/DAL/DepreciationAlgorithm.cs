using Microsoft.VisualBasic;
using System.Data;
using System.Globalization;
using ZulAssetsBackEnd_API.Model;

namespace ZulAssetsBackEnd_API.DAL
{
    public class DepreciationAlgorithm
    {
        public static DataTable CalcDepAnnual(Int16 salvageYear, Int16 salvageMonth, DateOnly lastBookUpdateDate, string totalCost, string currentBookVal, int intDepreciationType, double salvageValue, DateOnly serviceDate)
        //public DataTable CalcDepAnnial(Int16 salvageYear, Int16 salvageMonth, int intDepType, double totalCost, double salvageValue, DateOnly lastBookUpdateDate, DateOnly serviceDate, double currentBookValue)
        {
            DataTable dt = new DataTable();
            try
            {
                if (salvageYear > 0)
                {
                    lastBookUpdateDate = GetLastDayInMonth(lastBookUpdateDate);

                    int totalSalvageMonth = (salvageYear * 12 + salvageMonth);
                    double depValue;
                    double accDepValue = 0.0;
                    DateOnly fiscalYear;

                    fiscalYear = lastBookUpdateDate.AddMonths(totalSalvageMonth);

                    dt.Columns.Add("CurrDate", Type.GetType("System.String"));
                    dt.Columns.Add("StartValue", Type.GetType("System.Double"));
                    dt.Columns.Add("Dep", Type.GetType("System.Double"));
                    dt.Columns.Add("AccDep", Type.GetType("System.Double"));
                    dt.Columns.Add("CBV", Type.GetType("System.Double"));

                    int monthLeft;
                    monthLeft = DateDiff(DateInterval.Month, lastBookUpdateDate, fiscalYear);

                    DateOnly preCounter = lastBookUpdateDate;
                    DateOnly dtCounter = new DateOnly(preCounter.Year, 12, 31).AddDays(1);

                    double totalCostDouble = Convert.ToDouble(totalCost);
                    double currentBookValDouble = Convert.ToDouble(currentBookVal);

                    if (totalCostDouble != currentBookValDouble)
                    {
                        preCounter = lastBookUpdateDate.AddDays(1);
                        dtCounter = new DateOnly(preCounter.Year, 12, 31).AddDays(1);
                    }

                    if (monthLeft > 0)
                    {
                        for (int i = 0; i < monthLeft - 1; i++)
                        {
                            dtCounter = GetLastDayInMonth(dtCounter);
                            depValue = CalcDepValue(salvageYear, salvageMonth, intDepreciationType, totalCostDouble, salvageValue, dtCounter, preCounter);
                            if (Convert.ToDouble(currentBookValDouble - accDepValue - depValue) >= salvageValue)
                            {
                                accDepValue += depValue;
                                dt.Rows.Add(preCounter, Convert.ToDouble(currentBookValDouble - accDepValue + depValue), Convert.ToDouble(depValue), Convert.ToDouble(accDepValue), Convert.ToDouble(currentBookValDouble - accDepValue));
                            }
                            else
                            {
                                if (currentBookValDouble - accDepValue - salvageValue <= 0)
                                {
                                    break;
                                }
                                else
                                {
                                    accDepValue += depValue;
                                    dt.Rows.Add(preCounter, Convert.ToDouble(currentBookValDouble - accDepValue + depValue), Convert.ToDouble(currentBookValDouble - accDepValue + depValue - salvageValue), currentBookValDouble - salvageValue, salvageValue);
                                    break;
                                }
                            }
                            preCounter = dtCounter;
                            dtCounter = dtCounter.AddYears(1);
                        }
                    }
                }

                return dt;

            }
            catch (Exception ex)
            {
                //   throw;
                return dt;
            }
        }

        public static DataTable CalcDepMonthly(Int16 salvageYear, Int16 salvageMonth, DateOnly lastBookUpdateDate, string totalCost, string currentBookVal, int intDepreciationType, double salvageValue, DateOnly serviceDate)
        {
            try
            {
                DataTable dt2 = new DataTable();
                if (salvageYear > 0)
                {
                    int totalSalvageMonth = (salvageYear * 12 + salvageMonth);
                    double depValue;
                    double accDepValue = 0.0;
                    DateOnly fiscalYear;

                    fiscalYear = lastBookUpdateDate.AddMonths(totalSalvageMonth);
                    DataTable dt = new DataTable();

                    dt.Columns.Add("CurrDate", Type.GetType("System.String"));
                    dt.Columns.Add("StartValue", Type.GetType("System.Double"));
                    dt.Columns.Add("Dep", Type.GetType("System.Double"));
                    dt.Columns.Add("AccDep", Type.GetType("System.Double"));
                    dt.Columns.Add("CBV", Type.GetType("System.Double"));

                    int MonthLeft = DateDiff(DateInterval.Month, lastBookUpdateDate, fiscalYear);

                    

                    DateOnly startOfMonth = lastBookUpdateDate;
                    //DateTime startOfMonth = Convert.ToDateTime(lastBookUpdateDate);

                    DateTime MyDate = DateTime.Now;
                    int DaysInMonth = DateTime.DaysInMonth(lastBookUpdateDate.Year, lastBookUpdateDate.Month);
                    DateOnly LastDayInMonthDate = new DateOnly(lastBookUpdateDate.Year, lastBookUpdateDate.Month, DaysInMonth);
                    DateOnly LastDayInMonthDateTime = LastDayInMonthDate;

                    DateOnly endOfMonth = GetLastDayInMonth(lastBookUpdateDate.AddDays(1));
                    DateOnly endOfMonth2 = GetLastDayInMonth(LastDayInMonthDate.AddDays(1));

                    if (totalCost != currentBookVal)
                    {
                        startOfMonth = lastBookUpdateDate.AddDays(1);
                        endOfMonth = GetLastDayInMonth(lastBookUpdateDate.AddDays(1)).AddDays(1);
                    }

                    double totalCostDouble = Convert.ToDouble(totalCost);
                    double currentBookValDouble = Convert.ToDouble(currentBookVal);

                    if (MonthLeft > 0)
                    {
                        for (int i = 0; i < MonthLeft; i++)
                        {
                            //depValue = CalcDepValue(SalYr, SalMon, intDeptype, TotalCost, SalValue, endOfMonth, startOfMonth);
                            depValue = CalcDepValue(salvageYear, salvageMonth, intDepreciationType, totalCostDouble, salvageValue, endOfMonth2, LastDayInMonthDateTime);

                            var dateCheckMonth = startOfMonth.Month;
                            var dateCheckYear = startOfMonth.Year;

                            string monthAbbreviation = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateCheckMonth);

                            var concat = monthAbbreviation + " " + dateCheckYear;


                            if (Convert.ToDouble(currentBookValDouble - accDepValue - depValue) >= salvageValue)
                            {
                                accDepValue += depValue;
                                dt.Rows.Add(concat, Convert.ToDouble(currentBookValDouble - accDepValue + depValue), Convert.ToDouble(depValue), Convert.ToDouble(accDepValue), Convert.ToDouble(currentBookValDouble - accDepValue));
                            }
                            else
                            {
                                if (currentBookValDouble - Math.Round(accDepValue, 6) - salvageValue <= 0)
                                {
                                    break;
                                }
                                else
                                {
                                    accDepValue += depValue;
                                    dt.Rows.Add(concat, Convert.ToDouble(currentBookValDouble - accDepValue + depValue), Convert.ToDouble(currentBookValDouble - accDepValue + depValue - salvageValue), currentBookValDouble - salvageValue, salvageValue);
                                    //dt.Rows.Add(startOfMonth, Convert.ToDouble(currentBookValDouble - accDepValue + depValue), Convert.ToDouble(currentBookValDouble - accDepValue + depValue - salvageValue), currentBookValDouble - salvageValue, salvageValue);
                                    break;
                                }
                            }

                            startOfMonth = startOfMonth.AddMonths(1);
                            endOfMonth = endOfMonth.AddMonths(1);
                        }

                        return dt;
                    }
                }
                return dt2;
            }
            catch (Exception ex)
            {
                DataTable qwe = new DataTable();
                return qwe;
            }
            
        }

        public static DateOnly GetLastDayInMonth(DateOnly Dat)
        {
            DateOnly dt = Dat.AddDays(DateTime.DaysInMonth(Dat.Year, Dat.Month) - Dat.Day);
            return dt;
        }

        public static int DateDiff(DateInterval dt1, DateOnly lastBookUpdateDate, DateOnly fiscalYear)
        {
            int aa = ((fiscalYear.Year - lastBookUpdateDate.Year) * 12) + fiscalYear.Month - lastBookUpdateDate.Month;
            return aa;
        }

        public static double CalcDepValue(Int16 salvageYear, Int16 salvageMonth, int intDepType, double totalCost, double salvageValue, DateOnly fiscalYear, DateOnly serviceDate)
        {
            try
            {
                var MonthConfig = DepMonthConfig.HalfMonth;
                double depValue = 0.0;

                #region Straight Line Method

                //Straigth Line Method
                if (intDepType == 1)
                {
                    int MonthLeft;
                    MonthLeft = monthDiff(serviceDate, fiscalYear);

                    depValue = ((totalCost - salvageValue) / (salvageYear * 12 + salvageMonth));
                    depValue = depValue * MonthLeft;
                }

                #endregion

                #region Sum of Years Method


                else if (intDepType == 2)
                {
                    double sumOfYear = 0.0;
                    double yearLeft;
                    double prYrDepVal = 0.0;
                    Int32 monthLeft;
                    yearLeft = salvageYear - DateDiff(DateInterval.Year, serviceDate, fiscalYear);
                    for (double j = 1; j < salvageYear; j++)
                    {
                        sumOfYear += j;
                    };
                    for (int i = 0; i < (salvageYear - yearLeft - 1); i++)
                    {
                        prYrDepVal += ((totalCost - salvageValue) * (salvageYear - i)) / sumOfYear;
                    }

                    Int32 diff = DateDiff(DateInterval.Day, serviceDate, fiscalYear);
                    Int32 diffDivide = diff / 30;
                    Int32 diffDivideMod = diffDivide % 12;
                    monthLeft = diffDivideMod;
                    depValue = ((totalCost - salvageValue) * (yearLeft)) / (sumOfYear);
                    depValue = (depValue / 12) * monthLeft;
                    depValue = prYrDepVal + depValue;
                }

                #endregion

                #region Double Declining Method

                // Double Declining
                else if (intDepType == 3)
                {
                    double yrleft;
                    double prYrDepVal = 0.0;
                    Int32 monthLeft;
                    yrleft = salvageYear - DateDiff(DateInterval.Year, serviceDate, fiscalYear);
                    depValue = 0.0;
                    for (double i = 0; i < (salvageYear - yrleft - 1); i++)
                    {
                        prYrDepVal += (totalCost - prYrDepVal) / salvageYear;
                    }
                    Int32 diff = DateDiff(DateInterval.Day, serviceDate, fiscalYear);
                    Int32 diffDivide = diff / 30;
                    Int32 diffDivideMod = diffDivide % 12;
                    monthLeft = diffDivideMod;
                    depValue = (totalCost - prYrDepVal) / salvageYear;
                    depValue = (depValue / 12) * monthLeft;
                    depValue = prYrDepVal + depValue;
                }

                #endregion

                return depValue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static int monthDiff(DateOnly startDateOnly, DateOnly endDateOnly)
        {
            int startDateYear = startDateOnly.Year;
            int endDateYear = endDateOnly.Year;
            int monthsApart = 12 * (startDateYear - endDateYear) + startDateOnly.Month - endDateOnly.Month;
            return Math.Abs(monthsApart);
        }
    }
}
