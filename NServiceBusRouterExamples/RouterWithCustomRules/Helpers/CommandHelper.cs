using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RouterWithCustomRules.Json;
using System.Text;

namespace RouterWithCustomRules.Helpers
{
    public static class CommandHelper
    {
        public static string Serialize(object command)
        {
            return JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static object DeserializeMessageToObject(byte[] message)
        {
            var bodyBase64 = Encoding.UTF8.GetString(message);

            var obj = JsonConvert.DeserializeObject(bodyBase64, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new JsonDefaultContractResolver()
            });

            return obj;
        }
    }
}
