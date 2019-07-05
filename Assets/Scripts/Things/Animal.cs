using System;
using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	public class Animal : Pathfinding, ICreature {

		public AnimalDef Def;

		private const ThingType TYPE = Enums.ThingType.Creature;

		private bool _didInitialize;

		public void Initialize (AnimalDef def) {
			InitializePathfinding(def.StatBases.MoveSpeed);

			Def = def;
			Child.localPosition = new Vector2(.5f, .5f);
			Child.localScale = AdjustScale(Def.SpriteScale);
			SetSprite(AssetLoader.Get(TYPE, def.DefName), false);
			SetTint(AdjustTint(Def.Tint));
			IsSelectable = true;
			_didInitialize = true;
		}

		public ThingType ThingType () {
			return TYPE;
		}

		private static Vector3 AdjustScale (float s) {
			return s > 0 ? new Vector3(s, s, 1) : Vector3.one;
		}

		private static Color AdjustTint (IReadOnlyList<float> t) {
			if (t == null) {
				return Color.white;
			}

			switch (t.Count) {
				case 3:
					return new Color(t[0], t[1], t[2], 1);
				case 1:
					return new Color(t[0], t[0], t[0], 1);
				default:
					return Color.white;
			}
		}

		[UsedImplicitly]
		//todo move to manual update
		private void Update () {
			if (_didInitialize == false) {
				return;
			}

			if (DirectionChanged) {
				SetSprite(AssetLoader.Get(TYPE, Def.DefName, Facing), Facing == Direction.West);
				DirectionChanged = false;
			}

			//todo set to random time
			if (Moving || Random.value < .995) {
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