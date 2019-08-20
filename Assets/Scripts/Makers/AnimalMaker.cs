using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		[UsedImplicitly, SerializeField] private GameObject _chunkContainer = null;
		[UsedImplicitly, SerializeField] private GameObject _container = null;

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
				_animalPrefab = new GameObject("Animal Prefab", typeof(Animal));
				_animalPrefab.transform.SetParent(_container.transform);
				_animalPrefab.SetActive(false);
			}

			Populate(_animalPrefab);
		}

		private void Populate (GameObject prefab) {
			foreach (Transform c in _chunkContainer.transform) {
				foreach (Transform t in c) {
					if (Random.value < .998f) {
						continue;
					}

					if (!IsValid(t.GetComponent<Tile>().Type)) {
						continue;
					}

					AnimalDef def = DefLoader.GetRandomAnimalDef();
					int x = (int) t.transform.position.x;
					int y = (int) t.transform.position.y;

					if (def.Solitary) {
						Initialize(prefab, def, x, y);
					} else {
						InitializeGroup(prefab, def, x, y);
					}
				}
			}
		}

		private void InitializeGroup (GameObject prefab, AnimalDef def, int x, int y) {
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

		private void Initialize (GameObject prefab, AnimalDef def, int x, int y) {
			Vector3 pos = new Vector3(x, y, Order.ANIMAL);
			GameObject go = Instantiate(prefab, pos, Quaternion.identity, _container.transform);
			go.name = def.DefName;
			Animal animal = Animal.Create(go.GetComponent<Animal>(), def);
			animal.Initialize();
		}

	}

}