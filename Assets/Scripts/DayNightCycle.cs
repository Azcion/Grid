using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class DayNightCycle : MonoBehaviour {

		public static float Progress;

		[UsedImplicitly]
		public Color ColorDay;

		[UsedImplicitly]
		public Color ColorNight;

		private Light _light;
		private bool _daytime;
		private int _direction;

		[UsedImplicitly]
		private void Start () {
			_light = GetComponent<Light>();
			_daytime = true;
			_direction = 1;
			Progress = .5f;
		}

		[UsedImplicitly]
		private void Update () {
			if (Toggles.DoCycle) {
				if (_daytime) {
					Lerp(ColorNight, ColorDay);
				} else {
					Lerp(ColorDay, ColorNight);
				}
			}
		}

		private void Lerp (Color from, Color to) {
			const float speed = 1f / 60;
			float r = Mathf.Lerp(from.r, to.r, Progress);
			float g = Mathf.Lerp(from.g, to.g, Progress);
			float b = Mathf.Lerp(from.b, to.b, Progress);

			_light.color = new Color(r, g, b);
			Progress += speed * Time.deltaTime * _direction;

			if (Progress >= 1) {
				_direction = -1;
			} else if (Progress <= 0) {
				_direction = 1;
			}
		}

	}

}