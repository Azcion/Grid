using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

	public class StartInterface : MonoBehaviour {

		private static StartInterface _instance;
		private static Slider _mapSizeSliderComponent;
		private static Text _seedText;
		private static Text _mapSizeSliderHandleText;

		[UsedImplicitly, SerializeField] private GameObject _seed = null;
		[UsedImplicitly, SerializeField] private GameObject _mapSizeSlider = null;
		[UsedImplicitly, SerializeField] private GameObject _mapSizeSliderText = null;
		[UsedImplicitly, SerializeField] private GameObject _startButton = null;

		public static string GetSeed => _seedText.text;
		public static int GetMapSize => int.Parse(_mapSizeSliderHandleText.text);

		[UsedImplicitly]
		public void OnSliderMove () {
			// The slider moves in steps of 1 between 0 and 10
			int value = Map.CSIZE * 5 * ((int) _mapSizeSliderComponent.value + 1);
			_mapSizeSliderHandleText.text = value.ToString();
		}

		public static void Hide () {
			_instance.DestroyAll();
		}

		private void DestroyAll () {
			Destroy(_seed);
			Destroy(_mapSizeSlider);
			Destroy(_startButton);
		}

		[UsedImplicitly]
		private void Start () {
			_instance = this;
			_mapSizeSliderComponent = _mapSizeSlider.transform.GetComponent<Slider>();
			_seedText = _seed.transform.Find("Text")?.GetComponent<Text>();
			_mapSizeSliderHandleText = _mapSizeSliderText.GetComponent<Text>();
			OnSliderMove();
		}

	}

}