using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Things {

	public interface IThing {

		GameObject Go { get; }
		ThingType Type { get; }

	}

}