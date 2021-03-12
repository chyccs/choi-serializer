namespace ChoiSerializer.Annotation
{
    public class SerializableCulumn : SerializerBaseAttribute
    {
        public int Index { get; set; }

        public int Length { get; set; } = 0;
    }
}
