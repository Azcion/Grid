using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Pathfinding {

	public class PathRequestManager : MonoBehaviour {
		
		private static readonly Queue<PathResult> Results = new Queue<PathResult>();

		public static void RequestPath (PathRequest request) {
			ThreadStart threadStart = delegate {
				Pathfinder.FindPath(request, FinishedProcessingPath);
			};

			threadStart.Invoke();
		}

		private static void FinishedProcessingPath (PathResult result) {
			lock (Results) {
				Results.Enqueue(result);
			}
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