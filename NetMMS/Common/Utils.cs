using System;
using System.Collections.Generic;

namespace NetMMS.Common {
    public static class Utils {
        public static bool Empty(this string s) => string.IsNullOrEmpty(s);

        public static bool NonEmpty(this string s) => !s.Empty();

        public static void Foreach<T>(this IEnumerable<T> items, Action<T> action) {
            foreach(var item in items) {
                action(item);
            }
        }
    }
}
