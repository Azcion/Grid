using System.Diagnostics.CodeAnalysis;

namespace Assets.Scripts {

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public struct RGBT {

		public readonly float R;
		public readonly float G;
		public readonly float B;
		public readonly float T;

		public RGBT (int r, int g, int b, float t) {
			R = r / 255f;
			G = g / 255f;
			B = b / 255f;
			T = t;
		}

	}

}