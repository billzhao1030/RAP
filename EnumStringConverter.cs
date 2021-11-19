using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RAP {
    public class EnumStringConverter {

        // Parse the given string to related enum value
        public static T ParseEnum<T>(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }

        // get the description of a given Enum value
        public static string GetDescription (Enum value) {
            string description = value.ToString();

            var info = value.GetType().GetField(value.ToString());

            if (info != null) {
                var attr = info.GetCustomAttributes(typeof(DescriptionAttribute), true);

                if (attr != null && attr.Length > 0) {
                    description = ((DescriptionAttribute)attr[0]).Description;
                }
            }


            return description;
        }
    }
}
