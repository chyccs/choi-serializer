using ChoiSerializer;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingDir = Directory.GetCurrentDirectory() + @"\..\..\..";
            string imageContentFile = workingDir + @"\image_content.data";

            Console.WriteLine("Hello World! Working with Choi's Serializer");

            // Example of reading an image file and serializing it into a single binary file
            var images = Directory.EnumerateFiles(workingDir + @"\images");
            using (var context = new SerializationContext())
            {
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
                File.WriteAllBytes(imageContentFile, container.Serialize());
            }

            // Example of deserializing an binary file
            using (var context = new SerializationContext(imageContentFile))
            {
                var container = new ContentContainer(context);
                container.Deserialize();
            }
        }
    }
}
