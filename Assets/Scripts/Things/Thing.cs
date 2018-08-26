using Assets.Scripts.Graphics;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[RequireComponent(typeof(BoxCollider2D))]
	public class Thing : MonoBehaviour {

		protected Transform Sprite;

		private SpriteRenderer _renderer;
		private bool _didStart;

		protected void AssertActive () {
			if (gameObject.activeSelf == false) {
				gameObject.SetActive(true);
			}

			if (_didStart == false) {
				Start();
			}
		}

		[UsedImplicitly]
		private void Start () {
			if (_didStart) {
				return;
			}

			AssertRequiredComponents();

			Vector2 v = transform.localPosition;
			transform.localPosition = new Vector3(v.x, v.y, Order.THING);
			_didStart = true;
		}

		[UsedImplicitly]
		private void OnMouseDown () {
			Selector.Select(transform);
		}

		private void AssertRequiredComponents () {
			if (transform.GetComponent<BoxCollider2D>() == null) {
				BoxCollider2D bc = gameObject.AddComponent<BoxCollider2D>();
				bc.offset = Vector2.zero;
				bc.size = Vector2.one;
			}

			if (transform.Find("Sprite") == null) {
				CreateChildSprite();
			}
		}

		private void CreateChildSprite () {
			Sprite = new GameObject("Sprite").transform;
			Sprite.SetParent(transform);
			Sprite.localPosition = Vector3.zero;
			_renderer = Sprite.gameObject.AddComponent<SpriteRenderer>();

			SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();

			if (sr != null) {
				_renderer.sprite = sr.sprite;
				_renderer.material = AssetLoader.DiffuseMat;
				Destroy(sr);
			}
		}

	}

}