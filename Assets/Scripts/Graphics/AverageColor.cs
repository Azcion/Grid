using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public static class AverageColor {

		private static readonly Color[] TileColors;

		static AverageColor () {
			TileColors = new Color[Enum.GetNames(typeof(TileType)).Length];
		}

		public static void Initialize () {
			foreach (TileType type in Enum.GetValues(typeof(TileType))) {
				Average(type);
			}
		}

		public static Color Get (TileType type) {
			return TileColors[(int) type];
		}

		private static void Average (TileType type) {
			Color32[] tc = AssetLoader.Get(type, 0, 0).texture.GetPixels32();
			int t = tc.Length;
			float r = 0;
			float g = 0;
			float b = 0;

			foreach (Color32 c in tc) {
				r += c.r;
				g += c.g;
				b += c.b;
			}

			TileColors[(int) type] = new Color32((byte) (r / t), (byte) (g / t), (byte) (b / t), 255);
		}

	}

}