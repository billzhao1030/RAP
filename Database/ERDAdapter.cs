
/** The ERDAdaptor class
 * 
 *  This file provide the connection to the database
 *  And fetch the details from database for controller classes
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RAP.Research;

namespace RAP.Database {
    public static class ERDAdapter {

        // The connection to remote database
        private static MySqlConnection conn = null;

        // The details to login to the database
        private const string data = "kit206";
        private const string user = "kit206";
        private const string password = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";
        


        // Return the database connection, make sure there's only one connection exist
        private static MySqlConnection GetConnection() {
            if (conn == null) {
                string connectionDetails = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", data, server, user, password);
                conn = new MySqlConnection(connectionDetails);
            }

            return conn;
        }


        // Get the basic information of the list of researchers for display in ResearcherListView
        // Including the ID, staff type, name, title, position level and email
        public static List<Researcher> FetchBasicResearcherDetails() {
            List<Researcher> researchers = new List<Researcher>(); // Researcher list that need to be returned

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                // Read all basic information into each researcher
                MySqlCommand getResearcherBasic = new MySqlCommand("select id, type, given_name, family_name, title, level, email from researcher", conn);
                rdr = getResearcherBasic.ExecuteReader();

                // Some details like position level won't read into "Student" Researcher
                while (rdr.Read()) {
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
                            CurrentLevel = PositionLevel.Student,
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

            // Rearrange the order of the list, according to the name, alphabetically
            researchers.Sort((r1, r2) => r1.FamilyName.CompareTo(r2.FamilyName));

            return researchers;
        }


        // Get the full details of a specific researcher, which need to be displayed in ResearcherDetailsView
        public static void FetchFullResearcherDetails(Researcher r) {
            List<Publication> publications = new List<Publication>(); // A list of publication current researcher has

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                // Read details in to the current researcher
                MySqlCommand getResearcherDetail = new MySqlCommand("select unit, campus, photo, degree, supervisor_id, utas_start, " +
                                                        "current_start from researcher where id = '" + r.Id + "'", conn);
                rdr = getResearcherDetail.ExecuteReader();

                if (rdr.Read()) {
                    r.Unit = rdr.GetString(0);
                    r.Photo = rdr.GetString(2);

                    // For Cradle Coast case remove space, since the enum type is CradleCoast
                    r.Campus = EnumStringConverter.ParseEnum<Campus>(rdr.GetString(1).Replace(" ", ""));
                    r.UtasStart = rdr.GetDateTime(5);
                    r.CurrentStart = rdr.GetDateTime(6);

                    // Staff need to have previous positions and supervisees
                    if (r is Staff) {
                        var s = r as Staff;

                        List<string> supervisees = new List<string>();
                        List<Position> positions = new List<Position>();

                        // Fetch the supervision name list (sorted)
                        rdr.Close();
                        MySqlCommand getSupervisionList = new MySqlCommand("select title, given_name, family_name from researcher " +
                                                                           "where supervisor_id = " + r.Id, conn);
                        rdr = getSupervisionList.ExecuteReader();

                        while (rdr.Read()) {
                            supervisees.Add(string.Format("{0} {1} {2}", rdr.GetString(0), rdr.GetString(1), rdr.GetString(2)));
                        }

                        supervisees.Sort();
                        s.Supervisees = supervisees;

                        // fetch the position list (sorted)
                        rdr.Close();
                        MySqlCommand getPositions = new MySqlCommand("select * from position where id = '" + r.Id + "'", conn);
                        rdr = getPositions.ExecuteReader();

                        while (rdr.Read()) {
                            // only get the previous position (the end year is null)
                            if (!rdr.IsDBNull(3)) {
                                positions.Add(new Position {
                                    Level = EnumStringConverter.ParseEnum<PositionLevel>(rdr.GetString(1)),
                                    Start = rdr.GetDateTime(2),
                                    End = rdr.GetDateTime(3)      
                                });
                                
                            }
                        }
                        
                        // sort the position list according to the datetime
                        positions.Sort((p1, p2) => p2.Start.CompareTo(p1.Start)); 

                        s.Positions = positions;
                    } else if (r is Student) {
                        var s = r as Student;
                        s.Degree = rdr.GetString(3);

                        int supervisorID = rdr.GetInt32(4);

                        // Read the supervisor names 
                        rdr.Close();
                        MySqlCommand getSupervisor = new MySqlCommand("select title, given_name, family_name from researcher " +
                                                                     "where id = '" + supervisorID + "'", conn);
                        rdr = getSupervisor.ExecuteReader();

                        if (rdr.Read()) {
                            s.Supervisor = string.Format("{0} {1} {2}", rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
                        }
                    }
                }

                // Get the publication list (basic), just include the DOI, Title, year information
                rdr.Close();
                MySqlCommand getPublicationList = new MySqlCommand("select publication.doi, year, title from publication, researcher_publication " +
                                                                   "where researcher_publication.doi = publication.doi and " +
                                                                   "researcher_publication.researcher_id = " + r.Id, conn);
                rdr = getPublicationList.ExecuteReader();

                while (rdr.Read()) {
                    publications.Add(new Publication { 
                        Doi = rdr.GetString(0),
                        Title = rdr.GetString(2),
                        Year = rdr.GetInt32(1)
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


        // Get full details of the given publication, which need to be diplayed in PublicaitonListView
        public static void FetchFullPublicationDetails(Publication p) {
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                // Read all the details for current publoication
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


        // Return a list of researcher id (with duplication)
        // The number of duplication represent the count of publications for this researcher in previous 3 years
        public static List<int> FetchAuthorPublicationCount() {
            List<int> authorIDs = new List<int>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try {
                conn.Open();

                // Only see the publication's author and the year
                MySqlCommand getAuthorIDs = new MySqlCommand("select researcher_publication.researcher_id, publication.year " +
                                                             "from researcher_publication, publication " +
                                                             "where researcher_publication.doi = publication.doi", conn);
                rdr = getAuthorIDs.ExecuteReader();

                int currentYear = DateTime.Today.Year;
                while (rdr.Read()) {
                    int year = rdr.GetInt32(1);
                    if (year < currentYear && year >= currentYear - 3) {
                        authorIDs.Add(rdr.GetInt32(0));
                    }
                }
            } catch (MySqlException e) {
                Console.WriteLine("Error connecting to the Database: " + e);
            } finally {
                if (rdr != null) { rdr.Close(); }
                if (conn != null) { conn.Close(); }
            }

            return authorIDs;
        }
    }
}
