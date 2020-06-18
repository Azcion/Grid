using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Pathfinding;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Pathfinding : Thing {

		public bool DidDesignateAction;

		protected bool Moving;
		protected Direction Facing;
		protected bool DirectionChanged;

		private Thing _target;
		private Action _action;
		private Vector2[] _path;
		private float _speed;
		private bool _actionOnComplete;

		protected void InitializePathfinding (float speed) {
			Moving = false;
			DirectionChanged = false;
			_speed = speed / 3;
		}

		public bool FindPath (Thing target, Action action) {
			bool found = FindPath(target.transform.position, true);
			_target = target;
			_action = action;

			return found;
		}

		protected bool FindPath (Vector2 target, bool actionOnComplete = false) {
			if (NodeGrid.GetNodeAt(target).Walkable == false) {
				return false;
			}

			_actionOnComplete = actionOnComplete;
			PathRequestManager.RequestPath(new PathRequest(transform.localPosition, target, OnPathFound));

			return true;
		}

		private void OnPathFound (Vector2[] path, bool success) {
			if (success == false) {
				return;
			}

			Moving = true;
			_path = path;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}

		[UsedImplicitly]
		private IEnumerator FollowPath () {
			if (_actionOnComplete) Debug.Log($"{name} {Name.Get(_action)} {(_target as Thing)?.name}");
			Vector2 currentWaypoint = _path[0];
			AdjustDirection(currentWaypoint);
			int pathLength = _path.Length - (_actionOnComplete ? 1 : 0);
			int index = 0;

			while (true) {
				if ((Vector2) transform.position == currentWaypoint) {
					++index;

					if (index >= pathLength) {
						Moving = false;

						if (!_actionOnComplete) {
							yield break;
						}

						_actionOnComplete = false;

						switch (_target.Heir.Type) {
							case ThingType.Plant:
								Plant plant = _target.Heir as Plant;

								switch (_action) {
									case Action.ChopWood:
										plant?.Action_ChopWood();
										break;
									case Action.Harvest:
										plant?.Action_Harvest();
										break;
								}

								break;
						}

						yield break;
					}

					currentWaypoint = _path[index];
					AdjustDirection(currentWaypoint);
				}

				float speed = _speed * Time.deltaTime;
				Vector2 v = Vector2.MoveTowards(transform.localPosition, currentWaypoint, speed);
				transform.localPosition = new Vector3(v.x, v.y, transform.localPosition.z);

				yield return null;
			}
		}

		private void AdjustDirection (Vector2 waypoint) {
			int x0 = (int) transform.position.x;
			int y0 = (int) transform.position.y;
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