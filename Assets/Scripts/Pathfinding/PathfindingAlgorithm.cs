using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class PathfindingAlgorithm : MonoBehaviour {

		public Transform Seeker, Target;

		private NodeGrid _grid;
		
		[UsedImplicitly]
		private void Awake () {
			_grid = GetComponent<NodeGrid>();
		}

		[UsedImplicitly]
		private void Update () {
			if (Input.GetButtonDown("Jump")) {
				FindPath(Seeker.position, Target.position);
			}
		}

		public void FindPath (Vector2 start, Vector2 target) {
			Node startNode = _grid.GetNodeAt(start);
			Node targetNode = _grid.GetNodeAt(target);

			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();

			openSet.Add(startNode);

			while (openSet.Count > 0) {
				Node node = openSet[0];

				for (int i = 1; i < openSet.Count; ++i) {
					Node nextNode = openSet[i];
					int nf = nextNode.FCost;
					int cf = node.FCost;

					if (nf < cf || nf == cf) {
						if (nextNode.HCost < node.HCost) {
							node = nextNode;
						}
					}
				}

				openSet.Remove(node);
				closedSet.Add(node);

				if (node == targetNode) {
					RetracePath(startNode, targetNode);

					return;
				}

				foreach (Node neighbor in _grid.GetNeighbors(node)) {
					if (neighbor.Walkable == false || closedSet.Contains(neighbor)) {
						continue;
					}

					int newCost = node.GCost + GetDistance(node, neighbor);

					if (newCost < neighbor.GCost || openSet.Contains(neighbor) == false) {
						neighbor.GCost = newCost;
						neighbor.HCost = GetDistance(neighbor, targetNode);
						neighbor.Parent = node;

						if (openSet.Contains(neighbor) == false) {
							openSet.Add(neighbor);
						}
					}
				}
			}
		}

		private static int GetDistance (Node start, Node target) {
			int dX = Mathf.Abs(start.X - target.X);
			int dY = Mathf.Abs(start.Y - target.Y);

			if (dX > dY) {
				return 14 * dY + 10 * (dX - dY);
			}

			return 14 * dX + 10 * (dY - dX);
		}

		private void RetracePath (Node start, Node target) {
			List<Node> path = new List<Node>();
			Node currentNode = target;

			while (currentNode != start) {
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}

			path.Reverse();

			_grid.Path = path;
		}

	}

}