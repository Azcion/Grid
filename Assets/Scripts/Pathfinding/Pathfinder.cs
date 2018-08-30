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
			path = MakeDiagonals(path);

			return path.ToArray();
		}

		private static List<Vector2> MakeDiagonals (List<Vector2> path) {
			List<Vector2> nodesToRemove = new List<Vector2>();

			for (int i = 2; i < path.Count; ++i) {
				Vector2 a = path[i - 2];
				Vector2 c = path[i];
				int ax = (int) a.x;
				int ay = (int) a.y;
				int cx = (int) c.x;
				int cy = (int) c.y;

				if (ax == cx || ay == cy) {
					continue;
				}

				Vector2 b = path[i - 1];
				int bx = (int) b.x;
				Vector2 opposite = ax == bx ? new Vector2(cx, ay) : new Vector2(ax, cy);
				Node bNode = NodeGrid.GetNodeAt(opposite);

				if (bNode.Walkable) {
					nodesToRemove.Add(b);
				}
			}

			foreach (Vector2 v in nodesToRemove) {
				path.Remove(v);
			}

			return path;
		}

	}

}