using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Main;
using Assets.Scripts.Terrain;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	[SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
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

			for (int y = 0; y < Map.YTiles; ++y) {
				List<Tile> row = new List<Tile>();

				for (int x = 0; x < Map.YTiles; ++x) {
					row.Add(null);
				}

				_tiles.Add(row);
			}

			_transitionFlags = GetTransitionFlags();
			Create(Seed.IsDebugSurfaces);
			TerrainController.Assign(Map.YTiles, _types);
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
		}

		private void Create (bool debugSurfaces) {
			for (int y = 0; y < Map.YChunks; ++y) {
				for (int x = 0; x < Map.YChunks; ++x) {
					Vector3 pos = new Vector3(Map.CSIZE * x, Map.CSIZE * y, Order.GROUND);
					GameObject c = Instantiate(_chunkPrefab, pos, Quaternion.identity, _container.transform);
					c.name = "Chunk " + y + "y " + x + "x";

					if (debugSurfaces) {
						foreach (Transform t in c.transform) {
							int tx = (int) t.position.x;
							int ty = (int) t.position.y;
							int type = (tx + ty) / 3;

							if (type >= _typeCount) {
								type = Random.Range(0, _typeCount);
							}

							InitializeTile(t, type);
						}
					} else {
						foreach (Transform t in c.transform) {
							InitializeTile(t);
						}
					}
				}
			}

			_ready = true;
			ApplicationController.NotifyReady();
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
			}

			SmoothTiles st = t.GetComponent<SmoothTiles>();
			st.OverlapOrder = _typeCount - (int) type;
			st.Color = color;*/
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

			if (type == TileType.RoughStone) {
				return type;
			}

			if (v1 > dw) {
				type = TileType.DeepWater;
			} else if (v1 > sw) {
				type = TileType.Marsh;
			} else if (v1 > m) {
				type = TileType.Mud;
			} else if (v1 > 1 - m) {
				if (!(v1 < .55) || !(v1 > .45)) {
					return type;
				}

				float v2 = Noise.Sum(x + _seed, y + _seed, .02f, 4, 2.2f, .5f);

				switch (type) {
					case TileType.Soil when v2 > .60:
						type = TileType.SoftSand;
						break;
					case TileType.Soil: {
						if (v2 > .55) {
							type = TileType.Sand;
						}

						break;
					}

					case TileType.Sand when v2 < .33:
						type = TileType.SoilRich;
						break;
					case TileType.Sand: {
						if (v2 < .36) {
							type = TileType.Soil;
						}

						break;
					}
				}
			} else if (v1 > 1 - sw) {
				type = TileType.Mud;
			} else if (v1 > 1 - dw) {
				type = TileType.Marsh;
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