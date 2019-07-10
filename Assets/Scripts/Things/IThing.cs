using Assets.Scripts.Enums;

namespace Assets.Scripts.Things {

	public interface IThing {

		ThingType ThingType ();

		void UpdateSortingOrder ();

	}

}