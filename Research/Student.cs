
/** Student class
 *  
 *  This class file provides the definition of Student entity
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

namespace RAP.Research {
    public class Student : Researcher {

        // Attributes directly from database
        public string Degree { get; set; }

        // Supervisor name instead of ID for simple display
        public string Supervisor { get; set; }
    }
}
