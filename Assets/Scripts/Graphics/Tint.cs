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
		}

		public static void Initialize () {
			Tiles[TileType.DeepWater] = new Color32(100, 112, 145, 255);
			Tiles[TileType.ShallowWater] = new Color32(120, 143, 155, 255);
			Tiles[TileType.RoughStone] = new Color32(143, 148, 142, 255);
			Tiles[TileType.WoodFloor] = new Color32(189, 126, 75, 255);

			Bases[LinkedType.Rock] = new Color32(132, 132, 132, 255);
			Bases[LinkedType.Planks] = new Color32(109, 109, 109, 255);

			Materials[ThingMaterial.Rock] = new Color32(143, 148, 142, 255);
			Materials[ThingMaterial.Wood] = new Color32(189, 126, 75, 255);
		}

		public static Color Get (TileType type) {
			switch (type) {
				case TileType.RoughStone:
				case TileType.RoughHewnRock:
				case TileType.SmoothStone:
					return Tiles[TileType.RoughStone];
				default:
					return Tiles[type];
			}
		}

		public static Color Get (LinkedType type) {
			return Bases[type];
		}

		public static Color Get (ThingMaterial type) {
			return Materials[type];
		}

	}

}