using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AIFAutoFillDB.ViewModel;
using System.Windows;
using System.IO;
using System.Diagnostics;
using AIFAutoFillDB.Common;
using AIFAutoFillDB.Model;
using AIFAutoFillDB;
using System.Reflection;

namespace AIFAutoFillDB.Service
{
    public class DataBaseService
    {
        #region FIELDS

        private static DataBaseService _instance;
        private AppHelper _appHelper;
        private StringBuilder sb = new StringBuilder();
        private MySqlConnection connection;
        private string server;
        private string port;
        private string database;
        private string uid;
        private string password;
        private string connectionString;
        private string SslCert;
        private string SslKey;
        private string SslCa;
        private MySqlSslMode SslMode;

        private string PersonFields= "PersonID, First_Name, Last_Name, English_Name, Gender, Date_of_Birth, Country_of_Birth, Province_of_Birth, Citizenship, Tax_Status, Live_in_Canada_Since, Marital_Status, Cellphone, HomePhone, WorkPhone, Email, PersonType, Bankruptcy, Discharge_Date";
        private string AddressFields = "PersonID,Apt_No,Street_No,Street_Name,City,Province,Country,Postcode,Homephone,Living_Status,Start_Date,End_Date,Current_Flag,Verify_Date,Notes";
        private string IDFields = "PersonID,ID_Type,ID_Number,Issue_Date,Expiry_Date,Issue_Country,Issue_Province,Current_Flag,Verify_Date,Notes";
        private string EmploymentFields = "PersonID,Employment_Status,Employer,Industry,Occupation,Unit,Street_No,Street_Name,City,Province,Country,Postcode,Workphone,Annual_Income,Start_Date,End_Date,Current_Flag,verify_Date,Notes";
        private string AssetsFields = "PersonID,Assets_Type,Market_Value,Institution,Address_ID,Verify_Date,Notes";



        private bool isdirectlogin = false;

        #endregion FIELDS

        public DataBaseService()
        {

        }

