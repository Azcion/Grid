using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	[UsedImplicitly]
	public class Tile : MonoBehaviour {

		public GameObject Chunk;
		public TileType Type;

		public int X;
		public int Y;

	}

}