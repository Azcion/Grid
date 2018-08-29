using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class Node : IHeapItem<Node> {

		public readonly int X;
		public readonly int Y;
		public readonly bool Walkable;
		public readonly int Penalty;

		public Vector2 WorldPosition;
		public int GCost;
		public int HCost;
		public Node Parent;
		public int HeapIndex { get; set; }
		public int FCost => GCost + HCost;

		public Node (int x, int y, bool walkable, int penalty) {
			X = x;
			Y = y;
			WorldPosition = new Vector2(x, y);
			Walkable = walkable;
			Penalty = penalty;
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