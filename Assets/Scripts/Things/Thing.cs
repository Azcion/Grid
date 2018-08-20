using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
	public class Thing : MonoBehaviour {

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