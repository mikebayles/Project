using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprites
{
    class Player
    {
        KeyboardState currentKbState;
        KeyboardState pastKbState;

        Texture2D texture;
        Rectangle frameRectangle;
        public Vector2 position;
        public Vector2 origin;
        public Vector2 velocity;

        int currentFrame;
        int frameWidth;
        int frameHeight;
        int level = 0;

        float timer;
        float interval = 50;

        float scale = 1.8f;
        public float rotation = 0;
        Vector2 distance;

        int normalSpeed = 3;
        int extraSpeed = 6;

        bool hasJumped;
        float jumpSpeed = 0;
        Vector2 startPos;

        public int HP { get; set; }

        SpriteEffects currentEffect = SpriteEffects.None;

        public bool rocketLauncherAvailable = false;
        public enum Weapon
        {
            MachineGun,
            RocketLauncher
        }

        public Weapon SelectedWeapon { get; protected set; }
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    frameRectangle.Width,
                    frameRectangle.Height);
            }
        }


        public Player(Texture2D texture, Vector2 position, int frameHeight, int frameWidth)
        {
            this.texture = texture;
            this.position = position;
            startPos = position;
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
            HP = 10000;
            hasJumped = true;
            SelectedWeapon = Weapon.MachineGun;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, position, frameRectangle, Color.White, 0f, origin, scale, currentEffect, 0f);
            
        }

        public void Update(GameTime gametime)
        {
            MouseState mouse = Mouse.GetState();

            distance.X = mouse.X - position.X;
            distance.Y = mouse.Y - position.Y;

            if (mouse.X < position.X)
                currentEffect = SpriteEffects.FlipHorizontally;
            else
                currentEffect = SpriteEffects.None;
            


            rotation = (float)(Math.Atan2(distance.Y,distance.X));

            //if (rotation < -1.8 && currentFrame < 8)
            //    currentFrame = 8;
            //else if (rotation > -1.8 && currentFrame >= 8)
            //    currentFrame = 0;
           
           

            frameRectangle = new Rectangle(currentFrame * frameWidth, frameHeight * level, frameWidth, frameHeight);
            origin = new Vector2(frameRectangle.Width / 2, frameRectangle.Height / 2);
            position = position + velocity;



            currentKbState = Keyboard.GetState();


            if(currentKbState.IsKeyDown(Keys.Right) || currentKbState.IsKeyDown(Keys.D))
            {
                AnimateRight(gametime);
                if (currentKbState.IsKeyDown(Keys.Space))
                {
                    interval = 20;
                    velocity.X = extraSpeed;
                }
                else
                {
                    interval = 50;
                    velocity.X = normalSpeed;
                }
            }
            else if(currentKbState.IsKeyDown(Keys.Left) || currentKbState.IsKeyDown(Keys.A))
            {
                AnimateRight(gametime);
                if (currentKbState.IsKeyDown(Keys.Space))
                {
                    interval = 20;
                    velocity.X = -extraSpeed;
                }
                else
                {
                    interval = 50;
                    velocity.X = -normalSpeed;
                }
            }
            else
                velocity = Vector2.Zero;



            if (hasJumped)
            {
                position.Y += jumpSpeed;
                jumpSpeed += 1;
                if (position.Y > startPos.Y)
                {
                    position.Y = startPos.Y;
                    hasJumped = false;
                }
            }

            else 
            {
                if (currentKbState.IsKeyDown(Keys.Up) || currentKbState.IsKeyDown(Keys.W))
                {                  
                    hasJumped = true;
                    jumpSpeed = -19;
                }
            }

            if (currentKbState.IsKeyDown(Keys.D1))
            {
                level = 0;
                SelectedWeapon = Weapon.MachineGun;
            }
            else if (currentKbState.IsKeyDown(Keys.D2) && rocketLauncherAvailable)
            {
                level = 1;
                SelectedWeapon = Weapon.RocketLauncher;
            }

            if (currentKbState.IsKeyDown(Keys.Down))
            {
                scale = 1.5f;
                if (!pastKbState.IsKeyDown(Keys.Down))
                    position.Y += 30;
            }
            else
            {
                scale = 1.8f;
                if (pastKbState.IsKeyDown(Keys.Down))
                    position.Y -= 30;
            }


            pastKbState = currentKbState;

        }

        public void AnimateRight(GameTime gameTime)
        {
            
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            
            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 15 || currentFrame < 8)
                    currentFrame = 8;
            }
        }


    }
}
