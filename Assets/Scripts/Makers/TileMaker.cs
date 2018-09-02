using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

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

		private static int _typeCount;

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
			_typeCount = Enum.GetValues(typeof(TileType)).Length;
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
			float v0 = Noise.Sum(x + _seed, y + _seed, .01f, 8, 2.2f, .5f);

			TileType type;

			if (v0 > .575) {
				type = TileType.RoughHewnRock;
			} else if (v0 > .55) {
				type = TileType.RoughStone;
			} else if (v0 > .53) {
				type = TileType.Gravel;
			} else if (v0 > .44) {
				type = TileType.Soil;
			} else if (v0 > .35) {
				type = TileType.Sand;
			} else if (v0 > .25) {
				type = TileType.Soil;
			} else if (v0 > .23) {
				type = TileType.Gravel;
			} else {
				type = TileType.RoughStone;
			}

			float v1 = Noise.Sum(x + _seed, y + _seed, .005f, 6, 2.2f, .5f);
			const float dw = .80f;
			const float sw = dw - .015f;
			const float m = sw - .015f;

			if (type != TileType.RoughStone) {
				if (v1 > dw) {
					type = TileType.DeepWater;
				} else if (v1 > sw) {
					type = TileType.Marsh;
				} else if (v1 > m) {
					type = TileType.Mud;
				} else if (v1 > 1 - m) {
					if (v1 < .55 && v1 > .45) {
						float v2 = Noise.Sum(x + _seed, y + _seed, .02f, 4, 2.2f, .5f);

						if (type == TileType.Soil) {
							if (v2 > .60) {
								type = TileType.SoftSand;
							} else if (v2 > .55) {
								type = TileType.Sand;
							}
						} else if (type == TileType.Sand) {
							if (v2 < .33) {
								type = TileType.SoilRich;
							} else if (v2 < .36) {
								type = TileType.Soil;
							}
						}
					}
				} else if (v1 > 1 - sw) {
					type = TileType.Mud;
				} else if (v1 > 1 - dw) {
					type = TileType.Marsh;
				} else {
					type = TileType.DeepWater;
				}
			}

			int penalty = 0;
			Material mat = AssetLoader.DiffuseMat;
			Color color = Color.gray;

			switch (type) {
				case TileType.ShallowWater:
					penalty = 15;
					break;
				case TileType.Mud:
					penalty = 20;
					break;
				case TileType.Sand:
					penalty = 8;
					break;
				case TileType.Soil:
					penalty = 5;
					break;
				case TileType.Gravel:
					penalty = 2;
					break;
				case TileType.RoughStone:
					penalty = 0;
					break;
			}

			SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
			sr.material = mat;
			sr.sprite = AssetLoader.Get(type, x, y);

			SmoothTiles st = t.GetComponent<SmoothTiles>();
			st.OverlapOrder = _typeCount - (int) type;
			st.Color = color;

			Tile tile = t.GetComponent<Tile>();
			bool walkable = true;
			bool buildable = true;

			switch (type) {
				case TileType.DeepWater:
					st.CanBeTransitionedTo = false;
					walkable = false;
					break;
				case TileType.SmoothStone:
				case TileType.Carpet:
				case TileType.Concrete:
				case TileType.Flagstone:
				case TileType.GenericFloorTile:
				case TileType.PavedTile:
				case TileType.TileStone:
				case TileType.WoodFloor:
					st.CanTransition = false;
					break;
			}

			switch (type) {
				case TileType.DeepWater:
				case TileType.ShallowWater:
				case TileType.Mud:
				case TileType.Marsh:
				case TileType.MarshyTerrain:
				case TileType.SoftSand:
					buildable = false;
					break;
			}

			tile.Assign(t.parent.gameObject, x, y, type, walkable, buildable, penalty);
			_tiles[y][x] = tile;
		}

	}

}