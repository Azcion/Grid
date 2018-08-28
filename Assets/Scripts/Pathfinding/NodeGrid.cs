using System.Collections.Generic;
using Assets.Scripts.Graphics;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class NodeGrid : MonoBehaviour {

		public List<Node> Path;
		public int MaxSize => (int) _gridSize.x * (int) _gridSize.y;

		private bool _showOnlyPath = true;

		private static Vector2 _gridSize;
		private static Vector2 _center;

		private Node[,] _grid;

		public Node GetNodeAt (Vector2 position) {
			return _grid[(int) position.x, (int) position.y];
		}

		public List<Node> GetNeighbors (Node node) {
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
		}

		[UsedImplicitly]
		private void OnEnable () {
			_grid = new Node[TileMaker.YTILES, TileMaker.YTILES];
			Vector2 worldBottomLeft = new Vector2(.5f, .5f);

			for (int x = 0; x < TileMaker.YTILES; ++x) {
				for (int y = 0; y < TileMaker.YTILES; ++y) {
					Vector2 worldPoint = worldBottomLeft + Vector2.right * x + Vector2.up * y;
					bool walkable = TileMaker.Get(x, y).GetComponent<Tile>()?.Walkable ?? false;
					_grid[x, y] = new Node(walkable, worldPoint, x, y);
				}
			}
		}

		[UsedImplicitly]
		private void OnDrawGizmos () {
			Gizmos.DrawWireCube(_center, _gridSize);

			if (_showOnlyPath) {
				if (Path == null) {
					return;
				}

				foreach (Node n in Path) {
					Gizmos.color = Color.black;
					Gizmos.DrawCube(n.WorldPosition, Vector2.one * .5f);
				}
			} else {
				if (_grid == null) {
					return;
				}

				foreach (Node n in _grid) {
					Gizmos.color = n.Walkable ? Color.white : Color.red;

					if (Path != null) {
						if (Path.Contains(n)) {
							Gizmos.color = Color.black;
						}
					}

					Gizmos.DrawCube(n.WorldPosition, Vector2.one * .5f);
				}
			}

			
		}

	}

}