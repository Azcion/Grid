using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class Toggles : MonoBehaviour {

		public static bool DoCycle = true;

		[UsedImplicitly]
		public void ToggleCycle () {
			DoCycle = !DoCycle;
		}

	}

}