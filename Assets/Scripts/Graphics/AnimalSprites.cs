using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class AnimalSprites : MonoBehaviour {

		[UsedImplicitly] public GameObject[] GazellePrefabs;
		[UsedImplicitly] public GameObject[] IguanaPrefabs;
		[UsedImplicitly] public GameObject[] TortoisePrefabs;

		private static GameObject[] _gazellePrefabs;
		private static GameObject[] _iguanaPrefabs;
		private static GameObject[] _tortoisePrefabs;

		public static GameObject Get (AnimalType type) {
			switch (type) {
				case AnimalType.Gazelle:
					return _gazellePrefabs[0];
				case AnimalType.Iguana:
					return _iguanaPrefabs[0];
				case AnimalType.Tortoise:
					return _tortoisePrefabs[0];
				default:
					return null;
			}
		}

		[UsedImplicitly]
		private void OnEnable () {
			_gazellePrefabs = GazellePrefabs;
			_iguanaPrefabs = IguanaPrefabs;
			_tortoisePrefabs = TortoisePrefabs;
		}

	}

}