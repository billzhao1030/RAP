using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RAP.Research;

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

        // get the basic information of the list of researchers for display in ResearcherListView
        public static List<Researcher> FetchBasicResearcherDetails() {
            List<Researcher> researchers = new List<Researcher>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                MySqlCommand command = new MySqlCommand("select id, type, given_name, family_name, title, level, email from researcher", conn);
                rdr = command.ExecuteReader();

                while (rdr.Read()) {
                    // try better approach 
                    if (rdr.GetString(1) == "Staff") {
                        researchers.Add(new Staff {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = rdr.GetString(4),
                            CurrentLevel = (PositionLevel)Enum.Parse(typeof(PositionLevel), rdr.GetString(5)),
                            Email = rdr.GetString(6)
                        });
                    } else if (rdr.GetString(1) == "Student") {
                        researchers.Add(new Student {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = rdr.GetString(4),
                            Email = rdr.GetString(6)
                        });
                    }
                }
            } catch (MySqlException e) {
                Console.WriteLine("Error connecting to the Database: " + e); // Here try to give a visible reaction for user later
            } finally {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }

            return researchers;
        }

        // get the full details of a specific researcher
        public static void FetchFullResearcherDetails(Researcher researcher) {
            List<Publication> publications = new List<Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                MySqlCommand command = new MySqlCommand("select unit, campus, photo, degree, supervisor_id, utas_start, " +
                                                        "current_start from researcher where id = '" + researcher.Id + "'", conn);
                rdr = command.ExecuteReader();

                if (rdr.Read()) {
                    // TODO
                }
            } catch(MySqlException e) {
                Console.WriteLine("Error connecting to the Database: " + e);
            } finally {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
    }
}
