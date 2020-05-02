using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MTPHelper
{
    [Serializable()]
    public class Settings
    {
        public List<string> ImageTypes = new List<string>();
        public List<string> SoundTypes = new List<string>();
        public List<string> VideoTypes = new List<string>();
        public Settings(bool v)
        {
            ImageTypes.Add("*.png");
            ImageTypes.Add("*.jpg");
            ImageTypes.Add("*.jpeg");
            ImageTypes.Add("*.gif");
            ImageTypes.Add("*.jfif");
            ImageTypes.Add("*.bmp");
            ImageTypes.Add("*.ico");

            SoundTypes.Add("*.mp3");
            SoundTypes.Add("*.wav");
            SoundTypes.Add("*.wma");
            SoundTypes.Add("*.m4a");
            SoundTypes.Add("*.aac");
            SoundTypes.Add("*.ogg");

            VideoTypes.Add("*.mp4");
            VideoTypes.Add("*.mkv");
            VideoTypes.Add("*.mov");
        }

        public Settings()
        {

        }

        public void Save(string path)
        {
            XmlSerializer xml_serializer = new XmlSerializer(typeof(Settings));
            using (StringWriter string_writer = new StringWriter())
            {
                // Serialize.
                xml_serializer.Serialize(string_writer, this);

                File.WriteAllText(path, string_writer.ToString());
            }
        }

        public static Settings FromFile(string path)
        {
            string serialized = File.ReadAllText(path);
            XmlSerializer xml_serializer = new XmlSerializer(typeof(Settings));
            using (StringReader string_reader = new StringReader(serialized))
            {
                Settings s = (Settings)(xml_serializer.Deserialize(string_reader));
                return s;
            }
        }
    }
}