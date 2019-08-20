using System;
using UnityEngine;

namespace Assets.Scripts.Utils {

	public static class Calc {

		public static float Round (float value, int digits) {
			return (float) Math.Round(value, digits);
		}

		public static Vector2 Clamp (Vector2 v) {
			return Clamp(v, 0, Map.YTiles - 1, 0, Map.YTiles - 1);
		}

		public static Vector2Int Clamp (Vector2Int v) {
			Vector2 v2 = Clamp(new Vector2(v.x, v.y));
			return new Vector2Int((int) v2.x, (int) v2.y);
		}

		private static Vector2 Clamp (Vector2 v, int x0, int x1, int y0, int y1) {
			int x = (int) v.x;
			int y = (int) v.y;

			if (x < x0) {
				x = x0;
			} else if (x > x1) {
				x = x1;
			}

			if (y < y0) {
				y = y0;
			} else if (y > y1) {
				y = y1;
			}

			return new Vector2(x, y);
		}

	}

}