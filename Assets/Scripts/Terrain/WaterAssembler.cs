using System.Collections.Generic;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Terrain {

	public class WaterAssembler : MonoBehaviour {

		private static List<CombineInstance>[,] _chunks;
		private static bool[,] _changed;
		private static GameObject[,] _chunkObjects;
		private static MeshFilter[,] _meshFilters;
		private static GameObject _prefab;
		private static Mesh _quad;
		private static Transform _container;
		private static Material _material;

		public static void Apply () {
			for (int y = 0; y < Map.YChunks; ++y) {
				for (int x = 0; x < Map.YChunks; ++x) {
					if (!_changed[y, x]) {
						continue;
					}

					_changed[y, x] = false;
					GameObject chunkObject = _chunkObjects[y, x];
					MeshFilter meshFilter;

					if (chunkObject == null) {
						chunkObject = Instantiate(_prefab, new Vector3(0, 0, Order.COVER), Quaternion.identity, _container);
						chunkObject.name = $"Chunk {y} {x}";
						meshFilter = chunkObject.GetComponent<MeshFilter>();
						_chunkObjects[y, x] = chunkObject;
						_meshFilters[y, x] = meshFilter;
					} else {
						meshFilter = _meshFilters[y, x];
					}

					List<CombineInstance> combines = _chunks[y, x];

					if (combines.Count == 0) {
						meshFilter.mesh.Clear();
						Destroy(chunkObject);
						_chunkObjects[y, x] = null;
						_meshFilters[y, x] = null;
						continue;
					}

					meshFilter.mesh.Clear();
					meshFilter.mesh.CombineMeshes(combines.ToArray(), true, true);
					chunkObject.SetActive(true);
				}
			}
		}

		public static void Make (int x, int y) {
			Vector3 position = new Vector3(x, y);

			CombineInstance combine = new CombineInstance {
				mesh = _quad,
				transform = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one)
			};

			int yc = y / Map.CSIZE;
			int xc = x / Map.CSIZE;
			_chunks[yc, xc].Add(combine);
			_changed[yc, xc] = true;
		}

		[UsedImplicitly]
		private void Start () {
			_chunks = new List<CombineInstance>[Map.YChunks, Map.YChunks];

			for (int y = 0; y < Map.YChunks; ++y) {
				for (int x = 0; x < Map.YChunks; ++x) {
					_chunks[y, x] = new List<CombineInstance>();
				}
			}

			_changed = new bool[Map.YChunks, Map.YChunks];
			_chunkObjects = new GameObject[Map.YChunks, Map.YChunks];
			_meshFilters = new MeshFilter[Map.YChunks, Map.YChunks];
			_prefab = new GameObject("Chunk Prefab", typeof(MeshFilter), typeof(MeshRenderer));
			_prefab.SetActive(false);
			_material = Assets.WaterMat;
			_prefab.GetComponent<MeshRenderer>().sharedMaterial = _material;
			_quad = MeshBuilder.GetQuad();
			_container = transform;
		}

	}

}