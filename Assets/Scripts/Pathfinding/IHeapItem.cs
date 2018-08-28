using System;

namespace Assets.Scripts.Pathfinding {

	public interface IHeapItem<T> : IComparable<T> {
		
		int HeapIndex { get; set; }

	}

}