using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public static class Tint {

		private static readonly Dictionary<TileType, Color> Tiles;
		private static readonly Dictionary<LinkedType, Color> Bases;
		private static readonly Dictionary<ThingMaterial, Color> Materials;

		static Tint () {
			Tiles = new Dictionary<TileType, Color>();
			Bases = new Dictionary<LinkedType, Color>();
			Materials = new Dictionary<ThingMaterial, Color>();

			for (int i = 0; i < Name.TileType.Length; ++i) {
				Tiles.Add((TileType) i, Color.white);
			}

			for (int i = 0; i < Name.LinkedType.Length; ++i) {
				Bases.Add((LinkedType) i, Color.white);
			}

			for (int i = 0; i < Name.ThingMaterial.Length; ++i) {
				Materials.Add((ThingMaterial) i, Color.white);
			}

			Initialize();
		}

		public static Color Get (TileType type) {
			return Tiles[type];
		}

		public static Color Get (LinkedType type) {
			return Bases[type];
		}

		public static Color Get (ThingMaterial type) {
			return Materials[type];
		}

		private static void Initialize () {
			Tiles[TileType.DeepWater] = new Color32(100, 112, 145, 255);
			Tiles[TileType.ShallowWater] = new Color32(120, 143, 155, 255);
			Tiles[TileType.RoughStone] = new Color32(143, 148, 142, 255);
			Tiles[TileType.RoughGranite] = new Color32(106, 93, 97, 255);
			Tiles[TileType.RoughLimestone] = new Color32(160, 153, 131, 255);
			Tiles[TileType.RoughMarble] = new Color32(129, 135, 129, 255);
			Tiles[TileType.RoughSandstone] = new Color32(129, 102, 91, 255);
			Tiles[TileType.RoughHewnRock] = new Color32(143, 148, 142, 255);
			Tiles[TileType.RoughHewnGranite] = new Color32(106, 93, 97, 255);
			Tiles[TileType.RoughHewnLimestone] = new Color32(160, 153, 131, 255);
			Tiles[TileType.RoughHewnMarble] = new Color32(129, 135, 129, 255);
			Tiles[TileType.RoughHewnSandstone] = new Color32(129, 102, 91, 255);
			Tiles[TileType.SmoothStone] = new Color32(143, 148, 142, 255);
			Tiles[TileType.SmoothGranite] = new Color32(106, 93, 97, 255);
			Tiles[TileType.SmoothLimestone] = new Color32(160, 153, 131, 255);
			Tiles[TileType.SmoothMarble] = new Color32(129, 135, 129, 255);
			Tiles[TileType.SmoothSandstone] = new Color32(129, 102, 91, 255);
			Tiles[TileType.WoodFloor] = new Color32(189, 126, 75, 255);

			Bases[LinkedType.Rock] = new Color32(132, 132, 132, 255);
			Bases[LinkedType.Bricks] = new Color32(102, 102, 102, 255);
			Bases[LinkedType.Planks] = new Color32(109, 109, 109, 255);
			Bases[LinkedType.Smooth] = new Color32(127, 127, 127, 255);
			Bases[LinkedType.SmoothRock] = new Color32(127, 127, 127, 255);
			Bases[LinkedType.Blueprint] = new Color32(255, 255, 255, 255);

			Materials[ThingMaterial.Granite] = new Color32(106, 93, 97, 255);
			Materials[ThingMaterial.Limestone] = new Color32(160, 153, 131, 255);
			Materials[ThingMaterial.Marble] = new Color32(129, 135, 129, 255);
			Materials[ThingMaterial.Sandstone] = new Color32(129, 102, 91, 255);
			Materials[ThingMaterial.Wood] = new Color32(189, 126, 75, 255);
		}

	}

}