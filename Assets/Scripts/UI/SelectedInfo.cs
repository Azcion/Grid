using Assets.Scripts.Defs;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

	public class SelectedInfo : MonoBehaviour {

		public static bool IsHoveringOver;

		private static GameObject _container;
		private static Text _name;
		private static Text _description;

		public static void Set (IThingDef def) {
			_name.text = Format.Capitalize(def.GetLabel());
			_description.text = def.GetDescription();
		}

		public static void Show () {
			ArchitectController.Hide();
			_container.SetActive(true);
		}

		public static void Hide () {
			_container.SetActive(false);
			IsHoveringOver = false;
		}

		[UsedImplicitly]
		public void OnPointerEnter () {
			IsHoveringOver = true;
		}

		[UsedImplicitly]
		public void OnPointerExit () {
			IsHoveringOver = false;
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