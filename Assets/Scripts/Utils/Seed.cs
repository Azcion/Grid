using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Utils {

	public static class Seed {
		
		public static int Get (string str) {
			if (str == null) {
				Debug.Log("Null seed. Generating random...");
				return Hash();
			}

			if (int.TryParse(str, out int seed)) {
				Debug.Log("Valid seed: " + seed);
				return seed;
			}

			string seedStr = str;

			for (int i = 0; i < 5; ++i) {
				if (int.TryParse(seedStr, out seed)) {
					Debug.Log("Computed seed: " + seedStr);
					return seed;
				}

				seedStr = "1";

				foreach (char c in str) {
					if (i == 0) {
						seedStr += (c - 32).ToString();
					} else {
						seedStr += ((c - 32) >> 1).ToString();
					}
				}
			}

			Debug.Log("Invalid seed. Generating random...");
			return Hash();
		}

		private static int Hash (string str = null) {
			while (true) {
				if (string.IsNullOrEmpty(str)) {
					Debug.Log("Null seed. Generating random...");
					// ReSharper disable once SpecifyACultureInStringConversionExplicitly
					str = Random.value.ToString();
					continue;
				}

				MD5 hasher = MD5.Create();
				byte[] hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

				return BitConverter.ToInt32(hashed, 0);
			}
		}
	}

}