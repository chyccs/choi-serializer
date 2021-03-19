using Choi.Serializer;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Choi.ByteBuffer;

namespace Example
{
    class Program
    {
        static string WORKING_DIR = Directory.GetCurrentDirectory() + @"\..\..\..";

        static Serializable BuildSerializableEntity(SerializationContext context)
        {
            var images = Directory.EnumerateFiles(WORKING_DIR + @"\images");
            var container = new ContentContainer(context);
            container.ChunkName = "IMAGE";
            container.DataCount = images.Count();
            var imageContents = new List<ImageContent>();
            foreach (var image in images)
            {
                var imageBytes = File.ReadAllBytes(image);
                var imageContent = new ImageContent(context);
                imageContent.Name = Path.GetFileName(image);
                imageContent.DataSize = imageBytes.Length;
                imageContent.Data = imageBytes;
                imageContents.Add(imageContent);
            }
            container.Data = imageContents;
            container.AttributeSize = 16;
            container.Attribute = new byte[16];
            container.SerialNumber = 123;
            return container;
        }

        static void ExampleUsingFormatter()
        {
            string imageContentFile = WORKING_DIR + @"\image_content_using_formatter.data";

            ChoiFormatter formatter = new ChoiFormatter(typeof(ContentContainer));

            // Example of reading an image file and serializing it into a single binary file
            using (var stream = new FileStream(imageContentFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var buffer = new StreamByteBuffer(stream))
            using (var context = new SerializationContext(buffer))
            {
                formatter.Serialize(stream, BuildSerializableEntity(context));
            }

            // Example of deserializing an binary file
            using (var stream = new FileStream(imageContentFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                ContentContainer data2 = (ContentContainer)formatter.Deserialize(stream);
            }
        }

        static void Example()
        {
            string imageContentFile = WORKING_DIR + @"\image_content.data";

            // Example of reading an image file and serializing it into a single binary file
            using (var buffer = new StreamByteBuffer(new FileStream(imageContentFile, FileMode.Create, FileAccess.Write, FileShare.None)))
            using (var context = new SerializationContext(buffer))
            {
                var container = BuildSerializableEntity(context);
                container.Serialize();
            }

            // Example of deserializing an binary file
            using (var buffer = new StreamByteBuffer(new FileStream(imageContentFile, FileMode.Open, FileAccess.Read, FileShare.None)))
            using (var context = new SerializationContext(buffer))
            {
                var container = new ContentContainer(context);
                container.Deserialize();
            }
        }

        static void Main(string[] args)
        {
            ExampleUsingFormatter();
            Example();
        }
    }
}
