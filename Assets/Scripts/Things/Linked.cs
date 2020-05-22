using Assets.Scripts.Enums;
using Assets.Scripts.Graphics;
using Assets.Scripts.Makers;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Linked : Thing, IThing {

		private int _x;
		private int _y;
		private LinkedType _type;

		public GameObject Go => gameObject;
		public ThingType Type => ThingType.Structure;

		public static Linked Create (Linked linked, LinkedType type) {
			linked._type = type;

			return linked;
		}

		public static Linked Create (string name, int x, int y, int z, Transform parent, LinkedType type) {
			GameObject go = new GameObject(name, typeof(Linked), typeof(BoxCollider2D));
			go.transform.SetParent(parent);
			go.transform.localPosition = new Vector3(x, y, z);
			BoxCollider2D bc = go.GetComponent<BoxCollider2D>();
			bc.isTrigger = true;
			bc.offset = new Vector2(.5f, .5f);
			bc.size = Vector2.one;
			Linked linked = go.GetComponent<Linked>();
			linked._type = type;

			return linked;
		}

		public void Initialize (bool planning = false) {
			PrepareChild();
			gameObject.isStatic = true;
			_x = (int) transform.position.x;
			_y = (int) transform.position.y;
			ChildRenderer.color = AdjustTint(_type);

			if (planning) {
				string assetName = $"{Name.Get(_type)}_Atlas";
				SetSprite(Assets.GetAtlasSprite(assetName, 12), false);
				ChildRenderer.color = AdjustOpacity(ChildRenderer.color, .5f);
				return;
			}

			InitializeSelf();
			gameObject.SetActive(true);
		}

		public void Refresh () {
			_x = (int) transform.position.x;
			_y = (int) transform.position.y;

			InitializeSelf();
			InitializeNeighbors();
		}

		private static Color AdjustOpacity (Color c, float opacity) {
			return new Color(c.r, c.g, c.b, opacity);
		}

		private static Color AdjustTint (LinkedType type) {
			switch (type) {
				case LinkedType.Rock:
					return TileTint.Get(TileType.RoughStone);
				case LinkedType.WallPlanks:
					return TileTint.Get(TileType.WoodFloor);
				case LinkedType.WallTiles:
					return TileTint.Get(TileType.SmoothStone);
				case LinkedType.WallSmooth:
					return TileTint.Get(TileType.SmoothStone);
				default:
					return Color.magenta;
			}
		}

		private static int GetIndex (int mask) {
			int mod = mask % 4;
			return 12 - (mask - mod) + mod;
		}

		private void InitializeSelf () {
			int mask = WallMaker.GetLinked(_x, _y + 1)?._type == _type ? 1 : 0;
			mask += WallMaker.GetLinked(_x + 1, _y)?._type == _type ? 2 : 0;
			mask += WallMaker.GetLinked(_x, _y - 1)?._type == _type ? 4 : 0;
			mask += WallMaker.GetLinked(_x - 1, _y)?._type == _type ? 8 : 0;

			mask += _y == Map.YTiles - 1 ? 1 : 0;
			mask += _x == Map.YTiles - 1 ? 2 : 0;
			mask += _y == 0 ? 4 : 0;
			mask += _x == 0 ? 8 : 0;
			int index = GetIndex(mask);

			string assetName = $"{Name.Get(_type)}_Atlas";
			SetSprite(Assets.GetAtlasSprite(assetName, index), false); 
			CoverCenterGaps(index);
			CoverEdgeGaps();
		}

		private void InitializeNeighbors () {
			for (int x = -1; x < 2; ++x) {
				for (int y = -1; y < 2; ++y) {
					if (x == 0 && y == 0) {
						continue;
					}

					Linked neighbor = WallMaker.GetLinked(_x + x, _y + y);

					if (neighbor == null) {
						continue;
					}

					for (int i = neighbor.transform.childCount - 1; i > 0; --i) {
						Destroy(neighbor.transform.GetChild(i).gameObject, .2f);
					}

					neighbor.InitializeSelf();
				}
			}
		}

		private void CoverCenterGaps (int index) {
			switch (index) {
				case 2:
				case 3:
				case 10:
				case 11:
					if (WallMaker.GetLinked(_x + 1, _y - 1)?._type == _type) {
						Make("Cover", new Vector3(1, .25f, -.5f), Vector3.one);
					}

					break;
			}
		}

		private void CoverEdgeGaps () {
			int max = Map.YTiles - 1;

			if (_x == 0) {
				if (_y == 0) {
					Make("Left Bottom Corner Cover", new Vector3(.25f, .25f, -.5f), new Vector3(.5f, .5f, 1));
				} else if (_y == max) {
					Make("Left Top Corner Cover", new Vector3(.25f, .75f, -.5f), new Vector3(.5f, .5f, 1));
				}

				if (WallMaker.GetLinked(0, _y - 1)?._type == _type) {
					Make("Left Edge Cover", new Vector3(.25f, .25f, -.5f), new Vector3(.5f, 1, 1));
				}
			} else if (_x == max) {
				if (_y == 0) {
					Make("Right Bottom Corner Cover", new Vector3(.75f, .25f, -.5f), new Vector3(.5f, .5f, 1));
				} else if (_y == max) {
					Make("Right Top Corner Cover", new Vector3(.75f, .75f, -.5f), new Vector3(.5f, .5f, 1));
				}

				if (WallMaker.GetLinked(max, _y - 1)?._type == _type) {
					Make("Right Edge Cover", new Vector3(.75f, .25f, -.5f), new Vector3(.5f, 1, 1));
				}
			}

			if (_y == 0) {
				if (WallMaker.GetLinked(_x - 1, 0)?._type == _type) {
					Make("Bottom Edge Cover", new Vector3(0, .25f, -.5f), new Vector3(1, .5f, 1));
				}
			} else if (_y == max) {
				if (WallMaker.GetLinked(_x - 1, max)?._type == _type) {
					Make("Top Edge Cover", new Vector3(0, .75f, -.5f), new Vector3(1, .5f, 1));
				}
			}
		}

		private void Make (string label, Vector3 position, Vector3 scale) {
			Transform t = new GameObject(label, typeof(SpriteRenderer)).transform;
			t.SetParent(transform);
			t.gameObject.isStatic = true;
			t.localPosition = position;
			t.localScale = scale;
			SpriteRenderer sr = t.gameObject.GetComponent<SpriteRenderer>();
			sr.sprite = _type == LinkedType.Rock ? Assets.GetSprite("RockTop") : Assets.GetSprite("WoodTop");
			sr.color = AdjustTint(_type);
			sr.sharedMaterial = Assets.CoverMat;
		}
	}

}