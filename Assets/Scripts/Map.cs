namespace Assets.Scripts {

	public static class Map {

		public const int CSIZE = 8;

		public static int YChunks;
		public static int YTiles;
		public static int YHalf;
		public static int Center;

		public static void InitializeMapMeasurements (int yChunks) {
			YChunks = yChunks;
			YTiles = CSIZE * yChunks;
			YHalf = YTiles / 2;
			Center = yChunks / 2;
		}

	}

}