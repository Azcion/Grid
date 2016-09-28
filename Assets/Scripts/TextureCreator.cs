using System;
using System.Linq;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class TextureCreator : MonoBehaviour {

		[UsedImplicitly]
		public string Seed;

		[UsedImplicitly]
		public string ActualSeed;

		[UsedImplicitly]
		[Range (1, 32)]
		public int YChunks = 8;

		[UsedImplicitly]
		[Range(1, 3)]
		public int Dimensions = 2;

		[UsedImplicitly]
		[Range(1, 8)]
		public int Octaves = 4;

		[UsedImplicitly]
		[Range(0f, 4f)]
		public float Frequency = .5f;

		[UsedImplicitly]
		[Range(1f, 4f)]
		public float Lacunarity = 2f;

		[UsedImplicitly]
		[Range(0f, 1f)]
		public float Persistence = .5f;

		[UsedImplicitly]
		public NoiseMethodType NoiseType = NoiseMethodType.Perlin;

		[UsedImplicitly]
		public BiomeType BiomeType = BiomeType.Highlands;

		private const int FACTOR = 8;

		private Transform _map;
		private MeshRenderer _mr;

		private float[,] _values;
		private float[,] _values2;
		private Biome _biome;
		private Texture2D _texture;
		private BiomeType _oldBiome;
		private int _yTiles;
		private int _yTiles2;

		public void ForceUpdate () {
			transform.hasChanged = false;
			_oldBiome = BiomeType;
			_yTiles = YChunks * Chunk.SIZE;
			_yTiles2 = _yTiles * FACTOR;
			_values = new float[_yTiles, _yTiles];
			_values2 = new float[_yTiles2, _yTiles2];

			FillValues();
			FillTexture();
		}

		private static int LongToInt (long value) {
			while (true) {
				if (value < int.MaxValue) {
					return (int) value;
				}

				if (value < uint.MaxValue) {
					return (int) (2L * int.MinValue + value);
				}

				value %= uint.MaxValue;
			}
		}

		private void FillValues () {
			Vector3 point00 = transform.TransformPoint(new Vector3(-.5f, -.5f));
			Vector3 point10 = transform.TransformPoint(new Vector3( .5f, -.5f));
			Vector3 point01 = transform.TransformPoint(new Vector3(-.5f,  .5f));
			Vector3 point11 = transform.TransformPoint(new Vector3( .5f,  .5f));

			NoiseMethod method = Noise.METHODS[(int) NoiseType][Dimensions - 1];
			float stepSize = 1f / _yTiles;

			for (int y = 0; y < _yTiles; y++) {
				Vector3 point0 = Vector3.Lerp(point00, point01, (y + .5f) * stepSize);
				Vector3 point1 = Vector3.Lerp(point10, point11, (y + .5f) * stepSize);

				for (int x = 0; x < _yTiles; x++) {
					Vector3 point = Vector3.Lerp(point0, point1, (x + .5f) * stepSize);
					float sample = Noise.Sum(method, point, Frequency, Octaves, Lacunarity, Persistence);

					if (NoiseType != NoiseMethodType.Value) {
						sample = sample * .64f + .5f;
					}

					_values[y, x] = sample;
				}
			}

			_biome = new Biome(transform.FindChild("Chunk Anchors").gameObject, BiomeType, _values, YChunks);
			stepSize = 1f / _yTiles2;

			if (_texture.width != _yTiles2) {
				_texture.Resize(_yTiles2, _yTiles2);
			}

			for (int y = 0; y < _yTiles2; y++) {
				Vector3 point0 = Vector3.Lerp(point00, point01, (y + .5f) * stepSize);
				Vector3 point1 = Vector3.Lerp(point10, point11, (y + .5f) * stepSize);

				for (int x = 0; x < _yTiles2; x++) {
					Vector3 point = Vector3.Lerp(point0, point1, (x + .5f) * stepSize);
					float sample = Noise.Sum(method, point, Frequency, Octaves, Lacunarity, Persistence);

					if (NoiseType != NoiseMethodType.Value) {
						sample = sample * .64f + .5f;
					}

					_values2[y, x] = sample;
				}
			}
		}

		private void FillTexture () {
			for (int y = 0; y < _yTiles2; y++) {
				for (int x = 0; x < _yTiles2; x++) {
					_texture.SetPixel(x, y, _biome.Coloring.Evaluate(_values2[y, x]));
				}
			}

			_texture.Apply();
		}

		[UsedImplicitly]
		private void OnEnable () {
			_yTiles = YChunks * Chunk.SIZE;
			_yTiles2 = _yTiles * FACTOR;

			if (_texture != null) {
				return;
			}

			_texture = new Texture2D(_yTiles2, _yTiles2, TextureFormat.RGB24, true) {
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Point
			};

			_map = transform.FindChild("Map");
			_mr = _map.GetComponent<MeshRenderer>();
			_mr.material.mainTexture = _texture;
			_mr.material.shader = Shader.Find("Sprites/Diffuse");
		}

		[UsedImplicitly]
		private void Start () {
			ProcessSeed();
			Noise.ScrambleHash();
		}

		[UsedImplicitly]
		private void Update () {
			if (transform.hasChanged || _oldBiome != BiomeType) {
				transform.hasChanged = false;
				_oldBiome = BiomeType;
				_yTiles = YChunks * Chunk.SIZE;
				_yTiles2 = _yTiles * FACTOR;
				_values = new float[_yTiles, _yTiles];
				_values2 = new float[_yTiles2, _yTiles2];

				FillValues();
				FillTexture();
			}
		}

		private void ProcessSeed () {
			long seed;

			if (Seed.Length == 0) {
				seed = Environment.TickCount;
			} else if (!long.TryParse(Seed, out seed)) {
				char[] seedChars = Seed.ToCharArray();
				int[] seedInts = Array.ConvertAll(seedChars, c => c - 32);
				string intsJoined = string.Join("", Array.ConvertAll(seedInts, c => c.ToString()));

				if (!long.TryParse(intsJoined, out seed)) {
					seed = seedInts.Aggregate<int, long>(23, (current, i) => current * 31 + i);
				}
			}

			int intSeed = LongToInt(seed);
			ActualSeed += intSeed;
			UnityEngine.Random.InitState(intSeed);
		}

	}

}