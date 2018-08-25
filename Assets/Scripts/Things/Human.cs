using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Human : Thing {

		[UsedImplicitly]
		public Sprite[] HumanSprites;

		private Sprite _humanSprite;
		private Direction _facing;
		private Pathfinder _pathfinder;
		private bool _wasAssigned;

		public void Assign () {
			AssertActive();
			_wasAssigned = true;

			_pathfinder = new Pathfinder(transform);
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

			if (_pathfinder.Moving == false && Random.value > .98) {
				_pathfinder.MoveByOffset(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)), true);
			}

			_pathfinder.Update();
		}

	}

}