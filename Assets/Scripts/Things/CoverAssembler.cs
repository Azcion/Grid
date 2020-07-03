using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class CoverAssembler : MonoBehaviour {

		private static readonly Dictionary<LinkedType, HashSet<ThingMaterial>> Combinations;
		private static readonly Dictionary<LinkedType, Dictionary<ThingMaterial, Mesh>> Meshes;

		private static List<CombineInstance>[,] _chunks;
		private static bool[,] _changed;
		private static GameObject[,] _chunkObjects;
		private static MeshFilter[,] _meshFilters;
		private static GameObject _prefab;
		private static Mesh _quad;
		private static Transform _container;
		private static Material _material;

		static CoverAssembler () {
			Combinations = new Dictionary<LinkedType, HashSet<ThingMaterial>> {
				{LinkedType.Rock, new HashSet<ThingMaterial> {
					ThingMaterial.Granite,
					ThingMaterial.Limestone,
					ThingMaterial.Marble,
					ThingMaterial.Sandstone
				}},
				{LinkedType.Bricks, new HashSet<ThingMaterial> {
					ThingMaterial.Granite,
					ThingMaterial.Limestone,
					ThingMaterial.Marble,
					ThingMaterial.Sandstone
				}},
				{LinkedType.Planks, new HashSet<ThingMaterial> {
					ThingMaterial.Wood
				}},
				{LinkedType.Smooth, new HashSet<ThingMaterial> {
					ThingMaterial.Wood // todo remove
				}},
				{LinkedType.SmoothRock, new HashSet<ThingMaterial> {
					ThingMaterial.Granite,
					ThingMaterial.Limestone,
					ThingMaterial.Marble,
					ThingMaterial.Sandstone
				}}
			};

			Meshes = new Dictionary<LinkedType, Dictionary<ThingMaterial, Mesh>>();
		}

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

		public static void Make (LinkedType type, ThingMaterial material, int x, int y, float xOffset, float yOffset, float xScale, float yScale) {
			Vector3 position = new Vector3(x + xOffset, y + yOffset);
			Vector3 scale = new Vector3(xScale, yScale, 1);
			
			if (!Combinations[type].Contains(material)) {
				Debug.LogWarning($"Invalid combination: {Name.Get(type)} {Name.Get(material)}");
				return;
			}

			CombineInstance combine = new CombineInstance {
				mesh = Meshes[type][material],
				transform = Matrix4x4.TRS(position, Quaternion.identity, scale)
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
			object[] mi = CoverMaterial.GetMaterialAndIndices();
			_material = mi[0] as Material;
			float baseIndex = (float) mi[1];
			float tintIndex = (float) mi[2];
			_prefab.GetComponent<MeshRenderer>().sharedMaterial = _material;
			_quad = MeshBuilder.GetQuad();
			_container = transform;

			for (int i = 1; i < Combinations.Count; ++i) {
				LinkedType type = (LinkedType) i;
				HashSet<ThingMaterial> materials = Combinations[type];
				Dictionary<ThingMaterial, Mesh> typeMaterials = new Dictionary<ThingMaterial, Mesh>();
				int j = 0;

				foreach (ThingMaterial material in materials) {
					Mesh mesh = Instantiate(_quad);
					Vector4[] uv2 = new Vector4[mesh.vertexCount];
					Vector4[] uv3 = new Vector4[mesh.vertexCount];

					for (int k = 0; k < mesh.vertexCount; ++k) {
						uv2[k].x = i * baseIndex;
						uv3[k].x = (int) material * tintIndex;
					}

					mesh.SetUVs(2, uv2);
					mesh.SetUVs(3, uv3);
					typeMaterials.Add(material, mesh);
					++j;
				}

				Meshes.Add(type, typeMaterials);
			}
		}

	}

}