using System.Collections;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Transform))]
	[SuppressMessage ("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage ("ReSharper", "ConvertToConstant.Global")]
	[SuppressMessage ("ReSharper", "FieldCanBeMadeReadOnly.Global")]
	public class SmoothTiles : MonoBehaviour {

		public Color Color;
		public float OverlapOrder;

		public bool CanTransition = true;
		public bool CanBeTransitionedTo = true;
		
		public bool CanTransitionUp = true;
		public bool CanTransitionDown = true;
		public bool CanTransitionLeft = true;
		public bool CanTransitionRight = true;

		public bool CanTransitionUpLeft = true;
		public bool CanTransitionUpRight = true;
		public bool CanTransitionDownLeft = true;
		public bool CanTransitionDownRight = true;

		private const float LENGTH_MULTIPLIER = .5f;

		private static GameObject _side;
		private static GameObject _corner;
		private static Sprite _sideSprite;
		private static Sprite _cornerSprite;

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
		private GameObject IsUpInfo () {
			return TileMaker.Get(_x, _y + 1);
		}

		private GameObject IsDownInfo () {
			return TileMaker.Get(_x, _y - 1);
		}

		private GameObject IsLeftInfo () {
			return TileMaker.Get(_x - 1, _y);
		}

		private GameObject IsRightInfo () {
			return TileMaker.Get(_x + 1, _y);
		}

		private GameObject IsUpLeftInfo () {
			return TileMaker.Get(_x - 1, _y + 1);
		}

		private GameObject IsUpRightInfo () {
			return TileMaker.Get(_x + 1, _y + 1);
		}

		private GameObject IsDownLeftInfo () {
			return TileMaker.Get(_x - 1, _y - 1);
		}

		private GameObject IsDownRightInfo () {
			return TileMaker.Get(_x + 1, _y - 1);
		}
		#endregion

		public static void GetStaticAssets () {
			_side = TileSprites.SideGo;
			_sideSprite = TileSprites.Side;
			_corner = TileSprites.CornerGo;
			_cornerSprite = TileSprites.Corner;
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
				CanTransitionDown = false;
				CanTransitionDownLeft = false;
				CanTransitionDownRight = false;
				CanTransitionLeft = false;
				CanTransitionRight = false;
				CanTransitionUp = false;
				CanTransitionUpLeft = false;
				CanTransitionUpRight = false;

				yield break;
			}

			// Test for transferability
			#region
			GameObject u = IsUpInfo();
			GameObject d = IsDownInfo();
			GameObject l = IsLeftInfo();
			GameObject r = IsRightInfo();
			GameObject ul = IsUpLeftInfo();
			GameObject ur = IsUpRightInfo();
			GameObject dl = IsDownLeftInfo();
			GameObject dr = IsDownRightInfo();

			if (u != null) {
				//Has TileTransition script
				if (u.GetComponent<SmoothTiles>() == null || u.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionUp = false;
				}
				//CanTransitionWithSelf
				if (u.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionUp = false;
				} else {
					CanTransitionUp = true;
				}
				//Overlap 
				if (u.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionUp = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (u.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionUp = false;
				}
			}

			if (d != null) {
				//Has TileTransition script
				if (d.GetComponent<SmoothTiles>() == null || d.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionDown = false;
				}
				//CanTransitionWithSelf
				if (d.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionDown = false;
				} else {
					CanTransitionDown = true;
				}
				//Overlap 
				if (d.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionDown = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (d.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionDown = false;
				}
			}

			if (l != null) {
				//Has TileTransition script
				if (l.GetComponent<SmoothTiles>() == null || l.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionLeft = false;
				}
				//CanTransitionWithSelf
				if (l.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionLeft = false;
				} else {
					CanTransitionLeft = true;
				}
				//Overlap 
				if (l.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionLeft = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (l.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionLeft = false;
				}
			}

			if (r != null) {
				//Has TileTransition script
				if (r.GetComponent<SmoothTiles>() == null || r.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionRight = false;
				}
				//CanTransitionWithSelf
				if (r.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionRight = false;
				} else {
					CanTransitionRight = true;
				}
				//Overlap 
				if (r.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionRight = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (r.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionRight = false;
				}
			}

			if (ul != null) {
				//Has TileTransition script
				if (ul.GetComponent<SmoothTiles>() == null || ul.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionUpLeft = false;
				}
				//CanTransitionWithSelf
				if (ul.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionUpLeft = false;
				} else {
					CanTransitionUpLeft = true;
				}
				//Overlap 
				if (ul.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionUpLeft = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (ul.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionUpLeft = false;
				}
			}

			if (ur != null) {
				//Has TileTransition script
				if (ur.GetComponent<SmoothTiles>() == null || ur.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionUpRight = false;
				}
				//CanTransitionWithSelf
				if (ur.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionUpRight = false;
				} else {
					CanTransitionUpRight = true;
				}
				//Overlap 
				if (ur.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionUpRight = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (ur.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionUpRight = false;
				}
			}

			if (dl != null) {
				//Has TileTransition script
				if (dl.GetComponent<SmoothTiles>() == null || dl.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionDownLeft = false;
				}
				//CanTransitionWithSelf
				if (dl.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionDownLeft = false;
				} else {
					CanTransitionDownLeft = true;
				}
				//Overlap 
				if (dl.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionDownLeft = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (dl.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionDownLeft = false;
				}
			}

			if (dr != null) {
				//Has TileTransition script
				if (dr.GetComponent<SmoothTiles>() == null || dr.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionDownRight = false;
				}
				//CanTransitionWithSelf
				if (dr.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionDownRight = false;
				} else {
					CanTransitionDownRight = true;
				}
				//Overlap 
				if (dr.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionDownRight = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (dr.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionDownRight = false;
				}
			}
			#endregion

			// Draw transitions
			#region
			Vector3 t = transform.position;

			if (IsLeft() && CanTransitionLeft) {
				GameObject sidePiece = Instantiate(_side);
				sidePiece.GetComponent<SpriteRenderer>().sprite = _sideSprite;
				sidePiece.transform.rotation = Quaternion.Euler(0, 0, 90);
				sidePiece.transform.position = new Vector3((int) t.x, t.y, t.z);
				//Features
				float si= sidePiece.GetComponent<SpriteRenderer>().bounds.size.y;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				sidePiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				sidePiece.transform.localScale = new Vector3(sidePiece.transform.localScale.x, sidePiece.transform.localScale.y * LENGTH_MULTIPLIER, sidePiece.transform.localScale.z);
				sidePiece.GetComponent<SpriteRenderer>().color = Color;
				sidePiece.transform.parent = transform;
			}

			if (IsRight() && CanTransitionRight) {
				GameObject sidePiece = Instantiate(_side);
				sidePiece.GetComponent<SpriteRenderer>().sprite = _sideSprite;
				sidePiece.transform.rotation = Quaternion.Euler(0, 0, 270);
				sidePiece.transform.position = new Vector3((int) t.x + 1, t.y, t.z);
				//Features
				float si= sidePiece.GetComponent<SpriteRenderer>().bounds.size.y;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				sidePiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				sidePiece.transform.localScale = new Vector3(sidePiece.transform.localScale.x, sidePiece.transform.localScale.y * LENGTH_MULTIPLIER, sidePiece.transform.localScale.z);
				sidePiece.GetComponent<SpriteRenderer>().color = Color;
				sidePiece.transform.parent = transform;
			}

			if (IsUp() && CanTransitionUp) {
				GameObject sidePiece = Instantiate(_side);
				sidePiece.GetComponent<SpriteRenderer>().sprite = _sideSprite;
				sidePiece.transform.rotation = Quaternion.Euler(0, 0, 0);
				sidePiece.transform.position = new Vector3(t.x, (int) t.y + 1, t.z);
				//Features
				float si= sidePiece.GetComponent<SpriteRenderer>().bounds.size.x;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				sidePiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				sidePiece.transform.localScale = new Vector3(sidePiece.transform.localScale.x, sidePiece.transform.localScale.y * LENGTH_MULTIPLIER, sidePiece.transform.localScale.z);
				sidePiece.GetComponent<SpriteRenderer>().color = Color;
				sidePiece.transform.parent = transform;
			}

			if (IsDown() && CanTransitionDown) {
				GameObject sidePiece = Instantiate(_side);
				sidePiece.GetComponent<SpriteRenderer>().sprite = _sideSprite;
				sidePiece.transform.rotation = Quaternion.Euler(0, 0, 180);
				sidePiece.transform.position = new Vector3(t.x, (int) t.y, t.z);
				//Features
				float si= sidePiece.GetComponent<SpriteRenderer>().bounds.size.x;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				sidePiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				sidePiece.transform.localScale = new Vector3(sidePiece.transform.localScale.x, sidePiece.transform.localScale.y * LENGTH_MULTIPLIER, sidePiece.transform.localScale.z);
				sidePiece.GetComponent<SpriteRenderer>().color = Color;
				sidePiece.transform.parent = transform;
			}

			if (IsUpLeft() && CanTransitionUpLeft && IsLeft() && CanTransitionLeft && IsUp() && CanTransitionUp) {
				GameObject cornerPiece = Instantiate(_corner);
				cornerPiece.GetComponent<SpriteRenderer>().sprite = _cornerSprite;
				cornerPiece.transform.rotation = Quaternion.Euler(0, 0, 90);
				cornerPiece.transform.position = new Vector3((int) t.x, (int) t.y + 1, t.z);
				//Features
				float si= cornerPiece.GetComponent<SpriteRenderer>().bounds.size.x*2;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				cornerPiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				cornerPiece.transform.localScale = new Vector3(cornerPiece.transform.localScale.x * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.y * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.z);
				cornerPiece.GetComponent<SpriteRenderer>().color = Color;
				cornerPiece.transform.parent = transform;
			}

			if (IsDownLeft() && CanTransitionDownLeft && IsLeft() && CanTransitionLeft && IsDown() && CanTransitionDown) {
				GameObject cornerPiece = Instantiate(_corner);
				cornerPiece.GetComponent<SpriteRenderer>().sprite = _cornerSprite;
				cornerPiece.transform.rotation = Quaternion.Euler(0, 0, 180);
				cornerPiece.transform.position = new Vector3((int) t.x, (int) t.y, t.z);
				//Features
				float si= cornerPiece.GetComponent<SpriteRenderer>().bounds.size.x*2;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				cornerPiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				cornerPiece.transform.localScale = new Vector3(cornerPiece.transform.localScale.x * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.y * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.z);
				cornerPiece.GetComponent<SpriteRenderer>().color = Color;
				cornerPiece.transform.parent = transform;
			}

			if (IsUpRight() && CanTransitionUpRight && IsRight() && CanTransitionRight && IsUp() && CanTransitionUp) {
				GameObject cornerPiece = Instantiate(_corner);
				cornerPiece.GetComponent<SpriteRenderer>().sprite = _cornerSprite;
				cornerPiece.transform.rotation = Quaternion.Euler(0, 0, 0);
				cornerPiece.transform.position = new Vector3((int) t.x + 1, (int) t.y + 1, t.z);
				//Features
				float si= cornerPiece.GetComponent<SpriteRenderer>().bounds.size.x*2;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				cornerPiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				cornerPiece.transform.localScale = new Vector3(cornerPiece.transform.localScale.x * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.y * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.z);
				cornerPiece.GetComponent<SpriteRenderer>().color = Color;
				cornerPiece.transform.parent = transform;
			}

			if (IsDownRight() && CanTransitionDownRight && IsRight() && CanTransitionRight && IsDown() && CanTransitionDown) {
				GameObject cornerPiece = Instantiate(_corner);
				cornerPiece.GetComponent<SpriteRenderer>().sprite = _cornerSprite;
				cornerPiece.transform.rotation = Quaternion.Euler(0, 0, 270);
				cornerPiece.transform.position = new Vector3((int) t.x + 1, (int) t.y, t.z);
				//Features
				float si= cornerPiece.GetComponent<SpriteRenderer>().bounds.size.x*2;
				float tr= transform.GetComponent<SpriteRenderer>().bounds.size.x;
				cornerPiece.transform.localScale = new Vector3(tr / si, tr / si, 1);
				cornerPiece.transform.localScale = new Vector3(cornerPiece.transform.localScale.x * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.y * LENGTH_MULTIPLIER, cornerPiece.transform.localScale.z);
				cornerPiece.GetComponent<SpriteRenderer>().color = Color;
				cornerPiece.transform.parent = transform;
			}
			#endregion

			ApplicationController.NotifyReady();
		}

		// todo implement update
		public void UpdateTransitions () {
			DeleteTransition("all");
			// todo
		}

		//Keywords are: all, left, right, top, bottom, topleft, topright, bottomleft, bottomright
		public void DeleteTransition (params string[] direction) {
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
						Debug.LogError("SmoothTiles2d - Error: Parameters for DeleteTransition are invalid, LOL! Possible parameter values include: 'all' 'left' 'right' 'top' 'bottom' 'topleft' 'topright' 'bottomleft' 'bottomright'");
						break;
				}
			}
		}

	}

}