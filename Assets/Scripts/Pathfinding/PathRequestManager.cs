using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class PathRequestManager : MonoBehaviour {

		public static PathRequestManager Instance;

		private Queue<PathRequest> _queue;
		private PathRequest _currentRequest;
		private PathfindingAlgorithm _pathfinding;
		private bool _isProcessingPath;

		public static void RequestPath (Vector2 start, Vector2 end, Action<Vector2[], bool> callback) {
			PathRequest request = new PathRequest(start, end, callback);
			Instance._queue.Enqueue(request);
			Instance.TryProcessNext();
		}

		public void FinishedProcessingPath (Vector2[] path, bool success) {
			_currentRequest.Callback(path, success);
			_isProcessingPath = false;
			TryProcessNext();
		}

		[UsedImplicitly]
		private void Awake () {
			Instance = this;
			_queue = new Queue<PathRequest>();
			_pathfinding = GetComponent<PathfindingAlgorithm>();
		}

		private void TryProcessNext () {
			if (_isProcessingPath || _queue.Count <= 0) {
				return;
			}

			_currentRequest = _queue.Dequeue();
			_isProcessingPath = true;
			_pathfinding.StartFindPath(_currentRequest);
		}
		
	}

}