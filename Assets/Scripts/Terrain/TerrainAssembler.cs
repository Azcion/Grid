using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	[UsedImplicitly]
	public class TerrainAssembler : MonoBehaviour {

		private static GameObject _prefab;
		private static GameObject[] _chunks;
		private static Mesh[][] _meshes;
		private static Material _material;
		private static float _index;

		public void Initialize (int[] types, bool[] transitionFlags) {
			GridDef.Initialize(types, transitionFlags);
			InitializeSelf(); 
			CombineInstance[] chunkMeshCombines = new CombineInstance[Map.CSIZE * Map.CSIZE];

			for (int yc = 0; yc < Map.YChunks; ++yc) {
				for (int xc = 0; xc < Map.YChunks; xc++) {
					Vector3 position = new Vector3(xc * Map.CSIZE, yc * Map.CSIZE, Order.GROUND);
					GameObject chunk = Instantiate(_prefab, position, Quaternion.identity, transform);
					chunk.name = $"Chunk {yc} {xc}";
					chunk.GetComponent<MeshRenderer>().sharedMaterial = _material;
					MeshFilter mf = chunk.GetComponent<MeshFilter>();

					for (int yt = 0; yt < Map.CSIZE; yt++) {
						for (int xt = 0; xt < Map.CSIZE; xt++) {
							int tilePosition = yc * Map.CSIZE * Map.YTiles + xc * Map.CSIZE + Map.YTiles * yt + xt;
							CombineInstance mesh = Create(mf, tilePosition, xt, yt);
							chunkMeshCombines[yt * Map.CSIZE + xt] = mesh;
						}
					}

					Mesh chunkMesh = new Mesh();
					chunkMesh.CombineMeshes(chunkMeshCombines, true, true);
					chunkMesh.name = chunk.name;
					mf.mesh = chunkMesh;
					chunk.SetActive(true);
					_chunks[yc * Map.YChunks + xc] = chunk;
				}
			}
		}
		
		private static void InitializeSelf () {
			_chunks = new GameObject[Map.YChunks * Map.YChunks];
			_prefab = new GameObject("Chunk Prefab", typeof(MeshFilter), typeof(MeshRenderer));
			_prefab.SetActive(false);

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

		private CombineInstance Create (MeshFilter mf, int position, int xt, int yt) {
			int type = GridDef.MeshTypes[position];
			int rotation = GridDef.MeshRotations[position];
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
			mf.mesh = mesh;
			mf.mesh.SetUVs(0, new List<Vector4>(uv4));

			CombineInstance combine = new CombineInstance {
				mesh = mf.mesh,
				transform = Matrix4x4.TRS(new Vector3(xt, yt), transform.localRotation, transform.localScale)
			};

			return combine;
		}
		
	}

}