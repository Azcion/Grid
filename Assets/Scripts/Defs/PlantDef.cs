using System.ComponentModel;
using System.Xml.Serialization;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Defs {

	[XmlType("PlantDef")]
	public class PlantDef : ThingDef, IThingDef {

		[DefaultValue((PlantSize) 0)] public PlantSize PlantSize;

	}

}