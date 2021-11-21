using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Research {
    public class Staff : Researcher {
        public List<Position> Positions { get; set; }
        public List<string> Supervisees { get; set; }
        public string CurrentLevelName { get { return EnumStringConverter.GetDescription(CurrentLevel); } }

        

        public List<string> GetSuperviees() {
            return Supervisees;
        }

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

        // calculate the previous three years publication average count
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
    }
}
