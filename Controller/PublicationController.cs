using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Database;
using RAP.Research;

namespace RAP.Controller {
    public static class PublicationController {
        public static Publication selectedPublication { get; private set; }
        public static List<string> PublicationYears { get; private set; }
        public static bool Ascending { get; private set; }


        // load the full details in PublicationDetailsView
        public static List<Publication> LoadPublications() {
            Researcher researcher = ResearcherController.selectedResearcher;
            Ascending = false;

            if (researcher != null) {
                var UtasStart = researcher.UtasStart.Year;
                var range = DateTime.Today.Year - UtasStart + 1;

                PublicationYears = Enumerable.Range(UtasStart, range).Select(o => o.ToString()).ToList();

                // add a default empty value for start
                PublicationYears.Insert(0, "");

                return Sorting(researcher.Publications);
            }

            return null;
        }

        private static List<Publication> Sorting(List<Publication> publications) {
            if (Ascending) {
                return publications.OrderBy(x => x.Year).ThenBy(x => x.Title).ToList();
            } else {
                return publications.OrderByDescending(x => x.Year).ThenBy(x => x.Title).ToList();
            }
        }

        public static List<Publication> Invert() {
            Ascending = !Ascending;
            Researcher researcher = ResearcherController.selectedResearcher;

            return Sorting(researcher.Publications);
        }

        public static List<Publication> FilterByYear(int year1, int year2) {
            int start = Math.Min(year1, year2);
            int end = Math.Max(year1, year2);

            var filter = from publication in ResearcherController.selectedResearcher.Publications
                         where publication.Year >= start && publication.Year <= end
                         select publication;
                         
            return Sorting(filter.ToList());
        }

        public static void LoadPublicationDetails(object selected) {
            if (selected != null) {
                selectedPublication = (Publication)selected;

                ERDAdapter.FetchFullPublicationDetails(selectedPublication);
            }
        }

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
