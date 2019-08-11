using System;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public static class AssetLoader {

		public static readonly Material DiffuseMat;

		public static readonly Sprite RockTop;
		public static readonly Sprite WoodTop;

		private static readonly Sprite[][] LinkedSprites;
		private static readonly string[] LinkedNames;
		private static readonly int LinkedTypeCount;

		static AssetLoader () {
			DiffuseMat = Resources.Load<Material>("Materials/Diffuse");
			
			RockTop = Resources.Load<Sprite>("sprites/other/RockTop");
			WoodTop = Resources.Load<Sprite>("sprites/other/WoodTop");

			LinkedNames = Enum.GetNames(typeof(LinkedType));
			LinkedTypeCount = LinkedNames.Length;
			LinkedSprites = new Sprite[LinkedTypeCount][];
			LoadLinked();
		}

		public static Sprite Get (LinkedType linked, int index) {
			return LinkedSprites[(int) linked][index];
		}

		private static void LoadLinked () {
			for (int i = 0; i < LinkedTypeCount; ++i) {
				string linked = LinkedNames[i];
				string loc = $"sprites/thing/building/linked/{linked}_Atlas";
				LinkedSprites[i] = Resources.LoadAll<Sprite>(loc);

				foreach (Sprite s in LinkedSprites[i]) {
					if (s == null) {
						Debug.Log($"Couldn't load {linked} properly in AssetLoader.");
					}
				}
			}
		}

	}

}