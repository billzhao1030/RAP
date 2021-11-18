using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Research {
    public enum Campus {
        Hobart,
        Launceston,
        CradleCoast
    };

    public abstract class Researcher {
        
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Title { get; set; }
        

    }
}
