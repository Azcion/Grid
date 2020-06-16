using System.Xml.Serialization;
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	[XmlType("HumanoidDef")]
	public class HumanoidDef : ThingDef, IThingDef {

		public string GetLabel => Label;
		public string GetDescription => Description;

	}

}