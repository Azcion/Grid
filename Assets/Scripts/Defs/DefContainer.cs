using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.Assertions;

namespace Assets.Scripts.Defs {

	[SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
	public class DefContainer<T> {

		public readonly List<T> Defs;

		public DefContainer (string path) {
			Type t = typeof(T);

			if (t == typeof(AnimalDef) ||
			    t == typeof(PlantDef) ||
			    t == typeof(HumanoidDef)) {
				Defs = Load(path);
				return;
			}

			throw new AssertionException($"{t} is not a valid def type.", "");
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