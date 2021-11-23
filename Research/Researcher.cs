
/** Researcher class (abstract)
 *  Author: Xunyi Zhao
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RAP.Research {
    public enum Campus {
        Hobart,
        Launceston,
        [Description("Cradle Coast")] CradleCoast
    };

    public abstract class Researcher {
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
        public List<Publication> Publications { get; set; }
        public PositionLevel CurrentLevel { get; set; }

        // Total years(fractional) since {UtasStart}
        public double Tenure { get { return DateTime.Today.Subtract(UtasStart).TotalDays / 365.24; } }
    }
}
