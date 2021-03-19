using Choi.Serializer;
using Choi.Serializer.Annotation;
using System;
using System.Collections.Generic;

namespace Example
{
    [Serializable]
    public class ContentContainer : Serializable
    {
        public override ISerializationContext Context { get; set; }

        public ContentContainer(SerializationContext context)
        {
            Context = context;
        }

        [SerializableCulumn(Length = 2)]
        public string ParcelStart { get; set; } = "PS";

        [MappedForLength(Target = "ContentContainer.Attribute")]
        [SerializableCulumn]
        public int AttributeSize { get; set; } = 16;

        [SerializableCulumn]
        public byte[] Attribute { get; set; }

        [SerializableCulumn]
        public short SerialNumber { get; set; } = 0;

        [MappedForType(Target = "ContentContainer.Data", ValueConverter = "ContentContainer.GetDataTypeFromChunkName")]
        [SerializableCulumn(Length = 10)]
        public string ChunkName { get; set; }

        public static Type GetDataTypeFromChunkName(object name)
        {
            switch ((string)name)
            {
                case "IMAGE":
                    return typeof(List<ImageContent>);
                case "DATA":
                    return typeof(byte[]);
            }
            return null;
        }

        [MappedForLength(Target = "ContentContainer.Data")]
        [SerializableCulumn]
        public int DataCount { get; set; }

        [SerializableCulumn]
        public object Data { get; set; }

        [SerializableCulumn(Length = 2)]
        public string ParcelTail { get; set; } = "PE";
    }
}
