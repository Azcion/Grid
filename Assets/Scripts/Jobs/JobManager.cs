using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Jobs {

	public class JobManager : MonoBehaviour {

		private const float CYCLE = 1 / 20f;
		private const float W = 7;

		private static readonly List<GameObject> PoolFree;
		private static readonly List<GameObject> PoolUsed;
		private static readonly Dictionary<uint, Coroutine> Coroutines;
		private static readonly Dictionary<uint, Transform> ProgressBars;
		private static readonly HashSet<uint> ActiveIDs;

		private static JobManager _instance;
		private static GameObject _progressBarPrefab;
		private static uint _coroutineId;

		static JobManager () {
			PoolFree = new List<GameObject>();
			PoolUsed = new List<GameObject>();
			Coroutines = new Dictionary<uint, Coroutine>();
			ProgressBars = new Dictionary<uint, Transform>();
			ActiveIDs = new HashSet<uint>();
		}

		public static uint Begin (Job job) {
			uint id = ++_coroutineId;
			Coroutine coroutine = _instance.StartCoroutine(_instance.Work(job, id));
			Coroutines.Add(id, coroutine);
			ActiveIDs.Add(id);

			return id;
		}

		public static void End (uint id) {
			if (!ActiveIDs.Contains(id)) {
				return;
			}

			_instance.StopCoroutine(Coroutines[id]);
			Retire(ProgressBars[id]);
			Remove(id);
		}

		[UsedImplicitly]
		private void Start () {
			_instance = this;
			GameObject go = new GameObject("Progress");
			go.SetActive(false);
			Transform t = go.transform;
			t.SetParent(transform);
			MakeBar("Background", new Vector3(W + 1, 2, 1), Vector3.zero).SetParent(t);
			MakeBar("Foreground", new Vector3(W, 1, 1), new Vector3(.05f, .05f, -.01f)).SetParent(t);
			_progressBarPrefab = go;
		}

		[UsedImplicitly]
		private IEnumerator Work (Job job, uint id) {
			float remainingWork = job.Work;
			Transform progress = Display(job.Thing.transform.position, job.Target.transform.position);
			Transform bar = progress.Find("Foreground");
			ProgressBars.Add(id, progress);
			SetProgress(bar, 0);

			while (remainingWork > 0) {
				remainingWork -= 1;
				SetProgress(bar, 1 - remainingWork / job.Work);

				yield return new WaitForSeconds(CYCLE);
			}

			Retire(progress);
			Remove(id);

			switch (job.Target.Type) {
				case ThingType.Plant:
					switch (job.Action) {
						case Action.ChopWood:
							job.Target.AsPlant.Action_ChopWood();
							break;
						case Action.Harvest:
							job.Target.AsPlant.Action_Harvest();
							break;
					}

					break;
			}
		}

		private Transform Display (Vector2 a, Vector2 b) {
			GameObject go;

			if (PoolFree.Count > 0) {
				go = PoolFree[0];
				PoolFree.RemoveAt(0);
			} else {
				go = Instantiate(_progressBarPrefab);
				go.transform.SetParent(transform);
			}

			Vector2 xy = Vector2.Lerp(a, b, .5f);
			go.transform.position = new Vector3(xy.x, xy.y, Order.SELECTOR);
			go.SetActive(true);
			PoolUsed.Add(go);

			return go.transform;
		}

		private static void Retire (Component progressBar) {
			GameObject go = progressBar.gameObject;
			go.SetActive(false);
			PoolFree.Add(go);
			PoolUsed.Remove(go);
		}

		private static void RetireAll () {
			foreach (GameObject go in PoolUsed) {
				go.SetActive(false);
			}

			PoolFree.AddRange(PoolUsed);
			PoolUsed.Clear();
		}

		private static Transform MakeBar (string label, Vector3 scale, Vector3 pos) {
			GameObject go = new GameObject(label, typeof(SpriteRenderer));
			go.transform.localPosition = pos;
			go.transform.localScale = scale;
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			sr.sprite = Assets.GetSprite("Progress" + label);
			sr.sharedMaterial = Assets.ThingMat;
			go.SetActive(true);

			return go.transform;
		}

		private static void SetProgress (Transform t, float progress) {
			Vector3 s = t.localScale;
			t.localScale = new Vector3(Mathf.Lerp(0, W, progress), s.y, 1);
		}

		private static void Remove (uint id) {
			Coroutines.Remove(id);
			ProgressBars.Remove(id);
			ActiveIDs.Remove(id);
		}
	}

}