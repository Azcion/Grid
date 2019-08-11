using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Utils {

	public class LoadAssetBundles : MonoBehaviour {

		[UsedImplicitly] public string[] AssetBundleNames;

		private static void LoadAssetBundle (string bundleName) {
			string path = Application.isEditor ? Application.dataPath : System.IO.Directory.GetCurrentDirectory();
			path += $"/Bundles/{bundleName}";
			AssetBundle loadedAssets = AssetBundle.LoadFromFile(path);
			Assets.Add(bundleName, loadedAssets);

			Debug.Log(loadedAssets != null ? "Assets loaded" : "Asset loading failed");
		}

		[UsedImplicitly]
		private void Start () {
			foreach (string bundleName in AssetBundleNames) {
				LoadAssetBundle(bundleName);
			}
		}

	}

}