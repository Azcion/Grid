using System;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Flora : Thing {

		public FloraType Type;

		private FloraSize _size;
		private float _growth;

		public void Assign (FloraType type, float growth, bool flipX=false) {
			AssertActive();

			_size = SizeOf(type);
			_growth = growth;

			Sprite.localPosition = AdjustPosition(_size, Sprite.localPosition);
			Sprite.localScale = AdjustScale(growth, flipX);
		}

		private static Vector3 AdjustScale (float scale, bool flipX) {
			return new Vector3(flipX ? -scale : scale, scale, 1);
		}

		private static Vector3 AdjustPosition (FloraSize size, Vector3 v) {
			float offset;

			switch (size) {
				case FloraSize.Tree:
					offset = -.4f;
					break;
				case FloraSize.Bush:
				default:
					offset = 0;
					break;
			}

			return new Vector3(v.x, (int) v.y + offset, v.z);
		}

		private static FloraSize SizeOf (FloraType type) {
			switch (type) {
				case FloraType.Palm:
					return FloraSize.Tree;
				case FloraType.Cactus:
				case FloraType.Agave:
				case FloraType.Grass:
					return FloraSize.Bush;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

	}

}