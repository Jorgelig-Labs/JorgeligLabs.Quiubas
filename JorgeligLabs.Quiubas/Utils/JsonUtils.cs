using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JorgeligLabs.Quiubas.Utils
{
    public static class JsonUtils
    {
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        public static readonly JsonSerializerSettings EscapeHtmlJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml
        };

        public static readonly JsonSerializerSettings StringEnumJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>(new[] {new StringEnumConverter()}),
            TypeNameHandling = TypeNameHandling.None
        };

        public static readonly JsonSerializerSettings MessageJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>(new[] {new StringEnumConverter()}),
            TypeNameHandling = TypeNameHandling.Objects
        };

        public static readonly JsonSerializerSettings StringEnumNoCamelCaseJsonSerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                Converters = new List<JsonConverter>(new[] {new StringEnumConverter()}),
                TypeNameHandling = TypeNameHandling.None
            };

        private static readonly ConcurrentDictionary<Type, JsonSerializerSettings> CachedInterfaceSerializerSettings
            = new ConcurrentDictionary<Type, JsonSerializerSettings>();

        public static JsonSerializerSettings GetInterfaceSerializerSettings<TInterface>() where TInterface : class
        {
            if (CachedInterfaceSerializerSettings.TryGetValue(typeof(TInterface), out var serializerSettings))
                return serializerSettings;
            serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                Converters = new List<JsonConverter>(new[] {new StringEnumConverter()}),
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new InterfaceContractResolver<TInterface>()
            };
            CachedInterfaceSerializerSettings.TryAdd(typeof(TInterface), serializerSettings);
            return serializerSettings;
        }
    }
}
