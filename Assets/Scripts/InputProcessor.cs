using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class InputProcessor : MonoBehaviour {

		[UsedImplicitly]
		private void LateUpdate () {
			if (Input.GetKeyUp("escape")) {
				Application.Quit();
			}
		}

	}

}