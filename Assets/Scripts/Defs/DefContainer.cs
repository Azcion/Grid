using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Defs {

	[SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
	public class DefContainer<T> where T : ThingDef {

		public readonly List<T> Defs;

		private readonly Dictionary<string, T> _defs;

		public DefContainer (string path) {
			if (!typeof(IThingDef).IsAssignableFrom(typeof(T))) {
				throw new AssertionException($"{typeof(T)} is not a valid def type.", "");
			}

			Defs = new List<T>();
			_defs = new Dictionary<string, T>();
			Add(path);
		}

		public void Add (string path) {
			List<T> defs = Load(path);

			foreach (T def in defs) {
				if (!(def is ThingDef thingDef)) {
					Debug.Log("Invalid def");
					continue;
				}

				string defName = thingDef.DefName;
				_defs.Add(defName, def);
			}

			Defs.AddRange(defs);
		}

		public T Get (string defName) {
			return _defs[defName];
		}

		public void Save (string path) {
			XmlSerializer serializer = new XmlSerializer(typeof(ListWrapper<T>));
			XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
			xns.Add(string.Empty, string.Empty);

			using (FileStream stream = new FileStream(path, FileMode.Create)) {
				serializer.Serialize(stream, new ListWrapper<T>(Defs), xns);
			}
		}

		private List<T> Load (string path) {
			XmlSerializer serializer = new XmlSerializer(typeof(ListWrapper<T>));

			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				return serializer.Deserialize(stream) as ListWrapper<T>;
			}
		}

	}

}