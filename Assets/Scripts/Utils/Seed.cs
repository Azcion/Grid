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

			int hashed = Hash(str);
			Debug.Log("Hashed seed: " + hashed);

			return hashed;
		}

		private static int Hash (string str = null) {
			while (true) {
				if (string.IsNullOrEmpty(str)) {
					str = Random.Range(int.MinValue, int.MaxValue).ToString();
					continue;
				}

				MD5 hasher = MD5.Create();
				byte[] hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(str));

				return BitConverter.ToInt32(hashed, 0);
			}
		}
	}

}