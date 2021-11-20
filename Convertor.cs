using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP;
using RAP.Research;
using RAP.Controller;
using System.Windows.Data;
using System.Globalization;

namespace RAP.View {
    public class Convertor : IValueConverter {
        public object Convert(object value, Type targetType, object o, CultureInfo info) {
            if (value is Campus) {
                return EnumStringConverter.GetDescription((Campus)value);
            } else if (o.ToString() == "PositionLevel") {
                return EnumStringConverter.GetDescription((PositionLevel)value);
            } else {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object o, CultureInfo info) {
            return null;
        }
    }
}
