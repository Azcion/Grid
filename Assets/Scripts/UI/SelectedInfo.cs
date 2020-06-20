using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
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
		private static Scrollbar _scrollbar;
		private static Dictionary<string, GameObject> _actions;
		private static List<GameObject> _visibleButtons;
		private static Thing _thing;

		public static void Set (Thing thing) {
			_thing = thing;
			IThingDef def = thing.ThingDef;
			_name.text = Format.Capitalize(def.GetLabel);
			_description.text = def.GetDescription.Replace("\\n", "\n");
			_scrollbar.value = 1;
			SetActions(thing.ValidActions);
		}

		public static void Show () {
			ArchitectController.Hide();
			_container.SetActive(true);
		}

		public static void Hide () {
			_thing = null;
			_container.SetActive(false);
			HideButtons();
			IsHoveringOver = false;
		}

		[UsedImplicitly]
		public void Action_ChopWood () {
			if (_thing.Heir.Type != ThingType.Plant) {
				return;
			}

			Plant plant = _thing.Heir as Plant;
			plant?.Action_ChopWood();
		}

		[UsedImplicitly]
		public void Action_Harvest () {
			if (_thing.Heir.Type != ThingType.Plant) {
				return;
			}

			Plant plant = _thing.Heir as Plant;
			plant?.Action_Harvest();
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
			Transform root = transform.GetChild(0);
			_container = root.gameObject;
			_name = root.Find("Name").GetComponent<Text>();
			_description = root.Find("Description Scroll View").GetChild(0).GetComponent<Text>();
			_scrollbar = root.Find("Description Scrollbar").GetComponent<Scrollbar>();
			_visibleButtons = new List<GameObject>();
			_actions = new Dictionary<string, GameObject>();
			Transform actionsContainer = root.Find("Actions");

			for (int i = 0; i < actionsContainer.childCount; ++i) {
				string actionStr = Name.Action[i];
				Transform child = actionsContainer.Find(actionStr);
				_actions.Add(actionStr, child.gameObject);
			}
		}

		private static void SetActions (IEnumerable<Action> actions) {
			HideButtons();

			foreach (Action action in actions) {
				string actionStr = Name.Get(action);
				GameObject button = _actions[actionStr];
				button.SetActive(true);
				_visibleButtons.Add(button);
			}
		}

		private static void HideButtons () {
			foreach (GameObject button in _visibleButtons) {
				button.SetActive(false);
			}

			_visibleButtons.Clear();
		}

	}

}