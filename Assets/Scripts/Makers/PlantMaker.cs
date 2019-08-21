using System.Collections.Generic;
using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Main;
using Assets.Scripts.Things;
using Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Makers {

	public class PlantMaker : MonoBehaviour {

        private enum Coverage {

            Empty,
            Grass,
            Random

        }

		private static GameObject _plantPrefab;

		[UsedImplicitly, SerializeField] private GameObject _container = null;

        private static readonly ulong ValidTilesMask;
        private static readonly ulong LowFertilityMask;

        private static readonly List<TileType> ValidTiles = new List<TileType> {
            TileType.Mossy, TileType.Sand, TileType.Soil, TileType.SoilRich, TileType.Gravel
        };

        private static readonly List<TileType> LowFertility = new List<TileType> {
            TileType.Sand, TileType.Gravel
        };

        static PlantMaker () {
            ValidTilesMask = 0;
            LowFertilityMask = 0;

            foreach (TileType type in ValidTiles) {
                ulong mask = (ulong) (1 << (int) type);
                ValidTilesMask |= mask;
            }

            foreach (TileType type in LowFertility) {
                ulong mask = (ulong) (1 << (int) type);
                LowFertilityMask |= mask;
            }
        }

        private static bool IsValid (TileType type) {
            return ((ulong) (1 << (int) type) & ValidTilesMask) > 0;
        }

        private static bool IsLowFertility (TileType type) {
            return ((ulong) (1 << (int) type) & LowFertilityMask) > 0;
        }

        private static Coverage GetCoverage (TileType type) {
            switch (type) {
                case TileType.Sand:
                case TileType.Gravel:
                    if (Random.value < .95) {
                        return Coverage.Empty;
                    }

                    if (Random.value < .85) {
                        return Coverage.Grass;
                    }

                    break;
                case TileType.Soil:
                case TileType.SoilRich:
                    if (Random.value < .35) {
                        return Coverage.Empty;
                    }

                    float value = Random.value;

                    if (value < .95) {
                        return Coverage.Grass;
                    }

                    break;
                default:
                    return Coverage.Empty;
            }

            return Coverage.Random;
        }

        [UsedImplicitly]
		private void Start () {
			if (!DefLoader.DidLoad) {
				Debug.Log("Defs not loaded, PlantMaker can't run.");
				return;
			}

			if (_plantPrefab == null) {
				_plantPrefab = new GameObject("Plant Prefab", typeof(Plant));
				_plantPrefab.transform.SetParent(_container.transform);
				_plantPrefab.SetActive(false);
			}

			Populate(_plantPrefab);
		}

		private void Populate (GameObject prefab) {
			for (int x = 0; x < Map.YTiles; ++x) {
				for (int y = Map.YTiles - 1; y >= 0; --y) {
                    TileType type = TileMaker.GetTile(x, y).Type;

                    if (!IsValid(type)) {
                        continue;
                    }

                    Coverage c = GetCoverage(type);

                    switch (c) {
                        case Coverage.Empty:
                            continue;
                        case Coverage.Grass:
                            Initialize(prefab, DefLoader.Grass, x, y);
                            break;
                        case Coverage.Random:
                            PlantDef def = DefLoader.GetRandomPlantDef();

                            if (def.Cluster) {
                                if (!IsLowFertility(type)) {
                                    InitializeGroup(prefab, def, x, y);
                                } else {
                                    Initialize(prefab, def, x, y);
                                }
                            } else {
                                Initialize(prefab, def, x, y);
                            }

                            break;
                    }

				}
			}

			ApplicationController.NotifyReady();
		}

        private void InitializeGroup (GameObject prefab, PlantDef def, int x, int y) {
            int groupSize = Random.Range(1, 4);
            float rv = Random.value;
            groupSize += rv > .5f ? 2 : 1;
            groupSize += rv > .8f ? 2 : 1;
            Vector2Int v0 = Calc.Clamp(new Vector2Int(x - 2, y - 2));
            Vector2Int v1 = Calc.Clamp(new Vector2Int(x + 3, y + 3));
            List<Vector2Int> usedPoints = new List<Vector2Int>();

            for (int i = 0; i < groupSize; i++) {
                Vector2Int v = new Vector2Int(Random.Range(v0.x, v1.x), Random.Range(v0.y, v1.y));

                if (usedPoints.Contains(v)) {
                    continue;
                }

                usedPoints.Add(v);

                if (IsValid(TileMaker.GetTile(v.x, v.y).Type)) {
                    Initialize(prefab, def, v.x, v.y);
                }
            }
        }

        private void Initialize (GameObject prefab, PlantDef def, int x, int y) {
            Vector3 pos = new Vector3(x, y, Order.PLANT + Map.SubY * y);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity, _container.transform);
            go.name = def.DefName;
            Plant plant = Plant.Create(go.GetComponent<Plant>(), def);
            plant.Initialize(Calc.Round(Random.Range(.3f, 1), 2));

            if (TileMaker.GetTile(x, y).TryAddThing(plant, true) == false) {
                Destroy(go);
            }
        }

    }

}