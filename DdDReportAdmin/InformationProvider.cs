using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DdDReportAdmin
{
    /// <summary>
    /// Summary description for InformationProvider
    /// </summary>
    public class InformationProvider
    {
        public InformationProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool Authenticate(string username, string password)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = string.Format("SELECT id,usertype,ControlArea,users2id,concern from AdministrativeUsers where username = '{0}' and password = '{1}'", username, password);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //only one
                        string id = reader.GetValue(0).ToString();
                        string usertype = reader.GetValue(1).ToString();
                        string ControlArea = reader.GetValue(2).ToString();
                        string u2id = reader.GetValue(3).ToString();
                        string concern = reader.GetValue(4).ToString();
                        HttpContext.Current.Session["userID"] = id;
                        HttpContext.Current.Session["users2ID"] = u2id;
                        HttpContext.Current.Session["userType"] = usertype;
                        HttpContext.Current.Session["control"] = ControlArea;
                        HttpContext.Current.Session["concern"] = concern;
                        reader.Close();
                        cmd.Connection.Close();
                        return true;
                    }
                    reader.Close();
                    cmd.Connection.Close();
                    return false;
                }
            }

        }
        public static List<string> GetAllCubes()
        {
            List<string> cubes = new List<string>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                //using (System.IO.TextWriter l = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\poolog.txt", true))
                //{
                //    l.WriteLine(DateTime.Now + ": inside get all cubes");
                //}
                cmd.CommandText = string.Format("SELECT name from [DdDreportMonitor].[dbo].[cubes]");
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cubes.Add(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                }
                cmd.Connection.Close();
            }
            return cubes;
        }

        public static string GetCubeName(int userid)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = string.Format("SELECT cubename from users2 where id = '{0}'", userid);
                string res = cmd.ExecuteScalar().ToString();
                cmd.Connection.Close();
                return res;
            }
        }

        public static List<string> GetConcernNames(int userid)
        {
            DdDReportUser user = DdDReportUser.GetUser(userid);
            List<string> cnames = new List<string>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = string.Format("select groupid from [DdDreportMonitor].[dbo].[groups] where cube in (select id from [DdDreportMonitor].[dbo].[cubes] where name = '{0}')", user.Cubename);
                //cmd.CommandText = string.Format("select concern from chainconcernrelation where chain in (select id from chain where name = '{0}')", user.Cubename);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cnames.Add(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                }
                cmd.Connection.Close();
                return cnames;
            }
        }

        public static int GetNextId()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                //using (System.IO.TextWriter l = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\poolog.txt", true))
                //{
                //    l.WriteLine(DateTime.Now + ": inside get new id in informationprovider");
                //}
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = "SELECT max(id) from users2";
                return Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            }
        }

        public static string GetDdDReportInformation(int userid)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = string.Format("SELECT username,password from users2 where id = {0}", userid);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string u = reader.GetValue(0).ToString();
                        string p = reader.GetValue(1).ToString();
                        string res = u + ";" + p;
                        reader.Close();
                        cmd.Connection.Close();
                        return res;
                    }
                    reader.Close();
                    cmd.Connection.Close();
                    return "";
                }

            }
        }

        public static bool ValidUsername(string username)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = string.Format("SELECT id from users2 where username = '{0}'", username);
                int res = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return res == 0;
            }
        }

        public static string GetClientName(string clientnumber)
        {

            bool debug = false;
            if (debug)
                return clientnumber;

            using (var oracle = new OleDbConnection(ReportLibrary.ConnectionHandler.OracleConnectionString))
            {
                oracle.Open();
                using (OleDbCommand command = oracle.CreateCommand())
                {
                    if (clientnumber == "")
                        return "-1"; //hack
                    command.CommandText = String.Format("SELECT Klientbeskrivelse FROM Satellite.KlientFile where Klientnummer = {0}", clientnumber);
                    try
                    {
                        string res = command.ExecuteScalar().ToString();
                        oracle.Close();
                        return res;
                    }
                    catch (Exception ex)
                    {
                        return clientnumber;
                    }
                }
            }

        }

        public static string GetConcern(string clientnumber)
        {
            using (var oracle = new OleDbConnection(ReportLibrary.ConnectionHandler.OracleConnectionString))
            {
                oracle.Open();
                using (OleDbCommand command = oracle.CreateCommand())
                {
                    if (clientnumber == "")
                        return "-1"; //hack
                    command.CommandText = String.Format("SELECT Klientbeskrivelse FROM Satellite.KlientFile where Klientnummer = {0}", clientnumber);
                    try
                    {
                        string res = command.ExecuteScalar().ToString();
                        oracle.Close();
                        return res;
                    }
                    catch (Exception ex)
                    {
                        oracle.Close();
                        return "-1";
                    }
                }
            }

        }

        public static List<string> getReports(string userid)
        {
            List<string> reports = new List<string>();
            DdDReportUser user = DdDReportUser.GetUser(Convert.ToInt16(userid));
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                cmd.Connection.Open();
                cmd.CommandText = string.Format("Select name from testpagestate where userid = '{0}' and DdDReport='false'", userid);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    cmd.Connection.Close();
                }
            }

            return reports;
        }

        public static bool DeleteUserReports(string userid)
        {
            string cubeName = DdDReportUser.GetCubeName(Convert.ToInt32(userid));
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(cubeName, false));
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("delete from testpagestate where userid = '{0}' and DdDReport='false'", userid);
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static void CopyReport(string reportname, string fromuser, string touser)
        {
            try
            {
                string cubename = DdDReportUser.GetCubeName(touser);
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(cubename, false));
                    cmd.Connection.Open();

                    cmd.CommandText = string.Format("select id from [ddd].[dbo].users2 where username = '{0}'", touser);
                    string toid = cmd.ExecuteScalar().ToString();
                    using (System.IO.StreamWriter l = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\copyreportlog.txt", true))
                    {
                        {
                            //  cmd.CommandText = "[" + user.Cubename + "].[dbo].CopyReports";
                            ////  cmd.CommandText = "CopyReports";
                            // // cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            //  cmd.Parameters.AddWithValue("reporttocopy", reportname);
                            //  cmd.Parameters.AddWithValue("fromuser", fromuser);
                            //  cmd.Parameters.AddWithValue("targetuser", toid);
                            //  cmd.ExecuteNonQuery();
                        }

                        l.AutoFlush = true;
                        cmd.CommandText = string.Format("[" + cubename + "].[dbo].CopyReports @reporttocopy='{0}',@fromuser={1},@targetuser={2}", reportname, fromuser, toid);
                        l.WriteLine("Command is : " + cmd.CommandText);

                    }
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void UpdateUserClientRelation(DdDReportUser user, List<string> clients)
        {
            List<string> toDelete = new List<string>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                using (System.IO.StreamWriter log = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\reportadmlog.txt", true))
                {
                    log.AutoFlush = true;
                    log.WriteLine("Hey updating userclientrelation. using connstring : " + cmd.Connection.ConnectionString);
                }
                cmd.Connection.Open();
                //Delete also.
                foreach (string client in clients)
                {
                    if (client == "")
                        continue;

                    //hack: concern relations.
                    if (user.UserType == "2") //Concern admin
                    {
                        if (clients.Count > 0)
                        {
                            string concern = clients[0].Substring(0, 3);
                            cmd.CommandText = string.Format("Update [ddd].[dbo].administrativeusers set controlArea = '{0}' where users2id = '{1}'", concern, user.Id);
                        }
                    }
                    //Check if exists:
                    cmd.CommandText = string.Format("select id From {2}.dbo.userclientsrelation where userid = '{0}' and clientid = '{1}'", user.Id, client, user.Cubename);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) == 0) //dosent exist
                    {
                        cmd.CommandText = string.Format("INSERT INTO {2}.dbo.userclientsrelation values ('{0}','{1}')", user.Id, client, user.Cubename);
                        cmd.ExecuteNonQuery();
                    }

                }
                cmd.CommandText = string.Format("select clientid from {1}.dbo.userclientsrelation where userid = '{0}'", user.Id, user.Cubename);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string clientid = reader.GetValue(0).ToString();
                        if (!clients.Contains(clientid))
                            toDelete.Add(clientid);
                    }
                    reader.Close();
                }
                foreach (string clientToDelete in toDelete)
                {
                    cmd.CommandText = string.Format("Delete from {2}.dbo.userclientsrelation where userid = '{0}' and clientid = '{1}'", user.Id, clientToDelete, user.Cubename);
                    cmd.ExecuteNonQuery();
                }
                cmd.Connection.Close();
            }



        }

        public static List<Client> AvailableClients(DdDReportUser user, List<string> notlist)
        {
            List<Client> availclients = new List<Client>();
            if (user == null)
                return availclients;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                cmd.Connection.Open();
                string usertypeOfLogin = HttpContext.Current.Session["userType"].ToString();
                switch (usertypeOfLogin)
                {
                    case "0":
                    case "1":
                        {
                            cmd.CommandText = string.Format("Select client from ETLWClients where chain = '{0}'", user.Cubename);
                            break;
                        }
                    case "2":  //restrict to Concern that etlClients are allready visible.
                        {
                            string dynamicLike = "";
                            foreach (string c in notlist)
                            {
                                dynamicLike = string.Format("Client like '{0}%' OR ", c.Substring(0, 3));
                            }

                            if (dynamicLike.Length > 4)
                                dynamicLike = dynamicLike.Substring(0, dynamicLike.Length - 4);

                            if (dynamicLike == "")
                            {
                                //No userclients attached, need to find some.
                                string concern = HttpContext.Current.Session["concern"].ToString();
                                cmd.CommandText = string.Format("Select client from ETLWClients where client like '{0}%'", concern);
                                // return availclients;
                            }
                            else
                                cmd.CommandText = string.Format("Select client from ETLWClients WHERE {0}", dynamicLike); //MGA FIX ME!
                            break;
                        }
                }

                // cmd.CommandText = string.Format("Select client from ETLWClients where chain = '{0}'",user.Cubename);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int c = Convert.ToInt32(reader.GetValue(0));
                        Client cl = new Client();
                        cl.Selected = false;
                        cl.ClientID = c;
                        cl.Name = InformationProvider.GetClientName(cl.ClientID.ToString());

                        if (!notlist.Contains(cl.ClientID.ToString()))
                            availclients.Add(cl);
                    }
                    reader.Close();
                }
                cmd.Connection.Close();
            }
            return availclients;
        }
    }
}