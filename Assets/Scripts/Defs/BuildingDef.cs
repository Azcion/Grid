using System.ComponentModel;
using System.Xml.Serialization;
using Assets.Scripts.Enums;

// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	public class BuildingDef : ThingDef, IThingDef {

		[DefaultValue(0)] public LinkedType LinkedType;

		public string GetLabel => Label;
		public string GetDescription => Description;
		
	}

}