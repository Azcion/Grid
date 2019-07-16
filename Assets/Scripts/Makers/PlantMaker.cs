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

		#region Object references
		[UsedImplicitly] public GameObject ChunkContainer;
		[UsedImplicitly] public GameObject Container;
		#endregion

		[UsedImplicitly]
		private void Start () {
			Populate();
		}

		private void Populate () {
			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) { 
					Initialize(TileMaker.Get(x, y).transform);
				}
			}

			ApplicationController.NotifyReady();
		}

		private void Initialize (Transform t) {
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
			Plant plant = Plant.Create(def, x, y, Order.THING, Container.transform);
			plant.Initialize(Calc.Round(Random.Range(.3f, 1), 2));

			if (TileMaker.GetTile(x, y).TryAddThing(plant) == false) {
				Debug.Log($"Tried to add plant to occupied tile. {x}, {y}");
			}
		}

	}

}