using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_WPF.Helpers
{
    public class FileHelper<T>
    {
        public static string Serialize(T myObject)
        {
            return JsonConvert.SerializeObject(myObject, Formatting.Indented);

        }

        public static T Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }

}
