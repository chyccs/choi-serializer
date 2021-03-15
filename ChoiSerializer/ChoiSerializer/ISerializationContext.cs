using ChoiSerializer.ByteBuffer;
using System;

namespace ChoiSerializer
{
    public interface ISerializationContext : IDisposable
    {
        IByteBuffer DataSource { get; set; }

        decimal GetToDecimal(string key, string suffix = "");

        void SetDecimal(string key, object value, string suffix = "");

        object Get(string key, string suffix = "");

        void Set(string key, object value, string suffix = "");
    }
}
