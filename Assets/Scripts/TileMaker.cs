using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class TileMaker : MonoBehaviour {

		public const int CSIZE = 8;
		public const int YCHUNKS = 25;
		public const int CHALF = CSIZE / 2;
		public const int YTILES = CSIZE * YCHUNKS;
		public const int THALF = YTILES / 2;
		public const int CENTER = YCHUNKS / 2;

		// Object references
		#region
		[UsedImplicitly]
		public GameObject ChunkPrefab;

		[UsedImplicitly]
		public GameObject Container;
		#endregion

		// Materials
		#region
		[UsedImplicitly]
		public Material DeepWaterMat;

		[UsedImplicitly]
		public Material ShallowWaterMat;

		[UsedImplicitly]
		public Material SandMat;

		[UsedImplicitly]
		public Material GrassMat;

		[UsedImplicitly]
		public Material DirtMat;

		[UsedImplicitly]
		public Material RockMat;

		[UsedImplicitly]
		public Material SnowMat;

		[UsedImplicitly]
		public Material DefaultMat;
		#endregion

		private static int _seed;
		private static bool _ready;

		private static List<List<GameObject>> _tiles;

		public static GameObject Get (int x, int y) {
			if (!_ready || x < 0 || x >= YTILES || y < 0 || y >= YTILES) {
				return null;
			}

			return _tiles[y][x];
		}

		[UsedImplicitly]
		private void Start () {
			_seed = ApplicationController.Seed;
			_tiles = new List<List<GameObject>>();

			for (int i = 0; i < YTILES; ++i) {
				_tiles.Add(new List<GameObject>());
			}

			StartCoroutine(Create());
		}

		private IEnumerator Create () {
			yield return new WaitForSeconds(.05f);

			for (int y = 0; y < YCHUNKS; ++y) {
				for (int x = 0; x < YCHUNKS; ++x) {
					Vector3 pos = new Vector3(CSIZE * x, CSIZE * y, ChunkPrefab.transform.position.z);
					GameObject c = Instantiate(ChunkPrefab, pos, Quaternion.identity, Container.transform);
					c.name = "Chunk " + y + "y " + x + "x";

					foreach (Transform t in c.transform) {
						InitializeTile(t);
					}
				}
			}

			_ready = true;
			ApplicationController.NotifyReady();
		}

		private void InitializeTile (Transform t) {
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			float v = Noise.Sum(x + _seed, y + _seed, .01f, 6, 2, .5f);

			TileType type;
			Material mat;
			Color color;

			if (v > .60) {
				type = TileType.Snow;
				mat = SnowMat;
				color = TileSprites.CSnow;
			} else if (v > .55) {
				type = TileType.Rock;
				mat = RockMat;
				color = TileSprites.CRock;
			} else if (v > .53) {
				type = TileType.Dirt;
				mat = DirtMat;
				color = TileSprites.CDirt;
			} else if (v > .48) {
				type = TileType.Grass;
				mat = GrassMat;
				color = TileSprites.CGrass;
			} else if (v > .35) {
				type = TileType.Sand;
				mat = SandMat;
				color = TileSprites.CSand;
			} else if (v > .30) {
				type = TileType.Grass;
				mat = GrassMat;
				color = TileSprites.CGrass;
			} else if (v > .25) {
				type = TileType.ShallowWater;
				mat = ShallowWaterMat;
				color = TileSprites.CShallowWater;
			} else {
				type = TileType.DeepWater;
				mat = DeepWaterMat;
				color = TileSprites.CDeepWater;
			}

			Tile tile = t.GetComponent<Tile>();
			tile.Chunk = t.parent.gameObject;
			tile.Type = type;
			tile.X = x;
			tile.Y = y;

			SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
			sr.material = mat;
			sr.sprite = TileSprites.Get(type, x, y);

			SmoothTiles st = t.GetComponent<SmoothTiles>();
			st.OverlapOrder = TileSprites.Order[(int) type];

			if (type == TileType.Rock || type == TileType.Grass) {
				st.CanTransition = false;
			} else {
				st.Color = color;
			}

			_tiles[y].Add(t.gameObject);
		}

	}

}