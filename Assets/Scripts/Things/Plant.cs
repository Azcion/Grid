using System;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Plant : Thing {

		public PlantType Type;

		private const float SPRITE_OFFSET_BUSH = 0;
		private const float SPRITE_OFFSET_TREE = -.4f;

		private PlantSize _size;
		private float _growth;

		public void Assign (PlantType type, float growth, bool flipX=false) {
			AssertActive();

			_size = SizeOf(type);
			_growth = growth;

			Sprite.localPosition = AdjustPosition(_size);
			Sprite.localScale = AdjustScale(growth, flipX);
		}

		private static Vector3 AdjustScale (float scale, bool flipX) {
			return new Vector3(flipX ? -scale : scale, scale, 1);
		}

		private static Vector3 AdjustPosition (PlantSize size) {
			switch (size) {
				case PlantSize.Tree:
					return new Vector2(.5f, .5f + SPRITE_OFFSET_TREE);
				default:
					return new Vector2(.5f, .5f + SPRITE_OFFSET_BUSH);
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