using System;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public struct PathRequest {

		public readonly Action<Vector2[], bool> Callback;

		public Vector2 Start;
		public Vector2 End;

		public PathRequest (Vector2 start, Vector2 end, Action<Vector2[], bool> callback) {
			Start = start;
			End = end;
			Callback = callback;
		}

	}

}