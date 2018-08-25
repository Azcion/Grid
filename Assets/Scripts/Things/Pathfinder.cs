using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Pathfinder {

		public bool Moving;

		private readonly Transform _transform;
		private readonly List<Mover> _movers;
		private readonly List<Mover> _moversToRemove;

		private Vector2 _currentTarget;

		public Pathfinder (Transform transform) {
			_transform = transform;
			_movers = new List<Mover>();
			_moversToRemove = new List<Mover>();
			_currentTarget = transform.localPosition;
		}
		
		public void MoveByOffset (Vector2 target, bool debug = false) {
			_currentTarget += target;
			_movers.Add(new Mover(_transform, _currentTarget, 1.5f, debug));
		}

		public void Update () {
			foreach (Mover mover in _movers) {
				mover.Update();

				if (mover.Finished) {
					mover.Destroy();
					_moversToRemove.Add(mover);
				} else {
					Moving = true;
				}
			}

			foreach (Mover mover in _moversToRemove) {
				_movers.Remove(mover);
			}

			if (_movers.Count == 0) {
				Moving = false;
			}

			_moversToRemove.Clear();
		}

	}

}