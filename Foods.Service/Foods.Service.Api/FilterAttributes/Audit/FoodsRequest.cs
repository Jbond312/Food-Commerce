using System;
using Newtonsoft.Json;

namespace Foods.Service.Api.FilterAttributes.Audit
{
    public class FoodsRequest
    {
        public object Value { get; set; }
        public Type Type { get; set; }

        public FoodsRequest Clone()
        {
            return JsonConvert.DeserializeObject<FoodsRequest>(JsonConvert.SerializeObject(this));
        }
    }
}