using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Defs {

	[SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
	public class DefContainer<T> {

		public readonly List<T> Defs;

		private readonly Dictionary<string, T> _defs;

		public DefContainer (string path) {
			Type t = typeof(T);

			if (t == typeof(AnimalDef) ||
			    t == typeof(PlantDef) ||
			    t == typeof(HumanoidDef) ||
			    t == typeof(ItemDef)) {
				Defs = Load(path);
				_defs = new Dictionary<string, T>();

				foreach (T def in Defs) {
					if (!(def is ThingDef thingDef)) {
						Debug.Log("def is null");
						continue;
					}

					string defName = thingDef.DefName;
					_defs.Add(defName, def);
				}

				return;
			}

			throw new AssertionException($"{t} is not a valid def type.", "");
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