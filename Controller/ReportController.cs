using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Database;
using RAP.Research;
using System.ComponentModel;

namespace RAP.Controller {
    public enum PerformanceLevel {
        [Description("Star Performers")]Star,
        [Description("Below Expectation")] BelowExpectations,
        [Description("Meeting Minimum")] MeetingMinimum,
        [Description("Poor Performers")] Poor
    };

    public static class ReportController {
        private static List<Staff> staffs;

        public static void LoadAllPerformance() {

        }

        public static void LoadEmail() {

        }
    }
}
