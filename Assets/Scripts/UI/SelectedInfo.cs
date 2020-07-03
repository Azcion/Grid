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
			string label = def.GetLabel;

			if ((def as ThingDef)?.ShowMaterial ?? false) {
				string material = Name.Get(thing.Material);
				label = string.IsNullOrEmpty(label) ? material : $"{material} {label}";
			}

			if (thing.IsBlueprint) {
				label += " (blueprint)";
			}

			_name.text = Format.Capitalize(label);
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
			//GUI.Busy = false;
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
			Transform actionPrefab = actionsContainer.GetChild(0);

			for (int i = 0; i < Name.Action.Length; ++i) {
				GameObject go = Instantiate(actionPrefab.gameObject);
				go.transform.SetParent(actionsContainer);
				Action action = (Action) i;
				string label = Name.Get(action);
				go.name = label;
				go.GetComponent<Button>().onClick.AddListener(() => Action_OnClick(action));
				string text = Format.Capitalize(Format.SeparateAtCapitalLetters(label).ToLower());
				go.transform.GetChild(0).GetComponent<Text>().text = text;
				string asset = label;
				go.transform.GetChild(1).GetComponent<Image>().sprite = Assets.GetSprite(asset);
				_actions.Add(label, go);
			}

			actionPrefab.gameObject.SetActive(false);
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

		private static void Action_OnClick (Action action) {
			switch (_thing.Heir.Type) {
				case ThingType.Plant:
					Plant plant = _thing.Heir as Plant;

					switch (action) {
						case Action.Harvest:
							plant?.Action_Harvest();
							break;
						case Action.ChopWood:
							plant?.Action_ChopWood();
							break;
					}

					break;
			}
		}

	}

}