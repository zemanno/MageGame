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


namespace SpriteClasses
{
    public class AnimatedSprite : Sprite
    {
        //fields
        private float timeSinceLastImage;

        //automatic properties
        ContentManager Content { get; set; }
        int CurrentImageNumber{ get; set; } 
        int NumberOfImages { get; set; }
        Texture2D[] Textures { get; set; }
        float TimeBetweenImages { get; set; }
       //constructor
        public AnimatedSprite(string firstTextureName, int numberOfImages, float timeBetweenImages, ContentManager content, Vector2 position, Vector2 velocity, bool useOrigin, float rotationSpeed, float scale, SpriteEffects spriteEffect, SoundEffect sound)
            : base(content.Load<Texture2D>(firstTextureName), position, velocity, useOrigin, rotationSpeed, scale, spriteEffect, sound)
        {
            NumberOfImages = numberOfImages;
            TimeBetweenImages = timeBetweenImages;
            Content = content;
            CurrentImageNumber = 0;
            Textures = new Texture2D[numberOfImages];
            Textures[0] = content.Load<Texture2D>(firstTextureName);
            for (int i = 1; i < numberOfImages; i ++)
            {
                Textures[i] = (content.Load<Texture2D>(StringUtilities.NextImageName(firstTextureName, i)));
            }
            Image = Textures[CurrentImageNumber];
        }
        public override void Update(GameTime gameTime, GraphicsDevice Device)
        {
            base.Update(gameTime, Device);
            timeSinceLastImage += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (timeSinceLastImage >= TimeBetweenImages)
            {
                timeSinceLastImage = 0f;
                if (Image.Name == Textures[CurrentImageNumber].Name)
                {
                    Image = Textures[(CurrentImageNumber+1)];
                    CurrentImageNumber++;
                    if (CurrentImageNumber >= (NumberOfImages - 1))
                    {
                        CurrentImageNumber = 0;
                    }
                }
                else
                {
                    Image = Textures[CurrentImageNumber];
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timeSinceLastImage += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (timeSinceLastImage >= TimeBetweenImages)
            {
                timeSinceLastImage = 0f;
                if (Image.Name == Textures[CurrentImageNumber].Name)
                {
                    Image = Textures[(CurrentImageNumber + 1)];
                    CurrentImageNumber++;
                    if (CurrentImageNumber >= (NumberOfImages - 1))
                    {
                        CurrentImageNumber = 0;
                    }
                }
                else
                {
                    Image = Textures[CurrentImageNumber];
                }
            }
        }
    }
}
