using System.ComponentModel;
using System.Xml.Serialization;
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	[XmlType("AnimalDef")]
	public class AnimalDef : ThingDef, IThingDef {

		[DefaultValue(true)] public bool Solitary;

	}

}