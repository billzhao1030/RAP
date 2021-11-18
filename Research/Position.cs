using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RAP.Research {
    public enum PositionLevel {
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

        // maybe a enum to string convert property called Title here
    }
}
