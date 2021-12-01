
/** Researcher class (abstract class)
 * 
 *  This class file provide the abstract definition of Researcher entity
 * 
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RAP.Research {

    // The campus name
    public enum Campus {
        Hobart,
        Launceston,
        [Description("Cradle Coast")] CradleCoast
    };

    public abstract class Researcher {

        // Attributs directly from database
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public Campus Campus { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public DateTime UtasStart { get; set; }
        public DateTime CurrentStart { get; set; }

        // Extra but necessary attribute
        public List<Publication> Publications { get; set; }
        public PositionLevel CurrentLevel { get; set; }

        // Total years(fractional) since {UtasStart}
        public double Tenure { get { return DateTime.Today.Subtract(UtasStart).TotalDays / 365.24; } }
    }
}
