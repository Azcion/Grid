using System;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class AnimalMaker : MonoBehaviour {
		
		#region Object references
		[UsedImplicitly] public GameObject ChunkContainer;
		[UsedImplicitly] public GameObject Container;
		#endregion

		[UsedImplicitly]
		private void Start () {
			StartCoroutine(Populate());
		}

		private IEnumerator Populate () {
			yield return new WaitForSeconds(.15f);

			foreach (Transform c in ChunkContainer.transform) {
				foreach (Transform t in c) {
					Initialize(t);
				}
			}
		}

		private void Initialize (Transform t) {
			AnimalType type;

			switch (t.GetComponent<Tile>().Type) {
				case TileType.Sand:
					if (Random.value < .998) {
						return;
					}

					type = Random.value < .75 ? AnimalType.Iguana : AnimalType.Elephant;
					break;
				case TileType.Grass:
				case TileType.Dirt:
					if (Random.value < .997) {
						return;
					}

					type = Random.value < .5 ? AnimalType.Tortoise : AnimalType.Gazelle;
					break;
				default:
					return;
			}

			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Vector3 v = new Vector3(x, y, Order.THING);
			GameObject go = new GameObject(Enum.GetName(typeof(AnimalType), type));
			go.transform.SetParent(Container.transform);
			go.transform.position = v;
			Animal animal = go.AddComponent<Animal>();
			animal.Initialize(type);
		}

	}

}