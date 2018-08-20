using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class FloraMaker : MonoBehaviour {

		// Object references
		#region
		[UsedImplicitly]
		public GameObject ChunkContainer;

		[UsedImplicitly]
		public GameObject PlantContainer;

		[UsedImplicitly]
		public GameObject TreeContainer;
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
			float scale = (float) System.Math.Round(Random.Range(.5f, 1), 2);
			Vector3 vScale = new Vector3(Random.value > .5 ? scale : -scale, scale, 1);
			Vector3 position = new Vector3(t.position.x, t.position.y, Order.THING);

			Transform container;
			FloraType type;

			switch (t.GetComponent<Tile>().Type) {
				case TileType.Sand:
					if (Random.value < .95) {
						return;
					}

					container = PlantContainer.transform;
					type = FloraType.Cactus;
					break;
				case TileType.Grass:
					if (Random.value < .75) {
						return;
					}

					if (Random.value < .50) {
						container = PlantContainer.transform;
						type = FloraType.Grass;
						break;
					}

					container = TreeContainer.transform;
					type = FloraType.Palm;
					break;
				case TileType.Dirt:
					if (Random.value < .90) {
						return;
					}

					container = PlantContainer.transform;
					type = FloraType.Agave;
					break;
				default:
					return;
			}

			GameObject f = Instantiate(FloraSprites.Get(type), position, Quaternion.identity, container);
			f.transform.localScale = vScale;
			f.GetComponent<Flora>().Type = type;
			f.SetActive(true);
		}

	}

}