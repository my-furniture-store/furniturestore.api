using System.Reflection;

namespace FurnitureStore.Tests.Common.Helpers;

public class EntityTestHelper
{
    public static void SetPrivateFieldValues<TObj, TValue>(TObj obj, TValue value, string propertyName)
    {
        var field = typeof(TObj).GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(obj,value);
    }
}
