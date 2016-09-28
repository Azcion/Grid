using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts {

	[Serializable]
	public class Tile {

		public readonly Chunk Chunk;

		public readonly Sprite Image;
		public readonly TileType Type;
		public readonly byte LocalID;
		public readonly string Name;

		private static readonly string[] TILE_NAME = {
			Enum.GetName(typeof(TileType), TileType.None),
			Enum.GetName(typeof(TileType), TileType.Water),
			Enum.GetName(typeof(TileType), TileType.Sand),
			Enum.GetName(typeof(TileType), TileType.Grass),
			Enum.GetName(typeof(TileType), TileType.Plant),
			Enum.GetName(typeof(TileType), TileType.Mountain),
			Enum.GetName(typeof(TileType), TileType.Snow)
		};

		public Tile (Chunk chunk, byte id, TileType type) {
			Chunk = chunk;
			Type = type;
			LocalID = id;
			Name = TILE_NAME[(int) type];

			switch (type) {
				case TileType.Water:
					Image = TileSprite.Water;
					break;
				case TileType.Sand:
					Image = TileSprite.Sand;
					break;
				case TileType.Grass:
					Image = TileSprite.Grass;
					break;
				case TileType.Plant:
					Image = TileSprite.Plant;
					break;
				case TileType.Mountain:
					Image = TileSprite.Mountain;
					break;
				case TileType.Snow:
					Image = TileSprite.Snow;
					break;
				default:
					Image = TileSprite.None;
					break;
			}

			if (Image == null) {
				Image = new Sprite();
			}
		}

	}

}