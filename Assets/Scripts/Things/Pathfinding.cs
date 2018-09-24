using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Pathfinding;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Pathfinding : Thing {

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
				Vector2 v = Vector2.MoveTowards(Tf.localPosition, currentWaypoint, speed);
				Tf.localPosition = new Vector3(v.x, v.y, Tf.localPosition.z);

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
				newDirection = y0 < y1 ? Direction.North : Direction.South;
			} else {
				newDirection = x0 < x1 ? Direction.East : Direction.West;
			}

			if (Facing == newDirection) {
				return;
			}

			Facing = newDirection;
			DirectionChanged = true;
		}

	}

}