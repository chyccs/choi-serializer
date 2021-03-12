using ChoiSerializer;
using ChoiSerializer.Annotation;
using System;

namespace Example
{
    [Serializable]
    public class SoundContent : Serializable
    {
        public SoundContent(SerializationContext context) : base(context)
        {
        }

        [SerializableCulumn(Index = 0, Length = 2)]
        public string ContentStart { get; set; } = "SS";

        [SerializableCulumn(Index = 1, Length = 20)]
        public string Name { get; set; }

        [SerializableCulumn(Index = 2)]
        public long PlayingTime { get; set; } = 0;

        [MappedForLength(Target = "SoundContent.Data")]
        [SerializableCulumn(Index = 3)]
        public int DataSize { get; set; }

        [SerializableCulumn(Index = 4)]
        public byte[] Data { get; set; }

        [SerializableCulumn(Index = 5)]
        public short Checksum { get; set; } = 0;

        [SerializableCulumn(Index = 6, Length = 2)]
        public string ContentTail { get; set; } = "SE";
    }
}
