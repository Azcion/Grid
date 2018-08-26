using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Pathfinder {

		public bool Moving;

		private readonly Transform _transform;
		private readonly List<Mover> _movers;

		private Vector2 _currentTarget;

		public Pathfinder (Transform transform) {
			_transform = transform;
			_movers = new List<Mover>();
			_currentTarget = transform.localPosition;
		}
		
		public void Update () {
			if (_movers.Count == 0) {
				return;
			}

			Mover mover = _movers[0];
			mover.Update();

			if (mover.Finished) {
				mover.Destroy();
				_movers.Remove(mover);

				if (_movers.Count == 0) {
					Moving = false;
				}
			} else {
				Moving = true;
			}
		}

		public void MoveByOffset (Vector2 targetOffset, bool debug = false) {
			_currentTarget += targetOffset;

			List<Vector2> offsets = SplitDiagonal(targetOffset);
			Vector2 firstTarget = (Vector2) _transform.localPosition + offsets[0];

			_movers.Add(new Mover(_transform, firstTarget, 1.5f, debug));
			
			if (offsets.Count == 2) {
				Vector2 secondTarget = firstTarget + offsets[1];
				Mover mover = new Mover(_transform, secondTarget, 1.5f, debug);
				mover.ChangeOrigin(firstTarget);
				_movers.Add(mover);
			}
		}

		private static List<Vector2> SplitDiagonal (Vector2 offset) {
			int x0 = (int) offset.x;
			int y0 = (int) offset.y;
			int x = Mathf.Abs(x0);
			int y = Mathf.Abs(y0);

			if (x == y) {
				return new List<Vector2> {offset};
			}
			
			if (x > y) {
				return new List<Vector2> {
					new Vector2(y * Mathf.Sign(x0), y0),
					new Vector2((x - y) * Mathf.Sign(x0), 0)
				};
			}

			return new List<Vector2> {
				new Vector2(x0, x * Mathf.Sign(y0)),
				new Vector2(0, (y - x) * Mathf.Sign(y0))
			};
		}

	}

}