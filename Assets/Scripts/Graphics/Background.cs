using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class Background : MonoBehaviour {

		[UsedImplicitly]
		private void Start () {
			transform.position = new Vector3(Map.YHalf, Map.YHalf, Order.BACKGROUND);
			transform.localScale = new Vector3(Map.YTiles + 1, Map.YTiles + 1);
			MeshRenderer mr = GetComponent<MeshRenderer>();
			mr.enabled = true;
		}

	}

}