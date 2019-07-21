using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class AnimalMaker : MonoBehaviour {

		private static GameObject _animalPrefab;

		[UsedImplicitly, SerializeField] private GameObject _chunkContainer = null;
		[UsedImplicitly, SerializeField] private GameObject _container = null;

		private static readonly List<TileType> ValidTiles = new List<TileType> {
			TileType.Mossy, TileType.Sand, TileType.Soil, TileType.SoilRich, TileType.Gravel, TileType.PackedDirt, TileType.Ice
		};

		[UsedImplicitly]
		private void Start () {
			if (_animalPrefab == null) {
				_animalPrefab = new GameObject("Animal Prefab", typeof(Animal), typeof(SpriteRenderer));
				_animalPrefab.transform.SetParent(_container.transform);
				_animalPrefab.SetActive(false);
			}

			Populate(_animalPrefab);
		}

		private void Populate (GameObject prefab) {
			foreach (Transform c in _chunkContainer.transform) {
				foreach (Transform t in c) {
					Initialize(prefab, t);
				}
			}
		}

		private void Initialize (GameObject prefab, Transform t) {
			if (Random.value < .998f) {
				return;
			}

			if (ValidTiles.Contains(t.GetComponent<Tile>().Type) == false) {
				return;
			}

			//todo entry point for animal selection
			AnimalDef def = DefLoader.GetRandomAnimalDef();
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Vector3 pos = new Vector3(x, y, Order.ANIMAL);
			GameObject go = Instantiate(prefab, pos, Quaternion.identity, _container.transform);
			Animal animal = Animal.Create(go.GetComponent<Animal>(), def);
			animal.Initialize();
		}

	}

}