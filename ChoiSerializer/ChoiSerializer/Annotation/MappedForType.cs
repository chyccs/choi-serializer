using System;
using System.Reflection;

namespace ChoiSerializer.Annotation
{
    public delegate Type ValueToTypeConvertDelegate(object value);

    public class MappedForType : SerializerBaseAttribute
    {
        public string Target { get; set; }

        public ValueToTypeConvertDelegate ValueConverterDelegate { get; set; }

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
                ValueConverterDelegate = (ValueToTypeConvertDelegate)Delegate.CreateDelegate(typeof(ValueToTypeConvertDelegate), callerType, arguments[1]);
            }
            get { return valueConverter; }
        }

        //private int GetLengthOfTarget(string valueConverter, object fieldValue)
        //{
        //    var arguments = valueConverter.Split('.');
        //    var assembly = Assembly.GetCallingAssembly();
        //    Type callerType = null;
        //    foreach (var type in assembly.ExportedTypes)
        //    {
        //        if (type.Name.Equals(arguments[0]))
        //            callerType = type;
        //    }
        //    ValueToLengthConvertDelegate valueConverterDelegate = (ValueToLengthConvertDelegate)Delegate.CreateDelegate(typeof(ValueToLengthConvertDelegate), callerType, arguments[1]);
        //    return valueConverterDelegate(fieldValue);
        //}

        //private Type GetTypeOfTarget(string valueConverter, object fieldValue)
        //{
        //    var arguments = valueConverter.Split('.');
        //    var assembly = Assembly.GetEntryAssembly();
        //    Type callerType = null;
        //    foreach (var type in assembly.ExportedTypes)
        //    {
        //        if (type.Name.Equals(arguments[0]))
        //            callerType = type;
        //    }
        //    ValueToTypeConvertDelegate valueConverterDelegate = (ValueToTypeConvertDelegate)Delegate.CreateDelegate(typeof(ValueToTypeConvertDelegate), callerType, arguments[1]);
        //    return valueConverterDelegate(fieldValue);
        //}
    }
}
