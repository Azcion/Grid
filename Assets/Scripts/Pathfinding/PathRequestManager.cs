using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class PathRequestManager : MonoBehaviour {
		
		private static readonly Queue<PathResult> Results = new Queue<PathResult>();

		private static PathRequestManager _instance;

		private Pathfinder _pathfinder;

		public static void RequestPath (PathRequest request) {
			ThreadStart threadStart = delegate {
				_instance._pathfinder.FindPath(request, FinishedProcessingPath);
			};

			threadStart.Invoke();
		}

		public static void FinishedProcessingPath (PathResult result) {
			lock (Results) {
				Results.Enqueue(result);
			}
		}

		[UsedImplicitly]
		private void Awake () {
			_instance = this;
			_pathfinder = GetComponent<Pathfinder>();
		}

		[UsedImplicitly]
		private void Update () {
			lock (Results) {
				if (Results.Count == 0) {
					return;
				}

				int itemsInQueue = Results.Count;

				for (int i = 0; i < itemsInQueue; ++i) {
					PathResult result = Results.Dequeue();
					result.Callback(result.Path, result.Success);
				}
			}
		}
		
	}

}