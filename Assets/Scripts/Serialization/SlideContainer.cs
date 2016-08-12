using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("SlideCollection")]
public class SlideContainer
{
    [XmlArray("Slides")]
    [XmlArrayItem("Slide")]
    public List<SlideData> slides = new List<SlideData>();

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(SlideContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static SlideContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(SlideContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as SlideContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static SlideContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(SlideContainer));
        return serializer.Deserialize(new StringReader(text)) as SlideContainer;
    }
}
