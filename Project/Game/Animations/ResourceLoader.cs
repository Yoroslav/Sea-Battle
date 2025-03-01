using SeaBattle.Project.Game.Resources;
using SFML.Graphics;
using System.IO;
using System.Collections.Generic;

public static class ResourceLoader
{
    private static List<Texture> _fireTextures;
    private static Texture _hitTexture;

    public static Color ShipColor => Color.Blue;
    public static Color MissColor => Color.White;

    public static void LoadResources()
    {
        _fireTextures = LoadFireTexturesFromSpriteSheet();
        _hitTexture = new Texture("hit.png");
    }

    public static List<Texture> GetFireTextures() => _fireTextures;
    public static Texture GetHitTexture() => _hitTexture;
    public static List<Texture> LoadFireTexturesFromSpriteSheet()
    {
        using var memoryStream = new MemoryStream(Units.FireSprite);
        var spriteSheetImage = new Image(memoryStream);

        const int frameSize = 140;
        int framesX = (int)(spriteSheetImage.Size.X / frameSize);
        int framesY = (int)(spriteSheetImage.Size.Y / frameSize);

        List<Texture> fireTextures = new(framesX * framesY);

        for (int y = 0; y < framesY; y++)
        {
            for (int x = 0; x < framesX; x++)
            {
                IntRect frameRect = new IntRect(
                    x * frameSize,
                    y * frameSize,
                    frameSize,
                    frameSize
                );

                var frameTexture = new Texture(spriteSheetImage, frameRect);
                fireTextures.Add(frameTexture);
            }
        }

        return fireTextures;
    }
    public static Texture LoadShipTexture()
    {
        return new Texture("ship_texture.png");
    }
}