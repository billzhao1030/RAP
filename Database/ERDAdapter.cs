using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RAP;
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

                MySqlCommand getResearcherBasic = new MySqlCommand("select id, type, given_name, family_name, title, level, email from researcher", conn);
                rdr = getResearcherBasic.ExecuteReader();

                while (rdr.Read()) {
                    // try better approach 
                    if (rdr.GetString(1) == "Staff") {
                        researchers.Add(new Staff {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = rdr.GetString(4),
                            CurrentLevel = EnumStringConverter.ParseEnum<PositionLevel>(rdr.GetString(5)), // need to be double check
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
        public static void FetchFullResearcherDetails(Researcher r) {
            List<Publication> publications = new List<Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                MySqlCommand getResearcherDetail = new MySqlCommand("select unit, campus, photo, degree, supervisor_id, utas_start, " +
                                                        "current_start from researcher where id = '" + r.Id + "'", conn);
                rdr = getResearcherDetail.ExecuteReader();

                if (rdr.Read()) {
                    r.Unit = rdr.GetString(0);
                    r.Photo = rdr.GetString(2);
                    r.Campus = EnumStringConverter.ParseEnum<Campus>(rdr.GetString(1).Replace(" ", ""));
                    r.UtasStart = rdr.GetDateTime(5);
                    r.CurrentStart = rdr.GetDateTime(6);

                    if (r is Staff) {
                        Staff s = r as Staff;

                        List<string> supervisees = new List<string>();
                        List<Position> positions = new List<Position>();

                        // fetch the supervision name list (sorted)
                        rdr.Close();
                        MySqlCommand getSupervisionList = new MySqlCommand("select title, given_name, family_name from researcher" +
                                                                           "where supervisor_id = '" + s.Id +"'", conn);
                        rdr = getSupervisionList.ExecuteReader();

                        while (rdr.Read()) {
                            supervisees.Add(string.Format("{0} {1} {2}", rdr.GetString(0), rdr.GetString(1), rdr.GetString(2)));
                        }

                        supervisees.Sort();
                        s.Supervisees = supervisees;

                        // fetch the position list (sorted)
                        rdr.Close();
                        MySqlCommand innerCommand2 = new MySqlCommand("select * from position where id = '" + r.Id + "'", conn);
                        rdr = innerCommand2.ExecuteReader();

                        while (rdr.Read()) {
                            // only get the previous position
                            if (!rdr.IsDBNull(3)) {
                                positions.Add(new Position {
                                    Level = EnumStringConverter.ParseEnum<PositionLevel>(rdr.GetString(1)),
                                    Start = rdr.GetDateTime(2),
                                    End = rdr.GetDateTime(3)      
                                });
                            }
                        }

                        positions.Sort((p1, p2) => p2.Start.CompareTo(p1.Start)); // sort based on the time
                        s.Positions = positions;
                        
                    } else if (r is Student) {
                        Student s = r as Student;
                        s.Degree = rdr.GetString(3);

                        int supervisorID = rdr.GetInt32(4);

                        rdr.Close(); // need to be closed or not?
                        MySqlCommand getSupervisor = new MySqlCommand("select title, given_name, family_name from researcher " +
                                                                     "where id = '" + supervisorID + "'", conn);
                        rdr = getSupervisor.ExecuteReader();

                        if (rdr.Read()) {
                            s.Supervisor = string.Format("{0} {1} {2}", rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
                        }
                    }
                }

                // get the publication list (basic)
                rdr.Close();
                MySqlCommand getPublicationList = new MySqlCommand("select publication.doi, year, title from publication, researcher_publication" +
                                                                   "where researcher_publication.doi = publication.doi and " +
                                                                   "researcher_publication.researcher_id = '" + r.Id + "'", conn);
                rdr = getPublicationList.ExecuteReader();

                while (rdr.Read()) {
                    publications.Add(new Publication { 
                        Doi = rdr.GetString(0),
                        Title = rdr.GetString(1),
                        Year = rdr.GetInt32(2)
                    });
                }

                r.Publications = publications;
            } catch(MySqlException e) {
                Console.WriteLine("Error connecting to the Database: " + e);
            } finally {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

       

        // get full details of the given publication
        public static void FetchFullPublicationDetails(Publication p) {
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                MySqlCommand getPublicationDetails = new MySqlCommand("select authors, type, cite_as, available from publication " +
                                                                      "where doi = '" + p.Doi + "'", conn);
                rdr = getPublicationDetails.ExecuteReader();

                if (rdr.Read()) {
                    p.Author = rdr.GetString(0);
                    p.Type = EnumStringConverter.ParseEnum<PublicationType>(rdr.GetString(1));
                    p.CiteAs = rdr.GetString(2);
                    p.Available = rdr.GetDateTime(3);
                }

            } catch (MySqlException e) {
                Console.WriteLine("Error connecting to the Database: " + e);
            } finally {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
    }
}
