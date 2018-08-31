using System;
using Assets.Scripts.Makers;
using UnityEngine;

namespace Assets.Scripts.Utils {

	public static class Calc {

		public static float Round (float value, int digits) {
			return (float) Math.Round(value, digits);
		}

		public static Vector2 Clamp (Vector2 v) {
			return Clamp(v, 0, TileMaker.YTILES - 1, 0, TileMaker.YTILES - 1);
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