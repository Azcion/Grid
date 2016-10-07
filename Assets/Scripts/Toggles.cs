using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class Toggles : MonoBehaviour {

		public static bool DoCycle = true;
		public static bool DoWind = true;

		[UsedImplicitly]
		public void ToggleCycle () {
			DoCycle = !DoCycle;
		}

		[UsedImplicitly]
		public void ToggleWind () {
			DoWind = !DoWind;
		}

	}

}