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
    public class Player2 : SpriteFromSheet
    {
        public Player2(ContentManager Content, Vector2 position)
            : base(Content.Load<Texture2D>("Images/MageBrown"), position, new Vector2(140), true, 0.0f, 1.0f, SpriteEffects.None, new Vector2(31, 32), new Vector2(0, 0), new Vector2(3, 1), 0.6f, null)
        { }
    }
}