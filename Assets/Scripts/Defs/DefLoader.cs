using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Defs {

	public class DefLoader : MonoBehaviour {

		public static bool DidLoad;
		public static DefContainer AnimalDefs;
		public static DefContainer PlantDefs;
		public static DefContainer HumanoidDefs;
		public static DefContainer BuildingDefs;
		public static DefContainer ItemDefs;
		public static Def Grass;
		public static Def Human;

		public static Def GetRandomAnimalDef () {
			return AnimalDefs.Defs[Random.Range(0, AnimalDefs.Defs.Count)];
		}

		public static Def GetRandomPlantDef () {
			return PlantDefs.Defs[Random.Range(0, PlantDefs.Defs.Count)];
		}

		public static Def GetRandomHumanoidDef () {
			return HumanoidDefs.Defs[Random.Range(0, HumanoidDefs.Defs.Count)];
		}

		public static Def GetBuilding (string defName) {
			return BuildingDefs.Get(defName);
		}

		public static Def GetItem (string defName) {
			return ItemDefs.Get(defName);
		}

		[UsedImplicitly]
		private void Start () {
			string path = Application.isEditor ? Application.dataPath : System.IO.Directory.GetCurrentDirectory();
			path += "/Defs/";
			AnimalDefs = new DefContainer(path + "Animals_Global.xml");
			AnimalDefs.Add(path + "Animals_Arid.xml");
			AnimalDefs.Add(path + "Animals_Tropical.xml");
			PlantDefs = new DefContainer(path + "Plants.xml");
			HumanoidDefs = new DefContainer(path + "Humanoids.xml");
			BuildingDefs = new DefContainer(path + "Buildings_Structure.xml");
			BuildingDefs.Add(path + "Buildings_Natural.xml");
			ItemDefs = new DefContainer(path + "Items.xml");

			Grass = PlantDefs.Get("Grass");
			Human = HumanoidDefs.Get("Scyther");

			DidLoad = true;
			SceneManager.LoadScene("Main");
		}

	}

}