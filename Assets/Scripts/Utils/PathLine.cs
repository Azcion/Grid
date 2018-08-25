using UnityEngine;

namespace Assets.Scripts.Utils {

	public class PathLine {

		public bool Drawing;

		private static readonly GameObject Parent;
		private static readonly Material LineMat;

		private readonly GameObject _line;
		private readonly LineRenderer _renderer;

		private float _duration;

		static PathLine () {
			Parent = new GameObject("Debug Path");
			LineMat = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		}

		public PathLine (Vector2 start, Vector2 end, Color color, float duration = .1f) {
			_line = new GameObject("Line");
			_line.transform.SetParent(Parent.transform);
			_line.transform.position = start;
			_renderer = _line.AddComponent<LineRenderer>();
			_renderer.material = LineMat;
			_renderer.startColor = color;
			_renderer.endColor = color;
			_renderer.startWidth = .1f;
			_renderer.endWidth = .1f;
			_renderer.SetPosition(0, start);
			_renderer.SetPosition(1, end);

			_duration = duration;
		}

		public void UpdatePos (Vector2 start, Vector2 end) {
			_renderer.SetPosition(0, start);
			_renderer.SetPosition(1, end);
		}

		
		public void Update () {
			if (Drawing == false) {
				return;
			}
			
			_duration -= Time.deltaTime;

			if (_duration <= 0) {
				Drawing = false;
				Object.Destroy(_line);
			}
		}

		public void Destroy () {
			Object.Destroy(_line);
		}

	}

}