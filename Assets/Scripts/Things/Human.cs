using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	public class Human : Pathfinding {

		[UsedImplicitly]
		public Sprite[] HumanSprites;

		private const float SPRITE_OFFSET = -.3f;

		private Direction _facing;
		private bool _wasAssigned;

		public void Initialize () {
			InitializePathfinding();

			Sprite.localPosition = new Vector2(.5f, Calc.Round(.5f + SPRITE_OFFSET, 2));
			_wasAssigned = true;
		}

		[UsedImplicitly]
		private void OnEnable () {
			if (_wasAssigned == false) {
				Initialize();
			}
		}

		[UsedImplicitly]
		//todo move to manual update
		private void Update () {
			if (_wasAssigned == false) {
				return;
			}

			if (Moving || Random.value < .985f) {
				return;
			}

			//todo implement smarter targeting
			Vector2 v = (Vector2) Tf.localPosition + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
			v = Calc.Clamp(v, 0, TileMaker.YTILES, 0, TileMaker.YTILES);

			FindPath(v);
		}

	}

}