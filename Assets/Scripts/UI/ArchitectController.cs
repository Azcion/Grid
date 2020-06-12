using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.UI {

	public class ArchitectController : MonoBehaviour {

		private GameObject _architect;
		private GameObject _structure;
		private GameObject _production;
		private GameObject _debug;
		private GameObject _activeCategory;

		[UsedImplicitly]
		public void Toggle () {
			_architect.SetActive(!_architect.activeSelf);
		}

		[UsedImplicitly]
		public void ToggleStructure () {
			ToggleActive();
			_activeCategory = _structure;
			ToggleActive();
		}

		[UsedImplicitly]
		public void ToggleProduction () {
			ToggleActive();
			_activeCategory = _production;
			ToggleActive();
		}

		[UsedImplicitly]
		public void ToggleDebug () {
			ToggleActive();
			_activeCategory = _debug;
			ToggleActive();
		}

		private void ToggleActive () {
			if (_activeCategory == null) {
				return;
			}

			_activeCategory.SetActive(!_activeCategory.activeSelf);
		}

		[UsedImplicitly]
		private void Start () {
			_architect = transform.Find("Container").gameObject;
			Transform debugButton = _architect.transform.Find("Debug");

			if (Application.isEditor) {
				_debug = debugButton.GetChild(1).gameObject;
			} else {
				debugButton.gameObject.SetActive(false);
			}
			
			_structure = _architect.transform.Find("Structure").GetChild(1).gameObject;
			_production = _architect.transform.Find("Production").GetChild(1).gameObject;
		}
		
	}

}