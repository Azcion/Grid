using Assets.Scripts.Makers;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class Node : IHeapItem<Node> {

		public readonly int X;
		public readonly int Y;
		public readonly Tile Tile;

		public bool Walkable => Tile.Walkable;
		public int Penalty => Tile.Penalty;

		public Vector2 WorldPosition;
		public int GCost;
		public int HCost;
		public Node Parent;
		public int HeapIndex { get; set; }
		public int FCost => GCost + HCost;
		
		public Node (int x, int y, Tile tile) {
			X = x;
			Y = y;
			Tile = tile;
			WorldPosition = new Vector2(x, y);
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