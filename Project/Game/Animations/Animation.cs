using SeaBattle.Project.Engine;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

public class Animation : IEntity
{
    private readonly List<Texture> _textures;
    private readonly Sprite _sprite;
    private readonly float _frameDuration;
    private float _timer;
    private int _currentFrame;

    public Vector2f Position
    {
        get => _sprite.Position;
        set => _sprite.Position = value;
    }

    public Animation(List<Texture> textures, Vector2i size, float frameRate)
    {
        _textures = textures;
        _frameDuration = 1.0f / frameRate;
        _sprite = new Sprite
        {
            Texture = textures[0],
            TextureRect = new IntRect(0, 0, size.X, size.Y)
        };
    }

    public void Update(float deltaTime)
    {
        _timer += deltaTime;
        if (_timer >= _frameDuration)
        {
            _currentFrame = (_currentFrame + 1) % _textures.Count;
            _sprite.Texture = _textures[_currentFrame];
            _timer = 0;
        }
    }

    public void Draw(RenderTarget target) => target.Draw(_sprite);
}
