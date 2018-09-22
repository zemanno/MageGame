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
    public class ShadowBat : SpriteFromSheet
    {
        public ShadowBat(ContentManager Content, Random random, GraphicsDevice device)
            : base(Content.Load<Texture2D>("images/ShadowBat"), new Vector2(random.Next(40, device.Viewport.Width - 39), random.Next(50, 251)), new Vector2(140, 0), true, 0.0f, 1.0f, SpriteEffects.None, new Vector2(51.3f, 48), new Vector2(16, 0), new Vector2(8, 1), 0.6f, null)
        { }
    }
}
