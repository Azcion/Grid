using System;
using UnityEngine;

namespace Assets.Scripts.Utils {

	public static class Calc {

		public static float Round (float value, int digits) {
			return (float) Math.Round(value, digits);
		}

		public static int Round (float value) {
			return (int) Math.Round(value);
		}

		public static Vector3 RoundVector (Vector3 v) {
			return new Vector3((int) v.x, (int) v.y, (int) v.z);
		}

	}

}