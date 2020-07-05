using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Assets.Scripts.Defs {

	public class DefContainer  {

		public readonly List<Def> Defs;

		private readonly Dictionary<string, Def> _defs;

		public DefContainer (string path) {
			Defs = new List<Def>();
			_defs = new Dictionary<string, Def>();
			Add(path);
		}

		public void Add (string path) {
			List<Def> defs = Load(path);

			foreach (Def def in defs) {
				_defs.Add(def.DefName, def);
			}

			Defs.AddRange(defs);
		}

		public Def Get (string defName) {
			return _defs[defName];
		}

		public void Save (string path) {
			XmlSerializer serializer = new XmlSerializer(typeof(ListWrapper<Def>));
			XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
			xns.Add(string.Empty, string.Empty);

			using (FileStream stream = new FileStream(path, FileMode.Create)) {
				serializer.Serialize(stream, new ListWrapper<Def>(Defs), xns);
			}
		}

		private List<Def> Load (string path) {
			XmlSerializer serializer = new XmlSerializer(typeof(ListWrapper<Def>));

			using (FileStream stream = new FileStream(path, FileMode.Open)) {
				return serializer.Deserialize(stream) as ListWrapper<Def>;
			}
		}

	}

}