using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	public class TerrainController : MonoBehaviour {

		private static int _height;
		private static int[] _types;

		[SerializeField, UsedImplicitly] private GameObject _tileAssembler = null;
		
		public static void Assign (int height, int[] types) {
			_height = height;
			_types = types;
		}

		[UsedImplicitly]
		private void Start () {
			_tileAssembler.SetActive(true);
			TerrainAssembler assembler = _tileAssembler.GetComponent<TerrainAssembler>();
			assembler.Initialize(_height, _types);
		}

	}

}