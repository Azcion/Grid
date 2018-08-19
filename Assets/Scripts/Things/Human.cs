using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Human : MonoBehaviour {

		[UsedImplicitly]
		public Sprite[] HumanSprites;

		private Sprite _humanSprite;
		private Direction _facing;

		[UsedImplicitly]
		private void OnEnable () {
			Vector2 v = transform.localPosition;
			transform.localPosition = new Vector3(v.x, v.y, Order.THING);
		}

		[UsedImplicitly]
		private void OnMouseDown () {
			Selection.Select(transform);
		}

	}

}