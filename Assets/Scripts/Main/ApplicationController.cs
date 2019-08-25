using Assets.Scripts.Graphics;
using Assets.Scripts.Makers;
using Assets.Scripts.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Main {

	public class ApplicationController : MonoBehaviour {

		public static int Seed;
		public static bool Ready;

		private const int INFO_REFRESH_FRAMES = 8;

		private static readonly string[] TileLabels = {
			"deep water", "shallow water", "marsh", "marshy terrain", "mud", "mossy",
			"sand", "soft sand", "soil", "rich soil", "gravel", "packed dirt", "ice",
			"rough stone", "rough-hewn rock", "smooth stone", "carpet", "concrete",
			"flagstone", "generic floor tile", "paved tile", "stone tile", "wood floor"
		};

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
		[UsedImplicitly, SerializeField] private GameObject _wallMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _plantMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _animalMaker = null;
		[UsedImplicitly, SerializeField] private GameObject _pathfinder = null;
		[UsedImplicitly, SerializeField] private GameObject _terrainAppController = null;

		[UsedImplicitly]
		public void OnStart () {
			_startTime = Time.realtimeSinceStartup;

			Seed = Utils.Seed.Get(StartInterface.GetSeed);
			Map.InitializeMapMeasurements(StartInterface.GetMapSize / Map.CSIZE);
			Random.InitState(Seed);
			_i = _infoBox.GetComponent<Text>();
			StartInterface.Hide();
			_architectButton.SetActive(true);
			_background.SetActive(true);
			CameraController.PointCameraAtMapCenter();
			_sun.SetActive(true);
			TileTint.Initialize();
			_tileMaker.SetActive(true);
            _terrainAppController.SetActive(true);
			_wallMaker.SetActive(true);
			_plantMaker.SetActive(true);
			_animalMaker.SetActive(true);
			_pathfinder.SetActive(true);
			_ready = true;

			SetReady();
		}

		public static void NotifyReady () {
			_loadTime = Time.realtimeSinceStartup - _startTime;
		}

		private static void SetReady () {
			Ready = true;
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
				_i.text += $"\n{TileLabels[(int) t.Type]}";
			} else {
				_i.text += "\nVoid";
			}

			//_i.text += $"\nLoad: {_loadTime:n2}s";
			//_i.text += $"\nSeed: {Seed}";
		}

	}

}