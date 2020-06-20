using System.ComponentModel;
using UnityEngine;

// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Defs {

	public abstract class ThingDef {

		[DefaultValue(null)] public string DefName;
		[DefaultValue(null)] public string Label;
		[DefaultValue(null)] public string Description;
		[DefaultValue(null)] public StatBases StatBases;
		[DefaultValue(null)] public string TexPath;
		[DefaultValue(0)] public float EcosystemWeight;
		[DefaultValue(0)] public float SpriteScale;
		[DefaultValue(null)] public Color32 Tint;
		[DefaultValue(true)] public bool Selectable;

	}

}