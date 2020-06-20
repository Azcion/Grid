using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Defs {

	public class DefLoader : MonoBehaviour {

		public static bool DidLoad = false;
		public static DefContainer<AnimalDef> AnimalDefs;
		public static DefContainer<PlantDef> PlantDefs;
		public static DefContainer<HumanoidDef> HumanoidDefs;
		public static DefContainer<ItemDef> ItemDefs;
		public static PlantDef Grass;
		public static HumanoidDef Human;

		public static AnimalDef GetRandomAnimalDef () {
			return AnimalDefs.Defs[Random.Range(0, AnimalDefs.Defs.Count)];
		}

		public static PlantDef GetRandomPlantDef () {
			return PlantDefs.Defs[Random.Range(0, PlantDefs.Defs.Count)];
		}

		public static HumanoidDef GetRandomHumanoidDef () {
			return HumanoidDefs.Defs[Random.Range(0, HumanoidDefs.Defs.Count)];
		}

		public static ItemDef Get (string defName) {
			return ItemDefs.Get(defName);
		}

		[UsedImplicitly]
		private void Start () {
			string path = Application.isEditor ? Application.dataPath : System.IO.Directory.GetCurrentDirectory();
			path += "/Defs/";
			AnimalDefs = new DefContainer<AnimalDef>(path + "Animals_Global.xml");
			AnimalDefs.Add(path + "Animals_Arid.xml");
			AnimalDefs.Add(path + "Animals_Tropical.xml");
			PlantDefs = new DefContainer<PlantDef>(path + "Plants.xml");
			HumanoidDefs = new DefContainer<HumanoidDef>(path + "Humanoids.xml");
			ItemDefs = new DefContainer<ItemDef>(path + "Items.xml");

			//Find grass def
			foreach (PlantDef def in PlantDefs.Defs) {
				if (def.DefName != "Grass") {
					continue;
				}

				Grass = def;
				break;
			}

			//Find human def
			foreach (HumanoidDef def in HumanoidDefs.Defs) {
				if (def.DefName != "Human") {
					continue;
				}

				Human = def;
				break;
			}

			DidLoad = true;
			SceneManager.LoadScene("Main");
		}

	}

}