using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

	public class ArchitectDescriptionController : MonoBehaviour {

		private static GameObject _container;
		private static Text _name;
		private static Text _description;

		public static void Set (string label, string description) {
			_name.text = label;
			_description.text = description;
		}

		public static void Show () {
			_container.SetActive(true);
		}

		public static void Hide () {
			_container.SetActive(false);
		}

		[UsedImplicitly]
		private void Start () {
			_container = transform.GetChild(0).gameObject;
			Text[] components = _container.GetComponentsInChildren<Text>();
			_name = components[0];
			_description = components[1];
		}

	}

}