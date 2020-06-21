using Assets.Scripts.Enums;
using Assets.Scripts.Things;

namespace Assets.Scripts.Jobs {

	public struct Job {

		public readonly Thing Thing;
		public readonly Thing Target;
		public readonly Action Action;
		public readonly float Work;

		public Job (Thing thing, Thing target, Action action, float work) {
			Thing = thing;
			Target = target;
			Action = action;
			Work = work;
		}

	}

}