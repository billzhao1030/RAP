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
        public static Researcher selectedResearcher { get; private set; }

        //TODO: how about a list of researchers which is to be displayed?

        // load researchers from database
        public static List<Researcher> LoadResearchers() {
            Researchers = ERDAdapter.FetchBasicResearcherDetails();

            return Researchers;
        }

        // load selected researcher details
        public static void LoadCurrentResearcher(object selected) {
            if (selected != null) { 
                selectedResearcher = (Researcher)selected;

                ERDAdapter.FetchFullResearcherDetails(selectedResearcher);
            }
        }

        public static List<Researcher> FilterBy(string name, object type) {
            List<Researcher> subGroup = Researchers;
            PositionLevel level = EnumStringConverter.ParseEnum<PositionLevel>(type.ToString());
            string value = name.ToUpper();

            if (level != PositionLevel.AllResearcher) {
                var filter =
                    from researcher in subGroup
                    where researcher.CurrentLevel == level
                    select researcher;

                subGroup = filter.ToList();
            }

            if (value.Trim() != "") {
                var filter =
                    from researcher in subGroup
                    where researcher.FamilyName.ToUpper().Contains(value) ||
                          researcher.GivenName.ToUpper().Contains(value)
                    select researcher;

                subGroup = filter.ToList();
            }
            return subGroup;
        }

        public static List<Researcher> GetResearchers() {
            return Researchers;
        }

        public static List<Position> GetPositions() {
            if(selectedResearcher != null) {
                return ((Staff)selectedResearcher).Positions;
            }
            return null; 
        }
    }
}
