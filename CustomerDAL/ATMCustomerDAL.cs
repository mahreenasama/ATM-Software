using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using LoginCredentialsBO;
using Microsoft.Data.SqlClient;

namespace CustomerDAL
{
    public class ATMCustomerDAL:ATMSoftwareDAL
    {
        /*public static new bool CheckLogin(LoginCredentials cred)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlParameter log = new SqlParameter("log", cred.Login);
            SqlParameter pin = new SqlParameter("pin", cred.PinCode);
            string query = "select * from UserLogin where Login=@log and PinCode=@pin and Type='customer'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(log);
            cmd.Parameters.Add(pin);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }*/
    }
}
