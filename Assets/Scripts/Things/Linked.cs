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

		public void Initialize (LinkedType type, bool planning = false) {
			InitializeThing();

			_type = type;
			_x = (int) Tf.position.x;
			_y = (int) Tf.position.y;
			ChildRenderer.color = AdjustTint(type);

			InitializeSelf();
		}

		public ThingType ThingType () {
			return Enums.ThingType.Structure;
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

		private static int GetIndex (byte mask) {
			int mod = mask % 4;
			return 12 - (mask - mod) + mod;
		}

		private void InitializeSelf () {
			int mask = WallMaker.GetLinked(_x, _y + 1)?._type == _type ? 1 : 0;
			mask += WallMaker.GetLinked(_x + 1, _y)?._type == _type ? 2 : 0;
			mask += WallMaker.GetLinked(_x, _y - 1)?._type == _type ? 4 : 0;
			mask += WallMaker.GetLinked(_x - 1, _y)?._type == _type ? 8 : 0;

			mask += _y == TileMaker.YTILES - 1 ? 1 : 0;
			mask += _x == TileMaker.YTILES - 1 ? 2 : 0;
			mask += _y == 0 ? 4 : 0;
			mask += _x == 0 ? 8 : 0;
			int index = GetIndex((byte) mask);

			if (index < 0) {
				return;
			}
			
			SetSprite(AssetLoader.Get(_type, index), false);
			CoverCenterGaps(index);
			CoverEdgeGaps();
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
			const int max = TileMaker.YTILES - 1;

			if (_x == 0) {
				if (WallMaker.GetLinked(0, _y - 1)?._type == _type) {
					Make("Left Edge Cover", new Vector3(.25f, .25f, -.5f), new Vector3(.5f, 1, 1));

					if (_y == 0) {
						Make("Left Bottom Corner Cover", new Vector3(.25f, .25f, -.5f), new Vector3(.5f, .5f, 1));
					} else if (_y == max) {
						Make("Left Top Corner Cover", new Vector3(.25f, .75f, -.5f), new Vector3(.5f, .5f, 1));
					}
				}
			} else if (_x == max) {
				if (WallMaker.GetLinked(max, _y - 1)?._type == _type) {
					Make("Right Edge Cover", new Vector3(.75f, .25f, -.5f), new Vector3(.5f, 1, 1));

					if (_y == 0) {
						Make("Right Bottom Corner Cover", new Vector3(.75f, .25f, -.5f), new Vector3(.5f, .5f, 1));
					} else if (_y == max) {
						Make("Right Top Corner Cover", new Vector3(.75f, .75f, -.5f), new Vector3(.5f, .5f, 1));
					}
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
			Transform t = new GameObject(label).transform;
			t.SetParent(Tf);
			t.localPosition = position;
			t.localScale = scale;
			SpriteRenderer sr = t.gameObject.AddComponent<SpriteRenderer>();
			sr.sprite = _type == LinkedType.Rock ? AssetLoader.RockTop : AssetLoader.WoodTop;
			sr.color = AdjustTint(_type);
			sr.sharedMaterial = AssetLoader.DiffuseMat;
		}
		
	}

}