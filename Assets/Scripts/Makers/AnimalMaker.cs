using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class AnimalMaker : MonoBehaviour {

		private static GameObject _animalPrefab;
		private static readonly ulong ValidTilesMask;

		private static readonly List<TileType> ValidTiles = new List<TileType> {
			TileType.Mossy, TileType.Sand, TileType.Soil, TileType.SoilRich, TileType.Gravel, TileType.PackedDirt, TileType.Ice
		};

		static AnimalMaker () {
			ValidTilesMask = 0;

			foreach (TileType type in ValidTiles) {
				ulong mask = (ulong) (1 << (int) type);
				ValidTilesMask |= mask;
			}
		}

		private static bool IsValid (TileType type) {
			return ((ulong) (1 << (int) type) & ValidTilesMask) > 0;
		}

		[UsedImplicitly]
		private void Start () {
			if (!DefLoader.DidLoad) {
				Debug.Log("Defs not loaded, AnimalMaker can't run.");
				return;
			}

			if (_animalPrefab == null) {
				_animalPrefab = new GameObject("Animal Prefab", typeof(Animal), typeof(BoxCollider2D));
				_animalPrefab.SetActive(false);
				_animalPrefab.GetComponent<Animal>().Prepare();
				_animalPrefab.transform.SetParent(transform);
				BoxCollider2D bc = _animalPrefab.GetComponent<BoxCollider2D>();
				bc.isTrigger = true;
				bc.offset = new Vector2(.5f, .5f);
				bc.size = Vector2.one;
			}

			Populate(_animalPrefab);
		}

		private void Populate (GameObject prefab) {
			for (int y = 0; y < Map.YTiles; y++) {
				for (int x = 0; x < Map.YTiles; x++) {
					
					if (Random.value < .998f) {
						continue;
					}

					if (!IsValid(TileMaker.GetTile(x, y).Type)) {
						continue;
					}

					Def def = DefLoader.GetRandomAnimalDef();

					if (def.Solitary) {
						Initialize(prefab, def, x, y);
					} else {
						InitializeGroup(prefab, def, x, y);
					}
				}
			}
		}

		private void InitializeGroup (GameObject prefab, Def def, int x, int y) {
			int groupSize = Random.Range(1, 4);
			float rv = Random.value;
			groupSize += rv > .5f ? 1 : 0;
			groupSize += rv > .8f ? 1 : 0;
			Vector2Int v0 = Calc.Clamp(new Vector2Int(x - 5, y - 5));
			Vector2Int v1 = Calc.Clamp(new Vector2Int(x + 6, y + 6));
			List<Vector2Int> usedPoints = new List<Vector2Int>();
			

			for (int i = 0; i < groupSize; i++) {
				Vector2Int v = new Vector2Int(Random.Range(v0.x, v1.x), Random.Range(v0.y, v1.y));
				
				if (usedPoints.Contains(v)) {
					continue;
				}

				usedPoints.Add(v);

				if (IsValid(TileMaker.GetTile(v.x, v.y).Type)) {
					Initialize(prefab, def, v.x, v.y);
				}
			}
		}

		private void Initialize (GameObject prefab, Def def, int x, int y) {
			Vector3 pos = new Vector3(x, y, Order.ANIMAL);
			GameObject go = Instantiate(prefab, pos, Quaternion.identity, transform);
			go.name = def.DefName;
			Animal animal = Animal.Create(go.GetComponent<Animal>(), def);
			animal.Initialize();
		}

	}

}