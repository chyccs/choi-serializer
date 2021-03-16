using ChoiSerializer;
using ChoiSerializer.Annotation;
using System;

namespace Example
{
    [Serializable]
    public class ImageContent : Serializable
    {
        public override ISerializationContext Context { get; set; }

        public ImageContent(SerializationContext context)
        {
            Context = context;
        }

        [SerializableCulumn(Length = 2)]
        public string ContentStart { get; set; } = "CS";

        [SerializableCulumn(Length = 100)]
        public string Name { get; set; }

        [MappedForLength(Target = "ImageContent.Data")]
        [SerializableCulumn]
        public int DataSize { get; set; }

        [SerializableCulumn]
        public byte[] Data { get; set; }

        [SerializableCulumn]
        public short Checksum { get; set; } = 0;

        [SerializableCulumn(Length = 2)]
        public string ContentTail { get; set; } = "CE";
    }
}
