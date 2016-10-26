using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class Toggles : MonoBehaviour {

		public static bool DoCycle = true;

		[UsedImplicitly]
		public void ToggleCycle () {
			DoCycle = !DoCycle;
		}

	}

}