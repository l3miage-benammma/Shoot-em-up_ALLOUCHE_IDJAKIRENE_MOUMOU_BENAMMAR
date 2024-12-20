using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Jeu.Models;

public class Ship
{
    public Vector2 Position;
    private Vector2 _direction = Vector2.Zero;
    private readonly float _speed = 500f;
    private readonly Texture2D _texture;
    private Rectangle _rectangle;
    private float _frameTime = 0.1f;
    private int _currentFrame;
    private int _increment = 1;
    private readonly Point _frameSize;
    public int Width => _frameSize.X;
    public int Height => _frameSize.Y;


    public Ship(Texture2D texture)
    {
        _texture = texture;
        _frameSize = new(texture.Width / 3, texture.Height / 3); // Diviser en 3x3 frames
    }

    private void UpdateAnimation(GameTime gameTime)
    {
        _frameTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_frameTime < 0)
        {
            _frameTime += 0.1f;
            _currentFrame += _increment;

            if (_currentFrame == 2) _increment = -1;
            if (_currentFrame == 0) _increment = 1;
        }
    }

    private void UpdateControls()
    {
        _direction = Vector2.Zero;

        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Left)) _direction.X = -1;
        if (keyboardState.IsKeyDown(Keys.Right)) _direction.X = 1;
        if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y = -1;
        if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y = 1;

        if (_direction != Vector2.Zero) _direction.Normalize();
    }

    private void UpdateRectangle()
    {
        var row = 0;
        if (_direction.X > 0) row = 1;
        if (_direction.X < 0) row = 2;
        Point location = new(_currentFrame * _frameSize.X, row * _frameSize.Y);

        _rectangle = new(location, _frameSize);
    }

    public void Update(GameTime gameTime, int screenWidth, int screenHeight)
    {
        UpdateAnimation(gameTime);
        UpdateControls();
        UpdateRectangle();

        Position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Empêcher de sortir des limites
        Position.X = MathHelper.Clamp(Position.X, 0, screenWidth - _frameSize.X);
        Position.Y = MathHelper.Clamp(Position.Y, 0, screenHeight - _frameSize.Y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, _rectangle, Color.White);
    }
}
