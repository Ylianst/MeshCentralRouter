using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshCentralRouter
{
    internal static class JsonHelper
    {
        public static Dictionary<string, object> Parse(string data)
        {
            var obj = JsonConvert.DeserializeObject(data);
            return ToCollections(obj) as Dictionary<string, object>;
        }

        public static object ToCollections(object obj)
        {
            if (obj is JObject jo)
            {
                return jo.ToObject<Dictionary<string, object>>()
                    .ToDictionary(k => k.Key, v => ToCollections(v.Value));
            }
            if (obj is JArray ja)
            {
                return new ArrayList(ja.ToObject<List<object>>().Select(ToCollections).ToArray());
            }
            if (obj is long jl && jl <= int.MaxValue)
            {
                return (int)jl;
            }
            return obj;
        }
    }
}
