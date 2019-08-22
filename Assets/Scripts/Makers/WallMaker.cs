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

		[UsedImplicitly, SerializeField] private GameObject _container = null;

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
			go.transform.SetParent(_instance._container.transform);
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

			_instance = this;
			_walls = new Linked[Map.YTiles, Map.YTiles];

			Populate();
		}

		private void Populate () {
			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) {
					Initialize(TileMaker.Get(x, y).transform);
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
		private void Initialize (Transform t) {
			if (t.GetComponent<Tile>().Type != TileType.RoughHewnRock) {
				return;
			}

			LinkedType type = LinkedType.Rock;
			string name = Enum.GetName(typeof(LinkedType), type);
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Linked linked = Linked.Create(name, x, y, Order.STRUCTURE, _container.transform, type);
			_walls[x, y] = linked;

			if (TileMaker.GetTile(x, y).TryAddThing(linked) == false) {
				Debug.Log($"Tried to add wall to an occupied tile. {x}, {y}");
			}
		}
	}

}