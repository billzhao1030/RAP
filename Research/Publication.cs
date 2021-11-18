
using System;

namespace RAP.Research {
    public enum PublicationType {
        Conference,
        Journal,
        Other
    };

    public class Publication {
        public string Doi { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public PublicationType Type { get; set; }
        public string CiteAs { get; set; }
        public DateTime Available { get; set; }
        public int Age { get { return DateTime.Today.Year - Year; } }
    }
}
