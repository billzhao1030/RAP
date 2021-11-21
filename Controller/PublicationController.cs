﻿using System;
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
        public static void LoadCurrentPublication(object selected) {
            if (selected != null) {
                selectedPublication = (Publication)selected;

                ERDAdapter.FetchFullPublicationDetails(selectedPublication);
            }
        }

        public static void LoadPublicationDetail(object selected) {
            if (selected != null) {
                selectedPublication = (Publication)selected;

                ERDAdapter.FetchFullPublicationDetails((Publication)selected);
            }
        }


    }
}
