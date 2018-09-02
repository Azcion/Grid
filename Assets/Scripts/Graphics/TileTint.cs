using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public static class TileTint {

		private static readonly Color[] TileTints;

		static TileTint () {
			TileTints = new Color[Enum.GetNames(typeof(TileType)).Length];
		}

		public static void Initialize () {
			Color32 c = new Color32(143, 148, 142, 255);
			TileTints[(int) TileType.RoughStone] = c;
		}

		public static Color Get (TileType type) {
			switch (type) {
				case TileType.RoughStone:
				case TileType.RoughHewnRock:
				case TileType.SmoothStone:
					return TileTints[(int) TileType.RoughStone];
				default:
					return TileTints[(int) type];
			}
		}

	}

}