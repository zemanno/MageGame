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
    public class MagicAttack : SpriteFromSheet
    {
        public MagicAttack(ContentManager Content, string image, Vector2 position, Vector2 velocity, Vector2 sheetXY, Vector2 sheetRowCol, float rotation, SpriteEffects spriteEffect)
            : base(Content.Load<Texture2D>(image), position, velocity, true, rotation, 1.0f, spriteEffect, sheetXY, new Vector2(0, 0), sheetRowCol, 0.6f, null)
        { }
    }
}