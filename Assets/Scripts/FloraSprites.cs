using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class FloraSprites : MonoBehaviour {

		// Object references
		#region
		#region Trees
		[UsedImplicitly]
		public GameObject[] PalmAPrefabs;

		[UsedImplicitly]
		public GameObject[] PalmBPrefabs;
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

		private static GameObject[] _palmA;
		private static GameObject[] _palmB;

		private static GameObject[] _agave;
		private static GameObject[] _cactus;
		private static GameObject[] _grass;

		public static GameObject Get (FloraType type) {
			switch (type) {
				case FloraType.Palm:
					return _palmA[Random.Range(0, _palmA.Length)];
				case FloraType.Agave:
					return _agave[Random.Range(0, _agave.Length)];
				case FloraType.Cactus:
					return _cactus[Random.Range(0, _cactus.Length)];
				case FloraType.Grass:
					return _grass[Random.Range(0, _grass.Length)];
			}

			return null;
		}

		[UsedImplicitly]
		private void OnEnable () {
			_palmA = PalmAPrefabs;
			_palmB = PalmBPrefabs;
			_agave = AgavePrefabs;
			_cactus = CactusPrefabs;
			_grass = GrassPrefabs;
		}

	}

}