using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[RequireComponent(typeof(BoxCollider2D))]
	public class Thing : MonoBehaviour {

		public bool IsSelectable { get; protected set; }

		protected Transform Tf;
		protected Transform Child;
		protected SpriteRenderer ChildRenderer;
		protected bool Selected;

		public void Deselect () {
			Selected = false;
		}

		protected void InitializeThing () {
			gameObject.SetActive(true);
			Tf = transform;
			AssertRequiredComponents();
		}

		protected void SetSprite (Sprite sprite, bool flipX) {
			ChildRenderer.sprite = sprite;
			ChildRenderer.flipX = flipX;
		}

		protected void SetTint (Color color) {
			ChildRenderer.color = color;
		}

		[UsedImplicitly]
		private void OnMouseDown () {
			Selector.Select(Tf, this);
			Selected = true;
		}

		private void AssertRequiredComponents () {
			BoxCollider2D bc = Tf.GetComponent<BoxCollider2D>();
			bc.isTrigger = true;
			bc.offset = new Vector2(.5f, .5f);
			bc.size = Vector2.one;
			CreateChildSprite();
		}

		private void CreateChildSprite () {
			Child = new GameObject("Sprite", typeof(SpriteRenderer)).transform;
			Child.SetParent(Tf);
			Child.localPosition = Vector3.zero;
			ChildRenderer = Child.gameObject.GetComponent<SpriteRenderer>();
			ChildRenderer.sharedMaterial = Assets.ThingMat;
		}

	}

}