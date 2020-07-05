using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	[UsedImplicitly]
	public class Tile {

		public readonly int X;
		public readonly int Y;

		public TileType Type;
		public bool Walkable;
		public bool Buildable;
		public int Penalty;

		private int _groundPenalty;
		private Thing _thingSlot;
		private bool _originalWalkable;
		private bool _originalBuildable;

		public Tile (int x, int y, TileType type, bool walkable, bool buildable, int penalty) {
			X = x;
			Y = y;
			Type = type;
			Walkable = walkable;
			Buildable = buildable;
			_originalWalkable = walkable;
			_originalBuildable = buildable;
			Penalty = penalty;
			_groundPenalty = penalty;
		}

		public bool TryAddThing (Thing thing) {
			if (ThingSlotVacant()) {
				AddThing(thing);

				return true;
			}

			if (!TryRemoveIgnorablePlants()) {
				return false;
			}

			AddThing(thing);
			return true;
		}

		public bool CanAddThing () {
			return ThingSlotVacant() || CanBuildOver();
		}

		private bool CanBuildOver () {
			return _thingSlot.Type == ThingType.Plant && ((Plant) _thingSlot).Def.CanBuildOver;
		}

		private void AddThing (Thing thing) {
			if (thing.Type == ThingType.Structure) {
				Walkable = false;
				Buildable = false;
			}

			_thingSlot = thing;
			UpdatePenalty();
		}

		private bool TryRemoveIgnorablePlants () {
			if (!CanBuildOver()) {
				return false;
			}

			Object.Destroy(_thingSlot.gameObject);
			_thingSlot = null;

			return true;
		}

		private bool ThingSlotVacant () {
			return _thingSlot == null;
		}

		private void UpdatePenalty () {
			int sum = _groundPenalty;

			switch (_thingSlot.Type) {
				case ThingType.Structure:
					sum += 1000;
					break;
				case ThingType.Object:
					sum += 80;
					break;
				case ThingType.Plant:
					switch (((Plant) _thingSlot).Size) {
						case PlantSize.Medium:
							sum += 20;
							break;
						case PlantSize.Large:
							sum += 80;
							break;
					}

					break;
			}

			Penalty = sum;
		}

	}

}