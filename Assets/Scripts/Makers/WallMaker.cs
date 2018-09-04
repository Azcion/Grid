using System;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	public class WallMaker : MonoBehaviour {

		#region Object references
		[UsedImplicitly] public GameObject ChunkContainer;
		[UsedImplicitly] public GameObject Container;
		#endregion

		private static Linked[,] _walls;
		private static bool _ready;

		public static Linked GetLinked (int x, int y) {
			if (!_ready || x < 0 || x >= TileMaker.YTILES || y < 0 || y >= TileMaker.YTILES) {
				return null;
			}

			return _walls[x, y];
		}

		[UsedImplicitly]
		private void Start () {
			_walls = new Linked[TileMaker.YTILES, TileMaker.YTILES];

			StartCoroutine(Populate());
		}

		private IEnumerator Populate () {
			yield return new WaitForSeconds(.20f);

			for (int x = 0; x < TileMaker.YTILES; ++x) {
				for (int y = TileMaker.YTILES - 1; y >= 0; --y) {
					Initialize(TileMaker.Get(x, y).transform);
				}
			}

			_ready = true;
			ApplicationController.NotifyReady();
		}


		private void Initialize (Transform t) {
			if (t.GetComponent<Tile>().Type != TileType.RoughHewnRock) {
				return;
			}

			LinkedType type = LinkedType.Rock;
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Vector3 v = new Vector3(x, y, Order.THING);
			GameObject go = new GameObject(Enum.GetName(typeof(LinkedType), type));
			go.transform.SetParent(Container.transform);
			go.transform.localPosition = v;
			Linked wall = go.AddComponent<Linked>();
			wall.Initialize(type);
			_walls[x, y] = wall;

			if (TileMaker.GetTile(x, y).TryAddThing(wall) == false) {
				Debug.Log($"Tried to add wall to occupied tile. {x}, {y}");
			}
		}
	}

}