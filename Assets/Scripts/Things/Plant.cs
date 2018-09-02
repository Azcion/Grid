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

			if (Size == PlantSize.Small) {
				for (int i = 0; i < Random.Range(0, 4); ++i) {
					CreateChildSprite();
				}
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
					x = Random.Range(.1f, .9f);
					y = Random.Range(.1f, .9f);
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

			Child.localScale = new Vector3(s, s, 1);
			Child.localPosition = new Vector2(x, y);
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

		private Transform CreateChildSprite () {
			Transform newSprite = new GameObject("Sprite").transform;
			newSprite.SetParent(Tf);
			float x = Random.Range(.1f, .9f);
			float y = Random.Range(.1f, .9f);
			float s = Mathf.Lerp(.65f, .85f, _growth);
			newSprite.localPosition = new Vector2(x, y);
			newSprite.localScale = new Vector3(s, s, 1);
			SpriteRenderer sr = newSprite.gameObject.AddComponent<SpriteRenderer>();
			sr.sprite = ChildRenderer.sprite;
			sr.sharedMaterial = ChildRenderer.sharedMaterial;
			return newSprite;
		}

	}

}