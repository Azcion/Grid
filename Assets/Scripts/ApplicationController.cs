using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {

	public class ApplicationController : MonoBehaviour {

		public static bool Ready;

		// Object references
		#region
		[UsedImplicitly]
		public GameObject InfoBox;

		[UsedImplicitly]
		public GameObject StartButton;

		[UsedImplicitly]
		public GameObject Sun;

		[UsedImplicitly]
		public GameObject TileMaker;

		[UsedImplicitly]
		public GameObject FloraMaker;
		#endregion

		private const int INFO_REFRESH_FRAMES = 8;

		private int _infoRefreshFrame;
		private float _loadTime;
		private Text _i;

		[UsedImplicitly]
		private void OnEnable () {
			_i = InfoBox.GetComponent<Text>();
		}

		[UsedImplicitly]
		private void Awake () {
			//QualitySettings.vSyncCount = 0;
		}

		[UsedImplicitly]
		private void OnClick () {
			float startTime = Time.realtimeSinceStartup;

			Destroy(StartButton);
			Sun.SetActive(true);
			TileMaker.SetActive(true);
			FloraMaker.SetActive(true);
			
			Ready = true;
			_loadTime = Time.realtimeSinceStartup - startTime;
		}

		[UsedImplicitly]
		private void LateUpdate () {
			if (!Ready) {
				return;
			}

			++_infoRefreshFrame;

			if (_infoRefreshFrame % INFO_REFRESH_FRAMES != 0) {
				return;
			}

			GameObject tileObj = CameraController.TileUnderCursor;
			_infoRefreshFrame = 0;
			_i.text = "";
			_i.text += "\n" + DayNightCycle.LightLevel + "% lit";

			if (tileObj != null) {
				Tile t = tileObj.GetComponent<Tile>();
				_i.text += "\n" + t.Chunk.name + " | " + tileObj.name;
				_i.text += "\n" + EnumName.Get(t.Type);
			} else {
				_i.text += "\nVoid";
			}

			_i.text += "\nT: " + _loadTime;
			_i.text += "\nSeed: " + Scripts.TileMaker.Seed;
		}

	}

}