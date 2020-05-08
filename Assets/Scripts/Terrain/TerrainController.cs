using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	public class TerrainController : MonoBehaviour {

		private static int[] _types;
		private static bool[] _transitionFlags;

		[SerializeField, UsedImplicitly] private GameObject _tileAssembler = null;
		
		public static void Assign (int[] types, bool[] transitionFlags) {
			_types = types;
			_transitionFlags = transitionFlags;
		}

		[UsedImplicitly]
		private void Start () {
			_tileAssembler.SetActive(true);
			TerrainAssembler assembler = _tileAssembler.GetComponent<TerrainAssembler>();
			assembler.Initialize(_types, _transitionFlags);
		}

	}

}