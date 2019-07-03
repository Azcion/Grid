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

		public static Object GetAsset (string bundleName, string assetName) {
			bool didGetBundle = Bundles.TryGetValue(bundleName, out AssetBundle bundle);
			return !didGetBundle ? null : bundle.LoadAsset(assetName);
		}

		public static Material GetMaterial (string bundleName, string assetName) {
			return GetAsset(bundleName, assetName) as Material;
		}

	}

}