using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	public class WallMaker : MonoBehaviour {

		private static readonly HashSet<TileType> AllowedSurfaces;

		private static WallMaker _instance;
		private static Linked[,] _walls;
		private static bool _ready;

		private static GameObject _wallPrefab;

		static WallMaker () {
			AllowedSurfaces = new HashSet<TileType> {
				TileType.RoughHewnGranite,
				TileType.RoughHewnLimestone,
				TileType.RoughHewnMarble,
				TileType.RoughHewnSandstone
			};
		}

		public static Linked Make (LinkedType type, ThingMaterial material, Vector3 pos, bool blueprint, Transform parent) {
			GameObject go = Instantiate(_wallPrefab, pos, Quaternion.identity, parent);
			go.name = Name.Get(type);
			Linked linked = Linked.Create(go.GetComponent<Linked>(), type, material, blueprint);

			return linked;
		}

		public static Linked GetLinked (int x, int y) {
			if (!_ready || x < 0 || x >= Map.YTiles || y < 0 || y >= Map.YTiles) {
				return null;
			}

			return _walls[x, y];
		}

		public static bool TryAdd (Linked wall) {
			int x = (int) wall.transform.position.x;
			int y = (int) wall.transform.position.y;
			Tile tile = TileMaker.GetTile(x, y);

			if (tile == null || tile.Buildable == false) {
				return false;
			}

			if (!tile.TryAddThing(wall)) {
				return false;
			}

			GameObject go = wall.gameObject;
			go.name = Name.Get(wall.Type);
			go.transform.SetParent(_instance.transform);
			go.transform.localPosition = new Vector3(x, y, Order.STRUCTURE);
			_walls[x, y] = wall;

			return true;
		}

		public static void Remove (Linked wall) {
			int x = (int) wall.transform.position.x;
			int y = (int) wall.transform.position.y;
			_walls[x, y] = null;
			wall.gameObject.SetActive(false);
			Destroy(wall.gameObject);
		}

		[UsedImplicitly]
		private void Start () {
			if (!DefLoader.DidLoad) {
				Debug.Log("Defs not loaded, WallMaker can't run.");
				return;
			}

			if (_wallPrefab == null) {
				_wallPrefab = new GameObject("Wall Prefab", typeof(Linked), typeof(BoxCollider2D));
				_wallPrefab.SetActive(false);
				_wallPrefab.GetComponent<Linked>().Prepare();
				_wallPrefab.transform.SetParent(transform);
				BoxCollider2D bc = _wallPrefab.GetComponent<BoxCollider2D>();
				bc.isTrigger = true;
				bc.offset = new Vector2(.5f, .5f);
				bc.size = Vector2.one;
			}

			_instance = this;
			_walls = new Linked[Map.YTiles, Map.YTiles];

			Populate();
		}

		private void Populate () {
			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) {
					Initialize(TileMaker.GetTile(x, y).Type, x, y);
				}
			}

			_ready = true;

			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) {
					Linked wall = _walls[x, y];

					if (wall == null) {
						continue;
					}

					wall.Initialize();
				}
			}

			CoverAssembler.Apply();
			ApplicationController.NotifyReady();
		}

		private void Initialize (TileType type, int x, int y) {
			if (!AllowedSurfaces.Contains(type)) {
				return;
			}

			ThingMaterial material;

			switch (type) {
				case TileType.RoughHewnGranite: 
					material = ThingMaterial.Granite;
					break;
				case TileType.RoughHewnLimestone: 
					material = ThingMaterial.Limestone;
					break;
				case TileType.RoughHewnMarble: 
					material = ThingMaterial.Marble;
					break;
				case TileType.RoughHewnSandstone: 
					material = ThingMaterial.Sandstone;
					break;
				default:
					Debug.LogWarning($"Material not found for {type}.");
					return;
			}

			Vector3 pos = new Vector3(x, y, Order.STRUCTURE);
			Linked linked = Make(LinkedType.Rock, material, pos, false, transform);
			_walls[x, y] = linked;

			if (TileMaker.GetTile(x, y).TryAddThing(linked) == false) {
				Debug.Log($"Tried to add wall to an occupied tile. {x}, {y}");
			}
		}
	}

}