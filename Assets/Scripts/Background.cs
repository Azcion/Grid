using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts {

	public class Background : MonoBehaviour {

		[UsedImplicitly]
		private void Start () {
			transform.position = new Vector3(TileMaker.THALF, TileMaker.THALF, transform.position.z);
			transform.localScale = new Vector3(TileMaker.YTILES + 1, TileMaker.YTILES + 1);
			MeshRenderer mr = GetComponent<MeshRenderer>();
			mr.material.color = new Color32(35, 35, 35, 255);
			mr.enabled = true;
		}

	}

}