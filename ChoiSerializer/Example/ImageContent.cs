using ChoiSerializer;
using ChoiSerializer.Annotation;
using System;

namespace Example
{
    [Serializable]
    public class ImageContent : Serializable
    {
        public ImageContent(SerializationContext context) : base(context)
        {
        }

        [SerializableCulumn(Index = 0, Length = 2)]
        public string ContentStart { get; set; } = "CS";

        [SerializableCulumn(Index = 1, Length = 100)]
        public string Name { get; set; } 

        [MappedForLength(Target = "ImageContent.Data")]
        [SerializableCulumn(Index = 2)]
        public int DataSize { get; set; }

        [SerializableCulumn(Index = 3)]
        public byte[] Data { get; set; }

        [SerializableCulumn(Index = 4)]
        public short Checksum { get; set; } = 0;

        [SerializableCulumn(Index = 5, Length = 2)]
        public string ContentTail { get; set; } = "CE";
    }
}
