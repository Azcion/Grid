using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Node {

		public int X;
		public int Y;
		public bool Walkable;
		public Vector2 WorldPosition;
		public int GCost;
		public int HCost;
		public Node Parent;

		public int FCost => GCost + HCost;

		public Node (bool walkable, Vector2 worldPos, int x, int y) {
			X = x;
			Y = y;
			Walkable = walkable;
			WorldPosition = worldPos;
		}

	}

}