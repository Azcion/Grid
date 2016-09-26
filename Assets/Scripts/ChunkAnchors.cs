using UnityEngine;

namespace Assets.Scripts {

	public class ChunkAnchors {

		public static GameObject[,] Anchors;

		public static float Scale;

		public ChunkAnchors (GameObject parent, int yChunks) {
			const float offset = -.5f;
			float yCf = yChunks;
			float oneDivY = 1 / yCf;
			float yScale = oneDivY * 100 / TileSprite.NONE.rect.width;

			Anchors = new GameObject[yChunks, yChunks];
			Scale = yScale;

			for (int y = 0; y < yChunks; ++y) {
				for (int x = 0; x < yChunks; ++x) {
					GameObject chunk = new GameObject("Chunk " + y + " " + x);
					chunk.transform.SetParent(parent.transform);
					chunk.SetActive(true);
					chunk.transform.localPosition = new Vector3(y / yCf + offset, x / yCf + offset);

					Anchors[y, x] = chunk;
				}
			}
		}

	}

}