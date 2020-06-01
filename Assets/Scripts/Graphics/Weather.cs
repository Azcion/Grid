using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class Weather : MonoBehaviour {

		private static MeshRenderer _renderer;

		public static void SetActive (bool flag) {
			_renderer.enabled = flag;
		}

		[UsedImplicitly]
		private void Start () {
			transform.position = new Vector3(Map.YHalf, Map.YHalf, Order.WEATHER);
			transform.localScale = new Vector3(Map.YTiles, Map.YTiles);
			_renderer = GetComponent<MeshRenderer>();
			_renderer.enabled = true;
		}
		
	}

}