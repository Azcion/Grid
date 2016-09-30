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
		public readonly List<Tile> Tiles;

		private static readonly GameObject TILE_PREFAB;

		private readonly TileType[] _tileTypes;

		static Chunk () {
			const float opacity = .05f;

			TILE_PREFAB = new GameObject();
			TILE_PREFAB.AddComponent<SpriteRenderer>().color = new Color(1, 1, 1, opacity);
			TILE_PREFAB.AddComponent<TileID>();

			BoxCollider2D box = TILE_PREFAB.AddComponent<BoxCollider2D>();
			box.offset = new Vector2(.5f, .5f);
			box.size = new Vector2(1, 1);
		}

		public Chunk (int yTiles, GameObject anchor, TileType[] tiles, int posX, int posY) {
			const float magic = 31.2515f;  // todo: 31.2515 appears to be the height of the whole map
			float magicScale = magic / yTiles * .32f;
			TILE_PREFAB.transform.localScale = new Vector2(magicScale, magicScale);

			X = posX;
			Y = posY;

			Anchor = anchor;

			GlobalID = posY * YChunks + posX;
			Tiles = new List<Tile>();

			_tileTypes = tiles;

			MakeTiles();
			InstantiateTiles();
		}

		public static void RemovePrefab () {
			TILE_PREFAB.SetActive(false);
		}

		private void MakeTiles () {
			for (int i = 0; i < SIZE * SIZE; ++i) {
				Tile tile = new Tile(this, (byte) i, _tileTypes[i]);
				Tiles.Add(tile);
			}
		}

		// todo determine @magic value
		private void InstantiateTiles () {
			float position = 10f / YChunks / SIZE;

			for (int y = 0; y < SIZE; ++y) {
				for (int x = 0; x < SIZE; ++x) {
					GameObject tile = Object.Instantiate(TILE_PREFAB);
					tile.name = "Tile " + y + " " + x;
					tile.transform.SetParent(Anchor.transform);
					tile.transform.localPosition = new Vector3(y * position, x * position);
					Tile t = Tiles[y * SIZE + x];
					tile.GetComponent<SpriteRenderer>().sprite = t.Image;
					tile.GetComponent<TileID>().Tile = t;
				}
			}
		}

	}

}