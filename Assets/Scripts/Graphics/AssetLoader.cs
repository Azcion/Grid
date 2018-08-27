using UnityEngine;

namespace Assets.Scripts.Graphics {

	public static class AssetLoader {

		public static readonly Material DiffuseMat;

		static AssetLoader () {
			DiffuseMat = Resources.Load<Material>("Materials/Diffuse");
		}

	}

}