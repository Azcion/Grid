using System;
using Assets.Scripts.Makers;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class CameraController : MonoBehaviour {

		public static Tile TileUnderCursor;

		private const float KEYBOARD_PAN_SPEED = .01f;
		private const float ZOOM_SPEED = 10;
		private const int ZOOM_RATE = 10;
		private const int MAX_SIZE = 35;
		private const int MIN_SIZE = 5;
		private const int INITIAL_SIZE = 10;

		private static CameraController _instance;

		private int _cHalf;
		private float _newSize;
		private Camera _camera;
		private Vector3 _lastPosition;

		public static void PointCameraAtMapCenter () {
			_instance.transform.position = new Vector3(Map.YHalf, Map.YHalf, Order.CAMERA);
		}

		[UsedImplicitly]
		private void Start () {
			_instance = this;
			_camera = GetComponent<Camera>();
			_camera.orthographicSize = INITIAL_SIZE;
			_newSize = _camera.orthographicSize;
			_cHalf = Map.CSIZE / 2;
		}

		[UsedImplicitly]
		private void LateUpdate () {
			if (!ApplicationController.Ready) {
				return;
			}

			DoKeyboard();
			DoZoom();
			DoPan();
			DoHover();
		}

		private void DoKeyboard () {
			if (!Input.anyKey) {
				return;
			}

			float speed = KEYBOARD_PAN_SPEED * _newSize;
			Vector3 target = transform.position;

			if (Input.GetKey(KeyCode.W)) {
				target.y += speed;
			}

			if (Input.GetKey(KeyCode.S)) {
				target.y -= speed;
			}

			if (Input.GetKey(KeyCode.A)) {
				target.x -= speed;
			}

			if (Input.GetKey(KeyCode.D)) {
				target.x += speed;
			}

			transform.position = target;
		}

		private void DoZoom () {
			float scroll = Input.GetAxis("Mouse ScrollWheel");

			if (Math.Abs(scroll) > .0001f) {
				_newSize -= scroll * ZOOM_SPEED;
				_newSize = Mathf.Clamp(_newSize, MIN_SIZE, MAX_SIZE);
			} else if (Math.Abs(_camera.orthographicSize - _newSize) < .0001f) {
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
				transform.position = new Vector3(Clamp(p.x), Clamp(p.y), p.z);
			}
		}

		private void DoHover () {
			Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
			TileUnderCursor = TileMaker.GetTile((int) pos.x, (int) pos.y);
		}

		private float Clamp (float value) {
			return Mathf.Clamp(value, _cHalf, Map.YTiles - _cHalf);
		}

	}

}