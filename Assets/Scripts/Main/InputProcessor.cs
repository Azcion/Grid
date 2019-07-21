using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class InputProcessor : MonoBehaviour {

		[UsedImplicitly]
		private void LateUpdate () {
			if (Input.GetKeyUp("escape")) {
				if (!Application.isEditor) {
					System.Diagnostics.Process.GetCurrentProcess().Kill();
				} else {
					Application.Quit();
				}
			}
		}

	}

}