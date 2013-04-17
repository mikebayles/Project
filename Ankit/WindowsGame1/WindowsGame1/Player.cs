using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    class Player
    {
        //playerSprites stores the Sprites for player
        public List<UserControlledSprite> playerSprites = new List<UserControlledSprite>();


        bool IsRuning;
        public bool IsShooting;
        bool IsRight = true;

        //player information
        int HP = 100;
        Vector2 position;


        public void GetInput()
        {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                IsRight = false;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                IsRight = true;
            }
          
            IsRuning = (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.D));
            IsShooting = keyboardState.IsKeyDown(Keys.M);

        }

        //update the status for all sprites
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {

            foreach (UserControlledSprite s in playerSprites)
                s.Update(gameTime, clientBounds);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GetInput();

            if (IsRight)
            {
                if (!IsRuning)
                {
                    if (!IsShooting)
                    {
                        playerSprites[0].Draw(gameTime, spriteBatch);
                        playerSprites[2].reverse = false;
                    }
                    else
                    {
                        playerSprites[2].Draw(gameTime, spriteBatch);
                        playerSprites[2].reverse = false;
                    }
                }
                else
                {
                    if (!IsShooting)
                    {
                        playerSprites[1].Draw(gameTime, spriteBatch);
                        playerSprites[1].reverse = false;

                    }
                    else
                    {
                        playerSprites[3].Draw(gameTime, spriteBatch);
                        playerSprites[3].reverse = false;

                    }
                }
            }
            else
            {
                if (!IsRuning)
                {
                    if (!IsShooting)
                    {
                        playerSprites[0].Draw(gameTime, spriteBatch);
                        playerSprites[0].reverse = true;
                    }
                    else
                    {
                        playerSprites[2].Draw(gameTime, spriteBatch);
                        playerSprites[2].reverse = true;
                    }
                }
                else
                {
                    if (!IsShooting)
                    {
                        playerSprites[1].Draw(gameTime, spriteBatch);
                        playerSprites[1].reverse = true;
                    }
                    else
                    {
                        playerSprites[3].Draw(gameTime, spriteBatch);
                        playerSprites[3].reverse = true;
                    }
                }
            }
        }

    }
}
