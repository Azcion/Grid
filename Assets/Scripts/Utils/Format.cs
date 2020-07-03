using System.Text;

namespace Assets.Scripts.Utils {

	public static class Format {

		public static string SeparateAtCapitalLetters (string text) {
			StringBuilder sb = new StringBuilder();
			char previous = char.MinValue;

			foreach (char c in text) {
				if (char.IsUpper(c)) {
					if (sb.Length != 0 && previous != ' ') {
						sb.Append(' ');
					}
				}

				sb.Append(c);
				previous = c;
			}

			return sb.ToString();
		}

		public static string Capitalize (string text) {
			return char.ToUpper(text[0]) + text.Substring(1);
		}
		
	}

}