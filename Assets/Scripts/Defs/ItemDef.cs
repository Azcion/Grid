using System.ComponentModel;
using System.Xml.Serialization;
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	[XmlType("ItemDef")]
	public class ItemDef : ThingDef, IThingDef {

		[DefaultValue(1)] public int TexCount;
		[DefaultValue(75)] public int StackLimit;

	}

}