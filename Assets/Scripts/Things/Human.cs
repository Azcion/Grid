using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Human : Thing {

		[UsedImplicitly]
		public Sprite[] HumanSprites;

		private Sprite _humanSprite;
		private Direction _facing;

	}

}