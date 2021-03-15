using ChoiSerializer.ByteBuffer;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace ChoiSerializer
{
    public class ChoiFormatter : IFormatter
    {
        public SerializationBinder Binder { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public StreamingContext Context { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ISurrogateSelector SurrogateSelector { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private Type type;

        public ChoiFormatter(Type type)
        {
            if (!Serializable.IsSerializableClass(type))
                throw new SerializationException();

            this.type = type;
        }

        public object Deserialize(Stream serializationStream)
        {
            using (var buffer = new StreamByteBuffer(serializationStream))
            using (var context = new SerializationContext(buffer))
            {
                Serializable o = Activator.CreateInstance(type, context) as Serializable;
                o.Deserialize();
                return o;
            }
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            if (!graph.GetType().Equals(type))
                throw new SerializationException();
            ((Serializable)graph).Serialize();
        }
    }
}
