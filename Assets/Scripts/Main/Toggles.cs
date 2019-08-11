using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class Toggles : MonoBehaviour {

		public static bool DoCycle;

		[UsedImplicitly]
		public void ToggleCycle () {
			DoCycle = !DoCycle;
		}

	}

}