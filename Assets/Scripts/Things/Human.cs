using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	public class Human : Thing {

		[UsedImplicitly]
		public Sprite[] HumanSprites;

		public Transform Target;

		private const float SPRITE_OFFSET = -.3f;

		private Direction _facing;
		private bool _wasAssigned;

		private Vector2[] _path;
		private int _targetIndex;
		private bool _moving;

		public void Assign () {
			AssertActive();

			Sprite.localPosition = new Vector2(.5f, Calc.Round(.5f + SPRITE_OFFSET, 2));
			_wasAssigned = true;
			_moving = false;
		}

		[UsedImplicitly]
		private void OnEnable () {
			if (_wasAssigned == false) {
				Assign();
			}
		}

		[UsedImplicitly]
		private void Update () {
			if (_wasAssigned == false) {
				return;
			}

			if (_moving || !(Random.value > .985f)) {
				return;
			}

			Vector2 v = (Vector2) transform.localPosition + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
			v = Calc.Clamp(v, 0, TileMaker.YTILES, 0, TileMaker.YTILES);
			PathRequestManager.RequestPath(transform.localPosition, v, OnPathFound);
		}

		private void OnPathFound (Vector2[] path, bool success) {
			if (success == false) {
				return;
			}

			_path = path;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}

		[UsedImplicitly]
		private IEnumerator FollowPath () {
			Vector2 currentWaypoint = _path[0];

			while (true) {
				if ((Vector2) transform.position == currentWaypoint) {
					if (++_targetIndex >= _path.Length) {
						yield break;
					}

					currentWaypoint = _path[_targetIndex];
				}

				transform.localPosition = Vector2.MoveTowards(transform.localPosition, currentWaypoint, .05f);

				yield return null;
			}
		}

	}

}