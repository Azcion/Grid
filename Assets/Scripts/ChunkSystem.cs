using System.Collections.Generic;
using Assets.Scripts.Enums;

namespace Assets.Scripts {

	public class ChunkSystem {

		public readonly Biome Biome;

		public readonly int XChunks;
		public readonly int YChunks;

		public readonly int XTiles;
		public readonly int YTiles;

		public readonly List<List<Chunk>> Chunks;

		private readonly float[,] _values;

		public ChunkSystem (Biome biome, float[,] values, int xChunks, int yChunks) {
			Biome = biome;

			XChunks = xChunks;
			YChunks = yChunks;

			XTiles = XChunks * Chunk.SIZE;
			YTiles = YChunks * Chunk.SIZE;

			Chunk.YChunks = yChunks;
			Chunks = new List<List<Chunk>>();

			_values = values;

			FillChunks();
			Chunk.RemovePrefab();
		}

		private void FillChunks () {
			for (int y = 0; y < YChunks; ++y) {
				List<Chunk> row = new List<Chunk>();

				for (int x = 0; x < XChunks; ++x) {
					row.Add(new Chunk(YTiles, ChunkAnchors.Anchors[y, x], MakeChunk(x, y), x, y));
				}

				Chunks.Add(row);
			}
		}

		private TileType[] MakeChunk (int xPos, int yPos) {
			TileType[] tiles = new TileType[Chunk.SIZE * Chunk.SIZE];

			for (int y = 0; y < Chunk.SIZE; ++y) {
				for (int x = 0; x < Chunk.SIZE; ++x) {
					float value = _values[yPos * Chunk.SIZE + y, xPos * Chunk.SIZE + x];
					tiles[y * Chunk.SIZE + x] = MakeTile(value);
				}
			}

			return tiles;
		}

		private TileType MakeTile (float value) {
			if (value <= Biome.LevelWater) {
				return TileType.Water;
			}

			if (value <= Biome.LevelSand) {
				return TileType.Sand;
			}

			if (value <= Biome.LevelGrass) {
				return TileType.Grass;
			}

			if (value <= Biome.LevelPlant) {
				return TileType.Plant;
			}

			// Deserts have Sand between Plant and Mountain
			if (Biome.Type == BiomeType.Desert) {
				return value >= Biome.LevelMountain ? TileType.Mountain : TileType.Sand;
			}

			// Archipelagos only have Mountain above Plant
			if (Biome.Type == BiomeType.Archipelago) {
				return TileType.Mountain;
			}

			if (value <= Biome.LevelMountain) {
				return TileType.Mountain;
			}

			// Highlands only have Snow above Mountain
			if (Biome.Type == BiomeType.Highlands) {
				return TileType.Snow;
			}

			if (value > Biome.LevelSnow) {
				return TileType.Snow;
			}

			return TileType.None;
		}

	}

}