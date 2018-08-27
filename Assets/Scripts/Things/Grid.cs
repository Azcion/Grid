using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Grid : MonoBehaviour {

		private static Vector2 _gridSize;
		private static Vector2 _center;

		private Node[,] _grid;

		[UsedImplicitly]
		private void Awake () {
			_gridSize = new Vector2(TileMaker.YTILES, TileMaker.YTILES);
			_center = new Vector3(TileMaker.THALF, TileMaker.THALF);
		}

		[UsedImplicitly]
		private void OnEnable () {
			_grid = new Node[TileMaker.YTILES, TileMaker.YTILES];
			Vector2 worldBottomLeft = new Vector2(.5f, .5f);

			for (int x = 0; x < TileMaker.YTILES; ++x) {
				for (int y = 0; y < TileMaker.YTILES; ++y) {
					Vector2 worldPoint = worldBottomLeft + Vector2.right * x + Vector2.up * y;
					bool walkable = TileMaker.Get(x, y).GetComponent<Tile>().Walkable;
					_grid[x, y] = new Node(walkable, worldPoint);
				}
			}
		}

		[UsedImplicitly]
		private void OnDrawGizmos () {
			Gizmos.DrawWireCube(_center, _gridSize);

			if (_grid != null) {
				foreach (Node n in _grid) {
					Gizmos.color = n.Walkable ? Color.white : Color.red;
					Gizmos.DrawCube(n.WorldPosition, Vector2.one * .5f);
				}
			}
		}

	}

}