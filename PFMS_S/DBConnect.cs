using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace PFMS_S
{
    public class DBConnect
    {
        public static SqlConnection ConnectSQLServer()
        {
            SqlConnection myConn;
            myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
            return myConn;
        }
    }
}