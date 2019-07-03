using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Defs {

	public class DefLoader : MonoBehaviour {

		public static DefContainer<AnimalDef> AnimalDefs;
		public static DefContainer<PlantDef> PlantDefs;

		public IThingDef GetDef () {

			return null;
		}

		[UsedImplicitly]
		private void Start () {
			string path = Application.dataPath + "/Generated/";
			AnimalDefs = new DefContainer<AnimalDef>(path + "Animals.xml");
			PlantDefs = new DefContainer<PlantDef>(path + "Plants.xml");
		}

	}

}