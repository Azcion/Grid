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

		public void Assign (GameObject chunk, int x, int y,
			TileType type, bool walkable, bool buildable, int penalty) {
			Chunk = chunk;
			X = x;
			Y = y;
			Type = type;
			Walkable = walkable;
			Buildable = buildable;
			Penalty = penalty;
			_groundPenalty = penalty;
			_thingSlot = new List<IThing>();
			_creatures = new List<ICreature>();
		}

		public void AddCreature (ICreature creature) {
			_creatures.Add(creature);
			UpdatePenalty();
		}

		public void RemoveCreature (ICreature creature) {
			_creatures.Remove(creature);
			UpdatePenalty();
		}

		public bool TryAddThing (IThing thing) {
			if (ThingSlotVacant() == false) {
				return false;
			}

			_thingSlot.Add(thing);
			UpdatePenalty();

			return true;
		}

		public void RemoveThing () {
			_thingSlot.Clear();
			UpdatePenalty();
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

			switch (thing.ThingType()) {
				case ThingType.Object:
					sum += 20;
					break;
				case ThingType.Plant:
					switch (((Plant) thing).Size) {
						case PlantSize.Medium:
							sum += 10;
							break;
						case PlantSize.Large:
							sum += 20;
							break;
					}

					break;
			}

			if (AnyCreaturesOnTile()) {
				sum += 5;
			}

			Penalty = sum;
		}

	}

}