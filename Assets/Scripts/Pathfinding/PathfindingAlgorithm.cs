using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class PathfindingAlgorithm : MonoBehaviour {

		private PathRequestManager _manager;
		private NodeGrid _grid;
		
		public void StartFindPath (PathRequest request) {
			StartCoroutine(FindPath(request.Start, request.End));
		}

		[UsedImplicitly]
		private void Awake () {
			_manager = GetComponent<PathRequestManager>();
			_grid = GetComponent<NodeGrid>();
		}

		private IEnumerator FindPath (Vector2 start, Vector2 target) {
			Vector2[] waypoints = new Vector2[0];
			bool success = false;

			Node startNode = NodeGrid.GetNodeAt(start);
			Node targetNode = NodeGrid.GetNodeAt(target);

			if (startNode.Walkable && targetNode.Walkable) {
				Heap<Node> openSet = new Heap<Node>(NodeGrid.MaxSize);
				HashSet<Node> closedSet = new HashSet<Node>();
				openSet.Add(startNode);

				while (openSet.Count > 0) {
					Node node = openSet.RemoveFirst();
					closedSet.Add(node);

					if (node == targetNode) {
						success = true;

						break;
					}

					foreach (Node neighbor in _grid.GetNeighbors(node)) {
						if (neighbor.Walkable == false || closedSet.Contains(neighbor)) {
							continue;
						}

						int newCost = node.GCost + GetDistance(node, neighbor);

						if (newCost >= neighbor.GCost && openSet.Contains(neighbor)) {
							continue;
						}

						neighbor.GCost = newCost;
						neighbor.HCost = GetDistance(neighbor, targetNode);
						neighbor.Parent = node;

						if (openSet.Contains(neighbor) == false) {
							openSet.Add(neighbor);
						}
						else {
							openSet.UpdateItem(neighbor);
						}
					}
				}
			}

			yield return null;

			if (success) {
				waypoints = RetracePath(startNode, targetNode);
			}

			_manager.FinishedProcessingPath(waypoints, success);
		}

		private static int GetDistance (Node start, Node target) {
			int dX = Mathf.Abs(start.X - target.X);
			int dY = Mathf.Abs(start.Y - target.Y);

			if (dX > dY) {
				return 14 * dY + 10 * (dX - dY);
			}

			return 14 * dX + 10 * (dY - dX);
		}

		private static Vector2[] RetracePath (Node start, Node target) {
			List<Vector2> path = new List<Vector2>();
			Node currentNode = target;

			while (currentNode != start) {
				path.Add(currentNode.WorldPosition);
				currentNode = currentNode.Parent;
			}

			path.Reverse();

			return path.ToArray();
		}

	}

}