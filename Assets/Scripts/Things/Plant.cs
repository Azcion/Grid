using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Plant : Thing, IThing {

		public PlantType Type;
		public PlantSize Size;

		private const float SPRITE_OFFSET_BUSH = 0;
		private const float SPRITE_OFFSET_TREE = -.4f;

		
		private float _growth;

		public void Initialize (PlantType type, float growth, bool flipX=false) {
			InitializeThing();

			Size = SizeOf(type);
			_growth = growth;

			Sprite.localPosition = AdjustPosition(Size);
			Sprite.localScale = AdjustScale(growth, flipX);
		}

		public ThingType ThingType () {
			return Enums.ThingType.Plant;
		}

		private static Vector3 AdjustScale (float scale, bool flipX) {
			return new Vector3(flipX ? -scale : scale, scale, 1);
		}

		private static Vector3 AdjustPosition (PlantSize size) {
			switch (size) {
				case PlantSize.Tree:
					return new Vector2(.5f, Calc.Round(.5f + SPRITE_OFFSET_TREE, 2));
				default:
					return new Vector2(.5f, Calc.Round(.5f + SPRITE_OFFSET_BUSH, 2));
			}
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