using System.ComponentModel;

namespace Assets.Scripts.Defs {

	public struct StatBases {

		[DefaultValue(0)] public int MaxHitPoints;
		[DefaultValue(0)] public float Nutrition;

		[DefaultValue(0)] public float MoveSpeed;
		[DefaultValue(0)] public float MarketValue;
		[DefaultValue(0)] public int ComfyTemperatureMin;
		[DefaultValue(0)] public int ComfyTemperatureMax;

	}

}