using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AnimatedSprites
{
    class Bullet
    {
        public Texture2D texture;

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;

        public bool isVisible;

        public int damageValue { get; set; }

        public Bullet(Texture2D texture,int damage)
        {
            this.texture = texture;
            this.damageValue = damage;
            isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture,position,null,Color.White,0f, origin, 1f, SpriteEffects.None, 0);

        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X ,
                    (int)position.Y ,
                    texture.Width,
                    texture.Height);
            }
        }
    }
}
