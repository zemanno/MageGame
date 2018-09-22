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

namespace SpriteClasses
{
    public class Sprite
    {
        //fields
        protected Vector2 initialVelocity, origin, position, velocity, magnitude;
        public Vector2 acceleration, Force;
        protected float mass;
        
        //properties
        public bool Alive { get; set; }
        public Texture2D Image { get; set; }
        public SoundEffect Sound { get; set; }
        public Vector2 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }   
        }
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        //rectangle around the sprite.
        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - Origin.X * Scale), (int)(position.Y - Origin.Y * Scale),
                    Convert.ToInt32(Image.Width * Scale), Convert.ToInt32(Image.Height * Scale));
            }
        }
        
        //automatic properties
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
        public float Scale { get; set; }
        public SpriteEffects SpriteEffect {get; set;}
        public bool UseOrigin { get; set; }
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        
        //sprite class constructor
        public Sprite(Texture2D textureImage, Vector2 position, Vector2 velocity, bool useOrigin, float rotationSpeed, float scale, SpriteEffects spriteEffect, SoundEffect sound)
        {
            Image = textureImage;
            Position = position;
            Velocity = velocity;
            InitialVelocity = velocity;
            UseOrigin = useOrigin;
            if (useOrigin == true)
            {
                origin = new Vector2(Image.Width / 2, Image.Height / 2);
            }
            RotationSpeed = rotationSpeed;
            Scale = scale;
            SpriteEffect = spriteEffect;
            Sound = sound;
            Alive = true;
        }
        
        //methods
        //draw method draws the sprite on the screen
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                spriteBatch.Draw(Image, Position, null, Color.White, Rotation, origin, Scale, SpriteEffect, 0.0f);
            }
        }
        //does not keep sprite on the screen
        public virtual void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float timelapse = gameTime.ElapsedGameTime.Milliseconds / 1000f;
                Rotation += RotationSpeed * timelapse;
                Rotation = Rotation % (MathHelper.Pi * 2.0f);

                position += velocity * timelapse;
            }
        }
        //keep the sprite on the screen using if statements
        public virtual void Update(GameTime gameTime, GraphicsDevice Device)
        {
            if (Alive)
            {
                Update(gameTime);
                if (position.Y < (origin.Y * Scale))
                {
                    position.Y = (origin.Y * Scale);
                    velocity.Y *= -1;
                }
                if (position.Y > Device.Viewport.Height - origin.Y * Scale)
                {
                    position.Y = Device.Viewport.Height - (origin.Y * Scale);
                    velocity.Y *= -1;
                }
                if (position.X < (origin.X * Scale))
                {
                    position.X = (origin.X * Scale);
                    velocity.X *= -1;
                }
                if (position.X > Device.Viewport.Width - (origin.X * Scale))
                {
                    position.X = Device.Viewport.Width - (origin.X * Scale);
                    velocity.X *= -1;
                }
            }
        }
        //checks rectangle collision with the passed mouse coordinates
        public virtual bool CollisionMouse(int x, int y)
        {
            return CollisionRectangle.Contains(x, y);
        }
       //checks rectangle collision with other rectangles
        public virtual bool CollisionSprite(Sprite sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }
        //moves the sprite down
        public virtual void Down()
        {
            velocity.Y += InitialVelocity.Y;
        }
        //moves the sprite left
        public virtual void Left()
        {
            velocity.X = -InitialVelocity.X;
            SpriteEffect = SpriteEffects.FlipHorizontally;
        }
        //moves the sprite right
        public virtual void Right()
        {
            velocity.X = InitialVelocity.X;
            SpriteEffect = SpriteEffects.None;
        }
        //moves the sprite up
        public virtual void Up()
        {
            velocity.Y -= InitialVelocity.Y;
        }
        //makes the sprite idle
        public virtual void Idle()
        {
            Velocity *= .85f;
        }
        //checks to see if the sprite goes off screen
        public virtual bool isOffScreen(Sprite sprite, GraphicsDevice device)
        {
            if (sprite.Position.Y - sprite.Image.Height >= device.Viewport.Height  
                || sprite.Position.Y + sprite.Image.Height <= 0 
                || sprite.Position.X - sprite.Image.Width >= device.Viewport.Width 
                || sprite.Position.X + sprite.Image.Width <= 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}