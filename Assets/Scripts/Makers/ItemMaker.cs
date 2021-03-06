﻿using Assets.Scripts.Defs;
using Assets.Scripts.Things;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Makers {

	public class ItemMaker : MonoBehaviour {

		private static Item[,] _items;
		private static Transform _container;
		private static GameObject _itemPrefab;

		public static void Make (string type, int count, int x, int y) {
			Vector3 pos = new Vector3(x, y, Order.ITEM);
			GameObject go = Instantiate(_itemPrefab, pos, Quaternion.identity, _container);
			go.name = type;
			Def def = DefLoader.GetItem(type);
			Item item = Item.Create(go.GetComponent<Item>(), def);
			item.Initialize(count);
			_items[y, x] = item;
		}

		[UsedImplicitly]
		private void Start () {
			if (!DefLoader.DidLoad) {
				Debug.Log("Defs not loaded, ItemMaker can't run.");
				return;
			}

			if (_itemPrefab == null) {
				_itemPrefab = new GameObject("Item Prefab", typeof(Item), typeof(BoxCollider2D));
				_itemPrefab.SetActive(false);
				_itemPrefab.GetComponent<Item>().Prepare();
				_itemPrefab.transform.SetParent(transform);
				BoxCollider2D bc = _itemPrefab.GetComponent<BoxCollider2D>();
				bc.isTrigger = true;
				bc.offset = new Vector2(.5f, .5f);
				bc.size = Vector2.one;
			}

			_items = new Item[Map.YTiles, Map.YTiles];
			_container = transform;
		}

	}

}