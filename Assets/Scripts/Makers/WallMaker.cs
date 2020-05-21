using System;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	public class WallMaker : MonoBehaviour {

		private static WallMaker _instance;
		private static Linked[,] _walls;
		private static bool _ready;

		private static GameObject _wallPrefab;

		[UsedImplicitly, SerializeField] private GameObject _coverContainer = null;

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
			go.name = Enum.GetName(typeof(ThingType), wall.Type);
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
				_wallPrefab.transform.SetParent(transform);
				BoxCollider2D bc = _wallPrefab.GetComponent<BoxCollider2D>();
				bc.isTrigger = true;
				bc.offset = new Vector2(.5f, .5f);
				bc.size = Vector2.one;
			}

			_instance = this;
			_walls = new Linked[Map.YTiles, Map.YTiles];

			Populate(_wallPrefab);
		}

		private void Populate (GameObject prefab) {
			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) {
					Initialize(prefab, TileMaker.GetTile(x, y), x, y);
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

			ApplicationController.NotifyReady();
		}

		// Cover RoughHewnRock with Rock Wall
		private void Initialize (GameObject prefab, Tile t, int x, int y) {
			if (t.Type != TileType.RoughHewnRock) {
				return;
			}

			Vector3 pos = new Vector3(x, y, Order.STRUCTURE);
			GameObject go = Instantiate(prefab, pos, Quaternion.identity, transform);
			LinkedType type = LinkedType.Rock;
			go.name = Enum.GetName(typeof(LinkedType), type);
			Linked linked = Linked.Create(go.GetComponent<Linked>(), type);
			_walls[x, y] = linked;

			if (TileMaker.GetTile(x, y).TryAddThing(linked) == false) {
				Debug.Log($"Tried to add wall to an occupied tile. {x}, {y}");
			}
		}
	}

}