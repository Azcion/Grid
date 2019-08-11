using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils {

	public static class Assets {

		private static readonly Dictionary<string, AssetBundle> Bundles;

		static Assets () {
			Bundles = new Dictionary<string, AssetBundle>();
		}

		public static void Add (string bundleName, AssetBundle bundle) {
			Bundles.Add(bundleName, bundle);
		}

		public static Material GetMaterial (string assetName) {
			return GetAsset("materials", assetName) as Material;
		}

		public static Sprite GetSprite (string assetName) {
			return GetAsset("sprites", assetName) as Sprite;
		}

		public static Object GetAsset (string bundleName, string assetName) {
			bool didGetBundle = Bundles.TryGetValue(bundleName, out AssetBundle bundle);
			return !didGetBundle ? null : bundle.LoadAsset(assetName);
		}

	}

}