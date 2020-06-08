using Assets.Scripts.Weather;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class Toggles : MonoBehaviour {

		public static bool DoCycle;

		private static bool _showWeather = true;

		[UsedImplicitly]
		public void ToggleCycle () {
			DoCycle = !DoCycle;
		}

		[UsedImplicitly]
		public void ToggleWeather () {
			_showWeather = !_showWeather;
			WeatherController.SetActive(_showWeather);
		}

		[UsedImplicitly]
		public void CycleWeather () {
			WeatherController.Next();
		}

	}

}