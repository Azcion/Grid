using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	public static class TerrainMaterial {

		public static object[] GetMaterialAndIndex () {
			string[] typeNames = Enum.GetNames(typeof(TileType));
			Texture2D[] textures = new Texture2D[typeNames.Length];

			for (int i = 0; i < textures.Length; i++) {
				int j = RemapDuplicate(i);
				string path = "Terrain/" + typeNames[j];
				textures[i] = Resources.Load<Texture2D>(path);
			}

			Texture2D t = textures[0];
			Texture2DArray textureArray = new Texture2DArray(t.width, t.height, textures.Length, TextureFormat.RGBA32, true, false) {
				filterMode = FilterMode.Point, 
				wrapMode = TextureWrapMode.Repeat
			};

			for (int i = 0; i < textures.Length; ++i) {
				textureArray.SetPixels(textures[i].GetPixels(0), i, 0);
			}

			textureArray.Apply();
			float index = 1f / (textures.Length - 1);
			int typeCount = Enum.GetNames(typeof(TileType)).Length;
			Texture2D tints = new Texture2D(typeCount, 1, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};

			for (int i = 0; i < typeCount; i++) {
				tints.SetPixel(i, 0, TileTint.Get((TileType) i));
			}

			tints.Apply();
			Material material = Assets.TerrainMat;
			material.SetTexture("_Textures", textureArray);
			material.SetTexture("_Tints", tints);
			material.SetFloat("_Index", index);

			return new object[] { material, index };
		}

		private static int RemapDuplicate (int i) {
			if ((TileType) i == TileType.DeepWater) {
				return (int) TileType.ShallowWater;
			}

			return i;
		}

	}

}