using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonPerf.Resolvers
{
	/// <summary>
	/// Can be used for any type to dynamically specify which properties are part of the serialization contract
	/// </summary>
	public class DynamicContractResolver : DefaultContractResolver
	{
		readonly List<string> _contractProperties;
		IList<JsonProperty> _jsonProperties;

		public DynamicContractResolver(params string[] contractProperties)
		{
			_contractProperties = contractProperties?.ToList();
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			var properties = base.CreateProperties(type, memberSerialization);

			if (_contractProperties == null || !_contractProperties.Any())
				return properties;

			// only serialize our dynamic contract properties
			_jsonProperties =
				properties
					.Join(
						 _contractProperties,
						 jsonProperty => jsonProperty.PropertyName,
						 typeProperty => typeProperty,
						 (jsonProperty, arg2) => jsonProperty)
					.ToList();

			return _jsonProperties;
		}
	}
}
