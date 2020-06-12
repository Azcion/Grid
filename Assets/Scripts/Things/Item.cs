using Assets.Scripts.Defs;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Things {

	public class Item : Thing, IThing {

		public ItemDef Def { get; private set; }
		public int Count { get; private set; }

		public GameObject Go => gameObject;
		public ThingType Type => ThingType.Item;

		public static Item Create (Item item, ItemDef def) {
			item.Def = def;

			return item;
		}

		public void Initialize (int count) {
			PrepareChild();
			Count = count;
			float capacity = (float) count / Def.StackLimit;
			string suffix = GetSuffix(Def.TexCount, capacity);
			Debug.Log(suffix + " " + count + " " + capacity);
			SetSprite(Assets.GetSprite(Def.DefName + suffix), false);
			Child.localPosition = new Vector2(.5f, .5f);
			Child.localScale = new Vector3(1.5f, 1.5f, 1);
			IsSelectable = true;
			gameObject.SetActive(true);
		}

		private static string GetSuffix (int texCount, float capacity) {
			switch (texCount) {
				case 2 when capacity < 1:
				case 3 when capacity < .04f:
					return "A";
				case 2:
				case 3 when capacity < 1:
					return "B";
				case 3:
					return "C";
			}

			return "";
		}

	}

}