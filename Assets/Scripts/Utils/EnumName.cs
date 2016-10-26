using System;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Utils {

	public static class EnumName {

		private static readonly string[] TILE_TYPE;

		static EnumName () {
			TILE_TYPE = Enum.GetNames(typeof(TileType));
		}

		public static string Get (TileType e) {
			return TILE_TYPE[(int) e];
		}

	}

}