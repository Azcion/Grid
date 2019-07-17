using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class AnimalMaker : MonoBehaviour {

		[UsedImplicitly, SerializeField] private GameObject _chunkContainer = null;
		[UsedImplicitly, SerializeField] private GameObject _container = null;

		private static readonly List<TileType> ValidTiles = new List<TileType> {
			TileType.Mossy, TileType.Sand, TileType.Soil, TileType.SoilRich, TileType.Gravel, TileType.PackedDirt, TileType.Ice
		};

		[UsedImplicitly]
		private void Start () {
			Populate();
		}

		private void Populate () {
			foreach (Transform c in _chunkContainer.transform) {
				foreach (Transform t in c) {
					Initialize(t);
				}
			}
		}

		private void Initialize (Transform t) {
			if (Random.value < .998f) {
				return;
			}

			if (ValidTiles.Contains(t.GetComponent<Tile>().Type) == false) {
				return;
			}

			//todo entry point for animal selection
			AnimalDef def = DefLoader.GetRandomAnimalDef();
			int x = (int) t.position.x;
			int y = (int) t.position.y;
			Animal animal = Animal.Create(def, x, y, Order.THING, _container.transform);
			animal.Initialize();
		}

	}

}