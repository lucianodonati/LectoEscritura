using System.Xml;
using System.Xml.Serialization;

public class SlideData {

    public SlideData()
    {

    }

    [XmlAttribute("name")]
    public string slideName;

    [XmlElement("ImagePath")]
    public string imagePath;

    [XmlElement("Syllables")]
    public string syllables;

}
