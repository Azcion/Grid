using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class Background : MonoBehaviour {

		[UsedImplicitly]
		private void OnEnable () {
			transform.position = new Vector3(TileMaker.THALF, TileMaker.THALF, Order.BACKGROUND);
			transform.localScale = new Vector3(TileMaker.YTILES + 1, TileMaker.YTILES + 1);
		}

		[UsedImplicitly]
		private void Start () {
			MeshRenderer mr = GetComponent<MeshRenderer>();
			mr.material.color = new Color32(35, 35, 35, 255);
			mr.enabled = true;
		}

	}

}