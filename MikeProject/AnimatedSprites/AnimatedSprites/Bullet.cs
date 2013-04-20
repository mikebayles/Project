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

        Vector2 aoeRange;
        float rotation;
        public int damageValue { get; set; }

        public Bullet(Texture2D texture,float rotation, int damage, int xRange, int yRange)
        {
            this.texture = texture;
            this.damageValue = damage;
            isVisible = false;
            aoeRange = new Vector2(xRange,yRange);
            this.rotation = rotation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture,position,null,Color.White,rotation, origin, 1f, SpriteEffects.None, 0);

        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X ,
                    (int)position.Y ,
                    texture.Width + ((int)aoeRange.X *2),
                    texture.Height + ((int)aoeRange.Y *2));
            }
        }
    }
}
