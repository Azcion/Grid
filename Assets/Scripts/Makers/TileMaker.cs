using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Terrain;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class TileMaker : MonoBehaviour {

		private static int[] _types;
		private static bool[] _transitionFlags;

		private static int _typeCount;
		private static List<List<Tile>> _tiles;
		private static int _seed;
		private static bool _ready;

		[UsedImplicitly, SerializeField] private GameObject _chunkPrefab = null;
		[UsedImplicitly, SerializeField] private GameObject _container = null;

		public static GameObject Get (int x, int y) {
			return GetTile(x, y)?.gameObject;
		}

		public static Tile GetTile (int x, int y) {
			if (!_ready || x < 0 || x >= Map.YTiles || y < 0 || y >= Map.YTiles) {
				return null;
			}

			return _tiles[y][x];
		}

		[UsedImplicitly]
		private void Start () {
			const float maxSeed = 2 << 22;
			float t = (float)(uint) ApplicationController.Seed / uint.MaxValue;
			_seed = (int) Mathf.Lerp(0, maxSeed, t);
			_typeCount = Enum.GetValues(typeof(TileType)).Length;
			_tiles = new List<List<Tile>>();
			_types = new int[Map.YTiles * Map.YTiles];
			_transitionFlags = GetTransitionFlags();

			for (int y = 0; y < Map.YTiles; ++y) {
				List<Tile> row = new List<Tile>();

				for (int x = 0; x < Map.YTiles; ++x) {
					row.Add(null);
					int type;

					if (Seed.IsDebugSurfaces) {
						type = (x + y) / 3;

						if (type >= _typeCount) {
							type = Random.Range(0, _typeCount);
						}
					} else {
						type = (int) GetType(x, y);
					}

					_types[Index(x, y)] = type;
				}

				_tiles.Add(row);
			}

			string changesDebug = "Type changes: ";

			for (int i = 0; i < 5; i++) {
				int changes = FixNeighborhoods();
				changesDebug += changes + " ";

				if (changes == 0) {
					break;
				}
			}

			Debug.Log(changesDebug);
			Create();
			TerrainController.Assign(_types, _transitionFlags);
		}

		private void Create () {
			for (int y = 0; y < Map.YChunks; ++y) {
				for (int x = 0; x < Map.YChunks; ++x) {
					Vector3 pos = new Vector3(Map.CSIZE * x, Map.CSIZE * y, Order.GROUND);
					GameObject c = Instantiate(_chunkPrefab, pos, Quaternion.identity, _container.transform);
					c.name = "Chunk " + y + "y " + x + "x";

					foreach (Transform t in c.transform) {
						int tx = (int) t.position.x;
						int ty = (int) t.position.y;
						InitializeTile(t, _types[Index(tx, ty)]);
					}
				}
			}

			_ready = true;
			ApplicationController.NotifyReady();
		}

		private static bool[] GetTransitionFlags () {
			bool[] flags = new bool[Enum.GetValues(typeof(TileType)).Length];
			
			TileType[] noTransitionTypes = {
				TileType.RoughStone,
				TileType.RoughHewnRock,
				TileType.SmoothStone,
				TileType.Carpet,
				TileType.Concrete,
				TileType.Flagstone,
				TileType.GenericFloorTile,
				TileType.PavedTile,
				TileType.TileStone,
				TileType.WoodFloor
			};

			for (int i = 0; i < flags.Length; i++) {
				TileType t = (TileType) i;
				bool matched = false;

				foreach (TileType other in noTransitionTypes) {
					if (t != other) {
						continue;
					}

					matched = true;
					break;
				}

				flags[i] = !matched;
			}

			return flags;
		}

		private static int FixNeighborhoods () {
			int changes = 0;

			for (int i = 0; i < _types.Length; ++i) {
				if (FixTile(i % Map.YTiles, i / Map.YTiles)) {
					++changes;
				}
			}

			return changes;
		}

		private static bool FixTile (int x, int y) {
			int i = Index(x, y);
			int t = _types[i];
				
			int[] neighbors = {
				TypeAt(t, x - 1, y - 1),
				TypeAt(t, x,     y - 1),
				TypeAt(t, x + 1, y - 1),
				TypeAt(t, x - 1, y),
				TypeAt(t, x + 1, y),
				TypeAt(t, x - 1, y + 1),
				TypeAt(t, x,     y + 1),
				TypeAt(t, x + 1, y + 1)
			};

			List<int> neighborTypes = new List<int> { t };
			int min = t;

			foreach (int neighbor in neighbors) {
				if (!neighborTypes.Contains(neighbor)) {
					neighborTypes.Add(neighbor);
				}

				if (neighbor < min) {
					min = neighbor;
				}
			}

			// No fixing needed
			if (neighborTypes.Count <= 3) {
				return false;
			}

			_types[i] = min;
			return true;
		}

		private static int TypeAt (int t, int x, int y) {
			if (x < 0 || x >= Map.YTiles || y < 0 || y >= Map.YTiles) {
				return t;
			}

			int type = _types[Index(x, y)];

			if (_transitionFlags[type]) {
				return t < type ? t : type;
			}

			return t;
		}

		private static int Index (int x, int y) {
			return x + y * Map.YTiles;
		}

		private static void InitializeTile (Transform t, int iType = -1) {
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			TileType type = iType == -1 ? GetType(x, y) : (TileType) iType;
			_types[x + y * Map.YTiles] = (int) type;
			int penalty = GetPenalty(type);

			/*switch (type) {
				case TileType.DeepWater:
				case TileType.ShallowWater:
				case TileType.RoughStone:
				case TileType.RoughHewnRock:
				case TileType.SmoothStone:
				case TileType.WoodFloor:
					sr.color = TileTint.Get(type);
					break;
			}*/

			Tile tile = t.GetComponent<Tile>();
			bool walkable = true;
			bool buildable = true;

			switch (type) {
				case TileType.DeepWater:
					walkable = false;
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

		private static TileType GetType (int x, int y) {
			float v0 = Noise.Sum(x + _seed, y + _seed, .01f, 8, 2.2f, .5f);

			if (true) {
				float percent = y / (float) Map.YTiles;
				const float water = .20f;
				const float mountains = .60f;

				if (percent <= water) {
					v0 -= .5f - percent / water * .5f;
				} else if (percent >= mountains) {
					v0 += (percent - mountains) / mountains;
				}
			}

			v0 = Mathf.Clamp01(v0 + v0 * .25f);
            TileType type;

			if (v0 > .575) {
				type = TileType.RoughHewnRock;
			} else if (v0 > .55) {
				type = TileType.RoughStone;
			} else if (v0 > .53) {
				type = TileType.Gravel;
			} else if (v0 > .44) {
				type = TileType.Soil;
			} else if (v0 > .40) {
				type = TileType.Sand;
			} else if (v0 > .25) {
				type = TileType.Soil;
			} else if (v0 > .23) {
				type = TileType.Mud;
			} else if (v0 > .215) {
				type = TileType.Marsh;
			} else if (v0 > .10) {
				type = TileType.ShallowWater;
			} else {
				type = TileType.DeepWater;
			}
			
			if (type == TileType.RoughHewnRock) {
				return type;
			}

			float v1 = Noise.Sum(x + _seed, y + _seed, .005f, 6, 2.2f, .5f);
			const float dw = .80f;
			const float sw = dw - .015f;
			const float m = sw - .015f;

			if (v1 > dw) {
				type = TileType.DeepWater;
			} else if (v1 > sw) {
				type = TileType.ShallowWater;
			} else if (v1 > m) {
                if (type != TileType.Sand && type != TileType.SoftSand) {
                    type = TileType.Mud;
                }
			} else if (v1 > 1 - m) {
				float v2 = Noise.Sum(x + _seed, y + _seed, .02f, 4, 2.2f, .5f);

				switch (type) {
					case TileType.Gravel when v2 > .58:
						type = TileType.SoilRich;
						break;
					case TileType.Soil when v2 > .58:
						type = TileType.SoftSand;
						break;
					case TileType.Soil when v2 > .55:
						type = TileType.Sand;
						break;
					case TileType.Sand when v2 < .33:
						type = TileType.SoilRich;
						break;
					case TileType.Sand when v2 < .36: {
						type = TileType.Soil;
						break;
					}
				}
			} else if (v1 > 1 - sw) {
                if (type != TileType.Sand && type != TileType.SoftSand) {
                    type = TileType.Mud;
                }
            } else if (v1 > 1 - dw) {
				type = TileType.ShallowWater;
			} else {
				type = TileType.DeepWater;
			}

			return type;
		}

		private static int GetPenalty (TileType type) {
			int penalty = 0;

			switch (type) {
				case TileType.ShallowWater:
				case TileType.MarshyTerrain:
				case TileType.SoftSand:
					penalty = 15;
					break;
				case TileType.Marsh:
				case TileType.Mud:
					penalty = 20;
					break;
				case TileType.Sand:
					penalty = 8;
					break;
				case TileType.Mossy:
				case TileType.Soil:
				case TileType.SoilRich:
					penalty = 5;
					break;
				case TileType.Gravel:
					penalty = 2;
					break;
				case TileType.PackedDirt:
				case TileType.Ice:
				case TileType.RoughStone:
				case TileType.RoughHewnRock:
				case TileType.SmoothStone:
				case TileType.Carpet:
				case TileType.Concrete:
				case TileType.Flagstone:
				case TileType.GenericFloorTile:
				case TileType.PavedTile:
				case TileType.TileStone:
				case TileType.WoodFloor:
					penalty = 0;
					break;
			}

			return penalty;
		}

	}

}