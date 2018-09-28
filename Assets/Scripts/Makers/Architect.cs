using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	public class Architect : MonoBehaviour {

		public static bool Planning;

		private bool _didStartPlanningThisCycle;
		private ThingType _selectedType;
		private LinkedType _linkedType;
		private IThing _thing;
		private Transform _transform;

		[UsedImplicitly]
		public void SelectThing_Wall () {
			if (Planning) {
				_transform.gameObject.SetActive(false);
				Destroy(_transform.gameObject);
			}

			Planning = true;
			_didStartPlanningThisCycle = true;
			_selectedType = ThingType.Structure;

			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x = (int) mousePos.x;
			int y = (int) mousePos.y;
			GameObject go = new GameObject("Architect Wall");
			go.transform.localPosition = new Vector3(x, y, Order.SELECTOR);
			Linked wall = go.AddComponent<Linked>();
			_linkedType = LinkedType.WallPlanks;
			wall.Initialize(_linkedType, true);

			_thing = wall;
			_transform = go.transform;
		}

		[UsedImplicitly]
		private void Start () {
			Planning = false;
		}

		[UsedImplicitly]
		private void Update () {
			if (Planning == false) {
				return;
			}

			Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			v = new Vector3((int) v.x, (int) v.y, Order.SELECTOR);
			_transform.position = v;

			if (_didStartPlanningThisCycle) {
				_didStartPlanningThisCycle = false;
				return;
			}

			if (Input.GetMouseButtonUp(1)) {
				Planning = false;
				_transform.gameObject.SetActive(false);
				Destroy(_transform.gameObject);
			} else if (Input.GetMouseButtonUp(0)) {
				Planning = false;

				switch (_selectedType) {
					case ThingType.Structure:
						Linked wall = _thing as Linked;

						if (wall == null) {
							Debug.Log("Tried to place null.");
							return;
						}

						if (WallMaker.TryAdd(wall)) {
							wall.Refresh();
							Debug.Log($"Placed wall at {(int) v.x} {(int) v.y}");
							return;
						}

						_transform.gameObject.SetActive(false);
						Destroy(_transform.gameObject);
						Debug.Log($"Couldn't place wall at {(int) v.x} {(int) v.y}");
						return;
				}
			}
		}

	}

}