using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JorgeligLabs.Quiubas.Utils
{
    public class InterfaceContractResolver<TInterface> : CamelCasePropertyNamesContractResolver where TInterface : class
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(typeof(TInterface), memberSerialization);
            return properties;
        }
    }
}
