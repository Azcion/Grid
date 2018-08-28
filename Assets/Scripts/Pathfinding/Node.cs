using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class Node : IHeapItem<Node> {

		public int X;
		public int Y;
		public bool Walkable;
		public Vector2 WorldPosition;
		public int GCost;
		public int HCost;
		public Node Parent;

		public int HeapIndex { get; set; }
		public int FCost => GCost + HCost;

		public Node (bool walkable, Vector2 worldPos, int x, int y) {
			X = x;
			Y = y;
			Walkable = walkable;
			WorldPosition = worldPos;
		}

		public int CompareTo (Node other) {
			int compare = FCost.CompareTo(other.FCost);

			if (compare == 0) {
				compare = HCost.CompareTo(other.HCost);
			}

			return -compare;
		}

		
	}

}