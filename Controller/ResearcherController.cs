using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Database;
using RAP.Research;

namespace RAP.Controller {
    public static class ResearcherController {

        public static List<Researcher> Researchers { get; private set; }
        public static List<Researcher> Displayed { get; private set; }
        public static Researcher selectedResearcher { get; private set; }

        //TODO: how about a list of researchers which is to be displayed?

        // load researchers from database
        public static List<Researcher> LoadResearchers() {
            Displayed = Researchers = ERDAdapter.FetchBasicResearcherDetails();

            return Researchers;
        }

        // load selected researcher details
        public static void LoadCurrentResearcher(object selected) {
            if (selected != null) { 
                selectedResearcher = (Researcher)selected;

                ERDAdapter.FetchFullResearcherDetails(selectedResearcher);
            }
        }

        public static List<Researcher> FilterByName(string query) {
            List<Researcher> subGroup = new List<Researcher>();
            string value = query.ToUpper();
            if (value.Trim() != "") {
                var filter =
                    from researcher in Displayed
                    where researcher.FamilyName.ToUpper().Contains(value) ||
                          researcher.GivenName.ToUpper().Contains(value)
                    select researcher;

                subGroup = Displayed = filter.ToList();
                return subGroup;
            } else {
                return Researchers;
            }
        }

        public static List<Researcher> FilterByLevel(object type) {
            List<Researcher> subGroup = new List<Researcher>();
            PositionLevel level = EnumStringConverter.ParseEnum<PositionLevel>(type.ToString());
            if (level != PositionLevel.AllResearcher) {
                var filter =
                    from researcher in Displayed
                    where researcher.CurrentLevel == level
                    select researcher;

                subGroup = Displayed = filter.ToList();

                return subGroup;
            } else {
                return Researchers;
            }
        }

        public static List<Researcher> GetResearchers() {
            return Researchers;
        }
    }
}
