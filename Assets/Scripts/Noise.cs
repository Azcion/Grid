using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Assets.Scripts {

	public delegate float NoiseMethod (Vector3 point, float frequency);

	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public static class Noise {

		public static readonly NoiseMethod[] VALUE_METHODS = {
			Value1D,
			Value2D,
			Value3D
		};

		public static readonly NoiseMethod[] PERLIN_METHODS = {
			Perlin1D,
			Perlin2D,
			Perlin3D
		};

		public static readonly NoiseMethod[][] METHODS = {
			VALUE_METHODS,
			PERLIN_METHODS
		};

		private const int HASH_MASK = 255;
		private const int GRADIENT_MASK_1D = 1;
		private const int GRADIENT_MASK_2D = 7;
		private const int GRADIENT_MASK_3D = 15;

		private static readonly int[] _hash = {
			151, 160, 137,  91,  90,  15, 131,  13, 201,  95,  96,  53, 194, 233,   7, 225,
			140,  36, 103,  30,  69, 142,   8,  99,  37, 240,  21,  10,  23, 190,   6, 148,
			247, 120, 234,  75,   0,  26, 197,  62,  94, 252, 219, 203, 117,  35,  11,  32,
			 57, 177,  33,  88, 237, 149,  56,  87, 174,  20, 125, 136, 171, 168,  68, 175,
			 74, 165,  71, 134, 139,  48,  27, 166,  77, 146, 158, 231,  83, 111, 229, 122,
			 60, 211, 133, 230, 220, 105,  92,  41,  55,  46, 245,  40, 244, 102, 143,  54,
			 65,  25,  63, 161,   1, 216,  80,  73, 209,  76, 132, 187, 208,  89,  18, 169,
			200, 196, 135, 130, 116, 188, 159,  86, 164, 100, 109, 198, 173, 186,   3,  64,
			 52, 217, 226, 250, 124, 123,   5, 202,  38, 147, 118, 126, 255,  82,  85, 212,
			207, 206,  59, 227,  47,  16,  58,  17, 182, 189,  28,  42, 223, 183, 170, 213,
			119, 248, 152,   2,  44, 154, 163,  70, 221, 153, 101, 155, 167,  43, 172,   9,
			129,  22,  39, 253,  19,  98, 108, 110,  79, 113, 224, 232, 178, 185, 112, 104,
			218, 246,  97, 228, 251,  34, 242, 193, 238, 210, 144,  12, 191, 179, 162, 241,
			 81,  51, 145, 235, 249,  14, 239, 107,  49, 192, 214,  31, 181, 199, 106, 157,
			184,  84, 204, 176, 115, 121,  50,  45, 127,   4, 150, 254, 138, 236, 205,  93,
			222, 114,  67,  29,  24,  72, 243, 141, 128, 195,  78,  66, 215,  61, 156, 180,
			151, 160, 137,  91,  90,  15, 131,  13, 201,  95,  96,  53, 194, 233,   7, 225,
			140,  36, 103,  30,  69, 142,   8,  99,  37, 240,  21,  10,  23, 190,   6, 148,
			247, 120, 234,  75,   0,  26, 197,  62,  94, 252, 219, 203, 117,  35,  11,  32,
			 57, 177,  33,  88, 237, 149,  56,  87, 174,  20, 125, 136, 171, 168,  68, 175,
			 74, 165,  71, 134, 139,  48,  27, 166,  77, 146, 158, 231,  83, 111, 229, 122,
			 60, 211, 133, 230, 220, 105,  92,  41,  55,  46, 245,  40, 244, 102, 143,  54,
			 65,  25,  63, 161,   1, 216,  80,  73, 209,  76, 132, 187, 208,  89,  18, 169,
			200, 196, 135, 130, 116, 188, 159,  86, 164, 100, 109, 198, 173, 186,   3,  64,
			 52, 217, 226, 250, 124, 123,   5, 202,  38, 147, 118, 126, 255,  82,  85, 212,
			207, 206,  59, 227,  47,  16,  58,  17, 182, 189,  28,  42, 223, 183, 170, 213,
			119, 248, 152,   2,  44, 154, 163,  70, 221, 153, 101, 155, 167,  43, 172,   9,
			129,  22,  39, 253,  19,  98, 108, 110,  79, 113, 224, 232, 178, 185, 112, 104,
			218, 246,  97, 228, 251,  34, 242, 193, 238, 210, 144,  12, 191, 179, 162, 241,
			 81,  51, 145, 235, 249,  14, 239, 107,  49, 192, 214,  31, 181, 199, 106, 157,
			184,  84, 204, 176, 115, 121,  50,  45, 127,   4, 150, 254, 138, 236, 205,  93,
			222, 114,  67,  29,  24,  72, 243, 141, 128, 195,  78,  66, 215,  61, 156, 180
		};

		private static readonly float SQRT_2 = Mathf.Sqrt(2f);
		private static readonly float[] GRADIENTS_1D = { 1f, -1f };

		private static readonly Vector2[] GRADIENTS_2D = {
			new Vector2( 1f,  0f),
			new Vector2(-1f,  0f),
			new Vector2( 0f,  1f),
			new Vector2( 0f, -1f),
			new Vector2( 1f,  1f).normalized,
			new Vector2(-1f,  1f).normalized,
			new Vector2( 1f, -1f).normalized,
			new Vector2(-1f, -1f).normalized
		};

		private static readonly Vector3[] GRADIENTS_3D = {
			new Vector3( 1f,  1f,  0f),
			new Vector3(-1f,  1f,  0f),
			new Vector3( 1f, -1f,  0f),
			new Vector3(-1f, -1f,  0f),
			new Vector3( 1f,  0f,  1f),
			new Vector3(-1f,  0f,  1f),
			new Vector3( 1f,  0f, -1f),
			new Vector3(-1f,  0f, -1f),
			new Vector3( 0f,  1f,  1f),
			new Vector3( 0f, -1f,  1f),
			new Vector3( 0f,  1f, -1f),
			new Vector3( 0f, -1f, -1f),
			new Vector3( 1f,  1f,  0f),
			new Vector3(-1f,  1f,  0f),
			new Vector3( 0f, -1f,  1f),
			new Vector3( 0f, -1f, -1f)
		};

		public static void ScrambleHash () {
			int n = _hash.Length / 2;

			while (n > 1) {
				--n;
				int k = Random.Range(0, n + 1);
				int value = _hash[k];
				_hash[k] = _hash[n];
				_hash[k + 256] = _hash[n];
				_hash[n] = value;
				_hash[n + 256] = value;
			}
		}

		public static float Value1D (Vector3 point, float frequency) {
			point *= frequency;
			int value = Mathf.FloorToInt(point.x);

			float t = Smooth(point.x - value);

			value &= HASH_MASK;

			int value1 = value + 1;

			int hash0 = _hash[value];
			int hash1 = _hash[value1];

			return Mathf.Lerp(hash0, hash1, t) * (1f / HASH_MASK);
		}

		public static float Value2D (Vector3 point, float frequency) {
			point *= frequency;
			int valueX = Mathf.FloorToInt(point.x);
			int valueY = Mathf.FloorToInt(point.y);

			float tX = Smooth(point.x - valueX);
			float tY = Smooth(point.y - valueY);

			valueX &= HASH_MASK;
			valueY &= HASH_MASK;

			int valueX1 = valueX + 1;
			int valueY1 = valueY + 1;

			int hash0 = _hash[valueX];
			int hash1 = _hash[valueX1];
			int hash00 = _hash[hash0 + valueY];
			int hash01 = _hash[hash0 + valueY1];
			int hash10 = _hash[hash1 + valueY];
			int hash11 = _hash[hash1 + valueY1];

			float lerp00To10 = Mathf.Lerp(hash00, hash10, tX);
			float lerp01To11 = Mathf.Lerp(hash01, hash11, tX);

			return Mathf.Lerp(lerp00To10, lerp01To11, tY) * (1f / HASH_MASK);
		}

		public static float Value3D (Vector3 point, float frequency) {
			point *= frequency;
			int valueX = Mathf.FloorToInt(point.x);
			int valueY = Mathf.FloorToInt(point.y);
			int valueZ = Mathf.FloorToInt(point.z);

			float tX = Smooth(point.x - valueX);
			float tY = Smooth(point.y - valueY);
			float tZ = Smooth(point.z - valueZ);

			valueX &= HASH_MASK;
			valueY &= HASH_MASK;
			valueZ &= HASH_MASK;

			int valueX1 = valueX + 1;
			int valueY1 = valueY + 1;
			int valueZ1 = valueZ + 1;

			int hash0 = _hash[valueX];
			int hash1 = _hash[valueX1];
			int hash00 = _hash[hash0 + valueY];
			int hash01 = _hash[hash0 + valueY1];
			int hash10 = _hash[hash1 + valueY];
			int hash11 = _hash[hash1 + valueY1];
			int hash000 = _hash[hash00 + valueZ];
			int hash001 = _hash[hash00 + valueZ1];
			int hash010 = _hash[hash01 + valueZ];
			int hash011 = _hash[hash01 + valueZ1];
			int hash100 = _hash[hash10 + valueZ];
			int hash101 = _hash[hash10 + valueZ1];
			int hash110 = _hash[hash11 + valueZ];
			int hash111 = _hash[hash11 + valueZ1];

			float lerp000To100 = Mathf.Lerp(hash000, hash100, tX);
			float lerp010To110 = Mathf.Lerp(hash010, hash110, tX);
			float lerp001To101 = Mathf.Lerp(hash001, hash101, tX);
			float lerp011To111 = Mathf.Lerp(hash011, hash111, tX);

			float lerp000100To010110 = Mathf.Lerp(lerp000To100, lerp010To110, tY);
			float lerp001101To011111 = Mathf.Lerp(lerp001To101, lerp011To111, tY);

			return Mathf.Lerp(lerp000100To010110, lerp001101To011111, tZ) * (1f / HASH_MASK);
		}

		public static float Perlin1D (Vector3 point, float frequency) {
			point *= frequency;
			int value = Mathf.FloorToInt(point.x);

			float t0 = point.x - value;
			float t1 = t0 - 1;

			value &= HASH_MASK;

			int value1 = value + 1;

			float grad0 = GRADIENTS_1D[_hash[value] & GRADIENT_MASK_1D];
			float grad1 = GRADIENTS_1D[_hash[value1] & GRADIENT_MASK_1D];

			float v0 = grad0 * t0;
			float v1 = grad1 * t1;

			float t = Smooth(t0);

			return Mathf.Lerp(v0, v1, t) * 2;
		}

		public static float Perlin2D (Vector3 point, float frequency) {
			point *= frequency;
			int valueX = Mathf.FloorToInt(point.x);
			int valueY = Mathf.FloorToInt(point.y);

			float tX0 = point.x - valueX;
			float tY0 = point.y - valueY;
			float tX1 = tX0 - 1;
			float tY1 = tY0 - 1;

			valueX &= HASH_MASK;
			valueY &= HASH_MASK;

			int valueX1 = valueX + 1;
			int valueY1 = valueY + 1;

			int hash0 = _hash[valueX];
			int hash1 = _hash[valueX1];

			Vector2 grad00 = GRADIENTS_2D[_hash[hash0 + valueY] & GRADIENT_MASK_2D];
			Vector2 grad01 = GRADIENTS_2D[_hash[hash0 + valueY1] & GRADIENT_MASK_2D];
			Vector2 grad10 = GRADIENTS_2D[_hash[hash1 + valueY] & GRADIENT_MASK_2D];
			Vector2 grad11 = GRADIENTS_2D[_hash[hash1 + valueY1] & GRADIENT_MASK_2D];

			float v00 = Dot(grad00, tX0, tY0);
			float v10 = Dot(grad10, tX1, tY0);
			float v01 = Dot(grad01, tX0, tY1);
			float v11 = Dot(grad11, tX1, tY1);

			float tX = Smooth(tX0);
			float tY = Smooth(tY0);

			float lerp00To10 = Mathf.Lerp(v00, v10, tX);
			float lerp01To11 = Mathf.Lerp(v01, v11, tX);

			return Mathf.Lerp(lerp00To10, lerp01To11, tY) * SQRT_2;
		}

		public static float Perlin3D (Vector3 point, float frequency) {
			point *= frequency;
			int valueX = Mathf.FloorToInt(point.x);
			int valueY = Mathf.FloorToInt(point.y);
			int valueZ = Mathf.FloorToInt(point.z);

			float tX0 = point.x - valueX;
			float tY0 = point.y - valueY;
			float tZ0 = point.z - valueZ;
			float tX1 = tX0 - 1f;
			float tY1 = tY0 - 1f;
			float tZ1 = tZ0 - 1f;

			valueX &= HASH_MASK;
			valueY &= HASH_MASK;
			valueZ &= HASH_MASK;

			int valueX1 = valueX + 1;
			int valueY1 = valueY + 1;
			int valueZ1 = valueZ + 1;

			int hash0 = _hash[valueX];
			int hash1 = _hash[valueX1];
			int hash00 = _hash[hash0 + valueY];
			int hash01 = _hash[hash0 + valueY1];
			int hash10 = _hash[hash1 + valueY];
			int hash11 = _hash[hash1 + valueY1];

			Vector3 grad000 = GRADIENTS_3D[_hash[hash00 + valueZ] & GRADIENT_MASK_3D];
			Vector3 grad001 = GRADIENTS_3D[_hash[hash00 + valueZ1] & GRADIENT_MASK_3D];
			Vector3 grad010 = GRADIENTS_3D[_hash[hash01 + valueZ] & GRADIENT_MASK_3D];
			Vector3 grad011 = GRADIENTS_3D[_hash[hash01 + valueZ1] & GRADIENT_MASK_3D];
			Vector3 grad100 = GRADIENTS_3D[_hash[hash10 + valueZ] & GRADIENT_MASK_3D];
			Vector3 grad101 = GRADIENTS_3D[_hash[hash10 + valueZ1] & GRADIENT_MASK_3D];
			Vector3 grad110 = GRADIENTS_3D[_hash[hash11 + valueZ] & GRADIENT_MASK_3D];
			Vector3 grad111 = GRADIENTS_3D[_hash[hash11 + valueZ1] & GRADIENT_MASK_3D];

			float v000 = Dot(grad000, tX0, tY0, tZ0);
			float v001 = Dot(grad001, tX0, tY0, tZ1);
			float v010 = Dot(grad010, tX0, tY1, tZ0);
			float v011 = Dot(grad011, tX0, tY1, tZ1);
			float v100 = Dot(grad100, tX1, tY0, tZ0);
			float v101 = Dot(grad101, tX1, tY0, tZ1);
			float v110 = Dot(grad110, tX1, tY1, tZ0);
			float v111 = Dot(grad111, tX1, tY1, tZ1);

			float tX = Smooth(tX0);
			float tY = Smooth(tY0);
			float tZ = Smooth(tZ0);

			float lerp000To100 = Mathf.Lerp(v000, v100, tX);
			float lerp010To110 = Mathf.Lerp(v010, v110, tX);
			float lerp001To101 = Mathf.Lerp(v001, v101, tX);
			float lerp011To111 = Mathf.Lerp(v011, v111, tX);

			float lerp000100To010110 = Mathf.Lerp(lerp000To100, lerp010To110, tY);
			float lerp001101To011111 = Mathf.Lerp(lerp001To101, lerp011To111, tY);

			return Mathf.Lerp(lerp000100To010110, lerp001101To011111, tZ);
		}

		public static float Sum (NoiseMethod method, Vector3 point,
				float frequency, int octaves, float lacunarity, float persistence) {
			float sum = method(point, frequency);
			float amplitude = 1f;
			float range = 1f;

			for (int i = 1; i < octaves; ++i) {
				frequency *= lacunarity;
				amplitude *= persistence;
				range += amplitude;
				sum += method(point, frequency) * amplitude;
			}

			return sum / range;
		}

		private static float Smooth (float t) {
			// 6t^5 - 15t^4 + 10t^3
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		private static float Dot (Vector2 g, float x, float y) {
			return g.x * x + g.y * y;
		}

		private static float Dot (Vector3 g, float x, float y, float z) {
			return g.x * x + g.y * y + g.z * z;
		}

	}

}