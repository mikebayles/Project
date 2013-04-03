/*
 * Public Static Void PAIN
 * Section #1
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprites
{
    class UserControlledSprite: Sprite
    {
        // Movement stuff
        bool jumping;
        int jumpspeed = 0;
        Vector2 startPos;
        // Get direction of sprite based on player input and speed
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState kbState = Keyboard.GetState();
                // If player pressed arrow keys, move the sprite
                if (kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
                    inputDirection.X -= 1;
                if (kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
                    inputDirection.X += 1;

                
                //if (Keyboard.GetState(  ).IsKeyDown(Keys.Up))
                    //inputDirection.Y -= 1;
                //if (Keyboard.GetState(  ).IsKeyDown(Keys.Down))
                    //inputDirection.Y += 1;

                // If player pressed the gamepad thumbstick, move the sprite
                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if(gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
               // if(gamepadState.ThumbSticks.Left.Y != 0)
                    //inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;

                return inputDirection * speed;
            }
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
            startPos = position;
        }


        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            startPos = position;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (jumping)
            {
                position.Y += jumpspeed;//Making it go up
                jumpspeed += 1;//Some math (explained later)
                if (position.Y >= startPos.Y)
                //If it's farther than ground
                {
                    position.Y = startPos.Y;//Then set it on
                    jumping = false;
                }
            }
            else
            {
                KeyboardState kbState = Keyboard.GetState();
                if (kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.W))
                {
                    jumping = true;
                    jumpspeed = -14;//Give it upward thrust
                }
            }

            // Move the sprite based on direction
            position += direction;


            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds);
        }
    }
}
