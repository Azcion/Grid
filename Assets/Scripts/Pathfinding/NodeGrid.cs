using System.Collections.Generic;
using Assets.Scripts.Makers;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class NodeGrid : MonoBehaviour {

		public static int MaxSize => (int) _gridSize.x * (int) _gridSize.y;
		
		private static Vector2 _gridSize;
		private static Node[,] _grid;

		public static Node GetNodeAt (Vector2 position) {
			return _grid[(int) position.x, (int) position.y];
		}

		public static IEnumerable<Node> GetNeighbors (Node node) {
			List<Node> neighbors = new List<Node>();

			if (node.X - 1 >= 0) {
				neighbors.Add(_grid[node.X - 1, node.Y]);
			}

			if (node.X + 1 < _gridSize.x) {
				neighbors.Add(_grid[node.X + 1, node.Y]);
			}

			if (node.Y - 1 >= 0) {
				neighbors.Add(_grid[node.X, node.Y - 1]);
			}

			if (node.Y + 1 < _gridSize.y) {
				neighbors.Add(_grid[node.X, node.Y + 1]);
			}

			return neighbors;
		}

		[UsedImplicitly]
		private void Awake () {
			_gridSize = new Vector2(TileMaker.YTILES, TileMaker.YTILES);
			CreateGrid();
		}

		private void CreateGrid () {
			_grid = new Node[TileMaker.YTILES, TileMaker.YTILES];

			for (int x = 0; x < TileMaker.YTILES; ++x) {
				for (int y = 0; y < TileMaker.YTILES; ++y) {
					_grid[x, y] = new Node(x, y, TileMaker.GetTile(x, y));
				}
			}
		}

	}

}