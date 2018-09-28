using System;
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
			Populate();
		}

		private void Populate () {
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
				case TileType.Soil:
					if (Random.value < .998) {
						return;
					}

					float value = Random.value;

					if (value > .90) {
						type = AnimalType.Elephant;
					} else if (value > .40) {
						type = AnimalType.Gazelle;
					} else if (value > .20) {
						type = AnimalType.Tortoise;
					} else {
						type = AnimalType.Iguana;
					}

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