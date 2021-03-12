using System.IO;
using System.Runtime.Serialization;

namespace ChoiSerializer
{
    public class ChoiFormatter : IFormatter
    {
        public SerializationBinder Binder { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public StreamingContext Context { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public ISurrogateSelector SurrogateSelector { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public object Deserialize(Stream serializationStream)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            throw new System.NotImplementedException();
        }
    }
}
