using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	[UsedImplicitly]
	public class Tile : MonoBehaviour {

		public GameObject Chunk;
		public int X;
		public int Y;
		public TileType Type;
		public bool Walkable;
		public bool Buildable;
		public int Penalty;

		public void Assign (GameObject chunk, int x, int y,
			TileType type, bool walkable, bool buildable, int penalty) {
			Chunk = chunk;
			X = x;
			Y = y;
			Type = type;
			Walkable = walkable;
			Buildable = buildable;
			Penalty = penalty;
		}

	}

}