using System.ComponentModel;
using Assets.Scripts.Enums;

// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	public class BuildingDef : ThingDef, IThingDef {

		[DefaultValue(null)] public string UITex;
		[DefaultValue(0)] public LinkedType LinkedType;
		[DefaultValue(0)] public ArchitectCategory Category;

		public string GetLabel => Label;
		public string GetDescription => Description;
		
	}

}