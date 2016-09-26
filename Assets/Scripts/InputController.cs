using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class InputController : MonoBehaviour {

		[UsedImplicitly]
		private void Update () {
			if (Input.GetButtonUp("escape")) {
				Application.Quit();
			}
		}

	}

}