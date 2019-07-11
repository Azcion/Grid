using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Things {

	public interface IThing {

		GameObject GameObject ();

		ThingType ThingType ();

		void UpdateSortingOrder ();

	}

}