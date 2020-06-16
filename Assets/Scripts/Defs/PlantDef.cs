using System.ComponentModel;
using System.Xml.Serialization;
using Assets.Scripts.Enums;
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	[XmlType("PlantDef")]
	public class PlantDef : ThingDef, IThingDef {

		[DefaultValue((PlantSize) 0)] public PlantSize PlantSize;
		[DefaultValue(1)] public int TexCount;
		[DefaultValue(false)] public bool Cluster;
		[DefaultValue(false)] public bool CanBuildOver;
		[DefaultValue(null)] public string Resource;
		[DefaultValue(0)] public int ResourceYield;

		public string GetLabel () {
			return Label;
		}

		public string GetDescription () {
			return Description;
		}
	}

}