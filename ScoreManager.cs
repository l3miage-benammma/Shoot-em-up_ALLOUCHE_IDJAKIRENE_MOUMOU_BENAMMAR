using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ScoreManager
{
    private readonly string _filePath;
    private readonly string _xsdPath;
    private XmlManager<ScoresRoot> _xmlManager;

    public ScoresRoot ScoresData { get; private set; }

    public ScoreManager()
    {
        // Définissez les chemins vers le fichier XML et le XSD
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "XML", "Scores.xml");
        _xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "XSD", "Scores.xsd");

        _xmlManager = new XmlManager<ScoresRoot>(_xsdPath);

        ScoresData = LoadScores();
    }

    public void AddScore(string playerName, int score, int temps)
    {
        var newScore = new ListeScores
        {
            Pseudo = playerName,
            Score = score,
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Temps = temps
        };

        ScoresData.Items.Add(newScore);
        SaveScores();
    }

    private void SaveScores()
    {
        _xmlManager.Save(_filePath, ScoresData);
    }

    private ScoresRoot LoadScores()
    {
        if (!File.Exists(_filePath))
        {
            // Si le fichier n'existe pas, on retourne un objet vide
            return new ScoresRoot();
        }

        try
        {
            return _xmlManager.Load(_filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors du chargement des scores: " + ex.Message);
            // En cas d'erreur (par ex. validation XSD), on retourne un nouvel objet vide
            return new ScoresRoot();
        }
    }
    
    [Serializable]
    [XmlRoot("Scores")]
    public class ScoresRoot
    {
        [XmlElement("ListeScores")]
        public List<ListeScores> Items { get; set; }

        public ScoresRoot()
        {
            Items = new List<ListeScores>();
        }
    }

    [Serializable]
    public class ListeScores
    {
        public string Pseudo { get; set; }
        public int Score { get; set; }
        public string Date { get; set; }
        public int Temps { get; set; }
    }
}
