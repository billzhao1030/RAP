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
        public static void LoadResearchers() {
            Researchers = ERDAdapter.FetchBasicResearcherDetails();
        }



        // load selected researcher details
        public static void LoadCurrentResearcher(object selected) {
            if (selected != null) { 
                selectedResearcher = (Researcher)selected;

                ERDAdapter.FetchFullResearcherDetails(selectedResearcher);
            }
        }
    }
}
