using System;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public struct PathResult {

		public Vector2[] Path;
		public bool Success;
		public Action<Vector2[], bool> Callback;

		public PathResult (Vector2[] path, bool success, Action<Vector2[], bool> callback) {
			Path = path;
			Success = success;
			Callback = callback;
		}

	}

}