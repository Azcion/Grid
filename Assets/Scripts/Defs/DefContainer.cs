using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.Assertions;

namespace Assets.Scripts.Defs {

	public class DefContainer<T> {

		public readonly List<T> Defs;

		public DefContainer (string path) {
			Type t = typeof(T);

			if (t != typeof(PlantDef) && t != typeof(AnimalDef)) {
				throw new AssertionException("Can only store PlantDef or AnimalDef.", "");
			}

			Defs = Load(path);
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