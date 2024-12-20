using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Jeu.Models;

public class Projectile
{
    public Vector2 Position;
    public Vector2 Direction;
    private readonly float _speed = 300f;
    private readonly Texture2D _texture;

    public Projectile(Texture2D texture, Vector2 position, Vector2 direction)
    {
        _texture = texture;
        Position = position;
        Direction = direction;
        Direction.Normalize(); // Assurer une direction normalisée
    }

    public void Update(GameTime gameTime)
    {
        Position += Direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Color.White);
    }
}