using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class TileMaker : MonoBehaviour {

		public const int CSIZE = 8;
		public const int YCHUNKS = 10;
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

		[UsedImplicitly]
		public GameObject Pathfinder;
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

		private static List<List<GameObject>> _tiles;

		private static int _seed;
		private static bool _ready;

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

			for (int y = 0; y < YTILES; ++y) {
				List<GameObject> row = new List<GameObject>();

				for (int x = 0; x < YTILES; ++x) {
					row.Add(null);
				}

				_tiles.Add(row);
			}

			StartCoroutine(Create());
		}

		private IEnumerator Create () {
			yield return new WaitForSeconds(.05f);

			for (int y = 0; y < YCHUNKS; ++y) {
				for (int x = 0; x < YCHUNKS; ++x) {
					Vector3 pos = new Vector3(CSIZE * x, CSIZE * y, Order.GROUND);
					GameObject c = Instantiate(ChunkPrefab, pos, Quaternion.identity, Container.transform);
					c.name = "Chunk " + y + "y " + x + "x";

					foreach (Transform t in c.transform) {
						InitializeTile(t);
					}
				}
			}

			_ready = true;
			ApplicationController.NotifyReady();

			Pathfinder.SetActive(true);
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
			st.Color = color;

			switch (type) {
				case TileType.DeepWater:
					st.CanBeTransitionedTo = false;
					tile.Walkable = false;
					break;
				case TileType.Rock:
				case TileType.Grass:
					st.CanTransition = false;
					tile.Walkable = true;
					break;
				default:
					tile.Walkable = true;
					break;
			}

			switch (type) {
				case TileType.DeepWater:
				case TileType.ShallowWater:
					tile.Buildable = false;
					break;
				case TileType.Sand:
				case TileType.Grass:
				case TileType.Dirt:
				case TileType.Rock:
				case TileType.Snow:
					tile.Buildable = true;
					break;
			}

			_tiles[y][x] = t.gameObject;
		}

	}

}