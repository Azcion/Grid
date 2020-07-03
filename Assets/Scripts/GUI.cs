using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class GUI : MonoBehaviour {

		public static bool Busy {
			get => _instance._busy;
			set => _instance._busy = value;
		}

		private static GUI _instance;

		[UsedImplicitly, SerializeField] private bool _busy;

		[UsedImplicitly]
		private void Start () {
			_instance = this;
		}

	}

}