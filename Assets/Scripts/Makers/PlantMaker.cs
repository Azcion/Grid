using System;
using System.Collections;
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
			StartCoroutine(Populate());
		}

		private IEnumerator Populate () {
			yield return new WaitForSeconds(.10f);

			for (int x = 0; x < TileMaker.YTILES; ++x) {
				for (int y = TileMaker.YTILES - 1; y >= 0; --y) {
					Initialize(TileMaker.Get(x, y).transform);
				}
			}

			ApplicationController.NotifyReady();
		}

		private void Initialize (Transform t) {
			PlantType type;

			switch (t.GetComponent<Tile>().Type) {
				case TileType.Sand:
				case TileType.Gravel:
					if (Random.value < .95) {
						return;
					}

					if (Random.value > .75) {
						if (Random.value > .75) {
							type = PlantType.TreeDrago;
						} else {
							type = PlantType.SaguaroCactus;
						}
					} else {
						type = PlantType.Grass;
					}

					break;
				case TileType.Soil:
				case TileType.SoilRich:
					if (Random.value < .35) {
						return;
					}

					float value = Random.value;

					if (value > .925) {
						if (Random.value > .25) {
							type = PlantType.TreeDrago;
						} else {
							type = PlantType.SaguaroCactus;
						}
					} else if (value > .90) {
						type = PlantType.Agave;
					} else {
						type = PlantType.Grass;
					}

					break;
				default:
					return;
			}

			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Vector3 v = new Vector3(x, y, Order.THING);
			GameObject go = new GameObject(Enum.GetName(typeof(PlantType), type));
			go.transform.SetParent(Container.transform);
			go.transform.position = v;
			Plant plant = go.AddComponent<Plant>();
			plant.Initialize(type, Calc.Round(Random.Range(.3f, 1), 2));

			Tile tile = TileMaker.GetTile(x, y);

			if (tile.TryAddThing(plant) == false) {
				Plant plantDebug = tile.GetThing() as Plant;
				Debug.Log($"Tried to add plant to occupied tile. {x}, {y}, {plantDebug?.Type}");
			}
		}

	}

}