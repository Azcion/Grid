using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class PlantMaker : MonoBehaviour {

		// Object references
		#region
		[UsedImplicitly]
		public GameObject ChunkContainer;

		[UsedImplicitly]
		public GameObject Container;
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

					type = PlantType.Cactus;
					break;
				case TileType.Grass:
					if (Random.value < .75) {
						return;
					}

					if (Random.value < .50) {
						type = PlantType.Grass;
						break;
					}

					type = PlantType.Palm;
					break;
				case TileType.Dirt:
					if (Random.value < .90) {
						return;
					}

					type = PlantType.Agave;
					break;
				default:
					return;
			}

			Vector3 v = new Vector3((int) t.position.x, (int) t.position.y, Order.THING);
			GameObject f = Instantiate(PlantSprites.Get(type), v, Quaternion.identity, Container.transform);
			f.GetComponent<Plant>().Assign(type, Calc.Round(Random.Range(.5f, 1), 2), Random.value > .5);
			f.SetActive(true);
		}

	}

}