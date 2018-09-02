using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Makers;
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

			// This causes every affected row to be drawn separately, significantly raising draw calls!
			if (Size == PlantSize.Large) {
				const float factor = 1f / TileMaker.YTILES;
				Vector3 v = Tf.position;
				Tf.position = new Vector3(v.x, v.y, v.z - 1 + v.y * factor);
			}
		}

		public ThingType ThingType () {
			return Enums.ThingType.Plant;
		}

		private void AdjustTransform (float growth) {
			growth = Mathf.Clamp(growth, .15f, 1);
			float s;
			float x;
			float y;
			
			switch (Size) {
				case PlantSize.Small:
					s = Mathf.Lerp(.65f, .85f, growth);
					x = Random.Range(.2f, .8f);
					y = Random.Range(.2f, .8f);
					break;
				case PlantSize.Medium:
					s = Mathf.Lerp(1, 1.5f, growth);
					x = .5f;
					y = Mathf.Lerp(.2f, .04f, growth);
					break;
				case PlantSize.Large:
				default:
					s = Mathf.Lerp(1.28f, 1.95f, growth);
					x = .5f;
					y = .04f;
					break;
			}

			Sprite.localScale = new Vector3(s, s, 1);
			Sprite.localPosition = new Vector2(x, y);
		}

		private static PlantSize SizeOf (PlantType type) {
			switch (type) {
				case PlantType.Agave:
				case PlantType.SaguaroCactus:
				case PlantType.TreeDrago:
				case PlantType.TreePalm:
					return PlantSize.Large;
				case PlantType.Grass:
					return PlantSize.Small;
				default:
					return PlantSize.Large;
			}
		}

	}

}