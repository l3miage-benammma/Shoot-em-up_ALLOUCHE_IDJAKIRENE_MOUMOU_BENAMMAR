using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class XmlManager<T>
{
    private readonly string _xsdPath;

    public XmlManager(string xsdPath = null)
    {
        _xsdPath = xsdPath;
    }

    public void Save(string path, T obj)
    {
        using (var writer = new StreamWriter(path))
        {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj);
        }
    }

    public T Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Le fichier {path} est introuvable.");

        // Validation XML si XSD existe
        if (!string.IsNullOrEmpty(_xsdPath) && File.Exists(_xsdPath))
        {
            ValidateXml(path, _xsdPath);
        }

        using (var reader = new StreamReader(path))
        {
            var xml = new XmlSerializer(typeof(T));
            return (T)xml.Deserialize(reader);
        }
    }

    private void ValidateXml(string xmlPath, string xsdPath)
    {
        var settings = new XmlReaderSettings();
        settings.Schemas.Add(null, xsdPath);
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += ValidationCallBack;

        using (XmlReader reader = XmlReader.Create(xmlPath, settings))
        {
            while (reader.Read()) { }
        }
    }

    private void ValidationCallBack(object sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Error)
        {
            throw new Exception($"Erreur de validation XML: {e.Message}");
        }
        else
        {
            Console.WriteLine($"Avertissement de validation XML: {e.Message}");
        }
    }
}