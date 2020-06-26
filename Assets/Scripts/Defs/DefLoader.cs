using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Defs {

	public class DefLoader : MonoBehaviour {

		public static bool DidLoad;
		public static DefContainer<AnimalDef> AnimalDefs;
		public static DefContainer<PlantDef> PlantDefs;
		public static DefContainer<HumanoidDef> HumanoidDefs;
		public static DefContainer<BuildingDef> BuildingDefs;
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

		public static BuildingDef GetBuilding (string defName) {
			return BuildingDefs.Get(defName);
		}

		public static ItemDef GetItem (string defName) {
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
			BuildingDefs = new DefContainer<BuildingDef>(path + "Buildings_Structure.xml");
			BuildingDefs.Add(path + "Buildings_Natural.xml");
			ItemDefs = new DefContainer<ItemDef>(path + "Items.xml");

			Grass = PlantDefs.Get("Grass");
			Human = HumanoidDefs.Get("Scyther");

			DidLoad = true;
			SceneManager.LoadScene("Main");
		}

	}

}