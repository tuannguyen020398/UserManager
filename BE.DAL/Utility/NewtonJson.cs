using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Utility
{
    public class NewtonJson
    {
        public class OrderedContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                return (from p in base.CreateProperties(type, memberSerialization)
                        orderby p.PropertyName
                        select p).ToList();
            }
        }

        private static readonly JsonSerializerSettings MicrosoftDateFormatSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        public static string Serialize(object @object, string dateTimeFormat)
        {

            return JsonConvert.SerializeObject(@object, new IsoDateTimeConverter
            {
                DateTimeFormat = dateTimeFormat
            });
        }

        public static string Serialize(object @object)
        {

            return JsonConvert.SerializeObject(@object, MicrosoftDateFormatSettings);
        }

        public static string SerializeAndOrderProperty(object @object)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new OrderedContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };

            return JsonConvert.SerializeObject(@object, Formatting.Indented, settings);
        }

        public static T Deserialize<T>(string jsonString)
        {

            return JsonConvert.DeserializeObject<T>(jsonString, MicrosoftDateFormatSettings);

        }
        public static T Deserialize<T>(string jsonString, string dateTimeFormat)
        {

            return JsonConvert.DeserializeObject<T>(jsonString, new JsonConverter[1]
            {
                    new IsoDateTimeConverter
                    {
                        DateTimeFormat = dateTimeFormat
                    }
            });

        }
    }
}
