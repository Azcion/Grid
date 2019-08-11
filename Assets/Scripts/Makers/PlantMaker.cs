using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class PlantMaker : MonoBehaviour {

		private static GameObject _plantPrefab;

		[UsedImplicitly, SerializeField] private GameObject _container = null;

		[UsedImplicitly]
		private void Start () {
			if (!DefLoader.DidLoad) {
				Debug.Log("Defs not loaded, PlantMaker can't run.");
				return;
			}

			if (_plantPrefab == null) {
				_plantPrefab = new GameObject("Plant Prefab", typeof(Plant));
				_plantPrefab.transform.SetParent(_container.transform);
				_plantPrefab.SetActive(false);
			}

			Populate(_plantPrefab);
		}

		private void Populate (GameObject prefab) {
			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) { 
					Initialize(prefab, TileMaker.Get(x, y).transform);
				}
			}

			ApplicationController.NotifyReady();
		}

		private void Initialize (GameObject prefab, Transform t) {
			bool isGrass = false;

			switch (t.GetComponent<Tile>().Type) {
				case TileType.Sand:
				case TileType.Gravel:
					if (Random.value < .95) {
						//empty tiles
						return;
					}

					if (Random.value < .75) {
						isGrass = true;
					}

					break;
				case TileType.Soil:
				case TileType.SoilRich:
					if (Random.value < .35) {
						//empty tiles
						return;
					}

					float value = Random.value;

					if (value < .90) {
						isGrass = true;
					}

					break;
				default:
					return;
			}

			//todo entry point for plant selection
			PlantDef def = isGrass ? DefLoader.Grass : DefLoader.GetRandomPlantDef();
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Vector3 pos = new Vector3(x, y, Order.PLANT);
			GameObject go = Instantiate(prefab, pos, Quaternion.identity, _container.transform);
			go.name = def.DefName;
			Plant plant = Plant.Create(go.GetComponent<Plant>(), def);
			plant.Initialize(Calc.Round(Random.Range(.3f, 1), 2));

			if (TileMaker.GetTile(x, y).TryAddThing(plant) == false) {
				Debug.Log($"Tried to add plant to occupied tile. {x}, {y}");
			}
		}

	}

}