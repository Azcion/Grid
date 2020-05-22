using System;

namespace Assets.Scripts.Enums {

	public static class Name {

		public static readonly string[] Direction = Enum.GetNames(typeof(Direction));
		public static readonly string[] LinkedType = Enum.GetNames(typeof(LinkedType));
		public static readonly string[] PathfinderMode = Enum.GetNames(typeof(PathfinderMode));
		public static readonly string[] PlantSize = Enum.GetNames(typeof(PlantSize));
		public static readonly string[] TerrainType = Enum.GetNames(typeof(TerrainType));
		public static readonly string[] ThingType = Enum.GetNames(typeof(ThingType));
		public static readonly string[] TileType = Enum.GetNames(typeof(TileType));

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

		public static string Get (ThingType e) {
			return ThingType[(int) e];
		}

		public static string Get (TileType e) {
			return TileType[(int) e];
		}
		
	}

}