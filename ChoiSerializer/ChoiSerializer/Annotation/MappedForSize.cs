using System;
using System.Reflection;

namespace ChoiSerializer.Annotation
{
    public delegate int ValueToSizeConvertDelegate(object value);

    public class MappedForSize : SerializerBaseAttribute
    {
        public string Target { get; set; }

        public ValueToSizeConvertDelegate ValueConverterDelegate { get; set; }

        private string valueConverter = "";

        public string ValueConverter
        {
            set
            {
                valueConverter = value;
                var arguments = value.Split('.');
                var assembly = Assembly.GetExecutingAssembly();
                Type callerType = null;
                foreach (var type in assembly.ExportedTypes)
                {
                    if (type.Name.Equals(arguments[0]))
                        callerType = type;
                }
                ValueConverterDelegate = (ValueToSizeConvertDelegate)Delegate.CreateDelegate(typeof(ValueToSizeConvertDelegate), callerType, arguments[1]);
            }
            get { return valueConverter; }
        }
    }
}
