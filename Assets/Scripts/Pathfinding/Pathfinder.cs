using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	[UsedImplicitly]
	public class Pathfinder {

		public static void FindPath (PathRequest request, Action<PathResult> callback) {
			Vector2[] waypoints = new Vector2[0];
			bool success = false;

			Node startNode = NodeGrid.GetNodeAt(request.Start);
			Node targetNode = NodeGrid.GetNodeAt(request.End);
			startNode.Parent = startNode;

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

					foreach (Node neighbor in NodeGrid.GetNeighbors(node)) {
						if (neighbor.Walkable == false || closedSet.Contains(neighbor)) {
							continue;
						}

						int newCost = node.GCost + GetDistance(node, neighbor) + neighbor.Penalty;

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

			if (success) {
				waypoints = RetracePath(startNode, targetNode);
				success = waypoints.Length > 0;
			}

			callback(new PathResult(waypoints, success, request.Callback));
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