using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AnimatedSprites
{
    public class Background
    {
        public Texture2D texture;
        public Texture2D texture1;
        Vector2 bgPos1;
        Vector2 bgPos2;
        int speed;

        int width;

        public Background(int width)
        {
            texture = null;
            texture1 = null;
            bgPos1 = new Vector2(0,0);
            bgPos2 = new Vector2(width, 0);
            speed = 5;
            this.width = width;
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>(@"Images\background1");
            //texture1 = Content.Load<Texture2D>(@"Images\images2");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);
        }

        public void Update(GameTime gameTime, float speed)
        {
            bgPos1.X = bgPos1.X + speed*1.5f;
            bgPos2.X = bgPos2.X + speed*1.5f;

            if (bgPos1.X >= width)
            {
                bgPos1.X = 0;
                bgPos2.X = -width;
            }
        }

    }
}
