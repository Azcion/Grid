using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	public static class TerrainMaterial {

		public static object[] GetMaterialAndIndex () {
			Texture2D[] textures = new Texture2D[Name.TileType.Length];

			for (int i = 0; i < textures.Length; i++) {
				int j = (int) RemapDuplicate((TileType) i);
				string path = "Terrain/" + Name.Get((TileType) j);
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
			int typeCount = Name.TileType.Length;
			Texture2D tints = new Texture2D(typeCount, 1, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};

			for (int i = 0; i < typeCount; i++) {
				tints.SetPixel(i, 0, Tint.Get((TileType) i));
			}

			tints.Apply();
			Material material = Assets.TerrainMat;
			material.SetTexture("_Textures", textureArray);
			material.SetTexture("_Tints", tints);
			material.SetFloat("_Index", index);

			return new object[] { material, index };
		}

		private static TileType RemapDuplicate (TileType type) {
			switch (type) {
				case TileType.DeepWater:
					return TileType.ShallowWater;
				case TileType.RoughGranite:
				case TileType.RoughLimestone:
				case TileType.RoughMarble:
				case TileType.RoughSandstone:
					return TileType.RoughStone;
				case TileType.RoughHewnGranite:
				case TileType.RoughHewnLimestone:
				case TileType.RoughHewnMarble:
				case TileType.RoughHewnSandstone:
					return TileType.RoughHewnRock;
				case TileType.SmoothGranite:
				case TileType.SmoothLimestone:
				case TileType.SmoothMarble:
				case TileType.SmoothSandstone:
					return TileType.SmoothStone;
			}

			return type;
		}

	}

}