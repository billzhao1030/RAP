
/** Convertor class
 * 
 *  This class file provide the behind-code and control of Cumulative count view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System;
using RAP.Research;
using RAP.Controller;
using System.Windows.Data;
using System.Globalization;

namespace RAP.View {
    public class Convertor : IValueConverter {

        // Change the listBox or comboBox list to a user friendly string (for each enum type)
        public object Convert(object value, Type targetType, object o, CultureInfo info) {
            if (value is Campus) {
                return EnumStringConverter.GetDescription((Campus)value);
            } else if (o.ToString() == "PositionLevel") {
                return EnumStringConverter.GetDescription((PositionLevel)value);
            } else if (o.ToString() == "PerformanceLevel") {
                return EnumStringConverter.GetDescription((PerformanceLevel)value);
            } else {
                return null;
            }
        }


        // Have to realize it(for some reason)
        public object ConvertBack(object value, Type targetType, object o, CultureInfo info) {
            return null;
        }
    }
}
