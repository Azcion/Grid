using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Human : Thing {

		private const float SPRITE_OFFSET = -.3f;

		[UsedImplicitly]
		public Sprite[] HumanSprites;

		private Direction _facing;
		private Pathfinder _pathfinder;
		private bool _wasAssigned;

		public void Assign () {
			AssertActive();

			Sprite.localPosition = new Vector2(.5f, .5f + SPRITE_OFFSET);
			_pathfinder = new Pathfinder(transform);
			_wasAssigned = true;
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