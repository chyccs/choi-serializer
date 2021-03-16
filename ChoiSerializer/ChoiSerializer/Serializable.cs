using ChoiSerializer.Annotation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ChoiSerializer
{
    [Serializable]
    public abstract class Serializable
    {
        public abstract ISerializationContext Context { get; set; }

        public void Serialize()
        {
            Type myType = GetType();

            var prs = myType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.CustomAttributes.Where(att => att.AttributeType == typeof(SerializableCulumn)).Count() > 0).ToList();
            prs.OrderBy(p => (int)p.CustomAttributes.Where(att => att.AttributeType == typeof(SerializableCulumn)).FirstOrDefault().NamedArguments.Where(a => a.MemberName.Equals("Index")).FirstOrDefault().TypedValue.Value).ToList();

            foreach (var item in prs)
            {
                foreach (CustomAttributeData att in item.CustomAttributes)
                {
                    if (att.AttributeType == typeof(SerializableCulumn))
                    {
                        var culumnDefi = (SerializableCulumn)Attribute.GetCustomAttributes(item).ToList().Where(attr => attr is SerializableCulumn).FirstOrDefault();

                        int length = culumnDefi.Length;

                        switch (Type.GetTypeCode(item.PropertyType))
                        {
                            case TypeCode.Single:
                                Context.DataSource.Put(((float)item.GetValue(this)));
                                break;

                            case TypeCode.UInt16:
                                Context.DataSource.Put((ushort)item.GetValue(this));
                                break;

                            case TypeCode.UInt32:
                                Context.DataSource.Put((uint)item.GetValue(this));
                                break;

                            case TypeCode.UInt64:
                                Context.DataSource.Put((ulong)item.GetValue(this));
                                break;

                            case TypeCode.Int16:
                                Context.DataSource.Put((short)item.GetValue(this));
                                break;

                            case TypeCode.Int32:
                                Context.DataSource.Put((int)item.GetValue(this));
                                break;

                            case TypeCode.Int64:
                                Context.DataSource.Put((long)item.GetValue(this));
                                break;

                            case TypeCode.Byte:
                                Context.DataSource.Put((byte)item.GetValue(this));
                                break;

                            case TypeCode.Char:
                                Context.DataSource.Put((byte)(char)item.GetValue(this));
                                break;

                            case TypeCode.String:
                                Context.DataSource.Put(Encoding.UTF8.GetBytes((string)item.GetValue(this)), length);
                                break;

                            case TypeCode.Object:

                                if (item.GetValue(this) == null)
                                    continue;

                                if (item.PropertyType.IsGenericType && item.PropertyType.Name.StartsWith("List"))
                                {
                                    var objects = (IList)item.GetValue(this);
                                    if (objects != null)
                                    {
                                        foreach (Serializable obj in objects)
                                        {
                                            obj.Context = this.Context;
                                            obj.Serialize();
                                        }
                                    }
                                }
                                else if (item.PropertyType == typeof(byte[]))
                                {
                                    Context.DataSource.Put((byte[])item.GetValue(this));
                                }
                                else
                                {
                                    var obj = item.GetValue(this);
                                    if (obj is Serializable)
                                    {
                                        ((Serializable)obj).Context = this.Context;
                                        ((Serializable)obj).Serialize();
                                    }
                                    else if (obj is byte[] && obj != null)
                                    {
                                        Context.DataSource.Put((byte[])item.GetValue(this));
                                    }
                                    else if (obj.GetType().Name.StartsWith("List"))
                                    {
                                        var objects = (IList)obj;
                                        if (objects != null)
                                        {
                                            foreach (Serializable sobj in objects)
                                            {
                                                sobj.Context = this.Context;
                                                sobj.Serialize();
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                        //Console.WriteLine(item.DeclaringType.Name + "." + item.Name);
                    }
                }
            }
        }

        public void Deserialize()
        {
            Type myType = GetType();

            var prs = myType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.CustomAttributes.Where(att => att.AttributeType == typeof(SerializableCulumn)).Count() > 0).ToList();
            prs.OrderBy(p => (int)p.CustomAttributes.Where(att => att.AttributeType == typeof(SerializableCulumn)).FirstOrDefault().NamedArguments.Where(a => a.MemberName.Equals("Index")).FirstOrDefault().TypedValue.Value).ToList();

            foreach (var item in prs)
            {
                foreach (CustomAttributeData att in item.CustomAttributes)
                {
                    if (att.AttributeType == typeof(SerializableCulumn))
                    {
                        var culumnDefi = (SerializableCulumn)Attribute.GetCustomAttributes(item).ToList().Where(attr => attr is SerializableCulumn).FirstOrDefault();
                        //var mappedForSizeDefinition = (MappedForSize)Attribute.GetCustomAttributes(item).ToList().Where(attr => attr is MappedForSize).FirstOrDefault();
                        var mappedForLengthDefinition = (MappedForLength)Attribute.GetCustomAttributes(item).ToList().Where(attr => attr is MappedForLength).FirstOrDefault();
                        var mappedForTypeDefinition = (MappedForType)Attribute.GetCustomAttributes(item).ToList().Where(attr => attr is MappedForType).FirstOrDefault();

                        int length = culumnDefi.Length;

                        object fieldValue = null;
                        switch (Type.GetTypeCode(item.PropertyType))
                        {
                            case TypeCode.Single:
                                fieldValue = Context.DataSource.Get<float>(); //BitConverter.ToSingle(Context.DataSource.Get(Math.Max(4, length)), 0);
                                break;

                            case TypeCode.UInt16:
                                fieldValue = Context.DataSource.Get<ushort>(); //BitConverter.ToUInt16(Context.DataSource.Get(Math.Max(2, length)), 0);
                                break;

                            case TypeCode.UInt32:
                                fieldValue = Context.DataSource.Get<uint>(); //BitConverter.ToUInt32(Context.DataSource.Get(Math.Max(4, length)), 0);
                                break;

                            case TypeCode.UInt64:
                                fieldValue = Context.DataSource.Get<ulong>(); //BitConverter.ToUInt64(Context.DataSource.Get(Math.Max(8, length)), 0);
                                break;

                            case TypeCode.Int16:
                                fieldValue = Context.DataSource.Get<short>(); //BitConverter.ToInt16(Context.DataSource.Get(Math.Max(2, length)), 0);
                                break;

                            case TypeCode.Int32:
                                fieldValue = Context.DataSource.Get<int>(); //BitConverter.ToInt32(Context.DataSource.Get(Math.Max(4, length)), 0);
                                break;

                            case TypeCode.Int64:
                                fieldValue = Context.DataSource.Get<long>(); //BitConverter.ToInt64(Context.DataSource.Get(Math.Max(8, length)), 0);
                                break;

                            case TypeCode.Byte:
                                fieldValue = Context.DataSource.Get(1)[0];
                                break;

                            case TypeCode.Char:
                                fieldValue = Context.DataSource.Get<char>(); //(char)Context.DataSource.Get(1)[0];
                                break;

                            case TypeCode.String:
                                fieldValue = Context.DataSource.Get<string>(length); //Context.DataSource.GetString(length);
                                break;

                            case TypeCode.Object:
                                if (item.PropertyType.IsGenericType && item.PropertyType.Name.StartsWith("List"))
                                {
                                    var types = item.PropertyType.GetGenericArguments()[0];

                                    if (length <= 0)
                                    {
                                        length = FindLength(item.DeclaringType.Name, item.Name);
                                    }

                                    if (IsSerializableClass(types))
                                    {
                                        Type d1 = typeof(List<>);
                                        Type constructed = d1.MakeGenericType(types);
                                        IList o = Activator.CreateInstance(constructed) as IList;

                                        for (int i = 0; i < length; i++)
                                        {
                                            var obj = Activator.CreateInstance(types, Context);
                                            ((Serializable)obj).Deserialize();
                                            o.Add(obj);
                                        }

                                        fieldValue = o;
                                        item.SetValue(this, o);
                                    }
                                }
                                else if (item.PropertyType == typeof(byte[]))
                                {
                                    if (length <= 0)
                                    {
                                        length = FindLength(item.DeclaringType.Name, item.Name);
                                    }

                                    fieldValue = length <= 0 ? null : Context.DataSource.Get(length);
                                }
                                else if (item.PropertyType == typeof(object))
                                {
                                    Type type = (Type)Context.Get(item.DeclaringType.Name + "." + item.Name);

                                    if (type != null && type.Name.StartsWith("List"))
                                    {
                                        var types = type.GetGenericArguments()[0];

                                        if (length <= 0)
                                        {
                                            length = FindLength(item.DeclaringType.Name, item.Name);
                                        }

                                        if (IsSerializableClass(types))
                                        {
                                            Type d1 = typeof(List<>);
                                            Type constructed = d1.MakeGenericType(types);
                                            IList o = Activator.CreateInstance(constructed) as IList;

                                            for (int i = 0; i < length; i++)
                                            {
                                                var obj = Activator.CreateInstance(types, Context);
                                                ((Serializable)obj).Deserialize();
                                                o.Add(obj);
                                            }

                                            fieldValue = o;
                                            item.SetValue(this, o);
                                        }
                                    }
                                    else if (type != null && IsSerializableClass(type))
                                    {
                                        var obj = Activator.CreateInstance(type, Context);
                                        ((Serializable)obj).Deserialize();
                                        fieldValue = obj;
                                    }
                                    else if (type == typeof(byte[]))
                                    {
                                        if (length <= 0)
                                        {
                                            length = FindLength(item.DeclaringType.Name, item.Name);
                                        }

                                        try
                                        {
                                            fieldValue = Context.DataSource.Get(length);
                                        }
                                        catch
                                        {
                                            Debug.WriteLine(Context.DataSource.Size - Context.DataSource.PositionToRead);
                                        }
                                    }
                                }
                                else
                                {
                                    if (IsSerializableClass(item.PropertyType))
                                    {
                                        var obj = Activator.CreateInstance(item.PropertyType, Context);
                                        ((Serializable)obj).Deserialize();
                                        fieldValue = obj;
                                    }
                                }
                                break;
                        }

                        if (fieldValue != null)
                        {
                            item.SetValue(this, fieldValue);
                        }

                        if (!string.IsNullOrEmpty(mappedForLengthDefinition?.Target))
                        {
                            Context.SetDecimal(mappedForLengthDefinition.Target, mappedForLengthDefinition.ValueConverterDelegate != null ? mappedForLengthDefinition.ValueConverterDelegate(fieldValue) : fieldValue, "length");
                        }

                        if (!string.IsNullOrEmpty(mappedForTypeDefinition?.Target))
                        {
                            Context.Set(mappedForTypeDefinition.Target, mappedForTypeDefinition.ValueConverterDelegate != null ? mappedForTypeDefinition.ValueConverterDelegate(fieldValue) : fieldValue);
                        }

                        Debug.WriteLine(item.DeclaringType.Name + "." + item.Name + " => " + fieldValue);
                    }
                }
            }
        }

        private int FindLength(string type, string property)
        {
            return (int)Context.GetToDecimal(type + "." + property, "length");
        }

        private int FindSize(string type, string property)
        {
            return (int)Context.GetToDecimal(type + "." + property, "size");
        }

        private object GetAnnotationValue(PropertyInfo field, Type type, string property)
        {
            return field.CustomAttributes.Where(att => att.AttributeType == type).FirstOrDefault().NamedArguments.Where(a => a.MemberName.Equals(property)).FirstOrDefault().TypedValue.Value;
        }

        public static bool IsSerializableClass(Type type)
        {
            return Attribute.GetCustomAttributes(type).ToList().Where(attr => attr is SerializableAttribute).Count() > 0;
        }

        private PropertyInfo GetFieldUsingProperty(Type type, string propertyName, string propertyValue)
        {
            Type myType = GetType();
            return myType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => (string)GetAnnotationValue(p, type, propertyName) == propertyValue).FirstOrDefault();
        }

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            Type myType = GetType();
            return myType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.Name.Equals(propertyName)).FirstOrDefault();
        }
    }
}
