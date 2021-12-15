
/** Researcher controller class
 * 
 *  This file provides the control of the views that related to researchers
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System.Collections.Generic;
using System.Linq;
using RAP.Database;
using RAP.Research;

namespace RAP.Controller {
    public static class ResearcherController {

        // The list of researchers read from the database
        public static List<Researcher> Researchers { get; private set; }
        // The selected reseacher in the list view
        public static Researcher selectedResearcher { get; private set; }



        // Load researchers from database
        public static List<Researcher> LoadResearchers() {
            Researchers = ERDAdapter.FetchBasicResearcherDetails();

            return Researchers;
        }


        // Load selected researcher details
        public static void LoadCurrentResearcher(Researcher selected) {
            selectedResearcher = selected;

            ERDAdapter.FetchFullResearcherDetails(selectedResearcher);
        }


        // Filter displayed researchers by the name entered and the selected type
        public static List<Researcher> FilterBy(string value, PositionLevel level) {
            List<Researcher> subGroup = Researchers; // Reseachers displayed on the screen

            string name = value.Trim();

            // First filter by level
            if (level != PositionLevel.AllResearcher) {
                var filter =
                    from researcher in subGroup
                    where researcher.CurrentLevel == level
                    select researcher;

                subGroup = filter.ToList();
            }

            // Then by name (trim the front space first)
            if (name != "") {
                var filter =
                    from researcher in subGroup
                    where researcher.FamilyName.ToUpper().Contains(name) ||
                          researcher.GivenName.ToUpper().Contains(name)
                    select researcher;

                subGroup = filter.ToList();
            }
            return subGroup;
        }
    }
}
