using System;

namespace Assets.Scripts.Enums {

	public static class Name {

		public static readonly string[] Action = Enum.GetNames(typeof(Action));
		public static readonly string[] ArchitectCategory = Enum.GetNames(typeof(ArchitectCategory));
		public static readonly string[] Direction = Enum.GetNames(typeof(Direction));
		public static readonly string[] LinkedType = Enum.GetNames(typeof(LinkedType));
		public static readonly string[] PathfinderMode = Enum.GetNames(typeof(PathfinderMode));
		public static readonly string[] PlantSize = Enum.GetNames(typeof(PlantSize));
		public static readonly string[] TerrainType = Enum.GetNames(typeof(TerrainType));
		public static readonly string[] ThingMaterial = Enum.GetNames(typeof(ThingMaterial));
		public static readonly string[] ThingType = Enum.GetNames(typeof(ThingType));
		public static readonly string[] TileType = Enum.GetNames(typeof(TileType));
		public static readonly string[] WeatherType = Enum.GetNames(typeof(WeatherType));

		public static int StringToTileType (string s) {
			for (int i = 0; i < TileType.Length; ++i) {
				if (TileType[i] == s) {
					return i;
				}
			}

			return -1;
		}

		public static string Get (Action e) {
			return Action[(int) e];
		}

		public static string Get (ArchitectCategory e) {
			return ArchitectCategory[(int) e];
		}

		public static string Get (Direction e) {
			return Direction[(int) e];
		}

		public static string Get (LinkedType e) {
			return LinkedType[(int) e];
		}

		public static string Get (PathfinderMode e) {
			return PathfinderMode[(int) e];
		}

		public static string Get (PlantSize e) {
			return PlantSize[(int) e];
		}

		public static string Get (TerrainType e) {
			return TerrainType[(int) e];
		}

		public static string Get (ThingMaterial e) {
			return ThingMaterial[(int) e];
		}

		public static string Get (ThingType e) {
			return ThingType[(int) e];
		}

		public static string Get (TileType e) {
			return TileType[(int) e];
		}

		public static string Get (WeatherType e) {
			return WeatherType[(int) e];
		}
		
	}

}