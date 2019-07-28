using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Makers;
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

		private int _x;
		private int _y;
		private TileType _type;

		public static void LoadAssets () {
			_side = new GameObject("Transition Side", typeof(SpriteRenderer));
			_side.SetActive(false);
			SpriteRenderer sideSr = _side.GetComponent<SpriteRenderer>();
			sideSr.sprite = AssetLoader.TransitionSide;
			sideSr.sharedMaterial = AssetLoader.DiffuseMat;
			_corner = new GameObject("Transition Corner", typeof(SpriteRenderer));
			_corner.SetActive(false);
			SpriteRenderer cornerSr = _corner.GetComponent<SpriteRenderer>();
			cornerSr.sprite = AssetLoader.TransitionCorner;
			cornerSr.sharedMaterial = AssetLoader.DiffuseMat;
		}

		#region Neighbor objects
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

		[UsedImplicitly]
		private void Start () {
			Tile tile = GetComponent<Tile>();
			_x = tile.X;
			_y = tile.Y;
			_type = tile.Type;
			Color = AverageColor.Get(_type);

			Initialize();
		}

		private void Initialize () {
			if (!CanTransition) {
				return;
			}

			// Test for transferability
			bool canFadeU = Check(GetUp());
			bool canFadeD = Check(GetDown());
			bool canFadeL = Check(GetLeft());
			bool canFadeR = Check(GetRight());
			bool canFadeUl = Check(GetUpLeft());
			bool canFadeUr = Check(GetUpRight());
			bool canFadeDl = Check(GetDownLeft());
			bool canFadeDr = Check(GetDownRight());

			// Draw transitions
			Vector3 t = transform.position;

			if (canFadeU && GetUp()) {
				Create(0, t.x, (int) t.y + 1);
			}

			if (canFadeD && GetDown()) {
				Create(180, t.x, (int) t.y);
			}

			if (canFadeL && GetLeft()) {
				Create(90, (int) t.x, t.y, special: true);
			}

			if (canFadeR && GetRight()) {
				Create(270, (int) t.x + 1, t.y, special: true);
			}

			if (canFadeUl && canFadeL && canFadeU && GetUpLeft() && GetLeft() && GetUp()) {
				Create(90, (int) t.x, (int) t.y + 1, true);
			}

			if (canFadeUr && canFadeR && canFadeU && GetUpRight() && GetRight() && GetUp()) {
				Create(0, (int) t.x + 1, (int) t.y + 1, true);
			}

			if (canFadeDl && canFadeL && canFadeD && GetDownLeft() && GetLeft() && GetDown()) {
				Create(180, (int) t.x, (int) t.y, true);
			}

			if (canFadeDr && canFadeR && canFadeD && GetDownRight() && GetRight() && GetDown()) {
				Create(270, (int) t.x + 1, (int) t.y, true);
			}

			ApplicationController.NotifyReady();
		}

		private bool Check (GameObject t) {
			if (t == null) {
				return false;
			}

			if (t.GetComponent<Tile>().Type == _type) {
				return false;
			}

			SmoothTiles other = t.GetComponent<SmoothTiles>();

			return !(other.OverlapOrder >= OverlapOrder) && other.CanBeTransitionedTo;
		}

		private void Create (int r, float x, float y, bool corner=false, bool special=false) {
			GameObject t = Instantiate(corner ? _corner : _side, transform);
			t.SetActive(true);
			t.transform.rotation = Quaternion.Euler(0, 0, r);
			t.transform.position = new Vector3(x, y, Order.TRANSITION);
			SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
			Color tint = TileTint.Get(_type);
			sr.color = new Color(Color.r * tint.r, Color.g * tint.g, Color.b * tint.b);

			Vector2 size = sr.bounds.size;
			float yS = (special ? size.y : size.x) * (corner ? 2 : 1);
			float xS = transform.GetComponent<SpriteRenderer>().bounds.size.x;
			t.transform.localScale = new Vector3(xS / yS * (corner ? LENGTH : 1), xS / yS * LENGTH, 1);
		}

	}

}