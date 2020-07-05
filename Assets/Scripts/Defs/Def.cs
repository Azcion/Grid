using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Defs {

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	[SuppressMessage("ReSharper", "UnassignedField.Global")]
	public class Def {

		#region All
		public string DefName;
		public string Label;
		public string Description;
		public string TexPath;
		public float SpriteScale;
		public Color32 Tint;
		public bool Selectable;
		public int TexCount;
		#endregion

		#region Building
		public string UITex;
		public LinkedType LinkedType;
		public ArchitectCategory Category;
		public int StackLimit;
		public bool ShowMaterial;
		#endregion

		#region Animal, Plant
		public StatBases StatBases;
		public float EcosystemWeight;
		public string Resource;
		public int ResourceYield;
		#endregion

		#region Animal
		public bool Solitary;
		#endregion

		#region Plant
		public PlantSize PlantSize;
		public bool Cluster;
		public bool CanBuildOver;
		#endregion

	}

}