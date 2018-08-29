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

		[UsedImplicitly]
		public bool DisplayGridGizmos;

		public Transform Target;

		private const float SPRITE_OFFSET = -.3f;

		private Transform _t;
		private Direction _facing;
		private bool _wasAssigned;

		private Vector2[] _path;
		private int _targetIndex;
		private bool _moving;
		private float _speed;

		public void Assign () {
			AssertActive();

			Sprite.localPosition = new Vector2(.5f, Calc.Round(.5f + SPRITE_OFFSET, 2));
			_t = transform;
			_wasAssigned = true;
			_moving = false;
			_speed = 2;
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

			if (_moving || Random.value < .985f) {
				return;
			}

			//todo implement smarter targeting
			Vector2 v = (Vector2) _t.localPosition + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
			v = Calc.Clamp(v, 0, TileMaker.YTILES, 0, TileMaker.YTILES);

			if (NodeGrid.GetNodeAt(v).Walkable == false) {
				return;
			}
			
			PathRequestManager.RequestPath(_t.localPosition, v, OnPathFound);
		}

		[UsedImplicitly]
		private void OnDrawGizmos () {
			if (_path == null || DisplayGridGizmos == false) {
				return;
			}

			Gizmos.color = Color.cyan;

			if (_targetIndex < _path.Length) {
				Gizmos.DrawLine((Vector2) _t.localPosition + NodeGrid.Offset,
					_path[_targetIndex] + NodeGrid.Offset);
			}

			for (int i = _targetIndex; i < _path.Length - 1; ++i) {
				Vector2 current = _path[i] + NodeGrid.Offset;
				Vector2 next = _path[i + 1] + NodeGrid.Offset;
				
				Gizmos.DrawLine(current, next);
			}
		}

		private void OnPathFound (Vector2[] path, bool success) {
			if (success == false) {
				return;
			}

			_moving = true;
			_path = path;
			_targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}

		[UsedImplicitly]
		private IEnumerator FollowPath () {
			Vector2 currentWaypoint = _path[0];

			while (true) {
				if ((Vector2) _t.position == currentWaypoint) {
					if (++_targetIndex >= _path.Length) {
						_moving = false;
						yield break;
					}

					currentWaypoint = _path[_targetIndex];
				}

				float speed = _speed * Time.deltaTime;
				_t.localPosition = Vector2.MoveTowards(_t.localPosition, currentWaypoint, speed);

				yield return null;
			}
		}

	}

}