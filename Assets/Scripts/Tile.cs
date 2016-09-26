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

		public Tile (Chunk chunk, byte id, TileType type) {
			Chunk = chunk;
			Type = type;
			LocalID = id;

			switch (type) {
				case TileType.Water:
					Image = TileSprite.WATER;
					break;
				case TileType.Sand:
					Image = TileSprite.SAND;
					break;
				case TileType.Grass:
					Image = TileSprite.GRASS;
					break;
				case TileType.Plant:
					Image = TileSprite.PLANT;
					break;
				case TileType.Mountain:
					Image = TileSprite.MOUNTAIN;
					break;
				case TileType.Snow:
					Image = TileSprite.SNOW;
					break;
				default:
					Image = TileSprite.NONE;
					break;
			}

			if (Image == null) {
				Image = new Sprite();
			}
		}

	}

}