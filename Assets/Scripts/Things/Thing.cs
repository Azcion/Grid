using Assets.Scripts.Graphics;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[RequireComponent(typeof(BoxCollider2D))]
	public class Thing : MonoBehaviour {

		protected Transform Sprite;
		protected Transform Tf;

		protected bool Selected;

		private SpriteRenderer _renderer;

		public void Deselect () {
			Selected = false;
		}

		protected void InitializeThing () {
			gameObject.SetActive(true);
			Tf = transform;
			Vector2 v = Tf.localPosition;
			Tf.localPosition = new Vector3(v.x, v.y, Order.THING);
			AssertRequiredComponents();
		}

		[UsedImplicitly]
		private void OnMouseDown () {
			Selector.Select(Tf, this);
			Selected = true;
		}

		private void AssertRequiredComponents () {
			if (Tf.GetComponent<BoxCollider2D>() == null) {
				BoxCollider2D bc = gameObject.AddComponent<BoxCollider2D>();
				bc.offset = new Vector2(.5f, .5f);
				bc.size = Vector2.one;
			}

			if (Tf.Find("Sprite") == null) {
				CreateChildSprite();
			}
		}

		private void CreateChildSprite () {
			Sprite = new GameObject("Sprite").transform;
			Sprite.SetParent(Tf);
			Sprite.localPosition = Vector3.zero;
			_renderer = Sprite.gameObject.AddComponent<SpriteRenderer>();

			SpriteRenderer sr = Tf.GetComponent<SpriteRenderer>();

			if (sr == null) {
				return;
			}

			_renderer.sprite = sr.sprite;
			_renderer.material = AssetLoader.DiffuseMat;
			Destroy(sr);
		}

	}

}