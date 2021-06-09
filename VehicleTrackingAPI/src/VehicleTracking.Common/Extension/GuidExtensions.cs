using System;

namespace VehicleTracking.Common.Extension {
    public static class GuidExtensions
    {
        public static bool IsNull(this Guid? value)
        {
            bool result = value == null || value == Guid.Empty;
            return result;
        }
        public static bool IsNotNull(this Guid value)
        {
            return !IsNull(value);
        }
    }
}
