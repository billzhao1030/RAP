
/** Position entity class
 * 
 *  This class file provide the definition of the Position entity
 * 
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.ComponentModel;

namespace RAP.Research {

    // The level of position for researchers (add allResearcher for display in comboBox)
    public enum PositionLevel {
        [Description("All Researcher")] AllResearcher,
        Student,
        [Description("A, Post-Doc")] A,
        [Description("B, Lecturer")] B,
        [Description("C, Senior Lecturer")] C,
        [Description("D, Associate Professor")] D,
        [Description("E, Professor")] E
    };

    public class Position {

        // Attributes directly from database
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public PositionLevel Level { get; set; }

        // Extra attribute title that get the description of position level
        public string Title { get { return EnumStringConverter.GetDescription(Level); } } // The user-friendly description of the position
    }
}
