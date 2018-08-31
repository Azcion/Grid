using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Pathfinding;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Pathfinding : Thing {
		
		[UsedImplicitly] public bool DisplayPathGizmos;

		protected bool Moving;
		protected Direction Facing;
		protected bool DirectionChanged;

		private Vector2[] _path;
		private int _targetIndex;
		private float _speed;

		protected void InitializePathfinding (float speed = 2) {
			InitializeThing();

			Tf = transform;
			Moving = false;
			DirectionChanged = false;
			_speed = speed;
		}

		protected bool FindPath (Vector2 target) {
			if (NodeGrid.GetNodeAt(target).Walkable == false) {
				return false;
			}
			
			PathRequestManager.RequestPath(new PathRequest(Tf.localPosition, target, OnPathFound));

			return true;
		}

		[UsedImplicitly]
		private void OnDrawGizmos () {
			if (_path == null || DisplayPathGizmos == false) {
				return;
			}

			Gizmos.color = Color.cyan;

			if (_targetIndex < _path.Length) {
				Gizmos.DrawLine((Vector2) Tf.localPosition + NodeGrid.Offset,
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

			Moving = true;
			_path = path;
			_targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}

		[UsedImplicitly]
		private IEnumerator FollowPath () {
			Vector2 currentWaypoint = _path[0];
			AdjustDirection(currentWaypoint);

			while (true) {
				if ((Vector2) Tf.position == currentWaypoint) {
					if (++_targetIndex >= _path.Length) {
						Moving = false;
						yield break;
					}

					currentWaypoint = _path[_targetIndex];
					AdjustDirection(currentWaypoint);
				}

				float speed = _speed * Time.deltaTime;
				Tf.localPosition = Vector2.MoveTowards(Tf.localPosition, currentWaypoint, speed);

				yield return null;
			}
		}

		private void AdjustDirection (Vector2 waypoint) {
			int x0 = (int) Tf.position.x;
			int y0 = (int) Tf.position.y;
			int x1 = (int) waypoint.x;
			int y1 = (int) waypoint.y;
			Direction newDirection;

			if (x0 == x1) {
				newDirection = y0 < y1 ? Direction.Up : Direction.Down;
			} else {
				newDirection = x0 < x1 ? Direction.Right : Direction.Left;
			}

			if (Facing == newDirection) {
				return;
			}

			Facing = newDirection;
			DirectionChanged = true;
		}

	}

}