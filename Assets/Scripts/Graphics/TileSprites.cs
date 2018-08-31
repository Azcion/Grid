using Assets.Scripts.Enums;
using Assets.Scripts.Makers;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class TileSprites : MonoBehaviour {

		public static int[] Order;

		private static readonly int[] TransitionOrder = {4, 3, 2, 1, 0};

		#region Object references
		[UsedImplicitly] public GameObject TransitionSidePrefab;
		[UsedImplicitly] public GameObject TransitionCornerPrefab;
		#endregion

		private static Sprite[] _water;
		private static Sprite[] _sand;
		private static Sprite[] _grass;
		private static Sprite[] _soil;
		private static Sprite[] _roughStone;
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
				case TileType.Soil:
					return _soil[i];
				case TileType.RoughStone:
					return _roughStone[i];
				default:
					return null;
			}
		}

		[UsedImplicitly]
		private void OnEnable () {
			_water = Resources.LoadAll<Sprite>("Tiles/Water");
			_sand = Resources.LoadAll<Sprite>("sprites/terrain/surfaces/Sand");
			_soil = Resources.LoadAll<Sprite>("sprites/terrain/surfaces/Soil");
			_roughStone = Resources.LoadAll<Sprite>("sprites/terrain/surfaces/RoughStone");

			Order = TransitionOrder;

			SmoothTiles.GetStaticAssets(TransitionSidePrefab, TransitionCornerPrefab);
		}

	}

}