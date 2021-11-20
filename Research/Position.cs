
using System;
using System.ComponentModel;

namespace RAP.Research {
    public enum PositionLevel {
        [Description("All Researcher")] AllResearcher,
        Student,
        [Description("Post-Doc")] A,
        [Description("Lecturer")] B,
        [Description("Senior Lecturer")] C,
        [Description("Associate Professor")] D,
        [Description("Professor")] E
    };

    public class Position {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public PositionLevel Level { get; set; }
        public string Title { get { return EnumStringConverter.GetDescription(Level); } }
    }
}
