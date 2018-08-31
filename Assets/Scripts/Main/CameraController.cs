using System;
using Assets.Scripts.Makers;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class CameraController : MonoBehaviour {

		public static GameObject TileUnderCursor;
		public static float Size;

		private const float ZOOM_SPEED = 10;
		private const int ZOOM_RATE = 10;
		private const int MAX_SIZE = 35;
		private const int MIN_SIZE = 5;
		private const int INITIAL_SIZE = 10;

		private float _newSize;
		private Camera _camera;
		private Vector3 _lastPosition;

		[UsedImplicitly]
		private void OnEnable () {
			transform.position = new Vector3(TileMaker.THALF, TileMaker.THALF, Order.CAMERA);
			_camera = GetComponent<Camera>();
			_camera.orthographicSize = INITIAL_SIZE;
		}

		[UsedImplicitly]
		private void Start () {
			_newSize = _camera.orthographicSize;
		}

		[UsedImplicitly]
		private void LateUpdate () {
			if (ApplicationController.Ready) {
				DoZoom();
				DoPan();
				DoHover();

				Size = _camera.orthographicSize;
			}
		}

		private void DoZoom () {
			float scroll = Input.GetAxis("Mouse ScrollWheel");

			if (Math.Abs(scroll) > .0001) {
				_newSize -= scroll * ZOOM_SPEED;
				_newSize = Mathf.Clamp(_newSize, MIN_SIZE, MAX_SIZE);
			} else if (Math.Abs(_camera.orthographicSize - _newSize) < .0001) {
				_camera.orthographicSize = _newSize;
				return;
			}

			float delta = ZOOM_RATE * Time.deltaTime;
			_camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _newSize, delta);
		}

		private void DoPan () {
			const float magicSensitivity = -.002f;

			if (Input.GetMouseButtonDown(2)) {
				_lastPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton(2)) {
				float sensitivity = magicSensitivity * _camera.orthographicSize;
				Vector2 delta = Input.mousePosition - _lastPosition;
				_lastPosition = Input.mousePosition;
				transform.Translate(delta * sensitivity);

				Vector3 p = transform.position;
				
				transform.position = new Vector3(
						Mathf.Clamp(p.x, TileMaker.CHALF, TileMaker.YTILES - TileMaker.CHALF),
						Mathf.Clamp(p.y, TileMaker.CHALF, TileMaker.YTILES - TileMaker.CHALF),
						p.z);
			}
		}

		private void DoHover () {
			Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
			TileUnderCursor = TileMaker.Get((int) pos.x, (int) pos.y);
		}

	}

}