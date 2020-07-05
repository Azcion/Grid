using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Defs {

	[XmlRoot("Defs")]
	public class ListWrapper<T> : List<T> {

		public ListWrapper () {}

		public ListWrapper (IEnumerable<T> collection) {
			AddRange(collection);
		}

		public List<T> ToList () {
			return GetRange(0, Count);
		}

	}

}