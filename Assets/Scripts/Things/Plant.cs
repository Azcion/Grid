using System;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Plant : Thing {

		public PlantType Type;

		private PlantSize _size;
		private float _growth;

		public void Assign (PlantType type, float growth, bool flipX=false) {
			AssertActive();

			_size = SizeOf(type);
			_growth = growth;

			Sprite.localPosition = AdjustPosition(_size, Sprite.localPosition);
			Sprite.localScale = AdjustScale(growth, flipX);
		}

		private static Vector3 AdjustScale (float scale, bool flipX) {
			return new Vector3(flipX ? -scale : scale, scale, 1);
		}

		private static Vector3 AdjustPosition (PlantSize size, Vector3 v) {
			float offset;

			switch (size) {
				case PlantSize.Tree:
					offset = -.4f;
					break;
				case PlantSize.Bush:
				default:
					offset = 0;
					break;
			}

			return new Vector3(v.x, (int) v.y + offset, v.z);
		}

		private static PlantSize SizeOf (PlantType type) {
			switch (type) {
				case PlantType.Palm:
					return PlantSize.Tree;
				case PlantType.Cactus:
				case PlantType.Agave:
				case PlantType.Grass:
					return PlantSize.Bush;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

	}

}