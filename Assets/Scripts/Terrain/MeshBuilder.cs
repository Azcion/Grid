using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	public static class MeshBuilder {

		private static readonly Vector3 N = -Vector3.forward;

		public static Mesh GetFull () {
			Vector3[] vertices = {
				new Vector3(0, 0),
				new Vector3(.5f, 0), 
				new Vector3(1, 0),
				new Vector3(0, .5f),
				new Vector3(.5f, .5f), 
				new Vector3(1, .5f), 
				new Vector3(0, 1),
				new Vector3(.5f, 1), 
				new Vector3(1, 1)
			};

			int[] tris = {
				0, 3, 1,
				4, 1, 3,
				4, 5, 1,
				2, 1, 5,
				6, 7, 3,
				4, 3, 7,
				4, 7, 5,
				8, 5, 7
			};

			Mesh mesh = new Mesh {
				vertices = vertices, 
				triangles = tris, 
				normals = new[] { N, N, N, N, N, N, N, N, N }, 
				uv = V3ToV2(vertices)
			};

			return mesh;
		}

		public static Mesh GetEdge (int rotation = 0) {
			Vector3[] vertices;
			int[] tris;

			switch (rotation) {
				case 1:
				case 3:
					vertices = new[] {
						new Vector3(0, 0),
						new Vector3(.5f, 0),
						new Vector3(1, 0),
						new Vector3(0, 1),
						new Vector3(.5f, 1),
						new Vector3(1, 1)
					};

					tris = new[] {
						0, 3, 4,
						0, 4, 1,
						1, 4, 5,
						1, 5, 2
					};

					break;
				default:
					vertices = new[] {
						new Vector3(0, 0),
						new Vector3(1, 0),
						new Vector3(0, .5f),
						new Vector3(1, .5f), 
						new Vector3(0, 1),
						new Vector3(1, 1)
					};

					tris = new[] {
						0, 2, 1,
						3, 1, 2,
						2, 4, 3,
						5, 3, 4
					};

					break;
			}

			Mesh mesh = new Mesh {
				vertices = vertices, 
				triangles = tris, 
				normals = new[] { N, N, N, N, N, N }, 
				uv = V3ToV2(vertices)
			};

			return mesh;
		}

		public static Mesh GetCorner (int rotation = 0) {
			Vector3[] vertices;
			int[] tris;

			switch (rotation) {
				case 1:
					vertices = new[] {
						new Vector3(0, 0),
						new Vector3(.5f, 0),
						new Vector3(1, 0),
						new Vector3(0, .5f),
						new Vector3(0, 1),
						new Vector3(1, 1)
					};

					tris = new[] {
						0, 3, 1,
						2, 1, 5,
						3, 5, 1,
						4, 5, 3
					};

					break;
				case 2:
					vertices = new[] {
						new Vector3(0, 0),
						new Vector3(.5f, 0),
						new Vector3(1, 0),
						new Vector3(1, .5f),
						new Vector3(0, 1),
						new Vector3(1, 1)
					};

					tris = new[] {
						0, 4, 1,
						1, 4, 3,
						2, 1, 3,
						5, 3, 4
					};

					break;
				case 3:
					vertices = new[] {
						new Vector3(0, 0),
						new Vector3(1, 0),
						new Vector3(1, .5f),
						new Vector3(0, 1),
						new Vector3(.5f, 1),
						new Vector3(1, 1)
					};

					tris = new[] {
						0, 2, 1,
						0, 3, 4,
						0, 4, 2,
						5, 2, 4
					};

					break;
				default:
					vertices = new[] {
						new Vector3(0, 0),
						new Vector3(1, 0),
						new Vector3(0, .5f),
						new Vector3(0, 1),
						new Vector3(.5f, 1),
						new Vector3(1, 1)
					};

					tris = new[] {
						0, 2, 1,
						2, 4, 1,
						3, 4, 2,
						5, 1, 4
					};

					break;
			}

			Mesh mesh = new Mesh {
				vertices = vertices, 
				triangles = tris, 
				normals = new[] { N, N, N, N, N, N }, 
				uv = V3ToV2(vertices)
			};

			return mesh;
		}

		public static Mesh GetQuad () {
			Vector3[] vertices = {
				new Vector3(0, 0),
				new Vector3(1, 0),
				new Vector3(0, 1),
				new Vector3(1, 1)
			};

			int[] tris = {
				0, 2, 1,
				3, 1, 2
			};

			Mesh mesh = new Mesh {
				vertices = vertices, 
				triangles = tris, 
				normals = new[] { N, N, N, N }, 
				uv = V3ToV2(vertices)
			};

			return mesh;
		}

		private static Vector2[] V3ToV2 (IReadOnlyList<Vector3> arr3) {
			Vector2[] arr2 = new Vector2[arr3.Count];

			for (int i = 0; i < arr3.Count; i++) {
				Vector3 v = arr3[i];
				arr2[i] = new Vector2(v.x, v.y);
			}

			return arr2;
		}

	}


}