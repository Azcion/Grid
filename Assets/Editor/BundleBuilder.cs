using UnityEditor;
using UnityEngine;

namespace Assets.Editor {

	public class BundleBuilder : UnityEditor.Editor {

		[MenuItem("Assets/ Build AssetBundles")]
		private static void BuildAssetBundles () {
			string path = Application.dataPath + "/Bundles";
			BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
		}

	}

}