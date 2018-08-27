using System.Collections;
using Assets.Scripts.Graphics;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Main {

	public class ApplicationController : MonoBehaviour {

		public static int Seed;
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

		private static bool _ready;
		private static float _loadTime;
		private static float _startTime;

		private int _infoRefreshFrame;
		private Text _i;

		public static void NotifyReady () {
			_loadTime = Time.realtimeSinceStartup - _startTime;
		}

		private static IEnumerator SetReady () {
			yield return new WaitForSeconds(2);

			Ready = true;
		}

		[UsedImplicitly]
		private void OnEnable () {
			Seed = (int) (Random.value * 1000000);
			_i = InfoBox.GetComponent<Text>();
		}

		[UsedImplicitly]
		private void Awake () {
			//QualitySettings.vSyncCount = 0;
		}

		[UsedImplicitly]
		private void OnClick () {
			_startTime = Time.realtimeSinceStartup;

			Destroy(StartButton);
			Sun.SetActive(true);
			TileMaker.SetActive(true);
			FloraMaker.SetActive(true);
			_ready = true;

			StartCoroutine(SetReady());
		}

		[UsedImplicitly]
		private void LateUpdate () {
			if (!_ready) {
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

			_i.text += "\nLoad: " + _loadTime.ToString("n2") + "s";
			_i.text += "\nSeed: " + Seed;
		}

	}

}