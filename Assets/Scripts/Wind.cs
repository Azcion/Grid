using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {

	public class Wind : MonoBehaviour {

		private const float X_SPEED = .050f;
		private const float Y_SPEED = .015f;
		private const float X_OFFSET = .010f;
		private const float Y_OFFSET = .003f;
		private const float X_EXPANSION = .050f;
		private const float Y_EXPANSION = .015f;
		private const float X_OVERFLOW = X_EXPANSION + X_OFFSET;
		private const float Y_OVERFLOW = Y_EXPANSION + Y_OFFSET;

		private Renderer _renderer;

		private int _xDirection;
		private int _yDirection;

		private float _xMax;
		private float _yMax;
		private float _xMin;
		private float _yMin;

		private float _xMaxOverflow;
		private float _yMaxOverflow;
		private float _xMinOverflow;
		private float _yMinOverflow;

		private float _x;
		private float _y;

		[UsedImplicitly]
		private void OnEnable () {
			_renderer = GetComponent<Renderer>();

			_xDirection = (int) transform.position.x % 2 == 0 ? 1 : -1;
			_yDirection = (int) transform.position.y % 2 == 0 ? 1 : -1;

			Vector3 s = transform.localScale;

			_xMax = (float) Math.Round(s.x + X_EXPANSION / 2, 3);
			_yMax = (float) Math.Round(s.y + Y_EXPANSION / 2, 3);
			_xMin = (float) Math.Round(s.x - X_EXPANSION / 2, 3);
			_yMin = (float) Math.Round(s.y - Y_EXPANSION / 2, 3);

			_xMaxOverflow = (float) Math.Round(s.x + X_OVERFLOW / 2, 3);
			_yMaxOverflow = (float) Math.Round(s.y + Y_OVERFLOW / 2, 3);
			_xMinOverflow = (float) Math.Round(s.x - X_OVERFLOW / 2, 3);
			_yMinOverflow = (float) Math.Round(s.y - Y_OVERFLOW / 2, 3);

			_x = s.x;
			_y = s.y + Y_OFFSET + Y_OFFSET;
		}

		[UsedImplicitly]
		private void Update () {
			if (!_renderer.isVisible || CameraController.Size >= 15) {
				return;
			}

			float dx = X_SPEED * Time.deltaTime * _xDirection;
			float dy = Y_SPEED * Time.deltaTime * _yDirection;

			_x += dx;
			_y += dy;

			if (_x >= _xMaxOverflow) {
				_xDirection = -1;
			} else if (_x <= _xMinOverflow) {
				_xDirection = 1;
			}

			if (_y >= _yMaxOverflow) {
				_yDirection = -1;
			} else if (_y <= _yMinOverflow) {
				_yDirection = 1;
			}

			transform.localScale = new Vector3(Mathf.Clamp(_x, _xMin, _xMax), Mathf.Clamp(_y, _yMin, _yMax), 1);

		}

	}

}