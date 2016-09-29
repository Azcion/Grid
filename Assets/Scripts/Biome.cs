using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts {

	public class Biome {

		public readonly BiomeType Type;
		public readonly Gradient Coloring;

		public readonly ChunkAnchors ChunkAnchors;
		public readonly ChunkSystem ChunkSystem;

		public float LevelWater;
		public float LevelSand;
		public float LevelGrass;
		public float LevelPlant;
		public float LevelMountain;
		public float LevelSnow;

		public Biome (GameObject parent, BiomeType biome, float[,] values, int yChunks) {
			Type = biome;
			Coloring = BiomeGradient.BIOMES[(int) biome];

			GetLevels();

			ChunkAnchors = new ChunkAnchors(parent, yChunks);
			ChunkSystem = new ChunkSystem(this, values, yChunks, yChunks);
		}

		private void GetLevels () {
			RGBT[] g = BiomeGradient.BIOMES_INIT[(int) Type];

			switch (Type) {
				case BiomeType.Archipelago:
					LevelWater = (g[2].T + g[3].T) / 2f;
					LevelSand = (g[3].T + g[4].T) / 2f;
					LevelGrass = (g[4].T + g[5].T) / 2f;
					LevelPlant = (g[6].T + g[7].T) / 2f;
					LevelMountain = g[7].T;
					LevelSnow = 1f;
					break;
				case BiomeType.Desert:
					LevelWater = (g[1].T + g[2].T) / 2f;
					LevelSand = (g[2].T + g[3].T) / 2f;
					LevelGrass = (g[2].T + g[3].T) / 2f;
					LevelPlant = (g[3].T + g[4].T) / 2f;
					LevelMountain = g[6].T;
					LevelSnow = 1f;
					break;
				case BiomeType.Highlands:
					LevelWater = (g[1].T + g[2].T) / 2f;
					LevelSand = (g[2].T + g[3].T) / 2f;
					LevelGrass = (g[4].T + g[5].T) / 2f;
					LevelPlant = (g[5].T + g[6].T) / 2f;
					LevelMountain = g[6].T;
					LevelSnow = (g[6].T + g[7].T) / 2f;
					break;
			}
		}

	}

}