using System;
using Assets.Scripts.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Graphics {

	public static class AssetLoader {

		public static readonly Material DiffuseMat;
		public static readonly Sprite TransitionSide;
		public static readonly Sprite TransitionCorner;

		private static readonly Sprite[][] TileSprites;
		private static readonly string[] TileNames;
		private static readonly int TileTypeCount;

		private static readonly Sprite[][] PlantSprites;
		private static readonly string[] PlantNames;
		private static readonly int PlantTypeCount;

		private static readonly Sprite[][] AnimalSprites;
		private static readonly string[] AnimalNames;
		private static readonly int AnimalTypeCount;

		static AssetLoader () {
			DiffuseMat = Resources.Load<Material>("Materials/Diffuse");
			TransitionSide= Resources.Load<Sprite>("sprites/terrain/decals/TransitionSide");
			TransitionCorner = Resources.Load<Sprite>("sprites/terrain/decals/TransitionCorner");

			TileNames = Enum.GetNames(typeof(TileType));
			TileTypeCount = TileNames.Length;
			TileSprites = new Sprite[TileTypeCount][];
			LoadTiles();

			PlantNames = Enum.GetNames(typeof(PlantType));
			PlantTypeCount = PlantNames.Length;
			PlantSprites = new Sprite[PlantTypeCount][];
			LoadPlants();

			AnimalNames = Enum.GetNames(typeof(AnimalType));
			AnimalTypeCount = AnimalNames.Length;
			AnimalSprites = new Sprite[AnimalTypeCount][];
			LoadAnimals();
		}

		public static Sprite Get (TileType tile, int x, int y) {
			tile = tile == TileType.DeepWater ? TileType.ShallowWater : tile;
			int i = (int) tile;
			int size = (int) Mathf.Sqrt(TileSprites[i].Length);
			int xy = size * (size - 1) - size * (y % size) + x % size;
			return TileSprites[i][xy];
		}

		public static Sprite Get (PlantType plant) {
			int i = (int) plant;
			return PlantSprites[i][Random.Range(0, PlantSprites[i].Length)];
		}

		public static Sprite Get (AnimalType animal, Direction direction) {
			Direction d = direction == Direction.West ? Direction.East : direction;
			return AnimalSprites[(int) animal][(int) d];
		}

		private static void LoadTiles () {
			for (int i = 0; i < TileTypeCount; ++i) {
				string tile = TileNames[i];
				string loc = $"sprites/terrain/surfaces/{tile}";
				TileSprites[i] = Resources.LoadAll<Sprite>(loc);

				foreach (Sprite s in TileSprites[i]) {
					if (s == null) {
						Debug.Log($"Couldn't load {tile} properly in AssetLoader.");
					}
				}
			}
		}

		private static void LoadPlants () {
			for (int i = 0; i < PlantTypeCount; i++) {
				string plant = PlantNames[i];
				string loc = $"sprites/thing/plant/{plant.ToLower()}";
				PlantSprites[i] = Resources.LoadAll<Sprite>(loc);

				if (PlantSprites[i] == null) {
					Debug.Log($"Couldn't load {plant} properly in AssetLoader.");
				}
			}
		}

		private static void LoadAnimals () {
			for (int i = 0; i < AnimalTypeCount; ++i) {
				string animal = AnimalNames[i];
				string loc = $"sprites/thing/creature/{animal.ToLower()}/{animal}_";
				AnimalSprites[i] = new[] {
					Resources.Load<Sprite>(loc + "north"),
					Resources.Load<Sprite>(loc + "south"),
					Resources.Load<Sprite>(loc + "east")
				};

				foreach (Sprite s in AnimalSprites[i]) {
					if (s == null) {
						Debug.Log($"Couldn't load {animal} properly in AssetLoader.");
					}
				}
			}
		}

	}

}