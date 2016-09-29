using UnityEngine;

namespace Assets.Scripts {

	public class ChunkAnchors {

		public static GameObject[,] Anchors;

		public ChunkAnchors (GameObject parent, int yChunks) {
			const float offset = -.5f;
			float yCf = yChunks;

			Anchors = new GameObject[yChunks, yChunks];
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