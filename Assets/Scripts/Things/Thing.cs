using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Thing : MonoBehaviour {

		public Def Def { get; protected set; }
		public ThingType Type { get; protected set; }
		public Animal AsAnimal { get; protected set; }
		public Humanoid AsHumanoid { get; protected set; }
		public Item AsItem { get; protected set; }
		public Linked AsLinked { get; protected set; }
		public Pathfinding AsPathfinding { get; protected set; }
		public Plant AsPlant { get; protected set; }
		public List<Action> ValidActions { get; private set; }
		public ThingMaterial Material { get; protected set; }
		public bool IsBlueprint { get; protected set; }

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
		private void OnMouseOver () {
			if (!GUI.Busy && IsSelectable && !Selected && Input.GetMouseButtonUp(0)) {
				// select on left click
				Selector.Select(this);
				Selected = true;
			} else if (ValidActions.Count > 0 && Selector.Active && Selector.Thing.Type == ThingType.Player && Input.GetMouseButtonUp(1)) {
				// designate path and action on right click
				bool found = Selector.Thing.AsHumanoid.FindPath(this, ValidActions[0]);

				if (found) {
					Selector.Thing.AsHumanoid.DidDesignateAction = true;
				}
			}
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