using System.Diagnostics.CodeAnalysis;

namespace Assets.Scripts.Enums {

	[SuppressMessage ("ReSharper", "UnusedMember.Global")]
	public enum TileType : byte {

		None,
		Water,
		Sand,
		Grass,
		Plant,
		Mountain,
		Snow

	}

}