using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Jeu.Models;

public class Enemies
{
    public Vector2 Position; // Position de l'ennemi
    private readonly Texture2D _texture; // Texture de l'ennemi
    
    public static float GlobalSpeed { get; set; } = 100f; // Vitesse initiale partagée par tous les ennemis

    public Enemies(Texture2D texture, Vector2 startPosition){
        _texture = texture;
        Position = startPosition;
    }

    public void Update(GameTime gameTime){
        // Faire descendre l'ennemi à une vitesse constante
        Position.Y += GlobalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch){
        spriteBatch.Draw(_texture, Position, Color.White);
    }
}