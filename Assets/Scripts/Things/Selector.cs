using Assets.Scripts.Main;
using Assets.Scripts.Makers;
using Assets.Scripts.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[RequireComponent(typeof(SpriteRenderer))]
	public class Selector : MonoBehaviour {

		public static bool Active;
		public static Thing Thing;

		private static GameObject _instance;
		private static bool _didSelect;

		public static void Select (Thing thing) {
			if (Architect.Planning) {
				return;
			}

			Thing?.Deselect();
			_didSelect = true;
			_instance.transform.SetParent(thing.transform);
			Vector2 v = thing.transform.position;
			_instance.transform.position = new Vector3(v.x + .5f, v.y, Order.SELECTOR);
			_instance.SetActive(true);
			Thing = thing;
			Active = true;

			SelectedInfo.Set(thing);
			SelectedInfo.Show();
		}

		public static void Deselect (bool force = false) {
			if (!force && GUI.Busy) {
				return;
			}

			_instance.SetActive(false);
			_instance.transform.SetParent(null);
			Thing.Deselect();
			Active = false;
			SelectedInfo.Hide();
		}

		[UsedImplicitly]
		private void Start () {
			gameObject.SetActive(false);
			GetComponent<SpriteRenderer>().enabled = true;
			_instance = gameObject;
		}

		[UsedImplicitly]
		private void LateUpdate () {
			if (_didSelect) {
				_didSelect = false;
				return;
			}

			if (ApplicationController.Ready) {
				if (Input.GetMouseButtonUp(0)) {
					Deselect();
				}
			}
		}

	}

}