using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Jobs {

	public class JobManager : MonoBehaviour {

		private const float CYCLE = 1 / 20f;

		private static JobManager _instance;

		public static Coroutine Begin (Job job) {
			return _instance.StartCoroutine(_instance.Work(job));
		}

		public static void End (Coroutine coroutine) {
			_instance.StopCoroutine(coroutine);
		}

		[UsedImplicitly]
		private void Start () {
			_instance = this;
		}

		[UsedImplicitly]
		private IEnumerator Work (Job job) {
			float remainingWork = job.Work;

			while (remainingWork > 0) {
				remainingWork -= 1;
				yield return new WaitForSeconds(CYCLE);
			}

			switch (job.Target.Heir.Type) {
				case ThingType.Plant:
					Plant plant = job.Target.Heir as Plant;

					switch (job.Action) {
						case Action.ChopWood:
							plant?.Action_ChopWood();
							break;
						case Action.Harvest:
							plant?.Action_Harvest();
							break;
					}

					break;
			}
		}

	}

}