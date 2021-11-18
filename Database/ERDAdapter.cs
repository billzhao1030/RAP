using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RAP.Database {
    public static class ERDAdapter {

        // The connection to romote database
        private static MySqlConnection conn = null;

        // The details to login to the database
        private const string data = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string serv = "alacritas.cis.utas.edu.au";
        
        // Return the database connection
        private static MySqlConnection GetConnection() {
            if (conn == null) {
                string connectionDetails = String.Format("Database={0};Source={1};User Id={2};Password={3}", data, user, pass, serv);
                conn = new MySqlConnection(connectionDetails);
            }

            return conn;
        }

        
    }
}
