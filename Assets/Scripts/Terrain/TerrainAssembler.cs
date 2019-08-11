using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	[UsedImplicitly]
	public class TerrainAssembler : MonoBehaviour {

		private static TerrainTile[] _tiles;
		private static GameObject _prefab;
		private static Mesh[][] _meshes;
		private static Material _material;
		private static float _index;
		private static int _y;

		public void Initialize (int height, int[] types, bool[] transitionFlags) {
			transform.position = new Vector3(0, 0, Order.GROUND);
			GridDef.Initialize(height, types, transitionFlags);
			InitializeSelf(height);

			for (int y = 0; y < _y; ++y) {
				for (int x = 0; x < _y; x++) {
					Create(x, y);
				}
			}
		}
		
		private static void InitializeSelf (int height) {
			_y = height;
			_tiles = new TerrainTile[_y * _y];
			_prefab = new GameObject("Tile", typeof(MeshFilter), typeof(MeshRenderer), typeof(TerrainTile));

			_meshes = new[] {
				new[] {
					MeshBuilder.GetQuad()
				},
				new[] {
					MeshBuilder.GetCorner(),
					MeshBuilder.GetCorner(1),
					MeshBuilder.GetCorner(2),
					MeshBuilder.GetCorner(3)
				},
				new[] {
					MeshBuilder.GetEdge(),
					MeshBuilder.GetEdge(1),
					MeshBuilder.GetEdge(2),
					MeshBuilder.GetEdge(3)
				},
				new[] {MeshBuilder.GetFull()}
			};

			object[] mi = TerrainMaterial.GetMaterialAndIndex();
			_material = mi[0] as Material;
			_index = (float) mi[1];
		}

		private void Create (int x, int y) {
			Vector3 p = transform.position;
			Vector3 pos = p + new Vector3(x, y);
			GameObject t = Instantiate(_prefab, pos, Quaternion.identity, transform);
			t.GetComponent<MeshRenderer>().sharedMaterial = _material;
			int position = x + y * _y;
			int type = GridDef.MeshTypes[position];
			int rotation = GridDef.MeshRotations[position];
			t.name = $"T {position} {type} {rotation}";
			Mesh mesh = _meshes[type][rotation];

			// Paint vertices
			Color[] col = new Color[mesh.vertexCount];
			int surfaceType = GridDef.Types[position];
			List<int> surfaces = new List<int> { surfaceType };
			Vector4[] uv4 = new Vector4[mesh.vertexCount];

			for (int i = 0; i < mesh.vertexCount; ++i) {
				int c = GridDef.MeshColors[position][i];
				float uvz = surfaceType * _index;
				Color ic;
				Vector4 iv;

				if (c == -1 || c == surfaceType) {
					foreach (int j in GridDef.Neighborhood(type, i, rotation)) {
						iv = uv4[j];
						uv4[j] = new Vector4(iv.x, iv.y, uvz, iv.w);
					}

					ic = col[i];
					col[i] = new Color(1, ic.g, ic.b, ic.a);
					continue;
				}

				if (c != surfaceType && !surfaces.Contains(c)) {
					surfaces.Add(c);
				}

				float uva = c * _index;
				int indexOf = surfaces.IndexOf(c);

				switch (indexOf) {
					case 1:
						foreach (int j in GridDef.Neighborhood(type, i, rotation)) {
							iv = uv4[j];
							uv4[j] = new Vector4(iv.x, iv.y, iv.z, uva);
						}

						ic = col[i];
						col[i] = new Color(ic.r, 1, ic.b, ic.a);
						break;
					case 2:
						foreach (int j in GridDef.Neighborhood(type, i, rotation)) {
							ic = col[j];
							col[j] = new Color(ic.r, ic.g, ic.b, uva);
						}

						ic = col[i];
						col[i] = new Color(ic.r, ic.g, 1, ic.a);
						break;
					default:
						Debug.Log("Too many surfaces on one tile: " + surfaces.Count);
						break;
				}
				
			}

			mesh.colors = col;
			MeshFilter mf = t.GetComponent<MeshFilter>();
			mf.mesh = mesh;
			mf.mesh.SetUVs(0, new List<Vector4>(uv4));

			// Add type to tile
			TerrainTile tile = t.GetComponent<TerrainTile>();
			tile.Type = GridDef.Types[position];
			_tiles[position] = tile;
		}
		
	}

}