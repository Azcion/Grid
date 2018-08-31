using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Plant : Thing, IThing {

		public PlantType Type;
		public PlantSize Size;

		private float _growth;

		public void Initialize (PlantType type, float growth) {
			InitializeThing();

			Type = type;
			Size = SizeOf(type);
			_growth = growth;
			AdjustTransform(growth);
			SetSprite(AssetLoader.Get(type), Random.value < .5);
		}

		public ThingType ThingType () {
			return Enums.ThingType.Plant;
		}

		private void AdjustTransform (float growth) {
			growth = Mathf.Clamp(growth, .15f, 1);
			float s;
			
			switch (Size) {
				case PlantSize.Small:
					s = Mathf.Lerp(.65f, .85f, growth);
					break;
				case PlantSize.Medium:
					s = Mathf.Lerp(1, 1.5f, growth);
					break;
				case PlantSize.Large:
					s = Mathf.Lerp(1.28f, 1.95f, growth);
					break;
				default:
					s = Mathf.Lerp(.75f, 1, growth);
					break;
			}

			Sprite.localScale = new Vector3(s, s, 1);
			Sprite.localPosition = new Vector2(.5f, Mathf.Lerp(.2f, .04f, growth));
		}

		private static PlantSize SizeOf (PlantType type) {
			switch (type) {
				case PlantType.SaguaroCactus:
				case PlantType.TreeDrago:
				case PlantType.TreePalm:
					return PlantSize.Large;
				case PlantType.Agave:
					return PlantSize.Medium;
				case PlantType.Grass:
					return PlantSize.Small;
				default:
					return PlantSize.Medium;
			}
		}

	}

}