using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Makers;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

	public class ArchitectController : MonoBehaviour {

		private static GameObject _architect;
		private static Dictionary<string, GameObject> _actions;
		private static List<GameObject> _visibleButtons;
		private static Dictionary<ArchitectCategory, List<string>> _categories;

		public static void Hide () {
			_architect.SetActive(false);
			HideButtons();
		}

		[UsedImplicitly]
		public void Toggle () {
			_architect.SetActive(!_architect.activeSelf);

			if (!_architect.activeSelf) {
				HideButtons();
			}
		}

		[UsedImplicitly]
		private void Start () {
			_architect = transform.Find("Categories").gameObject;
			_visibleButtons = new List<GameObject>();
			_categories = new Dictionary<ArchitectCategory, List<string>>();
			Transform root = _architect.transform.Find("Buttons");
			Transform categoryPrefab = root.GetChild(0);

			for (int i = 1; i < Name.ArchitectCategory.Length; ++i) {
				ArchitectCategory category = (ArchitectCategory) i;
				_categories.Add(category, new List<string>());
				GameObject go = Instantiate(categoryPrefab.gameObject);
				go.SetActive(true);
				go.transform.SetParent(root);
				string label = Name.Get(category);
				go.name = label;
				string text = Format.Capitalize(Format.SeparateAtCapitalLetters(label));
				go.transform.GetChild(0).GetComponent<Text>().text = text;
				go.GetComponent<Button>().onClick.AddListener(() => SelectCategory(category));
			}

			categoryPrefab.gameObject.SetActive(false);
			Transform actionsContainer = transform.Find("Actions");
			Transform actionPrefab = actionsContainer.transform.GetChild(0);
			_actions = new Dictionary<string, GameObject>();

			foreach (BuildingDef def in DefLoader.BuildingDefs.Defs) {
				if (def.Category == ArchitectCategory.None) {
					continue;
				}

				GameObject go = Instantiate(actionPrefab.gameObject);
				go.SetActive(false);
				go.transform.SetParent(actionsContainer);
				go.name = def.Label;
				BuildingDef def1 = def;
				go.GetComponent<Button>().onClick.AddListener(() => Action_OnClick(def1));
				string text = Format.Capitalize(Format.SeparateAtCapitalLetters(def.Label));
				go.transform.GetChild(0).GetComponent<Text>().text = text;
				string asset;

				if (def.LinkedType != LinkedType.None) {
					asset = $"{def.LinkedType}_UI";
				} else {
					asset = string.IsNullOrEmpty(def.UITex) ? def.DefName : def.UITex;
				}

				Image image = go.transform.GetChild(1).GetComponent<Image>();
				image.overrideSprite = Assets.GetSprite(asset);
				image.color = Tint.Get(ThingMaterial.Wood);
				_actions.Add(def.Label, go);
				_categories[def.Category].Add(def.Label);
			}

			actionPrefab.gameObject.SetActive(false);
		}

		private static void SelectCategory (ArchitectCategory category) {
			SetActions(_categories[category]);
		}

		private static void SetActions (IEnumerable<string> actions) {
			HideButtons();

			foreach (string action in actions) {
				GameObject button = _actions[action];
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

		private static void Action_OnClick (BuildingDef def) {
			Architect.SelectThing(def);
		}
		
	}

}