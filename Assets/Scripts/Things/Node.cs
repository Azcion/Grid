using UnityEngine;

namespace Assets.Scripts.Things {

	public class Node {

		public bool Walkable;
		public Vector2 WorldPosition;

		public Node (bool walkable, Vector2 worldPos) {
			Walkable = walkable;
			WorldPosition = worldPos;
		}

	}

}