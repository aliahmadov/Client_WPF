using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_WPF.Helpers
{
    public class FileHelper<T> where T : class
    {
        public static string Serialize(T myObject)
        {
            return JsonConvert.SerializeObject(myObject);

        }

        public static object Deserialize(string str)
        {
            return JsonConvert.DeserializeObject(str);
        }
    }

}
