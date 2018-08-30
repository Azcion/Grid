using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	public class Human : Pathfinding, ICreature {

		[UsedImplicitly]
		public Sprite[] Sprites;

		private const float SPRITE_OFFSET = -.3f;

		private Direction _facing;
		private bool _didInitialize;

		public void Initialize () {
			InitializePathfinding();

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
			if (_didInitialize == false) {
				return;
			}

			Vector3 v = Tf.localPosition;

			if (Selected) {
				if (Input.GetMouseButtonUp(1)) {
					FindPath(Calc.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
				}

				return;
			}

			if (Moving || Random.value < .985f) {
				return;
			}

			//todo implement smarter targeting
			v += new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
			v = Calc.Clamp(v);

			FindPath(v);
		}

	}

}