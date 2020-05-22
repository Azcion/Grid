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
		private IThing _thingSlot;
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

		public bool TryAddThing (IThing thing) {
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

		private static bool CanBuildOver (IThing thing) {
			return thing.Type == ThingType.Plant && ((Plant) thing).Def.CanBuildOver;
		}

		private void AddThing (IThing thing) {
			if (thing.Type == ThingType.Structure) {
				Walkable = false;
				Buildable = false;
			}

			_thingSlot = thing;
			UpdatePenalty();
		}

		private bool TryRemoveIgnorablePlants () {
			if (!CanBuildOver(_thingSlot)) {
				return false;
			}

			Object.Destroy(_thingSlot.Go);
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