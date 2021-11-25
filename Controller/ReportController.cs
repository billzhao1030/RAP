using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP.Database;
using RAP.Research;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RAP.Controller {
    public enum PerformanceLevel {
        [Description("Poor Performers")] Poor,
        [Description("Below Expectation")] BelowExpectations,
        [Description("Meeting Minimum")] MeetingMinimum,
        [Description("Star Performers")] Star
    };

    public static class ReportController {
        private static List<Staff> staffs;
        private static double[] levelValues = { 0.0, 0.7, 1.1, 2.0 };

        // get all performances by basic retrieve from database
        public static void LoadAllPerformance() {
            List<int> authorIDs = ERDAdapter.FetchAuthorPublicationCount();
            List<Staff> authors;


            // filter out the staff in researchers
            var getAllStaff = from staff in ResearcherController.Researchers
                              where staff is Staff
                              select staff as Staff;
            authors = getAllStaff.ToList();

            var getCountPair = from count in authorIDs
                    group count by count into countPair
                    select new { key = countPair.Key, count = countPair.Count() };

            Dictionary<int, int> publicationCounts = getCountPair.ToDictionary(n => n.key, n => n.count);

            foreach (Staff staff in authors) {
                staff.LastThreeYearPubCount = publicationCounts.ContainsKey(staff.Id) ? publicationCounts[staff.Id] : 0;
            }

            staffs = authors;
        }

        private static List<Staff> FormatReport(List<Staff> staffs, bool ascending) {
            if (ascending) {
                staffs.Sort((s1, s2) => s1.ReportPerformance.CompareTo(s2.ReportPerformance));
            } else {
                staffs.Sort((s1, s2) => s2.ReportPerformance.CompareTo(s1.ReportPerformance));
            }

            return staffs;
        }

        private static List<Staff> chooseLevel(double lower, double upper) {
            var selectByLevel = from subGroup in staffs
                                where subGroup.ReportPerformance >= lower &&
                                      subGroup.ReportPerformance < upper
                                select subGroup;

            return selectByLevel.ToList();
        }

        public static List<Staff> GenerateReport(PerformanceLevel reportLevel) {
            switch (reportLevel) {
                case PerformanceLevel.Poor:
                    return FormatReport(chooseLevel(levelValues[0], levelValues[1]), true);
                case PerformanceLevel.BelowExpectations:
                    return FormatReport(chooseLevel(levelValues[1], levelValues[2]), true);
                case PerformanceLevel.MeetingMinimum:
                    return FormatReport(chooseLevel(levelValues[2], levelValues[3]), false);
                case PerformanceLevel.Star:
                    return FormatReport(chooseLevel(levelValues[3], double.MaxValue), false);
                default: 
                    return null;
            }
        }

        public static void LoadEmail(List<Staff> staffList) {
            string emails = "";

            foreach (Staff s in staffList) {
                emails += (s.Email + "  ");
            }

            Clipboard.SetText(emails);
        }
    }
}
