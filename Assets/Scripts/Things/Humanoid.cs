using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Humanoid : Pathfinding, ICreature {

		public HumanoidDef Def { get; private set; }

		private static readonly string[] DirectionSuffix = { "_north", "_south", "_east", "_east" };

		private bool _didInitialize;

		public GameObject Go => gameObject;
		public ThingType Type => Selected ? ThingType.Player : ThingType.Creature;

		public void Initialize () {
			PrepareChild();
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
				ThingDef = Def;
				Heir = this;
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
		}

		[UsedImplicitly]
		private void LateUpdate () {
			if (!Selected || DidDesignateAction) {
				DidDesignateAction = false;
				return;
			}

			if (Input.GetMouseButtonUp(1)) {
				FindPath(Calc.Clamp(CameraController.Main.ScreenToWorldPoint(Input.mousePosition)));
			}
		}

	}

}