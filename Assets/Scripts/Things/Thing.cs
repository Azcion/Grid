using Assets.Scripts.Graphics;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[RequireComponent(typeof(BoxCollider2D))]
	public class Thing : MonoBehaviour {

		protected Transform Sprite;
		protected SpriteRenderer Renderer;

		protected bool DidStart;

		protected void AssertActive () {
			if (gameObject.activeSelf == false) {
				gameObject.SetActive(true);
				Start();
			}
		}

		[UsedImplicitly]
		private void Start () {
			if (DidStart) {
				return;
			}

			AssertRequiredComponents();

			Vector2 v = transform.localPosition;
			transform.localPosition = new Vector3(v.x, v.y, Order.THING);
			DidStart = true;
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
			Renderer = Sprite.gameObject.AddComponent<SpriteRenderer>();

			SpriteRenderer sr = transform.GetComponent<SpriteRenderer>();

			if (sr != null) {
				Renderer.sprite = sr.sprite;
				Renderer.material = AssetLoader.DiffuseMat; //sr.material;
				Destroy(sr);
			}
		}

	}

}