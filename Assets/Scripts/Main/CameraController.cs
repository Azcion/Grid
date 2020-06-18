using System;
using Assets.Scripts.Makers;
using Assets.Scripts.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Main {

	public class CameraController : MonoBehaviour {

		public static Camera Main;
		public static Tile TileUnderCursor;

		private const float KEYBOARD_PAN_SPEED = .015f;
		private const float MOUSE_PAN_SPEED = 0.002f;
		private const float ZOOM_SPEED = 10;
		private const int ZOOM_RATE = 20;
		private const int MAX_SIZE = 35;
		private const int MIN_SIZE = 5;
		private const int INITIAL_SIZE = 10;

		private static CameraController _instance;

		private float _newSize;
		private Vector3 _lastPosition;

		public static void PointCameraAtMapCenter () {
			_instance.transform.position = new Vector3(Map.YHalf, Map.YHalf, Order.CAMERA);
		}

		[UsedImplicitly]
		private void Start () {
			_instance = this;
			Main = GetComponent<Camera>();
			Main.orthographicSize = INITIAL_SIZE;
			_newSize = Main.orthographicSize;
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
			if (SelectedInfo.IsHoveringOver) {
				return;
			}

			float scroll = Input.GetAxis("Mouse ScrollWheel");

			if (Math.Abs(scroll) > .0001f) {
				_newSize -= scroll * ZOOM_SPEED;
				_newSize = Mathf.Clamp(_newSize, MIN_SIZE, MAX_SIZE);
			} else if (Math.Abs(Main.orthographicSize - _newSize) < .0001f) {
				Main.orthographicSize = _newSize;
				return;
			}

			float delta = ZOOM_RATE * Time.deltaTime;
			Main.orthographicSize = Mathf.MoveTowards(Main.orthographicSize, _newSize, delta);
		}

		private void DoPan () {
			if (Input.GetMouseButtonDown(2)) {
				_lastPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton(2)) {
				float sensitivity = -MOUSE_PAN_SPEED * Main.orthographicSize;
				Vector2 delta = Input.mousePosition - _lastPosition;
				_lastPosition = Input.mousePosition;
				transform.Translate(delta * sensitivity);

				Vector3 p = transform.position;
				transform.position = new Vector3(Clamp(p.x), Clamp(p.y), p.z);
			}
		}

		private void DoHover () {
			Vector2 pos = Main.ScreenToWorldPoint(Input.mousePosition);
			TileUnderCursor = TileMaker.GetTile((int) pos.x, (int) pos.y);
		}

		private float Clamp (float value) {
			return Mathf.Clamp(value, 1, Map.YTiles - 1);
		}

	}

}