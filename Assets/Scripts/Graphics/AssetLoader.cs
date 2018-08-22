using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public class AssetLoader : MonoBehaviour {

		public static Material DiffuseMat;

		[UsedImplicitly]
		private void Start () {
			DiffuseMat = Resources.Load<Material>("Materials/Diffuse");
		}

	}

}