        public static DataBaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataBaseService();
                }
                return _instance;
            }
        }

        public void Init(AppHelper appHelper)
        {
            _appHelper = appHelper;
            //LiuyangMySQL();
            Initialize();
            //Select();
            //mysql();
        }
        #region public command

        //Initialize values
        private void Initialize()
        {
            string credential_path = @"Files/ai-financial-333500-0d35607195ee.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
            System.Environment.SetEnvironmentVariable("PATH", "% PATH %; "+ Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\");
            //ExecuteAsAdmin(@"cloud_sql_proxy_x64.exe", "-instances=ai-financial-33350=tcp:3306");
            String pwd = Directory.GetCurrentDirectory(); 
            String finalString = Path.Combine(pwd, "CloudProxy.bat");
            ExecuteAsAdmin("CloudProxy.bat", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //var client = ImageAnnotatorClient.Create();
            //var image = Image.FromFile(@"PATH TO IMAGE");
            //var response = client.DetectDocumentText(image);

            //string connectionString;
            //server = "35.192.49.178"; //"192.168.2.37";// "192.168.75.82";
            //port = "3306";
            //database = "testserver1";//"servertesting";
            //uid = "root";//"huang";// "andy";// "huang";
            //password = "AI0801";// "Password031102";// "Andy@123";// "Password031102";
            ////string connectionString;
            //connectionString = "SERVER=" + server + ";" + "PORT=" + port + ";" + "DATABASE=" +
            //database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            server = "127.0.0.1";// "35.192.49.178";//"35.225.4.226";// 
            port = "3306";
            database = "aif_db";//"hengyi_core";// "aif_db";// "Huang_Server";// "testserver1";//
            uid = "jadehuang";//"huang";// "jadehuang";// "jadehuang";// 
            password = "AI0801";//"hengyi2021";// "Password031102";//"AI0801";// 
            SslCert = @"./Files/client-cert.pem";// @"C:\liuyang\client-cert.pem";//@"..\Files\client-cert.pem";// 
            SslKey = @"./Files/client-key.pem";//@"C:\liuyang\client-key.pem";//
            SslCa = @"./Files/server-ca.pem";//@"C:\liuyang\server-ca.pem";//
            SslMode = MySqlSslMode.VerifyCA;
            connectionString =
                "SERVER=" + server + ";" +
                                "PORT=" + port + ";" +
                                "database=" + database + ";" +
                                "user=" + uid + ";" +
                                "password=" + password + ";"
                                //+ "SslMode=" + SslMode + ";" +
                                //"SslCert=" + SslCert + ";" +
                                //"SslKey=" + SslKey + ";" +
                                //"SslCa=" + SslCa + ";"
            /*+"SSL Mode=None"*/
            ;

            connection = new MySqlConnection(connectionString);
            //connection.Open();
            //mysql();
            //connection.Close();
        }
        public bool isLogin(LoginAccount la)
        {
            bool isuser = false;
            //connection = new MySqlConnection(connectionString.Replace("jadehuang", la.username).Replace("AI0801", la.Password));
            isdirectlogin = true;
            if (OpenConnection())
            {
                isuser = true;
                CloseConnection();
            }
            else
            {
                isdirectlogin = false;
                CloseConnection();
                connection = new MySqlConnection(connectionString);
                object cl = new List<Channel>();
                _appHelper.DBservice.Select("Channel","Channel_ID = '"+la.username+"' and Current_Flag = 1 ",out cl);
                if (cl == null || ((List<Channel>)cl).Count <1)
                {
                    MessageBox.Show("This username wasn't found!");
                    return false;
                }
                else
                {
                    la.PID = ((List<Channel>)cl)[0].PersonID;
                    la.Privilege = ((List<Channel>)cl)[0].ChannelPrivilege;

                    cl = new List<Channel>();
                    _appHelper.DBservice.Select("Channel", "PersonID  = '" + la.PID + "' and Channel_Type='AIFDBUser' and Current_Flag = 1 ", out cl);
                    if (cl == null || ((List<Channel>)cl).Count < 1)
                    {
                        MessageBox.Show("This person didn't register for this application! Please contact Administrator.");
                        return false;
                    }
                    else
                    {
                        la.Rolename = ((List<Channel>)cl)[0].ChannelID;
                        object pl = new List<Person>();
                        _appHelper.DBservice.Select("Person", "PersonID  = '" + la.PID+"'", out pl);
                        if (pl == null || ((List<Person>)pl).Count < 1)
                        {
                            MessageBox.Show("This person info wasn't found! Please contact Administrator.");
                            return false;
                        }
                        else
                        {
                            la.Firstname = ((List<Person>)pl)[0].FirstName;
                            la.Lastname = ((List<Person>)pl)[0].LastName;
                            la.Usertype = ((List<Person>)pl)[0].Usertype;
                            la.Gender= ((List<Person>)pl)[0].Gender;
                            la.Email= ((List<Person>)pl)[0].Email;
                            la.Phone= ((List<Person>)pl)[0].Cellphone;
                            isuser = true;
                        }
                    }
                }

            }
            return isuser;
        }

        public void mysql()
        {
            try
            {
                //var connstr = "Server=192.168.75.82;Port=3306;Uid=huang;Pwd=Password031102;database=servertesting";
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("No connection string!");
                    return;
                }
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from Person";//SELECT* FROM Person;
                        //cmd.Parameters.AddWithValue("@ID", "100");
                        //cmd.CommandText = "CREATE TABLE aif_db.Lookup_LivingStatus(ID varchar(10),LivingStatus varchar(30));" +
                        //    "Insert into aif_db.Lookup_LivingStatus Values('001', 'OWNER');"+
                        //    "Insert into aif_db.Lookup_LivingStatus Values('002', 'RENT');" +
                        //    "Insert into aif_db.Lookup_LivingStatus Values('003', 'WITH PARENTS');" +
                        //    "Insert into aif_db.Lookup_LivingStatus Values('004', 'WITH OTHERS');" +
                        //    "Insert into aif_db.Lookup_LivingStatus Values('005', 'OTHER');";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ii = reader.FieldCount;
                                for (int i = 0; i < ii; i++)
                                {
                                    if (reader[i] is DBNull)
                                        sb.AppendLine("");
                                    else
                                        sb.AppendLine(reader[i].ToString());
                                }

                            }
                        }
                    }
                    conn.Close();
                }
                MessageBox.Show(sb.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application"s response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                if (!isdirectlogin)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Cannot connect to server. Please Contact administrator.");
                            break;

                        case 1045:
                            MessageBox.Show("Invalid username/password, please try again or contact administrator.");
                            break;
                    }
                }
                return false;
            }
            catch (Exception ee)
            {
                if (!isdirectlogin)
                {
                    MessageBox.Show(ee.Message);
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(object Info)
        {
            string query = "";
            if (Info is Person)
            {
                Person pi = Info as Person;
                query = "SET FOREIGN_KEY_CHECKS=0;";
                query += "REPLACE INTO Person(" + PersonFields + ") VALUES('" +
                                pi.PersonIDNo + "','" +
                                pi.FirstName + "','" +
                                pi.LastName + "','" +
                                pi.EnglishName + "','" +
                                pi.Gender + "','" +
                                pi.DateofBirth.ToString("yyyyMMdd") + "','" +
                                pi.CountryofBirth + "','" +
                                pi.ProvinceofBirth + "','" +
                                pi.Citizenship + "','" +
                                pi.TaxStatus + "','" +
                                pi.LiveCAsince.ToString("yyyyMMdd") + "','" +
                                pi.MaritalStatus + "','" +
                                pi.Cellphone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "") + "','" +
                                pi.Homephone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "") + "','" +
                                pi.Workphone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "") + "','" +
                                pi.Email + "','" +
                                pi.Usertype + "'," +
                                pi.Bankrupcty + ",'" +
                                pi.DischargeDate.ToString("yyyyMMdd") + "');";
                query += "SET FOREIGN_KEY_CHECKS = 1; ";
            }
            else if (Info is Address)
            {
                Address ads = Info as Address;
                query = "REPLACE INTO Address(" + AddressFields + ") VALUES(" +
                                ads.PID + "," +
                                ads.AptNo + "," +
                                ads.StreetNo + "," +
                                ads.StreetName + "," +
                                ads.City + "," +
                                ads.Province + "," +
                                ads.Country + "," +
                                ads.Postcode + "," +
                                ads.Homephone + "," +
                                ads.LivingStatus + "," +
                                ads.StartDate + "," +
                                ads.EndDate + "," +
                                ads.CurrentFlag + "," +
                                ads.VerifyDate + "," +
                                ads.Notes;
            }
            else if (Info is ID)
            {
                ID ids = Info as ID;
                query = "REPLACE INTO Address(" + IDFields + ") VALUES(" +
                                ids.PID + "," +
                                ids.IdType + "," +
                                ids.IdNumber + "," +
                                ids.IssueDate + "," +
                                ids.ExpiryDate + "," +
                                ids.IssueCountry + "," +
                                ids.IssueProvince + "," +
                                ids.CurrentFlag + "," +
                                ids.VerifyDate + "," +
                                ids.Notes;
            }
            try
            {

                //open connection
                if (OpenConnection())
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    CloseConnection();
                }
            }
            catch (MySqlException se)
            {
                CloseConnection();
                MessageBox.Show(se.Message);
            }
            catch (Exception ee)
            {
                CloseConnection();
                MessageBox.Show(ee.Message);
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public bool Delete(string table, string where)
        {
            bool ret = false;
            string query = "DELETE FROM "+table+(string.IsNullOrEmpty(where)?"": " WHERE "+where);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                ret = true;
            }
            return ret;
        }

        //Select statement
        public void Select(string Tablename, string Wherestr, out object obj)//string pID)
        {
            obj = null;
            string query;
            try
            {
                switch (Tablename)
                {
                    #region person
                    case "Person":
                        query = "SELECT * FROM Person" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Person> lp = new List<Person>();

                        Person personInfo;// = new Person();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                personInfo = new Person();
                                personInfo.PersonIDNo = dataReader[0] is DBNull ? "" : dataReader[0].ToString(); //PersonID
                                personInfo.FirstName = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //First_Name
                                personInfo.LastName = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Last_Name
                                personInfo.EnglishName = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //English_Name
                                personInfo.Gender = dataReader[4] is DBNull ? "" : dataReader[4].ToString().ToUpper(); //Gender
                                personInfo.DateofBirth = dataReader[5] is DBNull ? new DateTime() : DateTime.Parse(dataReader[5].ToString()); //Date_of_Birth
                                personInfo.DobYear = dataReader[5] is DBNull ? "" : DateTime.Parse(dataReader[5].ToString()).ToString("yyyy");
                                personInfo.DobMonth = dataReader[5] is DBNull ? "" : DateTime.Parse(dataReader[5].ToString()).ToString("MM");
                                personInfo.DobDay = dataReader[5] is DBNull ? "" : DateTime.Parse(dataReader[5].ToString()).ToString("dd");
                                personInfo.CountryofBirth = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Country_of_Birth
                                personInfo.ProvinceofBirth = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Province_of_Birth
                                personInfo.Citizenship = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Citizenship
                                personInfo.TaxStatus = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //Tax_Status
                                personInfo.LiveCAsince = dataReader[10] is DBNull ? new DateTime() : DateTime.Parse(dataReader[10].ToString()); //Live_in_Canada_Since
                                personInfo.MaritalStatus = dataReader[11] is DBNull ? "" : dataReader[11].ToString(); //Marital_Status
                                personInfo.Cellphone = dataReader[12] is DBNull ? "" : dataReader[12].ToString(); //Cellphone
                                personInfo.Homephone = dataReader[13] is DBNull ? "" : dataReader[13].ToString(); //Homephone
                                personInfo.Workphone = dataReader[14] is DBNull ? "" : dataReader[14].ToString(); //Workphone
                                personInfo.Email = dataReader[15] is DBNull ? "" : dataReader[15].ToString(); //Email
                                personInfo.Usertype = dataReader[16] is DBNull ? "" : dataReader[16].ToString(); //Usertype
                                //MessageBox.Show(dataReader[17].ToString());
                                personInfo.Bankrupcty = dataReader[17] is DBNull ? false : dataReader[17].ToString()=="False"?false:true; //Bankrupcty
                                personInfo.DischargeDate = dataReader[18] is DBNull ? new DateTime() : DateTime.Parse(dataReader[18].ToString()); //Discharge_Date

                                lp.Add(personInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lp;
                        //return list to be displayed
                        break;
                    #endregion Person

                    #region Address
                    case "Address":
                        query = "SELECT * FROM Address" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Address> ladd = new List<Address>();

                        Address addressInfo;// = new Address();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                addressInfo = new Address();
                                addressInfo.AptNo = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Apt_No
                                addressInfo.StreetNo = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Street_No
                                addressInfo.StreetName = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Street_Name
                                addressInfo.City = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //City
                                addressInfo.Province = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Province
                                addressInfo.Country = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Country
                                addressInfo.Postcode = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Postcode
                                addressInfo.Homephone = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Homephone
                                addressInfo.LivingStatus = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //Living_Status
                                addressInfo.StartDate = dataReader[10] is DBNull ? "" : dataReader[10].ToString(); //Start_Date
                                addressInfo.EndDate = dataReader[11] is DBNull ? "" : dataReader[11].ToString(); //End_Date
                                addressInfo.CurrentFlag = (dataReader[12] is DBNull ? "" : dataReader[12].ToString().ToUpper())=="YES"?true:false; //Current_Flag
                                addressInfo.VerifyDate = dataReader[13] is DBNull ? "" : dataReader[13].ToString(); //Verify_Date
                                addressInfo.Notes = dataReader[14] is DBNull ? "" : dataReader[14].ToString(); //Notes

                                ladd.Add(addressInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = ladd;
                        //return list to be displayed
                        break;
                    #endregion Address

                    #region ID
                    case "ID":
                        query = "SELECT * FROM ID" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<ID> lid = new List<ID>();

                        ID idInfo;// = new ID();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                idInfo = new ID();
                                idInfo.IdType = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //ID_Type
                                idInfo.IdNumber = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //ID_Number
                                idInfo.IssueDate = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Issue_Date
                                idInfo.ExpiryDate = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Expiry_Date
                                idInfo.IssueCountry = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Issue_Country
                                idInfo.IssueProvince = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Issue_Province
                                idInfo.CurrentFlag = (dataReader[7] is DBNull ? "" : dataReader[7].ToString().ToUpper())=="YES"?true:false; //Current_Flag
                                idInfo.VerifyDate = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Verify_Date
                                idInfo.Notes = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //Notes

                                lid.Add(idInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lid;
                        //return list to be displayed
                        break;
                    #endregion ID

                    #region Employment 
                    case "Employment":
                        query = "SELECT * FROM ID" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Employment> lem = new List<Employment>();

                        Employment emplInfo;// = new Employment();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                emplInfo = new Employment();
                                emplInfo.EmplStatus = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Employment_Status
                                emplInfo.Employer = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Employer
                                emplInfo.Industry = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Industry
                                emplInfo.Occupation = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Occupation
                                emplInfo.Unit = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Unit
                                emplInfo.StNo = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Street_No
                                emplInfo.StName = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Street_Name
                                emplInfo.City = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //City
                                emplInfo.Prov = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //Province
                                emplInfo.Country = dataReader[10] is DBNull ? "" : dataReader[10].ToString(); //Country
                                emplInfo.PostCode = dataReader[11] is DBNull ? "" : dataReader[11].ToString(); //Postcode
                                emplInfo.WorkPhone = dataReader[12] is DBNull ? "" : dataReader[12].ToString(); //Workphone
                                emplInfo.Income = dataReader[13] is DBNull ? "" : dataReader[13].ToString(); //Annual_Income
                                emplInfo.StartDate = dataReader[14] is DBNull ? "" : dataReader[14].ToString(); //Start_Date
                                emplInfo.EndDate = dataReader[15] is DBNull ? "" : dataReader[15].ToString(); //End_Date
                                emplInfo.CurrentFlag = (dataReader[16] is DBNull ? "" : dataReader[16].ToString().ToUpper())=="YES"?true:false; //Current_Flag
                                emplInfo.VerifyDate = dataReader[17] is DBNull ? "" : dataReader[17].ToString(); //verify_Date
                                emplInfo.Notes = dataReader[18] is DBNull ? "" : dataReader[18].ToString(); //Notes

                                lem.Add(emplInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lem;
                        //return list to be displayed
                        break;
                    #endregion Employment 

                    #region Assets 
                    case "Assets":
                        query = "SELECT * FROM Assets" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Asset> las = new List<Asset>();

                        Asset assetsInfo;// = new Asset();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                assetsInfo = new Asset();
                                assetsInfo.AssetsType = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Assets_Type
                                assetsInfo.MarketValue = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Market_Value
                                assetsInfo.Institution = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Institution
                                //assetsInfo.AddressID = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Address_ID
                                assetsInfo.verifyDate = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Verify_Date
                                assetsInfo.Notes = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Notes

                                las.Add(assetsInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = las;
                        //return list to be displayed
                        break;
                    #endregion Assets 

                    #region Liabilities 
                    case "Liabilities":
                        query = "SELECT * FROM Liabilities" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Liability> llia = new List<Liability>();

                        Liability liabilityInfo;// = new Liability();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                liabilityInfo = new Liability();
                                liabilityInfo.LiType = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //L_Type
                                liabilityInfo.LiBalance = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //L_Balance
                                liabilityInfo.LiMonthlyPayt = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //L_Monthly_Payment
                                liabilityInfo.Institution = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Institution
                                //liabilityInfo.AddressID = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Address_ID
                                liabilityInfo.VerifyDate = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Verify_Date
                                liabilityInfo.Notes = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Notes

                                llia.Add(liabilityInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = llia;
                        //return list to be displayed
                        break;
                    #endregion Liabilities 

                    #region Beneficiary 
                    case "Beneficiary":
                        query = "SELECT * FROM Beneficiary" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Beneficiary> lbnf = new List<Beneficiary>();

                        Beneficiary bnfInfo;// = new Beneficiary();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                bnfInfo = new Beneficiary();
                                bnfInfo.InvestNo = dataReader[0] is DBNull ? "" : dataReader[0].ToString(); //AIF_Invest_No
                                bnfInfo.BnfPID = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Beneficiary_PID

                                bnfInfo.BnfRelationship = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //B_Relationship
                                //bnfInfo.StartDate = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Start_Date
                                //bnfInfo.EndDate = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //End_Date
                                bnfInfo.CurrentFlag = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Current_Flag
                                bnfInfo.VerifyDate = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Verify_Date
                                //bnfInfo.UpdateDate = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Update_Date
                                bnfInfo.TrusteePID = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Trustee_PID

                                bnfInfo.TrRelationship = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //T_Relationship

                                lbnf.Add(bnfInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lbnf;
                        //return list to be displayed
                        break;
                    #endregion Beneficiary

                    #region Loan 
                    case "Loan":
                        query = "SELECT * FROM Loan" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Loan> lln = new List<Loan>();

                        Loan loanInfo;// = new Loan();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                loanInfo = new Loan();
                                loanInfo.LoanNo = dataReader[0] is DBNull ? "" : dataReader[0].ToString(); //AIF_Loan_No
                                loanInfo.ApplyDate = dataReader[1] is DBNull ? "" : dataReader[1].ToString();// ApplyDate
                                //loanInfo.AdvisorInfo.AdvisorPID = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Advisor_PID
                                //loanInfo.Applicant.PersonIDNo = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Applicant_PID
                                //loanInfo.CoApplicant.PersonIDNo = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Co_Applicant_PID
                                //loanInfo.CoapplicationFlag = dataReader[5] is DBNull ? false : (dataReader[5].ToString() == "1" ? true : false); //Co_Applicant_Flag
                                loanInfo.Tdsr = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //TDSR
                                loanInfo.TdsrVerifyDate = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //TDSR_Verify_Date
                                loanInfo.SubmitDate = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Submit_Date
                                loanInfo.LoanFrom = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //Loan_From
                                loanInfo.ApplyAmount = dataReader[10] is DBNull ? "" : dataReader[10].ToString(); //Apply_Amount
                                loanInfo.LoanType = dataReader[11] is DBNull ? "" : dataReader[11].ToString(); //Loan_Type
                                loanInfo.SettleDate = dataReader[12] is DBNull ? "" : dataReader[12].ToString(); //Settle_Date
                                loanInfo.SettleAmount = dataReader[13] is DBNull ? "" : dataReader[13].ToString(); //Settle_Amount
                                //dataReader[14]是InvestNo 在Beneficiary定义过
                                loanInfo.Notes = dataReader[15] is DBNull ? "" : dataReader[15].ToString(); //Notes

                                lln.Add(loanInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lln;
                        //return list to be displayed
                        break;
                    #endregion Loan

                    #region Family 
                    case "Family":
                        query = "SELECT * FROM Family" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Families> lf = new List<Families>();

                        Families familyInfo;// = new Families();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                familyInfo = new Families();
                                familyInfo.PrimaryPID = dataReader[0] is DBNull ? "" : dataReader[0].ToString(); //Primary_PID
                                familyInfo.MemberPID = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Member_PID
                                familyInfo.Relationship = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Relationship
                                familyInfo.CurrentFlag = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Current_Flag
                                familyInfo.UpdateDate = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Update_Date
                                familyInfo.Notes = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Notes

                                lf.Add(familyInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lf;
                        //return list to be displayed
                        break;
                    #endregion Family

                    #region Cheque  
                    case "Cheque":
                        query = "SELECT * FROM Cheque" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Cheque> lchq = new List<Cheque>();

                        Cheque chequeInfo;// = new Cheque();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                chequeInfo = new Cheque();
                                chequeInfo.CheckID = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Check_ID
                                chequeInfo.TransitNo = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Transit_No
                                chequeInfo.InstitutionNo = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Institution
                                chequeInfo.AccountNo = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Account_No
                                chequeInfo.InstitutionName = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Bank_Name
                                chequeInfo.VerifyDate = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Verify_Date

                                lchq.Add(chequeInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lchq;
                        //return list to be displayed
                        break;
                    #endregion Cheque 

                    #region Advisor  
                    case "Advisor":
                        query = "SELECT * FROM Advisor" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Advisor> lad = new List<Advisor>();

                        Advisor agentInfo;// = new AgentInfo();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                agentInfo = new Advisor();
                                agentInfo.AdvisorNo = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Advisor_No
                                agentInfo.Agency = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Agency
                                agentInfo.Agency = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Agency_Code
                                agentInfo.AdvisorCode_iA = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Ad_Code_iA
                                agentInfo.AdvisorSU_iA = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Ad_SU_iA
                                agentInfo.AdvisorCode_CL = dataReader[6] is DBNull ? "" : dataReader[6].ToString(); //Ad_Code_CL
                                agentInfo.AdvisorCode_CL = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Ad_Code_B2B
                                agentInfo.AdvisorCode_NB = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Ad_Code_NB
                                agentInfo.AdvisorCode_ML_Loan = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //Ad_Code_ML_Loan
                                agentInfo.AdvisorCode_ML_Invest = dataReader[10] is DBNull ? "" : dataReader[10].ToString(); //Ad_Code_ML_Invest
                                agentInfo.AdvisorcommissionPercent = dataReader[11] is DBNull ? "" : dataReader[11].ToString(); //Ad_Comm_PCT
                                agentInfo.Licenses = dataReader[12] is DBNull ? "" : dataReader[12].ToString(); //License

                                lad.Add(agentInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lad;
                        //return list to be displayed
                        break;
                    #endregion Advisor

                    #region Investment  
                    case "Investment":
                        query = "SELECT * FROM Investment" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Investment> linv = new List<Investment>();

                        Investment investInfo;// = new Investment();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                investInfo = new Investment();
                                investInfo.OpenDate = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Open_Date
                                investInfo.Advisor1.AdvisorPID = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Advisor_PID
                                investInfo.Advisor1.AdvisorNo = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Advisor_No
                                investInfo.Applicant.PersonIDNo = dataReader[4] is DBNull ? "" : dataReader[4].ToString(); //Applicant_PID
                                investInfo.CoApplicant.PersonIDNo = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Co_Applicant_PID
                                investInfo.CoApplicationFlag = dataReader[6] is DBNull ? false : (dataReader[6].ToString() == "1" ? true : false); //Co_Applicant_Flag
                                investInfo.CapitalSourceType = dataReader[7] is DBNull ? "" : dataReader[7].ToString(); //Capital_Source
                                investInfo.InvestTo = dataReader[8] is DBNull ? "" : dataReader[8].ToString(); //Invest_To
                                //investInfo.KYCVersion = dataReader[9] is DBNull ? "" : dataReader[9].ToString(); //KYC_Version
                                //investInfo.KYCScore = dataReader[10] is DBNull ? "" : dataReader[10].ToString(); //KYC_Score
                                //investInfo.FirstSettleDate = dataReader[11] is DBNull ? "" : dataReader[11].ToString(); //First_Settle_Date
                                investInfo.PolicyNo = dataReader[12] is DBNull ? "" : dataReader[12].ToString(); //Policy_No
                                investInfo.SettleAmount = dataReader[13] is DBNull ? "" : dataReader[13].ToString(); //Settle_Amount
                                //investInfo.PayMethod = dataReader[14] is DBNull ? "" : dataReader[14].ToString(); //Pay_Method
                                //investInfo.PaymentType = dataReader[15] is DBNull ? "" : dataReader[15].ToString(); //Payment_Type
                                //investInfo.PADFrequency = dataReader[16] is DBNull ? "" : dataReader[16].ToString(); //PAD_Frequency

                                linv.Add(investInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = linv;
                        //return list to be displayed
                        break;
                    #endregion Investment

                    #region Channel  
                    case "Channel":
                        query = "SELECT * FROM Channel" + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                        List<Channel> lc = new List<Channel>();

                        Channel iChannelInfo;// = new Investment();
                        //Open connection
                        if (OpenConnection())
                        {
                            //Create Command
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            //Create a data reader and Execute the command
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            //Read the data and store them in the list
                            while (dataReader.Read())
                            {
                                iChannelInfo = new Channel();
                                iChannelInfo.PersonID = dataReader[0] is DBNull ? "" : dataReader[0].ToString(); //PersonID
                                iChannelInfo.ChannelType = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //Open_Date
                                iChannelInfo.ChannelID = dataReader[2] is DBNull ? "" : dataReader[2].ToString(); //Advisor_PID
                                iChannelInfo.ChannelPrivilege = dataReader[3] is DBNull ? "" : dataReader[3].ToString(); //Advisor_No
                                iChannelInfo.CurrentFlag = dataReader[4] is DBNull ? false : (dataReader[4].ToString() == "1" ? true : false); //Applicant_PID
                                iChannelInfo.verifyDate = dataReader[5] is DBNull ? "" : dataReader[5].ToString(); //Co_Applicant_PID

                                lc.Add(iChannelInfo);
                            }

                            //close Data Reader
                            dataReader.Close();

                            //close Connection
                            CloseConnection();
                        }
                        obj = lc;
                        //return list to be displayed
                        break;
                    #endregion Channel

                    #region Lookup Table
                    default:
                        if (Tablename.Substring(0, 7).ToUpper() == "LOOKUP_")
                        {
                            query = "SELECT * FROM " + Tablename + " " + (string.IsNullOrEmpty(Wherestr) ? " " : " Where " + Wherestr);
                            List<LookUpInfo> lookAssetsType = new List<LookUpInfo>();

                            LookUpInfo at;// = new Asset();
                                          //Open connection
                            if (OpenConnection())
                            {
                                //Create Command
                                MySqlCommand cmd = new MySqlCommand(query, connection);
                                //Create a data reader and Execute the command
                                MySqlDataReader dataReader = cmd.ExecuteReader();

                                //Read the data and store them in the list
                                while (dataReader.Read())
                                {
                                    at = new LookUpInfo();
                                    at.LookUpInfo_id = dataReader[0] is DBNull ? "" : dataReader[0].ToString(); //Assets_Type_id
                                    at.LookUpInfo_str = dataReader[1] is DBNull ? "" : dataReader[1].ToString(); //contents

                                    lookAssetsType.Add(at);
                                }

                                //close Data Reader
                                dataReader.Close();

                                //close Connection
                                CloseConnection();
                            }
                            obj = lookAssetsType;
                            //return list to be displayed
                        }
                        break;
                     #endregion Lookup Table

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //return lp;
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to Restore!"+"\n"+ ex.Message);
            }
        }

        public void MakeLookupList()
        {
            object _assetsList = new List<LookUpInfo>();
            _appHelper.DBservice.Select("Lookup_AssetType", "", out _assetsList);
        }

        public void ExecuteAsAdmin(string fileName, string arguments="")
        {
            ProcessStartInfo proc = new ProcessStartInfo("cmd.exe");// fileName);
            proc.WindowStyle = ProcessWindowStyle.Minimized;
            //var p = new Process();
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.Arguments = "/k yourmainprocess.exe";
            //p.Start();
            //p.WaitForExit();
            //proc.StartInfo.FileName = fileName;
            
            proc.Arguments = "/k "+ fileName+" \""+arguments+"\">>nul";
            //proc.UseShellExecute = true;
            //proc.CreateNoWindow = false;


            proc.RedirectStandardInput = true;
            proc.UseShellExecute = false;
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.CreateNoWindow = true;

            proc.Verb = "runas";
            Process p= Process.Start(proc);
            //Wait for the window to finish loading.
            //p.WaitForInputIdle();
            //Wait for the process to end.
            //if (p != null && !p.HasExited)
            //{
            //    p.WaitForExit();
            //}
        }
        #endregion public command
        #region Retrieve application Info
        public void RetrieveApplication()
        {

        }
        #endregion Retrieve application Info
        #region PUBLIC INTERFACE


        #endregion PUBLIC INTERFACE

        

    }
}
