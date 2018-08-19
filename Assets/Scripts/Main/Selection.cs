using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class Selection : MonoBehaviour {

		public static GameObject Instance;
		public static Transform Target;
		
		private static bool _didSelect;
		
		public static void Select (Transform target) {
			Target = target;
			_didSelect = true;
			Instance.transform.localPosition = target.localPosition;
			Instance.SetActive(true);
		}

		public static void Deselect () {
			Instance.SetActive(false);
		}

		[UsedImplicitly]
		private void Start () {
			gameObject.SetActive(false);
			Instance = gameObject;
		}



		[UsedImplicitly]
		private void LateUpdate () {
			if (_didSelect) {
				_didSelect = false;
				return;
			}

			if (ApplicationController.Ready) {
				if (Input.GetMouseButtonDown(0)) {
					Deselect();
				}
			}
		}

	}

}