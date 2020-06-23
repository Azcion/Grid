using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	public class Animal : Pathfinding, ICreature {

		public AnimalDef Def { get; private set; }

		private static readonly string[] DirectionSuffix = { "_north", "_south", "_east", "_east" };

		private bool _didInitialize;

		public GameObject Go => gameObject;
		public ThingType Type => ThingType.Creature;

		public static Animal Create (Animal animal, AnimalDef def) {
			animal.Def = def;
			animal.ThingDef = def;
			animal.Heir = animal;

			return animal;
		}

		public void Initialize () {
			PrepareChild();
			InitializePathfinding(Def.StatBases.MoveSpeed);

			Child.localPosition = new Vector2(.5f, .5f);
			Child.localScale = AdjustScale(Def.SpriteScale);
			string suffix = DirectionSuffix[(int) Facing];
			SetSprite(Assets.GetSprite(Def.DefName + suffix), false);
			SetTint(AdjustTint(Def.Tint));
			IsSelectable = true;
			_didInitialize = true;
			gameObject.SetActive(true);
		}

		private static Color AdjustTint (Color t) {
			return t == Color.clear ? Color.white : new Color(t.r, t.g, t.b, 1);
		}

		private static Vector3 AdjustScale (float s) {
			return s > 0 ? new Vector3(s, s, 1) : Vector3.one;
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

			//todo set to random time
			if (Moving || Random.value < .995) {
				return;
			}

			//todo implement smarter targeting
			Vector3 v = transform.localPosition;
			v += new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), 0);
			v = Calc.Clamp(v);

			FindPath(v);
		}

	}

}