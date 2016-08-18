using System.Xml;
using System.Xml.Serialization;

public class SlideData {

    public SlideData()
    {

    }

    [XmlAttribute("name")]
    public string slideName;

    [XmlElement("Syllables")]
    public string syllables;

    [XmlElement("AlreadyFilled")]
    public string alreadyFilled;

}
