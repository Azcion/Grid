using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	public class Architect : MonoBehaviour {

		public static bool Planning;

		private static readonly List<GameObject> DragCellPoolAvailable;
		private static readonly List<GameObject> DragCellPoolUsed;
		
		private static bool[,] _isDesignated;

		private bool _didStartPlanningThisCycle;
		private int _oldMx;
		private int _oldMy;
		private ThingType _selectedType;
		private LinkedType _linkedType;
		private string _itemType;
		private Vector3Int _dragStart;
		private Transform _designator;
		private GameObject _dragCellPrefab;

		[UsedImplicitly, SerializeField] private GameObject _dragCellsContainer = null;

		static Architect () {
			DragCellPoolAvailable = new List<GameObject>();
			DragCellPoolUsed = new List<GameObject>();
		}

		[UsedImplicitly]
		public void SelectThing_Wall () {
			StartSelect();
			_selectedType = ThingType.Structure;
		}

		[UsedImplicitly] // Debug
		public void SelectThing_Wood () {
			StartSelect();
			_selectedType = ThingType.Item;
			_itemType = "WoodLog";
		}

		private void StartSelect () {
			Planning = true;
			_didStartPlanningThisCycle = true;
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x = (int) mousePos.x;
			int y = (int) mousePos.y;
			_designator.position = new Vector3(x, y, Order.SELECTOR);
			_designator.gameObject.SetActive(true);
		}

		[UsedImplicitly]
		private void Start () {
			Planning = false;
			_designator = Create("Designator", "DesignatedGeneric").transform;
			_dragCellPrefab = Create("Drag Cell", "DragHighlightCell");
			_isDesignated = new bool[Map.YTiles, Map.YTiles];
		}

		[UsedImplicitly]
		private void Update () {
			if (Planning == false) {
				return;
			}

			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int mx = (int) mousePos.x;
			int my = (int) mousePos.y;
			Vector3Int v = new Vector3Int(mx, my, Order.SELECTOR);
			_designator.position = v + new Vector3(.5f, .5f);

			if (_didStartPlanningThisCycle) {
				_oldMx = mx;
				_oldMy = my;
				_didStartPlanningThisCycle = false;
				return;
			}

			int dx = mx - _dragStart.x;
			int dy = my - _dragStart.y;
			int dxAbs = Mathf.Abs(dx);
			int dyAbs = Mathf.Abs(dy);
			Direction direction;

			if (dxAbs >= dyAbs) {
				direction = dx > 0 ? Direction.East : Direction.West;
			} else {
				direction = dy > 0 ? Direction.North : Direction.South;
			}

			if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && (mx != _oldMx || my != _oldMy)) {
				_oldMx = mx;
				_oldMy = my;
				RetireDesignators();

				switch (direction) {
					case Direction.North:
						for (int y = _dragStart.y; y <= my; ++y) {
							Designate(_dragStart.x, y);
						}

						break;
					case Direction.South:
						for (int y = _dragStart.y; y >= my; --y) {
							Designate(_dragStart.x, y);
						}

						break;
					case Direction.East:
						for (int x = _dragStart.x; x <= mx; ++x) {
							Designate(x, _dragStart.y);
						}

						break;
					case Direction.West:
						for (int x = _dragStart.x; x >= mx; --x) {
							Designate(x, _dragStart.y);
						}

						break;
				}
			}

			if (Input.GetMouseButtonDown(0)) {
				// Left click
				_dragStart = v;
			} else if (Input.GetMouseButtonUp(1)) {
				// Right click
				Planning = false;
				RetireDesignators();
			} else if (Input.GetMouseButtonUp(0)) {
				// Left click release
				Planning = false;
				RetireDesignators();

				switch (_selectedType) {
					case ThingType.Structure:
						if (_dragStart.x != v.x || _dragStart.y != v.y) {
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

						CoverAssembler.Apply();
						break;
					case ThingType.Item:
						PlaceItem(v.x, v.y, _itemType);
						break;
				}
			}
		}

		private void Designate (int x, int y) {
			if (_isDesignated[y, x]) {
				return;
			}

			Tile tile = TileMaker.GetTile(x, y);
			bool canAdd = tile?.CanAddThing() ?? false;

			if (!canAdd) {
				return;
			}

			GameObject go;

			if (DragCellPoolAvailable.Count > 0) {
				go = DragCellPoolAvailable[0];
				DragCellPoolAvailable.RemoveAt(0);
			} else {
				go = Instantiate(_dragCellPrefab);
				go.transform.SetParent(_dragCellsContainer.transform);
			}

			go.transform.position = new Vector3(x + .5f, y + .5f, Order.SELECTOR);
			go.SetActive(true);
			DragCellPoolUsed.Add(go);
			_isDesignated[y, x] = true;
		}

		private void RetireDesignators () {
			foreach (GameObject go in DragCellPoolUsed) {
				go.SetActive(false);
				int x = (int) go.transform.position.x;
				int y = (int) go.transform.position.y;
				_isDesignated[y, x] = false;
			}

			DragCellPoolAvailable.AddRange(DragCellPoolUsed);
			DragCellPoolUsed.Clear();
			_designator.gameObject.SetActive(false);
		}

		private GameObject Create (string label, string sprite) {
			GameObject go = new GameObject(label, typeof(SpriteRenderer));
			go.SetActive(false);
			go.transform.SetParent(_dragCellsContainer.transform);
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			sr.sprite = Assets.GetSprite(sprite);
			sr.sharedMaterial = Assets.ThingMat;

			return go;
		}

		private static void BuildWall (int x, int y) {
			Vector3 pos = new Vector3(x, y, Order.SELECTOR);
			BuildingDef def = DefLoader.GetBuilding("Wall");
			Linked linked = WallMaker.Make(def, ThingMaterial.Wood, pos, true, null);
			linked.Initialize();

			if (WallMaker.TryAdd(linked)){
				linked.Refresh();
			} else {
				linked.gameObject.SetActive(false);
				Destroy(linked.gameObject);
			}
		}

		private static void PlaceItem (int x, int y, string type) {
			if (ItemMaker.TryMake(type, x, y)) {
				//todo
			} else {
				Debug.Log($"Couldn't place {type} at xy: {x} {y}");
			}
		}

	}

}