using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using UnityEngine;

namespace Assets.Scripts.Things {

	public static class CoverMaterial {

		public static object[] GetMaterialAndIndices () {
			int linkedTypeCount = Name.LinkedType.Length;
			int thingMaterialCount = Name.ThingMaterial.Length;

			Texture2D bases = new Texture2D(linkedTypeCount, 1, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};

			Texture2D tints = new Texture2D(thingMaterialCount, 1, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};

			for (int i = 0; i < linkedTypeCount; ++i) {
				bases.SetPixel(i, 0, Tint.Get((LinkedType) i));
			}

			for (int i = 0; i < thingMaterialCount; ++i) {
				tints.SetPixel(i, 0, Tint.Get((ThingMaterial) i));
			}

			bases.Apply();
			tints.Apply();
			float baseIndex = 1f / linkedTypeCount * 1.01f;
			float tintIndex = 1f / thingMaterialCount * 1.01f;
			Material material = Assets.CoverMat;
			material.SetTexture("_Bases", bases);
			material.SetTexture("_Tints", tints);

			return new object[] { material, baseIndex, tintIndex };
		}
		
	}

}