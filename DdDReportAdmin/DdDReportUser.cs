using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DdDRetail.Common.Logger.NLog;
using NLog;

namespace DdDReportAdmin
{
    /// <summary>
    /// Summary description for DdDReportUser
    /// </summary>
    [Serializable]
    public class DdDReportUser
    {
        private string username;
        private string id;
        private string password;
        private string email;
        private string currency;
        private string language;
        private List<string> clients;
        private List<string> concerns;
        //    private string chainName;

        private string cubename;
        private DateTime lastTransactionDate;
        private string userType;
        private List<Client> clientObjects;
        private bool clientsConnected;
        private string concern;

        private static NLogger logger = new NLogger(nameof(DdDReportUser));

        public DdDReportUser()
        {
            clients = new List<string>();
            clientObjects = new List<Client>();
            Concerns = new List<string>();
            //
            // TODO: Add constructor logic here
            //
        }
        #region prop
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }
        public string Concern
        {
            get
            {
                return this.concern;
            }
            set
            {
                this.concern = value;
            }
        }

        public bool ClientsConnected
        {
            get
            {
                return this.clients.Count > 0;
            }
            set
            {

            }
        }

        public string ClientLink
        {
            get
            {
                return "Clients";
            }
        }
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string UserType
        {
            get
            {
                return this.userType;
            }
            set
            {
                this.userType = value;
            }
        }
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        public string Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }

        public string Cubename
        {
            get
            {
                return this.cubename;
            }
            set
            {
                this.cubename = value;
            }
        }

        public DateTime LastTransactionDate
        {
            get
            {
                return this.lastTransactionDate;
            }
            set
            {
                this.lastTransactionDate = value;
            }
        }

        public List<string> Clients
        {
            get
            {
                return this.clients;
            }
            set
            {
                this.clients = value;
            }
        }

        public List<string> Concerns
        {
            get
            {
                return this.concerns;
            }
            set
            {
                this.concerns = value;
            }
        }

        public List<Client> ClientObjects
        {
            get
            {
                return this.clientObjects;
            }
            set
            {
                this.clientObjects = value;
            }
        }


        #endregion


        public static void UpdateUser(DdDReportUser user)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("update users2 set password= '{0}', currency = '{1}', username = '{2}', email = '{3}', language = '{4}' where  id = '{5}'", user.password, user.Currency, user.Username, user.email, user.Language, user.Id);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = string.Format("update administrativeusers set usertype = '{0}', concern='{2}' where users2id = '{1}'", user.UserType, user.id, user.concern);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(UpdateUser)}: {ex.Message}");
            }
        }

        public static void InsertUser(DdDReportUser user)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("insert into users2 (id,username,password,email,language,currency, cubename, functionality,ischain, concernName) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',3,1,'{7}')", user.id, user.username, user.password, user.Email, user.Language, user.Currency, user.Cubename, user.Concern);
                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(user.UserType) >= 0) //also insert in adm users.
                    {
                        string controlArea = "";
                        switch (user.UserType)
                        {
                            case "0": controlArea = "ALL"; break;
                            case "1":
                                {
                                    controlArea = user.Cubename;//InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"])); //FIXME : Should get concern name!!
                                    break;
                                }
                            case "2":
                                {

                                    //Insert concern
                                    // string concern = HttpContext.Current.Session["control"].ToString();
                                    //controlArea = concern;
                                    controlArea = user.Cubename;//InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                                    break;
                                }
                        }

                        cmd.CommandText = string.Format("insert into administrativeusers (username,password,usertype,ControlArea,users2id,concern) values ('{0}','{1}','{2}','{3}','{4}','{5}')", user.username, user.password, user.UserType, controlArea, user.id, user.concern);
                        cmd.ExecuteNonQuery();
                    }

                    //Copy standard reports.
                    using (System.IO.StreamWriter log = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\reportadmlog.txt", true))
                    {
                        log.AutoFlush = true;

                        cmd.CommandText = "[" + user.Cubename + "].[dbo].CopyStandardReports " + user.id;
                        log.WriteLine("Hey calling the stored procedure: " + cmd.CommandText);
                        cmd.ExecuteNonQuery();
                    }
                }
                //



            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(InsertUser)}: {ex.Message}");
            }
        }

        public static void DeleteClient(Client client)
        {
            try
            {
                DdDReportUser user = (DdDReportUser)HttpContext.Current.Session["UserObject"];
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();

                    cmd.CommandText = string.Format("delete from {2}.dbo.userclientsrelation  where  userid = '{0}' and clientid = '{1}'", user.Id, client.ClientID, user.cubename);
                    cmd.ExecuteNonQuery();

                    foreach (Client c in user.clientObjects)
                        if (c.ClientID == client.ClientID)
                        {
                            user.clientObjects.Remove(c);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(DeleteClient)}: {ex.Message}");
            }
        }

        public static void AddClient(Client client)
        {
            try
            {
                DdDReportUser user = (DdDReportUser)HttpContext.Current.Session["UserObject"];
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("Insert into {2}.dbo.userclientsrelation values ('{0}','{1}')", user.Id, client.ClientID, user.cubename);
                    cmd.ExecuteNonQuery();
                    user.clientObjects.Add(client);

                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(AddClient)}: {ex.Message}");
            }
        }



        public static void DeleteUser(DdDReportUser user)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("delete from users2 where id = '{0}'", user.Id);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = string.Format("delete from administrativeusers where users2id = '{0}'", user.Id);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = string.Format("delete from testpagestate where userid = '{0}'", user.Id);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = string.Format("delete from userclientsrelation where userid = '{0}'", user.Id);
                    cmd.ExecuteNonQuery();

                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(DeleteUser)}: {ex.Message}");
            }
        }

        public static bool UserHigherRightsThanMe(string user, int myself)
        {
            logger.Info($"Entering {nameof(UserHigherRightsThanMe)}({user}, {myself})");
            if (myself == 0) //ddd
                return false;

            bool result = true;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                cmd.Connection.Open();


                cmd.CommandText = string.Format("Select usertype from AdministrativeUsers where users2id = '{0}'", user);
                var reso = cmd.ExecuteScalar();
                int res = -1;
                if (reso != null)
                    res = Convert.ToInt32(reso);
                //need to see ifs an ordinary user
                //FIXME: Executescalar return 0 even if no entris is found.


                cmd.Connection.Close();
                switch (myself)
                {
                    case -1: result = false; break; //eveybody must see this.
                    case 1:
                        {
                            //Cube. must not see 0 users
                            if (res == 0)
                                result = true;
                            else
                                result = false;

                            break;
                        }
                    case 2:
                        {
                            //Concern. May see 2 and -1.
                            if (res == 2 || res == -1)
                                result = false;
                            else
                                result = true;
                            break;
                        }

                }
            }
            logger.Info($"{nameof(UserHigherRightsThanMe)} result: {result}");
            return result;
        }

        public static List<DdDReportUser> GetRecepientsUsers(string usertype, string control, string concern)
        {
            List<DdDReportUser> users = new List<DdDReportUser>();

            try
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = CommandUserText(usertype, control, false, concern);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //if (UserHigherRightsThanMe(reader.GetValue(6).ToString(), Convert.ToInt32(usertype)))
                            //    continue;

                            DdDReportUser user = new DdDReportUser();
                            user.username = reader.GetValue(0).ToString();
                            user.password = reader.GetValue(1).ToString();
                            user.email = reader.GetValue(2).ToString();
                            user.cubename = reader.GetValue(3).ToString();
                            user.currency = reader.GetValue(4).ToString();
                            user.language = reader.GetValue(5).ToString();
                            user.Id = reader.GetValue(6).ToString();
                            //Obmit users that have a higher rights than you self

                            users.Add(user);
                        }

                        reader.Close();
                    }

                    return users;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetRecepientsUsers)}: {ex.Message}");
            }
            return users;
        }

        public static List<DdDReportUser> GetUsers(string usertype, string control, string concern)
        {
            try
            {
                List<DdDReportUser> users = new List<DdDReportUser>();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = CommandUserText(usertype, control, false, concern);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //if (UserHigherRightsThanMe(reader.GetValue(6).ToString(), Convert.ToInt32(usertype)))
                            //    continue;

                            DdDReportUser user = new DdDReportUser();
                            user.username = reader.GetValue(0).ToString();
                            user.password = reader.GetValue(1).ToString();
                            user.email = reader.GetValue(2).ToString();
                            user.cubename = reader.GetValue(3).ToString();
                            user.currency = reader.GetValue(4).ToString();
                            user.language = reader.GetValue(5).ToString();
                            user.Id = reader.GetValue(6).ToString();
                            //Obmit users that have a higher rights than you self

                            users.Add(user);
                        }

                        reader.Close();
                    }

                    List<DdDReportUser> MarkForFiltering = new List<DdDReportUser>();
                    //Filter users that the concernuser may not see
                    DdDReportUser loginuser = DdDReportUser.GetUser(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                    //using (System.IO.TextWriter l = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\poolog.txt", true))
                    //{
                    //    l.WriteLine(DateTime.Now + ": inside get users");
                    //}
                    //Pool tjek fix
                    //  return users;

                    foreach (DdDReportUser user in users)
                    {
                        try
                        {
                            cmd.CommandText = string.Format("SELECT clientid from {1}.dbo.userclientsrelation where userid = '{0}'", user.Id, user.Cubename);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string clientNumber = reader.GetValue(0).ToString();
                                    if (usertype == "2") //Concerns only
                                    {
                                        foreach (string loginclient in loginuser.Clients)
                                            if (loginclient.StartsWith(clientNumber.Substring(0, 3)))
                                            {
                                                user.Clients.Add(clientNumber);
                                                Client c = new Client();
                                                c.ClientID = Convert.ToInt32(reader.GetValue(0));
                                                c.Name = InformationProvider.GetClientName(c.ClientID.ToString());
                                                user.ClientObjects.Add(c);
                                            }
                                            else
                                            {
                                                MarkForFiltering.Add(user);
                                                //skip
                                            }
                                    }
                                    else
                                    {
                                        user.Clients.Add(clientNumber);
                                        Client c = new Client();
                                        c.ClientID = Convert.ToInt32(reader.GetValue(0));
                                        c.Name = InformationProvider.GetClientName(c.ClientID.ToString());
                                        c.Selected = true;
                                        user.ClientObjects.Add(c);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"Error in {nameof(GetUsers)}: {ex.Message}");
                        }

                        cmd.CommandText = string.Format("SELECT usertype from administrativeusers where users2id = '{0}'", user.id);
                        var res = cmd.ExecuteScalar();

                        if (res == null)
                            user.UserType = "-1";
                        else
                        {
                            user.UserType = res.ToString();
                        }

                    }
                    for (int a = 0; a < MarkForFiltering.Count(); a++)
                    {
                        DdDReportUser u = MarkForFiltering[a];
                        users.Remove(u);
                    }


                }

                foreach (DdDReportUser u in users)
                {
                    try
                    {
                        GetConcerns(control, u.cubename);
                    }
                    catch
                    {
                        // Exceptions are already handled within the method.
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetUsers)}: {ex.Message}");
                return new List<DdDReportUser>();
            }
        }
        public static string GetCubeName(string username)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT cubename from users2 where username = '{0}'", username);
                    string res = cmd.ExecuteScalar().ToString();
                    cmd.Connection.Close();
                    return res;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetCubeName)}({username}): {ex.Message}");
                throw;
            }
        }

        public static string GetCubeName(int id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT cubename from users2 where id = '{0}'", id);
                    string res = cmd.ExecuteScalar().ToString();
                    cmd.Connection.Close();
                    return res;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetCubeName)}({id}): {ex.Message}");
                throw;
            }
        }

        public static bool IsDuplicateUsername(string username)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = string.Format("SELECT count(*) from users2 where username = '{0}'", username);
                int result = (int)cmd.ExecuteScalar();
                return result > 0;
            }
        }

        public static DdDReportUser GetUser(string username)
        {
            try
            {
                DdDReportUser user = new DdDReportUser();
                user.username = username;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT username,password,email,cubename,currency,language,id from users2 where username = '{0}'", username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.password = reader.GetValue(1).ToString();
                            user.email = reader.GetValue(2).ToString();
                            user.cubename = reader.GetValue(3).ToString();
                            user.currency = reader.GetValue(4).ToString();
                            user.language = reader.GetValue(5).ToString();
                            user.Id = reader.GetValue(6).ToString();

                        }

                        reader.Close();
                    }

                    if (string.IsNullOrEmpty(user.id))
                        return user;

                    cmd.CommandText = string.Format("SELECT userType,Concern from administrativeusers where users2id = '{0}'", user.id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.userType = reader.GetValue(0).ToString();
                            user.Concern = reader.GetValue(1).ToString();
                        }

                        reader.Close();
                    }

                }
                using (var log = new StreamWriter(@"C:\temp\adminReportLog.txt", true))
                {
                    log.AutoFlush = true;
                    log.WriteLine("Cube for user is: " + user.Cubename);
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT clientid from {1}.dbo.userclientsrelation where userid = '{0}'", user.Id, user.Cubename);
                    //using (System.IO.StreamWriter log = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\reportadmlog.txt", true))
                    //{
                    //    log.AutoFlush = true;
                    //    log.WriteLine("Getting the clients for user : " + user.id);
                    //}
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Clients.Add(reader.GetValue(0).ToString());
                            Client c = new Client();
                            c.ClientID = Convert.ToInt32(reader.GetValue(0));
                            c.Name = InformationProvider.GetClientName(c.ClientID.ToString());
                            user.ClientObjects.Add(c);
                        }
                    }

                }

                GetConcerns(HttpContext.Current.Session["userType"].ToString(), user.cubename);
                return user;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetUser)}({username}): {ex.Message}");
                throw;
            }
        }

        public static DdDReportUser GetUser(int id)
        {
            try
            {
                DdDReportUser user = new DdDReportUser();
                //  user = username;
                using (SqlCommand cmd = new SqlCommand())
                {
                    //using (System.IO.TextWriter l = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\poolog.txt", true))
                    //{
                    //    l.WriteLine(DateTime.Now + ": inside get image");
                    //}
                    cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT username,password,email,cubename,currency,language,id from users2 where id = '{0}'", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.username = reader.GetValue(0).ToString();
                            user.password = reader.GetValue(1).ToString();
                            user.email = reader.GetValue(2).ToString();
                            user.cubename = reader.GetValue(3).ToString();
                            user.currency = reader.GetValue(4).ToString();
                            user.language = reader.GetValue(5).ToString();
                            user.Id = reader.GetValue(6).ToString();


                        }

                        reader.Close();
                    }

                }

                if (string.IsNullOrEmpty(user.id))
                    return user;

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(user.Cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT clientid from {1}.dbo.userclientsrelation where userid = '{0}'", user.Id, user.Cubename);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Clients.Add(reader.GetValue(0).ToString());
                            Client c = new Client();
                            c.ClientID = Convert.ToInt32(reader.GetValue(0));
                            c.Name = InformationProvider.GetClientName(c.ClientID.ToString());
                            c.Selected = true;
                            user.ClientObjects.Add(c);
                        }
                    }
                }


                GetConcerns(HttpContext.Current.Session["userType"].ToString(), user.cubename);

                return user;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetUser)}({id}): {ex.Message}");
                throw;
            }
        }

        public static List<string> GetConcerns(string controlType, string cubename)
        {
            try
            {
                List<string> cs = new List<string>();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    switch (controlType)
                    {

                        case "0":
                        case "1": cmd.CommandText = string.Format("SELECT concern from chainconcernrelation where chain in (select id from chain where name = '{0}')", cubename); break;
                        case "2": cmd.CommandText = string.Format("SELECT concern from chainconcernrelation where concern = '{0}'", HttpContext.Current.Session["concern"]); break;
                        default: cmd.CommandText = string.Format("SELECT concern from chainconcernrelation where chain in (select id from chain where name = '{0}')", cubename); break;
                    }
                    //Remember to look up name in koncern file.
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cs.Add(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                    }

                    cmd.Connection.Close();
                }
                return cs;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetConcerns)}: {ex.Message}");
                throw;
            }
        }

        public static List<Client> GetClients(DdDReportUser user)
        {
            if (user != null)
                return user.ClientObjects;
            else
                return new List<Client>();
        }
        public string GetLog(DevExpress.Web.ASPxListBox list)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                DateTime lastmonth = DateTime.Now;
                lastmonth = lastmonth.AddMonths(-1);
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(this.Cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("SELECT time,message from logentry where userid = '{0}' and time> '{1}' order by time asc", this.Id, lastmonth.ToString("yyyy/MM/dd"));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(1).ToString() == "Login Success")
                                list.Items.Add("*****************NEW SESSION************************");
                            list.Items.Add(reader.GetValue(0) + ": " + reader.GetValue(1));
                        }
                        reader.Close();
                    }
                    cmd.Connection.Close();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetLog)}: {ex.Message}");
                throw;
            }
        }

        public void GetLastTransactionDate()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(ETLHelpers.GetUserInterfaceConnectionString(this.Cubename, false));
                    logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                    cmd.Connection.Open();
                    cmd.CommandText = string.Format("select distinct klient_navn from store{0} where klient_id = '{1}'", this.cubename, this.clients[0]);
                    string storename = cmd.ExecuteScalar().ToString();
                    var d = ETLHelpers.LastSaleDate(cubename, storename);

                    if (d == null)
                        d = DateTime.MinValue;

                    this.lastTransactionDate = Convert.ToDateTime(d);

                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(GetLastTransactionDate)}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get list of users
        /// </summary>
        /// <param name="usertype">0 = support, 1 = cubeowner, 2 = koncern</param>
        /// <param name="control">none / cubename / concern</param>
        /// <returns></returns>
        public static List<string> Usernames(string usertype, string control, string concern)
        {

            List<string> vals = new List<string>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                cmd.Connection.Open();
                cmd.CommandText = CommandUserText(usertype, control, true, concern);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vals.Add((string)reader.GetValue(0));
                    }
                    reader.Close();
                }
                cmd.Connection.Close();
            }

            return vals;

        }

        public static Dictionary<string, string> Userlist(string usertype, string control, string concern)
        {

            Dictionary<string, string> vals = new Dictionary<string, string>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                cmd.Connection.Open();
                cmd.CommandText = CommandUserText(usertype, control, true, concern);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            vals.Add(reader.GetValue(1).ToString(), (string)reader.GetValue(0));
                        }
                        catch (Exception ex)
                        {
                            //should never be reached. if so inconsistent data exists.
                        }
                    }
                    reader.Close();
                }
                cmd.Connection.Close();
            }

            return vals;

        }

        public static Dictionary<string, string> UserlistByCube(string usertype, string control, string concern, string cubename)
        {
            logger.Info($"Entering {nameof(UserlistByCube)}({usertype}, {control}, {concern}, {cubename})");
            Dictionary<string, string> vals = new Dictionary<string, string>();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ReportLibrary.ConnectionHandler.SqlConnectionString);
                logger.Info($"Connecting to {cmd.Connection.ConnectionString}");
                cmd.Connection.Open();
                cmd.CommandText = string.Format("SELECT username,id from users2 where cubename = '{0}'  order by username asc", cubename);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            vals.Add(reader.GetValue(1).ToString(), (string)reader.GetValue(0));
                        }
                        catch (Exception ex)
                        {
                            //should never be reached. if so inconsistent data exists.
                        }
                    }
                    reader.Close();
                }
                cmd.Connection.Close();
            }

            return vals;

        }

        public static string CommandUserText(string usertype, string control, bool onlyUserName, string concern)
        {

            string q = "";
            switch (usertype)
            {
                case "0":
                    {
                        if (onlyUserName)
                            q = "SELECT username,id from users2  order by username asc";
                        else
                            q = "SELECT username,password,email,cubename,currency,language,id from users2 order by username asc"; break;

                    }
                case "1":
                    {
                        if (onlyUserName)
                            q = string.Format("SELECT username,id from users2 where cubename = '{0}' and email not like '%@dddretail.com' order by username asc", control);
                        else
                            q = string.Format("SELECT username,password,email,cubename,currency,language,id from users2 where cubename = '{0}' and email not like '%@dddretail.com'  order by username asc", control); break;
                    }
                case "2":
                    {
                        //Perhaps wrong:
                        // q = string.Format("select username,id from users2 where id in (select userid from userclientsrelation where clientid like '{0}%') order by username asc", concern);

                        if (onlyUserName)
                            q = string.Format("select username,id from users2 where concernName = '{0}' and email not like '%@dddretail.com'  order by username asc", concern);
                        else
                            q = string.Format("SELECT username,password,email,cubename,currency,language,id from users2 where cubename = '{0}' and email not like '%@dddretail.com' ", control); break;
                    }
            }
            return q;
        }

    }
    [Serializable]
    public class Client
    {
        private string name;
        private int clientid;
        private bool selected;

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set { this.selected = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int ClientID
        {
            get
            {
                return this.clientid;
            }
            set { this.clientid = value; }
        }


    }

    public class Currency
    {
        string dbval;
        string val;

        public string DBVal
        {
            get
            {
                return this.dbval;
            }
        }

        public string Val
        {
            get
            {
                return this.val;
            }
        }

        public static List<Currency> GetValues()
        {
            List<Currency> cur = new List<Currency>();
            Currency dk = new Currency();
            dk.dbval = "dk";
            dk.val = "DKK";

            Currency se = new Currency();
            se.dbval = "se";
            se.val = "SEK";

            Currency eu = new Currency();
            eu.dbval = "eu";
            eu.val = "EUR";

            cur.Add(dk);
            cur.Add(se);
            cur.Add(eu);

            return cur;

        }

    }
}