using ChoiSerializer;
using ChoiSerializer.Annotation;
using System;
using System.Collections.Generic;

namespace Example
{
    [Serializable]
    public class ContentContainer : Serializable
    {
        public ContentContainer(SerializationContext context) : base(context)
        {
        }

        [SerializableCulumn(Index = 0, Length = 2)]
        public string ParcelStart { get; set; } = "PS";

        [MappedForLength(Target = "ContentContainer.Attribute")]
        [SerializableCulumn(Index = 1)]
        public int AttributeSize { get; set; } = 16; 

        [SerializableCulumn(Index = 2)]
        public byte[] Attribute { get; set; }

        [SerializableCulumn(Index = 3)]
        public short SerialNumber { get; set; } = 0;

        [MappedForType(Target = "ContentContainer.Data", ValueConverter = "ContentContainer.GetDataTypeFromChunkName")]
        [SerializableCulumn(Index = 4, Length = 10)]
        public string ChunkName { get; set; }

        public static Type GetDataTypeFromChunkName(object name)
        {
            switch ((string)name)
            {
                case "IMAGE":
                    return typeof(List<ImageContent>);
                case "SOUND":
                    return typeof(List<SoundContent>);
                case "DATA":
                    return typeof(byte[]);
            }
            return null;
        }

        [MappedForLength(Target = "ContentContainer.Data")]
        [SerializableCulumn(Index = 5)]
        public int DataCount { get; set; }

        [SerializableCulumn(Index = 6)]
        public object Data { get; set; }

        [SerializableCulumn(Index = 7, Length = 2)]
        public string ParcelTail { get; set; } = "PE";
    }
}
