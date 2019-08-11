using Assets.Scripts.Graphics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Defs {

	public class DefLoader : MonoBehaviour {

		public static bool DidLoad = false;
		public static DefContainer<AnimalDef> AnimalDefs;
		public static DefContainer<PlantDef> PlantDefs;
		public static PlantDef Grass;

		public static AnimalDef GetRandomAnimalDef () {
			return AnimalDefs.Defs[Random.Range(0, AnimalDefs.Defs.Count)];
		}

		public static PlantDef GetRandomPlantDef () {
			return PlantDefs.Defs[Random.Range(0, PlantDefs.Defs.Count)];
		}

		[UsedImplicitly]
		private void Start () {
			string path = Application.isEditor ? Application.dataPath : System.IO.Directory.GetCurrentDirectory();
			path += "/Defs/";
			AnimalDefs = new DefContainer<AnimalDef>(path + "Animals.xml");
			PlantDefs = new DefContainer<PlantDef>(path + "Plants.xml");

			//Find grass def
			foreach (PlantDef def in PlantDefs.Defs) {
				if (def.DefName != "Grass") {
					continue;
				}

				Grass = def;
				break;
			}

			DidLoad = true;
			SceneManager.LoadScene("Main");
		}

	}

}