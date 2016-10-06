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

		public bool CanBeTransitionedTo = true;
		public bool CanTransition = true;
		public bool CanTransitionLeft = true;
		public bool CanTransitionRight = true;
		public bool CanTransitionTop = true;
		public bool CanTransitionBottom = true;

		public bool CanTransitionTopRight = true;
		public bool CanTransitionBottomRight = true;
		public bool CanTransitionTopLeft = true;
		public bool CanTransitionBottomLeft = true;

		private const float LENGTH_MULTIPLIER = .5f;

		private static GameObject _side;
		private static GameObject _corner;
		private static Sprite _sideSprite;
		private static Sprite _cornerSprite;
		private static bool _gotStaticAssets;

		private int _lm;

		// Raycast bools
		#region
		private bool IsTop () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x, t.y + .5001f), new Vector2(t.x, t.y + 1), _lm);
		}

		private bool IsBottom () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x, t.y - .5001f), new Vector2(t.x, t.y - 1), _lm);
		}

		private bool IsLeft () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x - .5001f, t.y), new Vector2(t.x - 1, t.y), _lm);
		}

		private bool IsRight () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x + .5001f, t.y), new Vector2(t.x + 1, t.y), _lm);
		}

		private bool IsTopRight () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x + .5001f, t.y + .5001f), new Vector2(t.x + 1, t.y + 1), _lm);
		}

		private bool IsTopLeft () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x - .5001f, t.y + .5001f), new Vector2(t.x - 1, t.y + 1), _lm);
		}

		private bool IsBottomRight () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x + .5001f, t.y - .5001f), new Vector2(t.x + 1, t.y - 1), _lm);
		}

		private bool IsBottomLeft () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x - .5001f, t.y - .5001f), new Vector2(t.x - 1, t.y - 1), _lm);
		}
		#endregion

		// Raycast infos
		#region
		private RaycastHit2D IsTopInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x, t.y + .5001f), new Vector2(t.x, t.y + 1), _lm);
		}

		private RaycastHit2D IsBottomInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x, t.y - .5001f), new Vector2(t.x, t.y - 1), _lm);
		}

		private RaycastHit2D IsLeftInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x - .5001f, t.y), new Vector2(t.x - 1, t.y), _lm);
		}

		private RaycastHit2D IsRightInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x + .5001f, t.y), new Vector2(t.x + 1, t.y), _lm);
		}

		private RaycastHit2D IsTopRightInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x + .5001f, t.y + .5001f), new Vector2(t.x + 1, t.y + 1), _lm);
		}

		private RaycastHit2D IsTopLeftInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x - .5001f, t.y + .5001f), new Vector2(t.x - 1, t.y + 1), _lm);
		}

		private RaycastHit2D IsBottomRightInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x + .5001f, t.y - .5001f), new Vector2(t.x + 1, t.y - 1), _lm);
		}

		private RaycastHit2D IsBottomLeftInfo () {
			Vector2 t = transform.position;
			return Physics2D.Linecast(new Vector2(t.x - .5001f, t.y - .5001f), new Vector2(t.x - 1, t.y - 1), _lm);
		}
		#endregion

		[UsedImplicitly]
		private void Start () {
			GetStaticAssets();
			StartCoroutine(Initialize());

			// Initiate layer
			gameObject.layer = LayerMask.NameToLayer("Transferable");
			_lm = 1 << gameObject.layer;
		}

		private IEnumerator Initialize () {
			yield return new WaitForSeconds(1);
			
			if (!CanTransition) {
				CanTransitionBottom = false;
				CanTransitionBottomLeft = false;
				CanTransitionBottomRight = false;
				CanTransitionLeft = false;
				CanTransitionRight = false;
				CanTransitionTop = false;
				CanTransitionTopLeft = false;
				CanTransitionTopRight = false;

				yield break;
			}

			// Test for transferability
			#region
			if (IsRightInfo().transform != null) {
				//Has TileTransition script
				if (IsRightInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsRightInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionRight = false;
				}
				//CanTransitionWithSelf
				if (IsRightInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					//if(CanTransitionToSelf){}
					//else
					CanTransitionRight = false;
				} else {
					CanTransitionRight = true;
				}
				//Overlap 
				if (IsRightInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionRight = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsRightInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionRight = false;
				}
			}
			if (IsLeftInfo().transform != null) {
				//Has TileTransition script
				if (IsLeftInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionLeft = false;
				}
				//CanTransitionWithSelf
				if (IsLeftInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionLeft = false;
				} else {
					CanTransitionLeft = true;
				}
				//Overlap 
				if (IsLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionLeft = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionLeft = false;
				}
			}
			if (IsTopInfo().transform != null) {
				//Has TileTransition script
				if (IsTopInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsTopInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionTop = false;
				}
				//CanTransitionWithSelf
				if (IsTopInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionTop = false;
				} else {
					CanTransitionTop = true;
				}
				//Overlap 
				if (IsTopInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionTop = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsTopInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionTop = false;
				}
			}
			if (IsBottomInfo().transform != null) {
				//Has TileTransition script
				if (IsBottomInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsBottomInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionBottom = false;
				}
				//CanTransitionWithSelf
				if (IsBottomInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionBottom = false;
				} else {
					CanTransitionBottom = true;
				}
				//Overlap 
				if (IsBottomInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionBottom = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsBottomInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionBottom = false;
				}
			}

			if (IsBottomLeftInfo().transform != null) {
				//Has TileTransition script
				if (IsBottomLeftInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsBottomLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionBottomLeft = false;
				}
				//CanTransitionWithSelf
				if (IsBottomLeftInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionBottomLeft = false;
				} else {
					CanTransitionBottomLeft = true;
				}
				//Overlap 
				if (IsBottomLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionBottomLeft = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsBottomLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionBottomLeft = false;
				}
			}
			if (IsBottomRightInfo().transform != null) {
				//Has TileTransition script
				if (IsBottomRightInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsBottomRightInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionBottomRight = false;
				}
				//CanTransitionWithSelf
				if (IsBottomRightInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionBottomRight = false;
				} else {
					CanTransitionBottomRight = true;
				}
				//Overlap 
				if (IsBottomRightInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionBottomRight = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsBottomRightInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionBottomRight = false;
				}
			}
			if (IsTopRightInfo().transform != null) {
				//Has TileTransition script
				if (IsTopRightInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsTopRightInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionTopRight = false;
				}
				//CanTransitionWithSelf
				if (IsTopRightInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionTopRight = false;
				} else {
					CanTransitionTopRight = true;
				}
				//Overlap 
				if (IsTopRightInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionTopRight = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsTopRightInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionTopRight = false;
				}
			}
			if (IsTopLeftInfo().transform != null) {
				//Has TileTransition script
				if (IsTopLeftInfo().transform.gameObject.GetComponent<SmoothTiles>() == null || IsTopLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().enabled == false) {
					CanTransitionTopLeft = false;
				}
				//CanTransitionWithSelf
				if (IsTopLeftInfo().transform.gameObject.GetComponent<Tile>().Type == GetComponent<Tile>().Type) {
					CanTransitionTopLeft = false;
				} else {
					CanTransitionTopLeft = true;
				}
				//Overlap 
				if (IsTopLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().OverlapOrder >= OverlapOrder) {
					CanTransitionTopLeft = false;
				}
				//CanOtherSpritesTransitionToSelf
				if (IsTopLeftInfo().transform.gameObject.GetComponent<SmoothTiles>().CanBeTransitionedTo == false) {
					CanTransitionTopLeft = false;
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

			if (IsTop() && CanTransitionTop) {
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

			if (IsBottom() && CanTransitionBottom) {
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

			if (IsTopLeft() && CanTransitionTopLeft && IsLeft() && CanTransitionLeft && IsTop() && CanTransitionTop) {
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

			if (IsBottomLeft() && CanTransitionBottomLeft && IsLeft() && CanTransitionLeft && IsBottom() && CanTransitionBottom) {
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

			if (IsTopRight() && CanTransitionTopRight && IsRight() && CanTransitionRight && IsTop() && CanTransitionTop) {
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

			if (IsBottomRight() && CanTransitionBottomRight && IsRight() && CanTransitionRight && IsBottom() && CanTransitionBottom) {
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

		private void GetStaticAssets () {
			if (_gotStaticAssets) {
				return;
			}

			_side = TileSprites.SideGo;
			_sideSprite = TileSprites.Side;
			_corner = TileSprites.CornerGo;
			_cornerSprite = TileSprites.Corner;
			_gotStaticAssets = true;
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