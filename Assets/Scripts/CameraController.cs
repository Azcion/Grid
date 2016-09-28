using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {

	public class CameraController : MonoBehaviour {

		private const float DURATION = 1f;
		private const float ZOOM_RATE = 35f;
		private const float MIN_ZOOM = 1f;
		private const float MAX_ZOOM = .2f;

		private Camera _camera;
		private Vector3 _lastPosition;

		private bool _zoomInTransition;
		private float _zoomElapsed;

		private Transform _tileInfo;
		private Text _tileInfoText;

		[UsedImplicitly]
		public void Awake () {
			Application.targetFrameRate = 120;
			QualitySettings.vSyncCount = 0;
		}

		[UsedImplicitly]
		private void Start () {
			_camera = transform.GetComponent<Camera>();
			_camera.orthographicSize = MIN_ZOOM;
			_zoomElapsed = 0;
			_tileInfo = GameObject.Find("Canvas").transform.FindChild("Tile Info");
			_tileInfoText = _tileInfo.GetComponent<Text>();
		}

		[UsedImplicitly]
		private void Update () {
			InputMouseHover();
			InputCameraZoom();
			InputCameraPan();
		}

		private void InputMouseHover () {
			if (_lastPosition == Input.mousePosition) {
				return;
			}

			Vector2 position = new Vector2(
				_camera.ScreenToWorldPoint(Input.mousePosition).x,
				_camera.ScreenToWorldPoint(Input.mousePosition).y);

			RaycastHit2D tile = Physics2D.Raycast(position, Vector2.zero, 100f);

			if (tile) {
				Tile t = tile.transform.GetComponent<TileID>().Tile;
				_tileInfoText.text = "";
				_tileInfoText.text += (int) (DayNightCycle.Progress * 100) + "% lit\n";
				_tileInfoText.text += t.Chunk.GlobalID + ":" + t.LocalID + "\n";
				_tileInfoText.text += t.Name;
			} else {
				_tileInfoText.text = "Void";
			}
		}

		private void InputCameraZoom () {
			float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

			if (!_zoomInTransition) {
				if (Mathf.Abs(scrollDelta) > .0001) {
					_zoomInTransition = true;
					_zoomElapsed = 0f;
				} else {
					return;
				}
			}

			_zoomElapsed += Time.deltaTime * DURATION;
			float size = _camera.orthographicSize;
			float newSize = size - ZOOM_RATE * scrollDelta;
			_camera.orthographicSize = Mathf.Lerp(size, newSize, Time.deltaTime / DURATION);

			if (_camera.orthographicSize < MAX_ZOOM) {
				_camera.orthographicSize = MAX_ZOOM;
			} else if (_camera.orthographicSize > MIN_ZOOM) {
				_camera.orthographicSize = MIN_ZOOM;
			}

			if (_zoomElapsed > DURATION) {
				_zoomInTransition = false;
			}
		}

		// todo determine @magic value
		private void InputCameraPan () {
			const float magicSensitivity = -.00225f; // -.0043f

			if (Input.GetMouseButtonDown(2)) {
				_lastPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton(2)) {
				float sensitivity = magicSensitivity * _camera.orthographicSize;
				Vector2 delta = Input.mousePosition - _lastPosition;
				transform.Translate(delta.x * sensitivity, delta.y * sensitivity, 0);
				_lastPosition = Input.mousePosition;
			}
		}
	}

}