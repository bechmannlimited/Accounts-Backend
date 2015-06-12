using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace CompresJSON
{
    public static class Mapper
    {
        public static object ToObject(this IDictionary<string, object> source, object someObject)
        {
            //var someObject = new T();
            if (someObject != null)
            {
                var someObjectType = someObject.GetType();

                foreach (var item in source)
                {
                    var key = char.ToUpper(item.Key[0]) + item.Key.Substring(1);
                    var targetProperty = someObjectType.GetProperty(key);

                    if (targetProperty != null)
                    {
                        if (targetProperty.PropertyType.FullName == "System.String") // == typeof(string))
                        {
                            targetProperty.SetValue(someObject, item.Value);
                        }
                        else if (targetProperty.PropertyType.FullName == "System.DateTime") // == typeof(string))
                        {
                            targetProperty.SetValue(someObject, DateTime.Parse((string)item.Value));
                        }
                        else if (targetProperty.PropertyType.GenericTypeArguments.Count() > 0)
                        {
                            if (targetProperty.PropertyType.GenericTypeArguments.FirstOrDefault().FullName == "System.Int32")
                            {
                                targetProperty.SetValue(someObject, Convert.ToInt32(item.Value));
                            }
                        }
                        else
                        {

                            var parseMethod = targetProperty.PropertyType.GetMethod("TryParse",
                                BindingFlags.Public | BindingFlags.Static, null,
                                new[] { typeof(string), targetProperty.PropertyType.MakeByRefType() }, null);

                            if (parseMethod != null)
                            {
                                var parameters = new[] { item.Value, null };
                                var success = (bool)parseMethod.Invoke(null, parameters);
                                if (success)
                                {
                                    targetProperty.SetValue(someObject, parameters[1]);
                                }

                            }
                        }
                    }
                }

                return someObject;
            }

            return null;
        }
    }
}