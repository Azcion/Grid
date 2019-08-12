using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

	public static class Assets {

		public static readonly Material TerrainMat;
		public static readonly Material ThingMat;

		private static readonly Dictionary<string, AssetBundle> Bundles;

		static Assets () {
			Bundles = new Dictionary<string, AssetBundle>();
			TerrainMat = Resources.Load<Material>("Materials/Terrain");
			ThingMat = Resources.Load<Material>("Materials/Thing");
		}

		public static void Add (string bundleName, AssetBundle bundle) {
			Bundles.Add(bundleName, bundle);
		}

		public static Sprite GetSprite (string assetName) {
			return GetAsset("sprites", assetName) as Sprite;
		}

		public static Sprite GetAtlasSprite (string assetName, int index) {
			return GetAtlasAsset("sprites", assetName)[index + 1] as Sprite;
		}

		private static Object GetAsset (string bundleName, string assetName) {
			bool didGetBundle = Bundles.TryGetValue(bundleName, out AssetBundle bundle);
			return !didGetBundle ? null : bundle.LoadAsset(assetName);
		}

		private static Object[] GetAtlasAsset (string bundleName, string assetName) {
			bool didGetBundle = Bundles.TryGetValue(bundleName, out AssetBundle bundle);
			return !didGetBundle ? null : bundle.LoadAssetWithSubAssets(assetName);
		}

	}

}