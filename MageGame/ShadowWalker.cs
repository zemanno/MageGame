using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using SpriteClasses;
namespace MageGame
{
    public class ShadowWalker : SpriteFromSheet
    {
        public ShadowWalker(ContentManager Content, GraphicsDevice device, Vector2 position, Vector2 velocity)
            : base(Content.Load<Texture2D>("images/ShadowWalker"), position, velocity, true, 0.0f, 1.0f, SpriteEffects.None, new Vector2(52f, 66f), new Vector2(0, 0), new Vector2(8, 1), 1.8f, null)
        { }
    }
}