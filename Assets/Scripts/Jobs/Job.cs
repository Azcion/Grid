using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Things;

namespace Assets.Scripts.Jobs {

	public class Job {

		public readonly Thing Thing;
		public readonly Thing Target;
		public readonly Action Action;
		public readonly float Work;

		public Job (Thing thing, Thing target, Action action) {
			Thing = thing;
			Target = target;
			Action = action;
			Work = GetWork(target, action);
		}

		private static float GetWork (Thing target, Action action) {
			if (target.Heir.Type == ThingType.Plant) {
				switch (action) {
					case Action.Harvest:
						switch ((target.ThingDef as PlantDef)?.PlantSize) {
							case PlantSize.Small: return 15;
							case PlantSize.Medium: return 30;
							case PlantSize.Large: return 60;
							default: return 0;
						}
					case Action.ChopWood:
						switch ((target.ThingDef as PlantDef)?.PlantSize) {
							case PlantSize.Small: return 30;
							case PlantSize.Medium: return 60;
							case PlantSize.Large: return 120;
							default: return 0;
						}
				}
			}

			return 0;
		}

	}

}