using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts {

	public class Chunk {

		public const int SIZE = 8;

		public static int YChunks = 32;

		public readonly int X;
		public readonly int Y;

		public readonly GameObject Anchor;

		public readonly int GlobalID;
		public readonly Vector2 FirstTilePosition;
		public readonly List<Tile> Tiles;

		private readonly ChunkSystem _system;
		private readonly TileType[] _tileTypes;

		public Chunk (ChunkSystem system, GameObject anchor, TileType[] tiles, int posX, int posY) {
			X = posX;
			Y = posY;

			Anchor = anchor;

			GlobalID = posY * YChunks + posX;
			FirstTilePosition = new Vector2(posX * SIZE, posY * SIZE);
			Tiles = new List<Tile>();

			_system = system;
			_tileTypes = tiles;

			MakeTiles();
			InstantiateTiles();
		}

		public Tile TileAt (int x, int y) {
			return Tiles[y * SIZE + x];
		}

		public Vector2 LocationOf (Tile tile) {
			int x = tile.LocalID % SIZE;
			int y = tile.LocalID / SIZE;

			return new Vector2(x, y);
		}

		private void MakeTiles () {
			for (int i = 0; i < SIZE * SIZE; ++i) {
				Tile tile = new Tile(this, (byte) i, _tileTypes[i]);
				Tiles.Add(tile);
			}
		}

		// todo determine @magic value
		private void InstantiateTiles () {
			const float opacity = .1f;
			float position = 10f / YChunks / SIZE;
			float magicScale = 31.2515f / _system.YTiles;  // todo: 31.2515 appears to be the height of the whole map
			Vector3 scale = new Vector3(magicScale, magicScale);
			
			for (int y = 0; y < SIZE; ++y) {
				for (int x = 0; x < SIZE; ++x) {
					GameObject tile = new GameObject("Tile " + y + " " + x);
					tile.transform.SetParent(Anchor.transform);
					tile.SetActive(true);
					tile.transform.localPosition = new Vector3(y * position, x * position);
					tile.transform.localScale = scale;
					SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
					sr.sprite = Tiles[y * SIZE + x].Image;
					sr.color = new Color(1, 1, 1, opacity);
				}
			}
		}

	}

}