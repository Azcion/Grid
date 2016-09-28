using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	[SuppressMessage ("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage ("ReSharper", "UnassignedField.Global")]
	public class TileSprite : MonoBehaviour {

		public static Sprite None;
		public static Sprite Water;
		public static Sprite Sand;
		public static Sprite Grass;
		public static Sprite Plant;
		public static Sprite Mountain;
		public static Sprite Snow;

		public Sprite NoneSprite;
		public Sprite WaterSprite;
		public Sprite SandSprite;
		public Sprite GrassSprite;
		public Sprite PlantSprite;
		public Sprite MountainSprite;
		public Sprite SnowSprite;

		[UsedImplicitly]
		private void Start () {
			None = NoneSprite;
			Water = WaterSprite;
			Sand = SandSprite;
			Grass = GrassSprite;
			Plant = PlantSprite;
			Mountain = MountainSprite;
			Snow = SnowSprite;
		}

	}

}