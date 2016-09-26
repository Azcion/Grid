using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

	public static class BiomeGradient {

		public static readonly List<Gradient> BIOMES = new List<Gradient>();

		public static readonly RGBT[][] BIOMES_INIT = {
			new [] {  // ARCHIPELAGO
				new RGBT(  0,  60, 143, .320f),  // 0 deep water
				new RGBT(  2, 108, 255, .462f),  // 1 shallow water
				new RGBT(184, 252, 255, .518f),  // 2 waves
				new RGBT(184, 161,  85, .532f),  // 3 shore
				new RGBT(134, 169,  66, .566f),  // 4 grass
				new RGBT( 74, 120,  23, .605f),  // 5 plants
				new RGBT( 59,  96,  18, .704f),  // 6 dark plants
				new RGBT( 94, 109,  90, .783f)   // 7 mountain
			},
			new[] {  // DESERT
				new RGBT( 21, 101, 180, .000f),  // 0 deep water
				new RGBT( 63, 135,  84, .177f),  // 1 shore
				new RGBT(154, 110,  44, .200f),  // 2 beach
				new RGBT( 26,  70,  26, .210f),  // 3 plants
				new RGBT(208, 173,  83, .267f),  // 4 sand
				new RGBT(215, 170,  80, .544f),  // 5 sand
				new RGBT( 49,  38,  12, .657f),  // 6 mountain
				new RGBT(208, 173,  83, .885f)	 // 7 mountaintop
			},
			new[] {  // HIGHLANDS
				new RGBT( 21, 101, 180, .172f),  // 0 deep water
				new RGBT( 47, 111,  95, .246f),  // 1 shallow water
				new RGBT(154, 110,  44, .262f),  // 2 shore
				new RGBT( 60,  94,  54, .314f),  // 3 dark grass
				new RGBT( 68,  94,  39, .361f),  // 4 grass
				new RGBT( 33,  88,  33, .497f),  // 5 plants
				new RGBT(109, 116, 102, .609f),  // 6 mountain
				new RGBT(255, 255, 255, .754f)   // 7 snow
			}
		};

		private static readonly GradientAlphaKey[] ALPHA = {
			new GradientAlphaKey(1, 0),
			new GradientAlphaKey(1, 1)
		};

		static BiomeGradient () {
			foreach (RGBT[] biome in BIOMES_INIT) {
				GradientColorKey[] keys = new GradientColorKey[biome.Length];

				for (int i = 0; i < keys.Length; ++i) {
					RGBT c = biome[i];
					keys[i] = new GradientColorKey(new Color(c.R, c.G, c.B), c.T);
				}

				Gradient gradient = new Gradient();
				gradient.SetKeys(keys, ALPHA);

				BIOMES.Add(gradient);
			}
		}

	}

}