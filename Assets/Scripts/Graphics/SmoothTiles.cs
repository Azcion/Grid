using System.Collections;
using Assets.Scripts.Main;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class SmoothTiles : MonoBehaviour {

		public Color Color;
		public float OverlapOrder;

		public bool CanTransition = true;
		public bool CanBeTransitionedTo = true;

		private const float LENGTH = .5f;

		private static GameObject _side;
		private static GameObject _corner;

		private bool _canFadeU = true;
		private bool _canFadeD = true;
		private bool _canFadeL = true;
		private bool _canFadeR = true;

		private bool _canFadeUl = true;
		private bool _canFadeUr = true;
		private bool _canFadeDl = true;
		private bool _canFadeDr = true;

		private int _x;
		private int _y;
		
		// Neighbor bools
		#region
		private bool IsUp () {
			return TileMaker.Get(_x, _y + 1);
		}

		private bool IsDown () {
			return TileMaker.Get(_x, _y - 1);
		}

		private bool IsLeft () {
			return TileMaker.Get(_x - 1, _y);
		}

		private bool IsRight () {
			return TileMaker.Get(_x + 1, _y);
		}

		private bool IsUpLeft () {
			return TileMaker.Get(_x - 1, _y + 1);
		}

		private bool IsUpRight () {
			return TileMaker.Get(_x + 1, _y + 1);
		}

		private bool IsDownLeft () {
			return TileMaker.Get(_x - 1, _y - 1);
		}

		private bool IsDownRight () {
			return TileMaker.Get(_x + 1, _y - 1);
		}
		#endregion

		// Neighbor objects
		#region
		private GameObject GetUp () {
			return TileMaker.Get(_x, _y + 1);
		}

		private GameObject GetDown () {
			return TileMaker.Get(_x, _y - 1);
		}

		private GameObject GetLeft () {
			return TileMaker.Get(_x - 1, _y);
		}

		private GameObject GetRight () {
			return TileMaker.Get(_x + 1, _y);
		}

		private GameObject GetUpLeft () {
			return TileMaker.Get(_x - 1, _y + 1);
		}

		private GameObject GetUpRight () {
			return TileMaker.Get(_x + 1, _y + 1);
		}

		private GameObject GetDownLeft () {
			return TileMaker.Get(_x - 1, _y - 1);
		}

		private GameObject GetDownRight () {
			return TileMaker.Get(_x + 1, _y - 1);
		}
		#endregion

		public static void GetStaticAssets (GameObject side, GameObject corner) {
			_side = side;
			_corner = corner;
		}

		[UsedImplicitly]
		private void Start () {
			StartCoroutine(Initialize());

			Tile tile = GetComponent<Tile>();
			_x = tile.X;
			_y = tile.Y;
		}

		private IEnumerator Initialize () {
			yield return new WaitForSeconds(1);
			
			if (!CanTransition) {
				_canFadeD = false;
				_canFadeDl = false;
				_canFadeDr = false;
				_canFadeL = false;
				_canFadeR = false;
				_canFadeU = false;
				_canFadeUl = false;
				_canFadeUr = false;

				yield break;
			}

			// Test for transferability
			_canFadeU = Check(GetUp());
			_canFadeD = Check(GetDown());
			_canFadeL = Check(GetLeft());
			_canFadeR = Check(GetRight());
			_canFadeUl = Check(GetUpLeft());
			_canFadeUr = Check(GetUpRight());
			_canFadeDl = Check(GetDownLeft());
			_canFadeDr = Check(GetDownRight());

			// Draw transitions
			Vector3 t = transform.position;

			if (IsUp() && _canFadeU) {
				Create(0, t.x, (int) t.y + 1);
			}

			if (IsDown() && _canFadeD) {
				Create(180, t.x, (int) t.y);
			}

			if (IsLeft() && _canFadeL) {
				Create(90, (int) t.x, t.y, special: true);
			}

			if (IsRight() && _canFadeR) {
				Create(270, (int) t.x + 1, t.y, special: true);
			}

			if (IsUpLeft() && _canFadeUl && IsLeft() && _canFadeL && IsUp() && _canFadeU) {
				Create(90, (int) t.x, (int) t.y + 1, true);
			}

			if (IsUpRight() && _canFadeUr && IsRight() && _canFadeR && IsUp() && _canFadeU) {
				Create(0, (int) t.x + 1, (int) t.y + 1, true);
			}

			if (IsDownLeft() && _canFadeDl && IsLeft() && _canFadeL && IsDown() && _canFadeD) {
				Create(180, (int) t.x, (int) t.y, true);
			}

			if (IsDownRight() && _canFadeDr && IsRight() && _canFadeR && IsDown() && _canFadeD) {
				Create(270, (int) t.x + 1, (int) t.y, true);
			}

			ApplicationController.NotifyReady();
		}

		private bool Check (GameObject t) {
			if (t == null) {
				return false;
			}

			// Can transition with self
			bool canTransition = t.GetComponent<Tile>().Type != GetComponent<Tile>().Type;

			// Overlap
			if (t.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
				canTransition = false;
			}

			// Can other sprites transition to self
			if (t.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
				canTransition = false;
			}

			return canTransition;
		}

		private void Create (int r, float x, float y, bool corner=false, bool special=false) {
			GameObject t = Instantiate(corner ? _corner : _side, transform);
			t.transform.rotation = Quaternion.Euler(0, 0, r);
			t.transform.position = new Vector3(x, y, t.transform.position.z);
			t.GetComponent<SpriteRenderer>().color = Color;

			Vector2 size = t.GetComponent<SpriteRenderer>().bounds.size;
			float yS = (special ? size.y : size.x) * (corner ? 2 : 1);
			float xS = transform.GetComponent<SpriteRenderer>().bounds.size.x;
			t.transform.localScale = new Vector3(xS / yS * (corner ? LENGTH : 1), xS / yS * LENGTH, 1);
		}

		// todo implement update
		public void UpdateTransitions () {
			Delete("all");
			// todo
		}

		//Keywords are: all, left, right, top, bottom, topleft, topright, bottomleft, bottomright
		public void Delete (params string[] direction) {
			foreach (string d in direction) {
				switch (d) {
					case "all":
						foreach (Transform child in gameObject.transform) {
							if (child.gameObject.transform.IsChildOf(transform)) {
								Destroy(child.gameObject);
							}
						}
						break;
					case "left":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x > transform.GetChild(j).transform.position.x && transform.position.y == transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "right":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x < transform.GetChild(j).transform.position.x && transform.position.y == transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "top":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x == transform.GetChild(j).transform.position.x && transform.position.y < transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "bottom":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x == transform.GetChild(j).transform.position.x && transform.position.y > transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "topleft":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x > transform.GetChild(j).transform.position.x && transform.position.y < transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "topright":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x < transform.GetChild(j).transform.position.x && transform.position.y < transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "bottomleft":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x > transform.GetChild(j).transform.position.x && transform.position.y > transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					case "bottomright":
						for (int j = 0; j < transform.childCount; j++) {
							if (transform.position.x < transform.GetChild(j).transform.position.x && transform.position.y > transform.GetChild(j).transform.position.y) {
								if (transform.GetChild(j).gameObject.transform.IsChildOf(transform)) {
									Destroy(transform.GetChild(j).transform.gameObject);
								}
							}
						}
						break;
					default:
						Debug.LogError("SmoothTiles2d - Error: Parameters for Delete are invalid, LOL! Possible parameter values include: 'all' 'left' 'right' 'top' 'bottom' 'topleft' 'topright' 'bottomleft' 'bottomright'");
						break;
				}
			}
		}

	}

}