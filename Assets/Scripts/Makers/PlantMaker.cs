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

			foreach (Transform c in ChunkContainer.transform) {
				foreach (Transform t in c) {
					Initialize(t);
				}
			}

			ApplicationController.NotifyReady();
		}

		private void Initialize (Transform t) {
			PlantType type;

			switch (t.GetComponent<Tile>().Type) {
				case TileType.Sand:
					if (Random.value < .95) {
						return;
					}

					if (Random.value > .75) {
						type = PlantType.SaguaroCactus;
					} else {
						type = PlantType.Grass;
					}

					type = PlantType.SaguaroCactus;
					break;
				/*case TileType.Grass:
					if (Random.value < .75) {
						return;
					}

					if (Random.value < .50) {
						type = PlantType.Grass;
						break;
					}

					type = PlantType.TreePalm;
					break;*/
				case TileType.Soil:
					if (Random.value < .35) {
						return;
					}

					float value = Random.value;

					if (value > .95) {
						type = PlantType.SaguaroCactus;
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
			
			if (TileMaker.GetTile(x, y).TryAddThing(plant) == false) {
				Debug.Log($"Tried to add plant to occupied tile. {x}, {y}");
			}
		}

	}

}