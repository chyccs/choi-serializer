using System;
using System.Reflection;

namespace Choi.Serializer.Annotation
{
    public delegate int ValueToLengthConvertDelegate(object value);

    public class MappedForLength : SerializerBaseAttribute
    {
        public string Target { get; set; }

        public ValueToLengthConvertDelegate ValueConverterDelegate { get; set; }

        private string valueConverter = "";

        public string ValueConverter
        {
            set
            {
                valueConverter = value;
                var arguments = value.Split('.');
                var assembly = Assembly.GetEntryAssembly();
                Type callerType = null;
                foreach (var type in assembly.ExportedTypes)
                {
                    if (type.Name.Equals(arguments[0]))
                        callerType = type;
                }
                ValueConverterDelegate = (ValueToLengthConvertDelegate)Delegate.CreateDelegate(typeof(ValueToLengthConvertDelegate), callerType, arguments[1]);
            }
            get { return valueConverter; }
        }
    }
}
