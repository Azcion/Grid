using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.UI {

	public class ArchitectController : MonoBehaviour {

		[UsedImplicitly, SerializeField] private GameObject _architect = null;

		[UsedImplicitly]
		public void Toggle () {
			if (_architect == null) {
				return;
			}

			_architect.SetActive(!_architect.activeSelf);
		}
		
	}

}