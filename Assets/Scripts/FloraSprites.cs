using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class FloraSprites : MonoBehaviour {

		private static Sprite[] _palmA;
		private static Sprite[] _palmB;

		private static Sprite[] _agave;

		public static Sprite Get (FloraType type) {
			switch (type) {
				case FloraType.Palm:
					return _palmA[Random.Range(0, _palmA.Length)];
				case FloraType.Agave:
					return _agave[Random.Range(0, _agave.Length)];
			}

			return null;
		}

		[UsedImplicitly]
		private void OnEnable () {
			_palmA = Resources.LoadAll<Sprite>("Flora/Trees/PalmA");
			//_palmB = Resources.LoadAll<Sprite>("Flora/Trees/PalmB");

			_agave = Resources.LoadAll<Sprite>("Flora/Plants/Agave");
		}

	}

}