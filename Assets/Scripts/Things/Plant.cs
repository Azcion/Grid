using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using Assets.Scripts.Makers;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Things {

	[UsedImplicitly]
	public class Plant : Thing {

		public PlantSize Size;

		private static readonly Vector3[] Nodes = {
			new Vector3(.30f, -.05f),
			new Vector3(.65f, .15f),
			new Vector3(.25f, .45f),
			new Vector3(.75f, 0),
			new Vector3(.70f, .40f),
			new Vector3(.40f, .20f)
		};

		private static GameObject _childPrefab;

		private float _growth;

		static Plant () {
			for (int i = 0; i < Nodes.Length; i++) {
				Vector3 v = Nodes[i];
				Nodes[i] = new Vector3(v.x, v.y, Map.SubY * v.y);
			}
		}

		public static Plant Create (Plant plant, Def def) {
			plant.Def = def;
			plant.AsPlant = plant;
			plant.Type = ThingType.Plant;

			return plant;
		}

		public void Action_ChopWood () {
			gameObject.SetActive(false);
			int count = (int) (Def.ResourceYield * _growth);
			int x = (int) transform.position.x;
			int y = (int) transform.position.y;
			ItemMaker.Make(Def.Resource, count, x, y);

			if (Selector.Thing.AsPlant == this) {
				Selector.Deselect(true);
			}

			//todo disable and move to pool
			Destroy(gameObject);
		}

		public void Action_Harvest () {
			int count = (int) (Def.ResourceYield * _growth);
			int x = (int) transform.position.x;
			int y = (int) transform.position.y;
			//todo try place near
			ItemMaker.Make(Def.Resource, count, x, y);
			_growth = 0;
			AdjustTransform(0);
		}

		public void Initialize (float growth) {
			PrepareChild();
			Size = Def.PlantSize;
			_growth = growth;
			string suffix = "";

			if (Def.TexCount > 1) {
				suffix = ((char) ('A' + Random.Range(0, Def.TexCount))).ToString();
			}

			IsSelectable = Def.Selectable;
			SetValidActions();
			bool flipX = Random.value < .5;

			if (Size != PlantSize.Small) {
				SetSprite(Assets.GetSprite(Def.DefName + suffix), flipX);
				AdjustTransform(growth);
				gameObject.SetActive(true);
				return;
			}

			float nodeIndex = Random.Range(0, Nodes.Length);
			AdjustTransform(growth, (int) nodeIndex);

			/*if (Def.DefName == "Grass") {
				flipX = false;
				ChildRenderer.sharedMaterial = Assets.SwayMat;
			}*/

			SetSprite(Assets.GetSprite(Def.DefName + suffix), flipX);
			int cloneCount = Random.Range(0, 4);
			float nodeOrder = Random.value > .5f ? 1 : -1;
			nodeOrder *= Random.value > .8f ? 1.5f : 1;

			for (int i = 0; i < cloneCount; ++i) {
				nodeIndex += nodeOrder;
				int index = Mod((int) nodeIndex, Nodes.Length);
				CreateChildSprite(index);
			}

			gameObject.SetActive(true);
		}

		private void SetValidActions () {
			if (string.IsNullOrEmpty(Def.Resource)) {
				return;
			}

			switch (Def.Resource) {
				case "WoodLog":
					ValidActions.Add(Action.ChopWood);
					break;
				default:
					ValidActions.Add(Action.Harvest);
					break;
			}
		}

		private static int Mod (int n, int m) {
			int r = n % m;
			return r < 0 ? r + m : r;
		}

		[UsedImplicitly]
		private void Awake () {
			if (_childPrefab != null) {
				return;
			}

			_childPrefab = new GameObject("Child Prefab", typeof(SpriteRenderer));
			_childPrefab.SetActive(false);
		}

		private void AdjustTransform (float growth, int nodeIndex = 0) {
			growth = Mathf.Clamp(growth, .15f, 1);
			float s;
			float y = 0;

			switch (Size) {
				case PlantSize.Small:
					s = Mathf.Lerp(1, 1.5f, growth);
					break;
				case PlantSize.Medium:
					s = Mathf.Lerp(1, 1.5f, growth);
					y = Mathf.Lerp(.2f, .04f, growth);
					break;
				default: // Large
					s = Mathf.Lerp(1, 1.95f, growth);
					y = .04f;
					break;
			}

			Child.localScale = new Vector3(s, s, 1);
			
			if (Size == PlantSize.Small) {
				Child.localPosition = Nodes[nodeIndex];
			} else {
				Child.localPosition = new Vector2(.5f, y);
			}
		}

		private void CreateChildSprite (int nodeIndex) {
			float s = Mathf.Lerp(1, 1.5f, _growth);
			GameObject go = Instantiate(_childPrefab, transform.position, Quaternion.identity, transform);
			go.transform.localPosition = Nodes[nodeIndex];
			go.transform.localScale = new Vector3(s, s, 1);
			go.name = "Clone";
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			sr.sprite = ChildRenderer.sprite;
			sr.sharedMaterial = ChildRenderer.sharedMaterial;
			go.SetActive(true);
		}

	}

}