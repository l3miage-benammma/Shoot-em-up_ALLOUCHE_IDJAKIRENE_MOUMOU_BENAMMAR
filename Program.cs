using System;
using System.IO;

using var game = new Game1();
game.Run();
/*
 Le code en commentaire s'agit d'un test pour voir si les classes XmlManger ScoresRoot et ScoreManager marchent, 
 mais aussi que la valisation XML fonctionne bien avec un exemple
 
 class Program
{

    static void Main(string[] args)
    {
        string xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "XML", "Scores.xml");
        string xsdFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "XSD", "Scores.xsd");

        var xmlManager = new XmlManager<ScoresRoot>(xsdFilePath);

        ScoresRoot scoresRoot;

        // Essayer de charger le fichier s'il existe déjà
        if (File.Exists(xmlFilePath))
        {
            try
            {
                scoresRoot = xmlManager.Load(xmlFilePath);
                Console.WriteLine("Scores existants chargés :");
                foreach (var sc in scoresRoot.Items)
                {
                    Console.WriteLine($"{sc.Pseudo} - {sc.Score} - {sc.Date} - {sc.Temps}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur au chargement des scores existants : " + ex.Message);
                // En cas d'erreur de validation , on  repartira sur un nouveau fichier
                scoresRoot = new ScoresRoot();
            }
        }
        else
        {
            // S'il n'y a pas de fichier, on part sur un nouveau fichier .xml
            scoresRoot = new ScoresRoot();
        }

        // Ajouter de nouveaux scores
        scoresRoot.Items.Add(new ListeScores { Pseudo = "Alice", Score = 300, Date = "2024-12-20", Temps = 60 });

        
        xmlManager.Save(xmlFilePath, scoresRoot);
        Console.WriteLine("Nouveaux scores sauvegardés.");

        // Afficher la version finale
        Console.WriteLine("Contenu final du fichier :");
        Console.WriteLine(File.ReadAllText(xmlFilePath));
    }
}*/