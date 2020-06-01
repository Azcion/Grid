using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class Weather : MonoBehaviour {

		[UsedImplicitly]
		private void Start () {
			transform.position = new Vector3(Map.YHalf, Map.YHalf, Order.WEATHER);
			transform.localScale = new Vector3(Map.YTiles, Map.YTiles);
			MeshRenderer mr = GetComponent<MeshRenderer>();
			mr.enabled = true;
		}
		
	}

}