using UnityEngine;

namespace Assets.Scripts.Utils {

	public static class Noise {

		public static float Sum (int x, int y, float frequency, int octaves, float lacunarity, float persistence) {
			float sum = Mathf.PerlinNoise(x * frequency, y * frequency);
			float amplitude = 1f;
			float range = 1f;

			for (int i = 1; i < octaves; ++i) {
				frequency *= lacunarity;
				amplitude *= persistence;
				range += amplitude;
				sum += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
			}

			return sum / range;
		}

	}

}