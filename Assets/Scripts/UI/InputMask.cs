using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI {

	[RequireComponent(typeof(EventTrigger))]
	public class InputMask : MonoBehaviour {

		[UsedImplicitly]
		public void OnPointerEnter () {
			GUI.Busy = true;
		}

		[UsedImplicitly]
		public void OnPointerExit () {
			GUI.Busy = false;
		}

		[UsedImplicitly]
		private void Start () {
			EventTrigger et = GetComponent<EventTrigger>();

			if (et == null) {
				return;
			}

			EventTrigger.Entry onEnter = new EventTrigger.Entry();
			EventTrigger.Entry onExit = new EventTrigger.Entry();
			onEnter.eventID = EventTriggerType.PointerEnter;
			onExit.eventID = EventTriggerType.PointerExit;
			onEnter.callback.AddListener(eventData => OnPointerEnter());
			onExit.callback.AddListener(eventData => OnPointerExit());
			et.triggers.Add(onEnter);
			et.triggers.Add(onExit);
		}

	}

}