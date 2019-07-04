using Assets.Scripts.Defs;
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

		public static float MaxCapacity = Mathf.Clamp(TileMaker.YCHUNKS * TileMaker.YCHUNKS, 2.5f, 25);
		public static float CurrentCapacity;

		[UsedImplicitly]
		private void Start () {
			Populate();
		}

		private void Populate () {
			foreach (Transform c in ChunkContainer.transform) {
				foreach (Transform t in c) {
					Initialize(t);

					if (CurrentCapacity >= MaxCapacity) {
						return;
					}
				}
			}
		}

		private void Initialize (Transform t) {
			if (Random.value < .998) {
				return;
			}

			//todo entry point for animal selection
			AnimalDef def = DefLoader.GetRandomAnimalDef();
			CurrentCapacity += def.EcosystemWeight;

			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Vector3 v = new Vector3(x, y, Order.THING);
			GameObject go = new GameObject(def.DefName);
			go.transform.SetParent(Container.transform);
			go.transform.position = v;
			Animal animal = go.AddComponent<Animal>();
			animal.Initialize(def);
		}

	}

}