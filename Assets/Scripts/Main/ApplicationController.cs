using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Makers;
using Assets.Scripts.UI;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Main {

	public class ApplicationController : MonoBehaviour {

		public static int Seed;
		public static bool Ready;

		private const int INFO_REFRESH_FRAMES = 8;

		private static readonly Dictionary<TileType, string> TileLabels;

		private static bool _ready;
		private static float _loadTime;
		private static float _startTime;

		private int _infoRefreshFrame;
		private Text _i;

		[UsedImplicitly, SerializeField] private GameObject _background = null;
		[UsedImplicitly, SerializeField] private GameObject _infoBox = null;
		[UsedImplicitly, SerializeField] private GameObject _architectButton = null;
		[UsedImplicitly, SerializeField] private GameObject _sun = null;
		[UsedImplicitly, SerializeField] private GameObject _tileMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _coverAssembler = null;
		[UsedImplicitly, SerializeField] private GameObject _wallMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _plantMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _animalMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _pathfinder = null;
		[UsedImplicitly, SerializeField] private GameObject _terrainAppController = null;
		[UsedImplicitly, SerializeField] private GameObject _itemMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _weather = null;

		static ApplicationController () {
			TileLabels = new Dictionary<TileType, string>();

			for (int i = 0; i < Name.TileType.Length; ++i) {
				string label = Name.TileType[i];
				label = Format.SeparateAtCapitalLetters(label, ' ');
				TileLabels.Add((TileType) i, label.ToLower());
			}
		}

		public static void NotifyReady () {
			_loadTime = Time.realtimeSinceStartup - _startTime;
		}

		[UsedImplicitly]
		public void OnStart () {
			StartCoroutine(Initialize());
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

			Tile t = CameraController.TileUnderCursor;
			_infoRefreshFrame = 0;
			_i.text = "";
			_i.text += $"\n{DayNightCycle.LightLevel}% lit";

			if (t != null) {
				//_i.text += $"\nPenalty: {t.Penalty}";
				//_i.text += $"\n{(t.Walkable ? "" : "not ")}walkable, ";
				//_i.text += $"{(t.Buildable ? "" : "not ")}buildable";
				//_i.text += $"\n{t.Chunk.name} | {t.name}";
				_i.text += $"\n{TileLabels[t.Type]}";
			} else {
				_i.text += "\nVoid";
			}

			//_i.text += $"\nLoad: {_loadTime:n2}s";
			//_i.text += $"\nSeed: {Seed}";
		}

		private IEnumerator Initialize () {
			_startTime = Time.realtimeSinceStartup;
			Seed = Utils.Seed.Get(StartInterface.GetSeed);
			Map.InitializeMapMeasurements(StartInterface.GetMapSize / Map.CSIZE);
			Random.InitState(Seed);
			_i = _infoBox.GetComponent<Text>();
			StartInterface.TriggerStart();

			yield return new WaitForSeconds(.05f);

			_architectButton.SetActive(true);
			_background.SetActive(true);
			CameraController.PointCameraAtMapCenter();
			_sun.SetActive(true);
			Tint.Initialize();
			_tileMaker.SetActive(true);
			_terrainAppController.SetActive(true);
			_coverAssembler.SetActive(true);
			_wallMaker.SetActive(true);
			_plantMaker.SetActive(true);
			_animalMaker.SetActive(true);
			_pathfinder.SetActive(true);
			_itemMaker.SetActive(true);
			_weather.SetActive(true);
			_ready = true;
			Ready = true;
		}

	}

}