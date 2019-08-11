using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	[UsedImplicitly]
	public class Tile : MonoBehaviour {

		public GameObject Chunk;
		public int X;
		public int Y;
		public TileType Type;
		public bool Walkable;
		public bool Buildable;
		public int Penalty;

		private int _groundPenalty;
		private List<IThing> _thingSlot;
		private List<ICreature> _creatures;
		private bool _originalWalkable;
		private bool _originalBuildable;

		public void Assign (GameObject chunk, int x, int y, TileType type, bool walkable, bool buildable, int penalty) {
			Chunk = chunk;
			X = x;
			Y = y;
			Type = type;
			Walkable = walkable;
			Buildable = buildable;
			_originalWalkable = walkable;
			_originalBuildable = buildable;
			Penalty = penalty;
			_groundPenalty = penalty;
			_thingSlot = new List<IThing>();
			_creatures = new List<ICreature>();
		}

		public IThing GetThing () {
			return _thingSlot.Count == 0 ? null : _thingSlot[0];
		}

		public void AddCreature (ICreature creature) {
			_creatures.Add(creature);
			UpdatePenalty();
		}

		public void RemoveCreature (ICreature creature) {
			_creatures.Remove(creature);
			UpdatePenalty();
		}

		public bool TryAddThing (IThing thing, bool ignoreGrass = false) {
			if (ThingSlotVacant()) {
				AddThing(thing);

				return true;
			}

			if (!ignoreGrass) {
				return false;
			}

			if (!HasGrass()) {
				return false;
			}

			RemoveGrass(true);
			AddThing(thing);

			return true;

		}

		public void RemoveThings (bool destroy = false) {
			if (destroy) {
				foreach (IThing thing in _thingSlot) {
					if (thing.Type == ThingType.Structure) {
						Walkable = _originalWalkable;
						Buildable = _originalBuildable;
					}

					Destroy(thing.Go);
				}
			}

			string s = _thingSlot.Count.ToString();
			_thingSlot.Clear();
			s += " " + _thingSlot.Count;
			Debug.Log(s);
			UpdatePenalty();
		}

		private static bool IsGrass (IThing thing) {
			if (thing.Type != ThingType.Plant) {
				return false;
			}

			return (thing as Plant)?.Def.DefName == "Grass";
		}

		private void AddThing (IThing thing) {
			if (thing.Type == ThingType.Structure) {
				Walkable = false;
				Buildable = false;
			}

			_thingSlot.Add(thing);
			UpdatePenalty();
		}

		private bool HasGrass () {
			foreach (IThing thing in _thingSlot) {
				if (thing.Type != ThingType.Plant) {
					continue;
				}

				if (IsGrass(thing)) {
					return true;
				}
			}

			return false;
		}

		private void RemoveGrass (bool destroy = false) {
			foreach (IThing thing in _thingSlot) {
				if (!IsGrass(thing)) {
					continue;
				}

				_thingSlot.Remove(thing);

				if (destroy) {
					Destroy(thing.Go);
				}

				return;
			}
		}

		public bool IsBarren () {
			return ThingSlotVacant() && AnyCreaturesOnTile() == false;
		}

		public bool ThingSlotVacant () {
			return _thingSlot.Count == 0;
		}

		public bool AnyCreaturesOnTile () {
			return _creatures.Count > 0;
		}

		private void UpdatePenalty () {
			int sum = _groundPenalty;

			if (IsBarren()) {
				Penalty = sum;
				return;
			}

			IThing thing = _thingSlot[0];

			switch (thing.Type) {
				case ThingType.Structure:
					sum += 1000;
					break;
				case ThingType.Object:
					sum += 80;
					break;
				case ThingType.Plant:
					switch (((Plant) thing).Size) {
						case PlantSize.Medium:
							sum += 20;
							break;
						case PlantSize.Large:
							sum += 80;
							break;
					}

					break;
			}

			if (AnyCreaturesOnTile()) {
				sum += 20;
			}

			Penalty = sum;
		}

	}

}