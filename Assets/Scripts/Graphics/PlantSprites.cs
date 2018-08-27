using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class PlantSprites : MonoBehaviour {

		// Object references
		#region
		#region Trees
		[UsedImplicitly]
		public GameObject[] PalmAPrefabs;
		#endregion

		#region Plants
		[UsedImplicitly]
		public GameObject[] AgavePrefabs;

		[UsedImplicitly]
		public GameObject[] CactusPrefabs;

		[UsedImplicitly]
		public GameObject[] GrassPrefabs;
		#endregion
		#endregion

		private static GameObject[] _palm;

		private static GameObject[] _agave;
		private static GameObject[] _cactus;
		private static GameObject[] _grass;

		public static GameObject Get (PlantType type) {
			switch (type) {
				case PlantType.Palm:
					return _palm[Random.Range(0, _palm.Length)];
				case PlantType.Agave:
					return _agave[Random.Range(0, _agave.Length)];
				case PlantType.Cactus:
					return _cactus[Random.Range(0, _cactus.Length)];
				case PlantType.Grass:
					return _grass[Random.Range(0, _grass.Length)];
			}

			return null;
		}

		[UsedImplicitly]
		private void OnEnable () {
			_palm = PalmAPrefabs;
			_agave = AgavePrefabs;
			_cactus = CactusPrefabs;
			_grass = GrassPrefabs;
		}

	}

}