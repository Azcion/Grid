using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Thing : MonoBehaviour {

		public IThing Heir { get; protected set; }
		public IThingDef ThingDef { get; protected set; }
		public List<Action> ValidActions { get; private set; }

		protected Transform Child;
		protected SpriteRenderer ChildRenderer;
		protected bool IsSelectable;
		protected bool Selected;

		public void Prepare () {
			CreateChildSprite();
		}

		public void Deselect () {
			Selected = false;
		}

		protected void PrepareChild () {
			Child = transform.GetChild(0);
			ChildRenderer = Child.gameObject.GetComponent<SpriteRenderer>();
			ValidActions = new List<Action>();
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
			if (!IsSelectable) {
				return;
			}

			Selector.Select(transform, this);
			Selected = true;
		}

		private void CreateChildSprite () {
			Child = new GameObject("Sprite", typeof(SpriteRenderer)).transform;
			Child.SetParent(transform);
			Child.localPosition = Vector3.zero;
			ChildRenderer = Child.gameObject.GetComponent<SpriteRenderer>();
			ChildRenderer.sharedMaterial = Assets.ThingMat;
		}

	}

}