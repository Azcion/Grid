using System.Collections.Generic;
using Assets.Scripts.Graphics;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class NodeGrid : MonoBehaviour {

		public static int MaxSize => (int) _gridSize.x * (int) _gridSize.y;

		[UsedImplicitly]
		public bool DisplayGridGizmos;

		public static readonly Vector2 Offset;

		private static Vector2 _gridSize;
		private static Vector2 _center;

		private static Node[,] _grid;

		static NodeGrid () {
			Offset = new Vector2(.5f, .5f);
		}

		public static Node GetNodeAt (Vector2 position) {
			return _grid[(int) position.x, (int) position.y];
		}

		public IEnumerable<Node> GetNeighbors (Node node) {
			List<Node> neighbors = new List<Node>();

			for (int x = -1; x <= 1; ++x) {
				for (int y = -1; y <= 1; ++y) {
					if (x == 0 && y == 0) {
						continue;
					}

					int checkX = node.X + x;
					int checkY = node.Y + y;

					if (checkX >= 0 && checkX < _gridSize.x && checkY >= 0 && checkY < _gridSize.y) {
						neighbors.Add(_grid[checkX, checkY]);
					}
				}
			}

			return neighbors;
		}

		[UsedImplicitly]
		private void Awake () {
			_gridSize = new Vector2(TileMaker.YTILES, TileMaker.YTILES);
			_center = new Vector3(TileMaker.THALF, TileMaker.THALF);
			CreateGrid();
		}

		[UsedImplicitly]
		private void OnDrawGizmos () {
			Gizmos.DrawWireCube(_center, _gridSize);

			if (_grid == null || DisplayGridGizmos == false) {
				return;
			}

			foreach (Node n in _grid) {
				Gizmos.color = n.Walkable ? Color.white : Color.red;
				Gizmos.DrawCube(n.WorldPosition + Offset, Vector2.one * .5f);
			}
		}

		private void CreateGrid () {
			_grid = new Node[TileMaker.YTILES, TileMaker.YTILES];

			for (int x = 0; x < TileMaker.YTILES; ++x) {
				for (int y = 0; y < TileMaker.YTILES; ++y) {
					Tile tile = TileMaker.Get(x, y).GetComponent<Tile>();
					bool walkable = false;
					int penalty = 0;

					if (tile != null) {
						walkable = tile.Walkable;
						penalty = tile.Penalty;
					}

					_grid[x, y] = new Node(x, y, walkable, penalty);
				}
			}
		}

	}

}