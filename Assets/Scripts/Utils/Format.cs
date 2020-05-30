using System.Text;

namespace Assets.Scripts.Utils {

	public static class Format {

		public static string SeparateAtCapitalLetters (string text, char separator) {
			StringBuilder sb = new StringBuilder();
			char previous = char.MinValue;

			foreach (char c in text) {
				if (char.IsUpper(c)) {
					if (sb.Length != 0 && previous != separator) {
						sb.Append(separator);
					}
				}

				sb.Append(c);
				previous = c;
			}

			return sb.ToString();
		}
		
	}

}