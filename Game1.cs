using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Projet_Jeu;
using Projet_Jeu.Models;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Joueur
    private Ship _playerShip;
    private int _lives = 3; 
    private string _nomJoueur = "";

    // Timer pour le temps de jeu
    private float _elapsedSeconds = 0f; // Nouvelle variable pour le temps écoulé

    // Projectiles
    private List<Projectile> _projectiles;
    private Texture2D _projectileTexture;
    private float _projectileCooldown = 0.3f;
    private float _timeSinceLastShot = 0f;

    // Ennemis
    private List<Enemies> _enemies;
    private Texture2D _enemyTexture;
    private float _enemySpawnTimer;
    private float _ennemisIntervalle = 1f;

    // Score
    private int _score; 
    private SpriteFont _font;

    private ScoreManager _scoreManager; 
    
    // Background
    private Texture2D _backgroundTexture;

    // GameOver
    private bool _Gameover = false;

    public Game1(){
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize(){
        _projectiles = new List<Projectile>();
        _enemies = new List<Enemies>();
        _scoreManager = new ScoreManager();

        // Demander le nom du joueur
        Console.WriteLine("Enter your name:");
        _nomJoueur = Console.ReadLine();

        // Initialiser le timer dès que le nom est entré
        _elapsedSeconds = 0f;

        base.Initialize();
    }

    protected override void LoadContent(){
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _backgroundTexture = Content.Load<Texture2D>("background");

        var shipTexture = Content.Load<Texture2D>("ship");
        _playerShip = new Ship(shipTexture);
        _playerShip.Position = new Vector2(200,200); 

        _projectileTexture = Content.Load<Texture2D>("projectile1");
        _enemyTexture = Content.Load<Texture2D>("enemy3");

        _font = Content.Load<SpriteFont>("DefaultFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (_Gameover)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.R)) // Redémarrer le jeu
            {
                RestartGame();
            }
            return; 
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Incrémenter le temps écoulé
        _elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

        UpdateJoueur(gameTime);
        HandleShooting(gameTime);
        UpdateProjectiles(gameTime);
        SpawnEnemies(gameTime);
        UpdateEnnemis(gameTime);
        DetectCollisions();
        CheckPlayerLives();

        base.Update(gameTime);
    }

    private void UpdateJoueur(GameTime gameTime){
        _playerShip.Update(gameTime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    }

    private void HandleShooting(GameTime gameTime){
        var keyboardState = Keyboard.GetState();
        _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboardState.IsKeyDown(Keys.Space) && _timeSinceLastShot >= _projectileCooldown)
        {
            var projectileStartPosition = new Vector2(
                _playerShip.Position.X + (_playerShip.Width / 2) - (_projectileTexture.Width / 2),
                _playerShip.Position.Y
            );

            _projectiles.Add(new Projectile(_projectileTexture, projectileStartPosition, new Vector2(0, -1)));
            _timeSinceLastShot = 0f;
        }
    }

    private void UpdateProjectiles(GameTime gameTime){
        foreach (var projectile in _projectiles)
        {
            projectile.Update(gameTime);
        }

        _projectiles.RemoveAll(p => p.Position.Y < 0);
    }

    private void SpawnEnemies(GameTime gameTime){
        _enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_enemySpawnTimer > _ennemisIntervalle)
        {
            var enemyPosition = new Vector2(Random.Shared.Next(0, GraphicsDevice.Viewport.Width - 50), -50);
            _enemies.Add(new Enemies(_enemyTexture, enemyPosition));
            _enemySpawnTimer = 0f;
        }
    }

    private void UpdateEnnemis(GameTime gameTime){
        foreach (var enemy in _enemies){
            enemy.Update(gameTime);
        }
        _enemies.RemoveAll(e => e.Position.Y > GraphicsDevice.Viewport.Height);
    }

    private void DetectCollisions(){
        CollisionsProjectilesEnnemis();
        DetectPlayerEnemyCollisions();
    }

    private void CollisionsProjectilesEnnemis(){
        foreach (var enemy in _enemies.ToList()){
            foreach (var projectile in _projectiles.ToList()){
                var enemyRect = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, _enemyTexture.Width, _enemyTexture.Height);
                var projectileRect = new Rectangle((int)projectile.Position.X, (int)projectile.Position.Y, _projectileTexture.Width, _projectileTexture.Height);

                if (enemyRect.Intersects(projectileRect)){
                    _enemies.Remove(enemy);
                    _projectiles.Remove(projectile);
                    _score += 10;

                    if(_score % 100 == 0){
                        Enemies.GlobalSpeed += 20f;
                        _ennemisIntervalle -= 0.1f;
                    } 
                    break;
                }
            }
        }
    }

    private void DetectPlayerEnemyCollisions(){
        foreach (var enemy in _enemies.ToList()){
            var enemyRect = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, _enemyTexture.Width, _enemyTexture.Height);
            var playerRect = new Rectangle((int)_playerShip.Position.X, (int)_playerShip.Position.Y, _playerShip.Width, _playerShip.Height);

            if (enemyRect.Intersects(playerRect)){
                _enemies.Remove(enemy);
                _lives--;
            }
        }
    }

    private void CheckPlayerLives(){
        if (_lives <= 0){
            _Gameover = true;
            // On enregistre le temps écoulé dans le score
            _scoreManager.AddScore(_nomJoueur, _score, (int)_elapsedSeconds);
        }
    }

    private void RestartGame(){
        _Gameover = false; 
        _lives = 3; 
        _score = 0; 
        _enemies.Clear(); 
        _projectiles.Clear(); 
        _playerShip.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 100); 
        Enemies.GlobalSpeed = 100f; 
        _ennemisIntervalle -= 0.1f;

        // Réinitialiser le temps
        _elapsedSeconds = 0f;
    }

    protected override void Draw(GameTime gameTime){
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        if(_Gameover){
            _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2 - 80), Color.Red);
            _spriteBatch.DrawString(_font, "Press 'R' to Restart", new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 + 20), Color.White);
            _spriteBatch.DrawString(_font,$"Ton Score : {_score}", new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 - 20),Color.White);
            _spriteBatch.End();
            return;
        }

        _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
        _playerShip.Draw(_spriteBatch);

        foreach (var projectile in _projectiles)
        {
            projectile.Draw(_spriteBatch);
        }

        foreach (var enemy in _enemies)
        {
            enemy.Draw(_spriteBatch);
        }

        _spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(10, 10), Color.White);
        _spriteBatch.DrawString(_font, $"Lives: {_lives}", new Vector2(10, 40), Color.Red);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
