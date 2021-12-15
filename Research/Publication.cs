
/** Publication entity class
 * 
 *  This class file provides the definition of Publication entity
 * 
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System;

namespace RAP.Research {

    // The type of a publication
    public enum PublicationType {
        Conference,
        Journal,
        Other
    };

    public class Publication {

        // Attributes directly from database
        public string Doi { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public PublicationType Type { get; set; }
        public string CiteAs { get; set; }
        public DateTime Available { get; set; }

        // Extra attribute 
        public int Age { get { return (int)(DateTime.Today - Available).TotalDays; } } // The number of years since it published
    }
}
