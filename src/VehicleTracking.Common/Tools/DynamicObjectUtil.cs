using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking.Common.Tools {
    public class DynamicObjectUtil {
        public static List<T> GetListOfType<T>(T dyn, int? count = null) {
            return count.HasValue ? new List<T>(count.Value) : new List<T>();
        }

        public static T CastTo<T>(object x, T dyn) {
            return (T)x;
        }

        public static void DynamicUsing(object resource, Action action) {
            try {
                action();
            }
            finally {
                IDisposable d = resource as IDisposable;
                d?.Dispose();
            }
        }

        public static void CopyProperties(object source, object destination) {
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();
            var results = from srcProp in typeSrc.GetProperties()
                          let targetProperty = typeDest.GetProperty(srcProp.Name)
                          where srcProp.CanRead
                          && targetProperty != null
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate)
                          && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty = targetProperty };
            foreach (var props in results) {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
