using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {

	public class TileMaker : MonoBehaviour {

		public const int CSIZE = 8;
		public const int YCHUNKS = 25;
		public const int CHALF = CSIZE / 2;
		public const int YTILES = CSIZE * YCHUNKS;
		public const int THALF = YTILES / 2;
		public const int CENTER = YCHUNKS / 2;

		public static int Seed;

		// Object references
		#region
		[UsedImplicitly]
		public GameObject ChunkPrefab;

		[UsedImplicitly]
		public GameObject Container;
		#endregion

		// Materials
		#region
		[UsedImplicitly]
		public Material DeepWaterMat;

		[UsedImplicitly]
		public Material ShallowWaterMat;

		[UsedImplicitly]
		public Material SandMat;

		[UsedImplicitly]
		public Material GrassMat;

		[UsedImplicitly]
		public Material DirtMat;

		[UsedImplicitly]
		public Material RockMat;

		[UsedImplicitly]
		public Material SnowMat;

		[UsedImplicitly]
		public Material DefaultMat;
		#endregion

		private static bool _ready;

		private static List<List<GameObject>> _tiles;

		public static GameObject Get (int x, int y) {
			if (!_ready || x < 0 || x >= YTILES || y < 0 || y >= YTILES) {
				return null;
			}

			return _tiles[y][x];
		}

		[UsedImplicitly]
		private void Start () {
			Seed = (int) (Random.value * 1000000);
			_tiles = new List<List<GameObject>>();

			for (int i = 0; i < YTILES; ++i) {
				_tiles.Add(new List<GameObject>());
			}

			StartCoroutine(Create());
		}

		private IEnumerator Create () {
			yield return new WaitForSeconds(.05f);

			for (int y = 0; y < YCHUNKS; ++y) {
				for (int x = 0; x < YCHUNKS; ++x) {
					Vector3 pos = new Vector3(CSIZE * x, CSIZE * y, ChunkPrefab.transform.position.z);
					GameObject c = Instantiate(ChunkPrefab, pos, Quaternion.identity, Container.transform);
					c.name = "Chunk " + y + "y " + x + "x";

					foreach (Transform t in c.transform) {
						InitializeTile(t);
					}
				}
			}

			_ready = true;
		}

		private void InitializeTile (Transform t) {
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			float v = Noise.Sum(x + Seed, y + Seed, .01f, 6, 2, .5f);
			TileType type;

			if (v > .60) {
				type = TileType.Snow;
			} else if (v > .55) {
				type = TileType.Rock;
			} else if (v > .53) {
				type = TileType.Dirt;
			} else if (v > .48) {
				type = TileType.Grass;
			} else if (v > .35) {
				type = TileType.Sand;
			} else if (v > .30) {
				type = TileType.Grass;
			} else if (v > .25) {
				type = TileType.ShallowWater;
			} else {
				type = TileType.DeepWater;
			}

			Material mat;
			Color color;

			switch (type) {
				case TileType.DeepWater:
					mat = DeepWaterMat;
					color = TileSprites.CDeepWater;
					break;
				case TileType.ShallowWater:
					mat = ShallowWaterMat;
					color = TileSprites.CShallowWater;
					break;
				case TileType.Sand:
					mat = SandMat;
					color = TileSprites.CSand;
					break;
				case TileType.Grass:
					mat = GrassMat;
					color = TileSprites.CGrass;
					break;
				case TileType.Dirt:
					mat = DirtMat;
					color = TileSprites.CDirt;
					break;
				case TileType.Rock:
					mat = RockMat;
					color = TileSprites.CRock;
					break;
				case TileType.Snow:
					mat = SnowMat;
					color = TileSprites.CSnow;
					break;
				default:
					mat = GrassMat;
					color = TileSprites.CGrass;
					break;
			}

			Tile tile = t.GetComponent<Tile>();
			tile.Chunk = t.parent.gameObject;
			tile.Type = type;

			/*List<Vector2> uvs = new List<Vector2> {
				new Vector2(x * .125f, y * .125f),
				new Vector2((x + 1) * .125f, (y + 1) * .125f),
				new Vector2((x + 1) * .125f, y * .125f),
				new Vector2(x * .125f, (y + 1) * .125f)
				};

			t.GetComponent<MeshFilter>().mesh.SetUVs(0, uvs);

			MeshRenderer mr = t.GetComponent<MeshRenderer>();
			mr.material = mat;*/

			SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
			sr.material = mat;
			sr.sprite = TileSprites.Get(type, x, y);

			SmoothTiles st = t.GetComponent<SmoothTiles>();
			st.OverlapOrder = TileSprites.Order[(int) type];

			if (type == TileType.Rock || type == TileType.Grass) {
				st.CanTransition = false;
			} else {
				st.Color = color;
			}

			_tiles[y].Add(t.gameObject);
		}

	}

}