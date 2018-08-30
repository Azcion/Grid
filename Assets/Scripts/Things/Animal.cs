using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Animal : Pathfinding, ICreature {

		[UsedImplicitly]
		public Sprite[] Sprites;

		private const float SPRITE_OFFSET = 0;

		private Direction _facing;
		private bool _didInitialize;

		public void Initialize () {
			InitializePathfinding(.5f);

			Sprite.localPosition = new Vector2(.5f, Calc.Round(.5f + SPRITE_OFFSET, 2));
			_didInitialize = true;
		}

		public ThingType ThingType () {
			return Enums.ThingType.Creature;
		}

		[UsedImplicitly]
		private void OnEnable () {
			if (_didInitialize == false) {
				Initialize();
			}
		}

		[UsedImplicitly]
		//todo move to manual update
		private void Update () {
			if (_didInitialize == false || Moving || Random.value < .995) {
				return;
			}

			//todo implement smarter targeting
			Vector3 v = Tf.localPosition;
			v += new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
			v = Calc.Clamp(v);

			FindPath(v);
		}

	}

}