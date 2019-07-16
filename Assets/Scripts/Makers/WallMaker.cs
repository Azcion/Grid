using System;
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

		[UsedImplicitly, SerializeField] private GameObject _chunkContainer;
		[UsedImplicitly, SerializeField] private GameObject _container;

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

			if (tile == null) {
				return false;
			}

			// Grass can be built over
			bool isGrass = IsGrass(tile);

			if (!tile.TryAddThing(wall) && !isGrass) {
				return false;
			}

			if (isGrass) {
				tile.RemoveThing(true);
			}

			GameObject go = wall.gameObject;
			go.name = Enum.GetName(typeof(ThingType), wall.ThingType());
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

		private static bool IsGrass (Tile tile) {
			if (tile.IsBarren()) {
				return false;
			}

			return (tile.GetThing() as Plant)?.Def.DefName == "Plant_Grass";
		}

		[UsedImplicitly]
		private void Start () {
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