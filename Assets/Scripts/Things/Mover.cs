using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Mover {

		public bool Finished;

		private readonly Transform _transform;
		private readonly Vector2 _target;
		private readonly float _speed;

		private readonly bool _debug;
		private readonly PathLine _debugLine;

		public Mover (Transform transform, Vector2 target, float speed, bool debug = false) {
			_transform = transform;
			_target = target;
			_speed = speed;
			_debug = debug;

			if (debug) {
				_debugLine = new PathLine(transform.localPosition, target, Color.cyan);
			}
		}

		public void Update () {
			if (Finished) {
				Debug.Log("Path finished drawing. It should be destroyed.");
				return;
			}

			if (Vector3.Distance(_target, _transform.localPosition) <= .02f) {
				_transform.localPosition = new Vector3(_target.x, _target.y, _transform.localPosition.z);
				Finished = true;
				return;
			}
			
			// todo check performance
			Vector2 direction = (_target - (Vector2) _transform.localPosition).normalized; 
			_transform.localPosition = (Vector2) _transform.localPosition + direction * _speed * Time.deltaTime;

			if (_debug) {
				_debugLine.UpdatePos(_transform.localPosition, _target);
				_debugLine.Update();
			}
		}

		public void ChangeOrigin (Vector2 origin) {
			if (_debug) {
				_debugLine.UpdatePos(origin, _target);
			}
		}

		public void Destroy () {
			if (_debug) {
				_debugLine.Destroy();
			}
		}

	}

}