using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using JetBrains.Annotations;
using UnityEngine;

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
			Sprite.localPosition = new Vector2(.5f, 0);
			Sprite.localScale = new Vector3(growth, growth, 1);
			SetSprite(AssetLoader.Get(type), Random.value < .5);
		}

		public ThingType ThingType () {
			return Enums.ThingType.Plant;
		}

		private static PlantSize SizeOf (PlantType type) {
			switch (type) {
				case PlantType.Palm:
					return PlantSize.Tree;
				default:
					return PlantSize.Bush;
			}
		}

	}

}