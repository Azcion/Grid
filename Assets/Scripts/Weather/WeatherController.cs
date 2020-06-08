using System.Collections.Generic;
using Assets.Scripts.Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Weather {

	public class WeatherController : MonoBehaviour {

		private static MeshRenderer _renderer;
		private static List<Weather> _weather;
		private static int _currentWeather;

		public static void SetActive (bool flag) {
			_renderer.enabled = flag;
		}

		public static void Next () {
			++_currentWeather;

			if (_currentWeather >= _weather.Count) {
				_currentWeather = 0;
			}

			_renderer.sharedMaterial = _weather[_currentWeather].Material;
		}

		[UsedImplicitly]
		private void Start () {
			_weather = new List<Weather>();

			for (int i = 1; i < Name.WeatherType.Length; ++i) {
				Material material = Assets.Weather[i - 1];
				_weather.Add(new Weather((WeatherType) i, material));
			}

			transform.position = new Vector3(Map.YHalf, Map.YHalf, Order.WEATHER);
			transform.localScale = new Vector3(Map.YTiles, Map.YTiles);
			_renderer = GetComponent<MeshRenderer>();
			_renderer.enabled = true;
		}
		
	}

}