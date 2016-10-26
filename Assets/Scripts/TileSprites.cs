using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class TileSprites : MonoBehaviour {

		public static Sprite Side;
		public static Sprite Corner;

		public static GameObject SideGo;
		public static GameObject CornerGo;

		public static Color CDeepWater;
		public static Color CShallowWater;
		public static Color CSand;
		public static Color CGrass;
		public static Color CDirt;
		public static Color CRock;
		public static Color CSnow;

		public static int[] Order;

		[UsedImplicitly]
		public int[] TransitionOrder = new int[7];

		// Transitions
		#region
		[UsedImplicitly]
		public Sprite TransitionSide;

		[UsedImplicitly]
		public Sprite TransitionCorner;

		[UsedImplicitly]
		public GameObject TransitionSidePrefab;

		[UsedImplicitly]
		public GameObject TransitionCornerPrefab;
		#endregion

		private static Sprite[] _none;
		private static Sprite[] _water;
		private static Sprite[] _sand;
		private static Sprite[] _grass;
		private static Sprite[] _dirt;
		private static Sprite[] _rock;
		private static Sprite[] _snow;

		public static Sprite Get (TileType type, int x, int y) {
			const int size = TileMaker.CSIZE;
			const int zeroPos = size * (size - 1);
			int i = zeroPos - size * (y % size) + x % size;

			switch (type) {
				case TileType.DeepWater:
				case TileType.ShallowWater:
					return _water[i];
				case TileType.Sand:
					return _sand[i];
				case TileType.Grass:
					return _grass[i];
				case TileType.Dirt:
					return _dirt[i];
				case TileType.Rock:
					return _rock[i];
				case TileType.Snow:
					return _snow[i];
				default:
					return null;
			}
		}

		[UsedImplicitly]
		private void OnEnable () {
			Side = TransitionSide;
			Corner = TransitionCorner;
			SideGo = TransitionSidePrefab;
			CornerGo = TransitionCornerPrefab;

			_water = Resources.LoadAll<Sprite>("Tiles/Water");
			_sand = Resources.LoadAll<Sprite>("Tiles/Sand");
			_grass = Resources.LoadAll<Sprite>("Tiles/Grass");
			_dirt = Resources.LoadAll<Sprite>("Tiles/Dirt");
			_rock = Resources.LoadAll<Sprite>("Tiles/Rock");
			_snow = Resources.LoadAll<Sprite>("Tiles/Snow");

			CDeepWater = new Color32(22, 85, 163, 255);
			CShallowWater = new Color32(33, 121, 186, 255);
			CSand = new Color32(176, 162, 141, 255);
			CGrass = new Color32(0, 0, 0, 255);  // todo
			CDirt = new Color32(135, 111, 97, 255);
			CRock = new Color32(0, 0, 0, 255);  // todo
			CSnow = new Color32(188, 188, 197, 255);

			Order = TransitionOrder;

			SmoothTiles.GetStaticAssets();
		}

	}

}