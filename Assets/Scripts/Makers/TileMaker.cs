using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class TileMaker : MonoBehaviour {

		public const int CSIZE = 8;
		public const int YCHUNKS = 10;
		public const int CHALF = CSIZE / 2;
		public const int YTILES = CSIZE * YCHUNKS;
		public const int THALF = YTILES / 2;
		public const int CENTER = YCHUNKS / 2;

		#region Object references
		[UsedImplicitly] public GameObject ChunkPrefab;
		[UsedImplicitly] public GameObject Container;
		[UsedImplicitly] public GameObject Pathfinder;
		#endregion

		#region Materials
		[UsedImplicitly] public Material DeepWaterMat;
		[UsedImplicitly] public Material ShallowWaterMat;
		[UsedImplicitly] public Material SandMat;
		[UsedImplicitly] public Material GrassMat;
		[UsedImplicitly] public Material DirtMat;
		[UsedImplicitly] public Material RockMat;
		[UsedImplicitly] public Material SnowMat;
		[UsedImplicitly] public Material DefaultMat;
		#endregion

		private static List<List<Tile>> _tiles;

		private static int _seed;
		private static bool _ready;

		public static GameObject Get (int x, int y) {
			return GetTile(x, y)?.gameObject;
		}

		public static Tile GetTile (int x, int y) {
			if (!_ready || x < 0 || x >= YTILES || y < 0 || y >= YTILES) {
				return null;
			}

			return _tiles[y][x];
		}

		[UsedImplicitly]
		private void Start () {
			_seed = ApplicationController.Seed;
			_tiles = new List<List<Tile>>();

			for (int y = 0; y < YTILES; ++y) {
				List<Tile> row = new List<Tile>();

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
			int penalty = 0;
			Material mat;
			Color color;

			if (v > .55) {
				type = TileType.RoughStone;
			} else if (v > .45) {
				type = TileType.Soil;
			} else if (v > .35) {
				type = TileType.Sand;
			} else if (v > .27) {
				type = TileType.Soil;
			}/* else if (v > .26) {
				if (Random.value < .25) {
					type = TileType.Dirt;
				} else {
					type = TileType.Sand;
				}
			}*/
			else if (v > .23) {
				type = TileType.ShallowWater;
			} else {
				type = TileType.DeepWater;
			}

			switch (type) {
				case TileType.DeepWater:
					mat = DeepWaterMat;
					color = TileSprites.CDeepWater;
					break;
				case TileType.ShallowWater:
					penalty = 15;
					mat = ShallowWaterMat;
					color = TileSprites.CShallowWater;
					break;
				case TileType.Sand:
					penalty = 6;
					mat = SandMat;
					color = TileSprites.CSand;
					break;
				case TileType.Soil:
					penalty = 3;
					mat = DirtMat;
					color = TileSprites.CSoil;
					break;
				case TileType.RoughStone:
					penalty = 0;
					mat = RockMat;
					color = null;
					break;
				default:
					Debug.Log($"Failed to instantiate tile of type {type} in TileMaker.");
					return;
			}

			SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
			sr.material = mat;
			sr.sprite = TileSprites.Get(type, x, y);

			SmoothTiles st = t.GetComponent<SmoothTiles>();
			st.OverlapOrder = TileSprites.Order[(int) type];
			st.Color = color;

			Tile tile = t.GetComponent<Tile>();
			bool walkable = true;
			bool buildable = true;

			switch (type) {
				case TileType.DeepWater:
					st.CanBeTransitionedTo = false;
					walkable = false;
					break;
				case TileType.RoughStone:
					st.CanTransition = false;
					break;
			}

			switch (type) {
				case TileType.DeepWater:
				case TileType.ShallowWater:
					buildable = false;
					break;
			}

			tile.Assign(t.parent.gameObject, x, y, type, walkable, buildable, penalty);
			_tiles[y][x] = tile;
		}

	}

}