using DdDRetail.Common.Logger.NLog;
using ReportLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DdDReportAdmin
{
    public class ETLHelpers
    {
        private static NLogger logger = new NLogger(nameof(DdDReportUser));

        public static object LastSaleDate(string cubename, string storename)
        {
            var adomdConnStr = ConnectionHandler.AdomdConnectionString(cubename);
            DateTime lastdate = DateTime.Now;

            using (var con = new Microsoft.AnalysisServices.AdomdClient.AdomdConnection())
            {
                con.ConnectionString = adomdConnStr;
                //using (System.IO.StreamWriter log = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\reportadmlog.txt", true))
                //{
                //    log.WriteLine("Using ado md string : " + con.ConnectionString);
                //    log.AutoFlush = true;
                //}
                con.Open();
                string query = "SELECT TAIL(FILTER([Time].[PK_Date].[PK_Date].members(0):ClosingPeriod([Time].[PK_Date].[PK_Date]),[Measures].[Afsætning] <> 0)) on columns " + String.Format("FROM [{0}] where [Store].[Klient_Navn].[Klient_Navn].&[{1}]", cubename, storename);

                using (var adapter = new Microsoft.AnalysisServices.AdomdClient.AdomdDataAdapter(query, con))
                {
                    DataTable table = new DataTable();
                    try
                    {
                        adapter.Fill(table);
                        lastdate = ExtractStoreTime(table);
                    }
                    catch
                    {

                        return null;
                    }
                }

                //Helpers.debug("{0}, {1}", dates[0], dates[1]);

                con.Close();
            }

            return lastdate;
        }

        public static string GetUserInterfaceConnectionString(string chain, bool OleDB)
        {
            try
            {
                //returns the default conn string if the database not has been migrated yet.
                using (SqlConnection conn = new SqlConnection(ConnectionHandler.SqlConnectionString))
                {
                    logger.Info($"Connecting to {conn.ConnectionString}");
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = string.Format("select ConnString from ConnectionStrings where chain = '{0}'", chain);
                        var res = cmd.ExecuteScalar();
                        if (res == null)
                        {
                            res = ConnectionHandler.SqlConnectionString;
                        }
                        conn.Close();
                        if (OleDB)
                        {
                            Helpers.Debug("ETL HELPER connstring : " + String.Format("{0}Provider=SQLOLEDB", res.ToString()));
                            return String.Format("{0};Provider=SQLOLEDB", res.ToString());
                        }
                        else
                        {
                            Helpers.Debug("ETL HELPER connstring : " + String.Format(res.ToString()));
                            return res.ToString().Replace("Connect Timeout=0", "Connect Timeout=30");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetUserInterfaceConnectionString)}({chain}, {OleDB}): {ex.Message}");
                throw;
            }
        }
        private static DateTime ExtractStoreTime(DataTable table)
        {
            var name = table.Columns[0].ColumnName;
            var startValue = name.IndexOf("&[") + 2;
            var datetimeStr = name.Substring(startValue, name.IndexOf("]", startValue) - startValue);
            return new DateTime(int.Parse(datetimeStr.Substring(0, 4)), int.Parse(datetimeStr.Substring(5, 2)), int.Parse(datetimeStr.Substring(8, 2)));
        }

    }
}