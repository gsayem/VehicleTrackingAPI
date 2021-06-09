using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.Common.Tools;

namespace VehicleTracking.Common.Extension {
    public static class EnumerableExtender {
        public static void ForEach<T>(this IEnumerable<T> _this, Action<T> callback) {
            foreach (T entry in _this) {
                callback(entry);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            var hash = new HashSet<TKey>();
            return source.Where(p => hash.Add(keySelector(p)));
        }

        public static int Count(this IEnumerable source) {
            var col = source as ICollection;
            int c = 0;
            if (col != null) {
                c = col.Count;
            }
            else {
                var e = source.GetEnumerator();
                DynamicObjectUtil.DynamicUsing(e, () => {
                    while (e.MoveNext())
                        c++;
                });
            }
            return c;
        }
    }
}
