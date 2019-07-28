using Assets.Scripts.Enums;
using Assets.Scripts.Things;
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
		private Vector3Int _dragStart;

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
			Linked wall = Linked.Create("Arc Wall", x, y, Order.SELECTOR, null, LinkedType.WallPlanks);
			wall.Initialize(true);
			_thing = wall;
			_transform = wall.gameObject.transform;
		}

		private static void BuildWall (int x, int y) {
			Linked w = Linked.Create("Arc Wall", x, y, Order.SELECTOR, null, LinkedType.WallPlanks);
			w.Initialize();

			if (WallMaker.TryAdd(w)) {
				w.Refresh();
			} else {
				w.gameObject.SetActive(false);
				Destroy(w.gameObject);
				//WallMaker.Remove(w);
			}
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

			Vector2 cv = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3Int v = new Vector3Int((int) cv.x, (int) cv.y, Order.SELECTOR);
			_transform.position = v;

			if (_didStartPlanningThisCycle) {
				_didStartPlanningThisCycle = false;
				return;
			}

			if (Input.GetMouseButtonDown(0)) {
				// Left click
				_dragStart = v;
			} else if (Input.GetMouseButtonUp(1)) {
				// Right click
				Planning = false;
				_transform.gameObject.SetActive(false);
				Destroy(_transform.gameObject);
			} else if (Input.GetMouseButtonUp(0)) {
				// Left click release
				Planning = false;

				switch (_selectedType) {
					case ThingType.Structure:
						Linked wall = _thing as Linked;

						if (wall == null) {
							Debug.Log("Tried to place null.");
							return;
						}

						int dx = v.x - _dragStart.x;
						int dy = v.y - _dragStart.y;
						
						if (_dragStart.x != v.x || _dragStart.y != v.y) {
							Direction directionX = dx > 0 ? Direction.East : Direction.West;
							Direction directionY = dy > 0 ? Direction.North : Direction.South;
							Direction direction = Mathf.Abs(dx) >= Mathf.Abs(dy) ? directionX : directionY;

							switch (direction) {
								case Direction.North:
									for (int y = _dragStart.y; y <= v.y; ++y) {
										BuildWall(_dragStart.x, y);
									}

									break;
								case Direction.South:
									for (int y = _dragStart.y; y >= v.y; --y) {
										BuildWall(_dragStart.x, y);
									}

									break;
								case Direction.East:
									for (int x = _dragStart.x; x <= v.x; ++x) {
										BuildWall(x, _dragStart.y);
									}

									break;
								case Direction.West:
									for (int x = _dragStart.x; x >= v.x; --x) {
										BuildWall(x, _dragStart.y);
									}

									break;
							}
						} else {
							BuildWall(v.x, v.y);
						}

						// Destroy architect wall
						_transform.gameObject.SetActive(false);
						Destroy(_transform.gameObject);

						break;
				}
			}
		}

	}

}