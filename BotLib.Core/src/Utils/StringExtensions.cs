using System.Text.RegularExpressions;

namespace BotLib.Core.Utils {
    public static class StringExtensions {

        public static Regex ToRegex(this string str) {
            return new Regex(str);
        }

        public static Regex ToRegex(this string str, RegexOptions regexOptions) {
            return new Regex(str, regexOptions);
        }

    }
}