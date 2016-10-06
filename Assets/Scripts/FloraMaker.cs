using System.Collections;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class FloraMaker : MonoBehaviour {

		// Object references
		#region
		[UsedImplicitly]
		public GameObject PlantPrefab;

		[UsedImplicitly]
		public GameObject TreePrefab;

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
		}

		private void Initialize (Transform t) {
			GameObject prefab;
			Transform container;
			Sprite sprite;

			float scale = Random.Range(.5f, 1);
			Vector2 vScale = new Vector2(Random.value > .5 ? scale : -scale, scale);
			

			switch (t.GetComponent<Tile>().Type) {
				case TileType.Grass:
					if (Random.value < .85) {
						return;
					}

					prefab = TreePrefab;
					container = TreeContainer.transform;
					sprite = FloraSprites.Get(FloraType.Palm);
					break;
				case TileType.Dirt:
					if (Random.value < .95) {
						return;
					}

					prefab = PlantPrefab;
					container = PlantContainer.transform;
					sprite = FloraSprites.Get(FloraType.Agave);
					break;
				default:
					return;
			}

			GameObject f = Instantiate(prefab, t.position, Quaternion.identity, container);
			f.transform.localScale = vScale;
			f.GetComponent<SpriteRenderer>().sprite = sprite;
			f.SetActive(true);
		}

	}

}