using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Humanoid : Pathfinding, ICreature {

		public HumanoidDef Def { get; private set; }

		private static readonly string[] DirectionSuffix = { "_north", "_south", "_east", "_east" };

		private bool _didInitialize;

		public GameObject Go => gameObject;
		public ThingType Type => ThingType.Creature;

		public void Initialize () {
			InitializePathfinding(Def.StatBases.MoveSpeed);

			Child.localPosition = new Vector2(.5f, .5f);
			string suffix = DirectionSuffix[(int) Facing];
			SetSprite(Assets.GetSprite(Def.DefName + suffix), false);
			IsSelectable = true;
			_didInitialize = true;
		}

		[UsedImplicitly]
		// Initial testing
		private void OnEnable () {
			if (_didInitialize == false) {
				transform.position = new Vector3(transform.position.x, transform.position.y, Order.ANIMAL);
				Def = DefLoader.GetRandomHumanoidDef();
				Initialize();
			}
		}

		[UsedImplicitly]
		//todo move to manual update
		private void Update () {
			if (_didInitialize == false) {
				return;
			}

			if (DirectionChanged) {
				string suffix = DirectionSuffix[(int) Facing];
				SetSprite(Assets.GetSprite(Def.DefName + suffix), Facing == Direction.West);
				DirectionChanged = false;
			}

			if (Selected) {
				if (Input.GetMouseButtonUp(1)) {
					FindPath(Calc.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
				}

				return;
			}

			return;

			/*if (Moving || Random.value < .985f) {
				return;
			}

			//todo implement smarter targeting
			Vector3 v = Tf.localPosition;
			v += new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
			v = Calc.Clamp(v);

			FindPath(v);*/
		}

	}

}