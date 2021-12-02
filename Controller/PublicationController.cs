
/** Publication controller class
 * 
 *  This file provides control to the views related to publication
 * 
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.Collections.Generic;
using System.Linq;
using RAP.Database;
using RAP.Research;

namespace RAP.Controller {
    public static class PublicationController {

        public static Publication selectedPublication { get; private set; } // The publication that user select
        public static List<string> PublicationYears { get; private set; } // The list of years, from utas_start to now
        public static bool Ascending { get; private set; } // The sorting way
        public static List<Publication> Publications { get; private set; }


        // Load the full details in PublicationDetailsView
        public static List<Publication> LoadPublications() {
            Researcher researcher = ResearcherController.selectedResearcher;
            Ascending = false; // Initially Descending

            if (researcher != null) {
                var UtasStart = researcher.UtasStart.Year; // Start year
                var range = DateTime.Today.Year - UtasStart + 1; // The range from start year to now

                PublicationYears = Enumerable.Range(UtasStart, range).Select(o => o.ToString()).ToList();

                // Add a default empty value for start
                PublicationYears.Insert(0, "");

                Publications = researcher.Publications;

                return Sorting(Publications);
            }

            return null;
        }


        // Sort the publication list (ascending or descending)
        private static List<Publication> Sorting(List<Publication> publications) {
            if (Ascending) {
                return publications.OrderBy(x => x.Year).ThenBy(x => x.Title).ToList();
            } else {
                return publications.OrderByDescending(x => x.Year).ThenBy(x => x.Title).ToList();
            }
        }


        // Invert the displayed publication list
        public static List<Publication> Invert() {
            Ascending = !Ascending; // Invert the sorting way

            return Sorting(Publications);
        }


        // Filter the displayed publication list by year range
        public static List<Publication> FilterByYear(int year1, int year2) {
            int start = Math.Min(year1, year2);
            int end = Math.Max(year1, year2);

            var filter = from publication in ResearcherController.selectedResearcher.Publications
                         where publication.Year >= start && publication.Year <= end
                         select publication;

            Publications = filter.ToList();
                         
            return Sorting(Publications);
        }


        // Fetch the details of a selected publication (if selected)
        public static void LoadPublicationDetails(Publication selected) {
            selectedPublication = selected;

            ERDAdapter.FetchFullPublicationDetails(selectedPublication);
        }


        // Cumulative count function, return a tuple <year, count>
        public static List<Tuple<int, int>> CumulativeCount() {
            List<Tuple<int, int>> table = new List<Tuple<int, int>>();
            Researcher researcher = ResearcherController.selectedResearcher;
            
            if (researcher != null) {
                int start = researcher.UtasStart.Year;
                int end = DateTime.Today.Year;

                for (int i = start; i <= end; i++) {
                    var query = from publication in researcher.Publications
                                where publication.Year == i
                                select publication;

                    table.Add(new Tuple<int, int>(i, query.Count()));
                }

                return table;
            }

            return null;
        }
    }
}
