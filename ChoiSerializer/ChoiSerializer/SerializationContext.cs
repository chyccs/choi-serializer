using Choi.ByteBuffer;
using System;
using System.Collections.Generic;
using System.IO;

namespace Choi.Serializer
{
    public class SerializationContext : Dictionary<string, object>, ISerializationContext
    {
        public IByteBuffer DataSource { get; set; }

        public SerializationContext()
        {
            DataSource = new InMemoryByteBuffer();
        }

        public SerializationContext(IByteBuffer dataSource)
        {
            DataSource = dataSource;
        }

        public SerializationContext(string filepath)
        {
            DataSource = new InMemoryByteBuffer(File.ReadAllBytes(filepath));
        }

        public SerializationContext(byte[] file)
        {
            DataSource = new InMemoryByteBuffer(file);
        }

        public decimal GetToDecimal(string key, string suffix = "")
        {
            suffix = !string.IsNullOrEmpty(suffix) ? "_" + suffix : suffix;
            object value = 0;
            TryGetValue(key + suffix + "_decimal", out value);
            return value != null ? (decimal)value : decimal.Zero;
        }

        public void SetDecimal(string key, object value, string suffix = "")
        {
            suffix = !string.IsNullOrEmpty(suffix) ? "_" + suffix : suffix;
            Remove(key + suffix + "_decimal");
            Add(key + suffix + "_decimal", Convert.ToDecimal(value));
        }

        public object Get(string key, string suffix = "")
        {
            suffix = !string.IsNullOrEmpty(suffix) ? "_" + suffix : suffix;
            object value;
            TryGetValue(key + suffix + "_object", out value);
            return value;
        }

        public void Set(string key, object value, string suffix = "")
        {
            suffix = !string.IsNullOrEmpty(suffix) ? "_" + suffix : suffix;
            Remove(key + suffix + "_object");
            Add(key + suffix + "_object", value);
        }

        public void Dispose()
        {
            DataSource?.Dispose();
            DataSource = null;
            GC.SuppressFinalize(this);
        }
    }
}
