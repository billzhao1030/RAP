
/** Staff class
 * 
 *  This class file provides the definition of Staff entity
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System;
using System.Collections.Generic;

namespace RAP.Research {
    public class Staff : Researcher {
        public List<Position> Positions { get; set; }
        public List<string> Supervisees { get; set; } // The list of supervisee's name
        public string CurrentLevelName { get { return EnumStringConverter.GetDescription(CurrentLevel); } }


        // The expected number of publications according to the position level
        private double ExpectedNumPublications {
            get {
                switch(CurrentLevel) {
                    case PositionLevel.A: return 0.5;
                    case PositionLevel.B: return 1;
                    case PositionLevel.C: return 2;
                    case PositionLevel.D: return 3.2;
                    case PositionLevel.E: return 4;
                    default: return 0.0;
                }
            }
        }

        // Calculate the previous three years publication average count
        public double ThreeYearAverage {
            get {
                int num = 0;
                int thisYear = DateTime.Now.Year;

                foreach (var publication in Publications)
                {
                    int publishedYear = publication.Year;

                    if (thisYear - 3 <= publishedYear && thisYear > publishedYear) {
                        num++;
                    }
                }
                return num / 3.0;
            }
        }

        public double Performance { get { return ThreeYearAverage / ExpectedNumPublications; } }

        // Two assistant field for report generation
        public int LastThreeYearPubCount { get; set; } // The number of publications in last three years

        public double ReportPerformance { get { return LastThreeYearPubCount / 3.0 / ExpectedNumPublications; } } // Same as performance, but for report use
    }
}
