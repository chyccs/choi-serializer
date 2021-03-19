[![NuGet](https://img.shields.io/nuget/v/ChoiSerializer)](https://www.nuget.org/packages/ChoiSerializer/)
[![NuGet](https://img.shields.io/nuget/dt/ChoiSerializer)](https://www.nuget.org/packages/ChoiSerializer/)

ChoiSerializer
================

Serialization library based on data location and length for the .NET platform. 
This turns your instance into a lightweight binary file.

## Usage ##

### Create model class ###

Inherit the Serializable class to the target class to be serialized.
Add the SerializableCulumn attribute to the property to be serialized. You can specify the length according to your needs.
If the type and length of the property must be determined dynamically, the MappedForType and MappedForLength attributes can be used.
(The example is the contents of creating a binary file that contains several image files in a container.)

```c#
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
```

```c#
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
```

```c#
static Serializable BuildSerializableEntity(SerializationContext context)
{
    var images = Directory.EnumerateFiles(@"~\images");
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
```

### Serialization using formatter ###

```c#
string imageContentFile = @"~\image_content_using_formatter.data";

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
```

### Serializing without formatter ###

```c#
string imageContentFile = @"~\image_content.data";

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
```


