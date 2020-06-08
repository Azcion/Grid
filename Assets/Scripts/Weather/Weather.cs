using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Weather {

	public class Weather {

		public readonly WeatherType Type;
		public readonly Material Material;

		public Weather (WeatherType type, Material material) {
			Type = type;
			Material = material;
		}
		
	}

}