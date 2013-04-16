using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    class AutomatedSprite : Sprite
    {
        // Sprite is automated. Direction is same as speed
        public bool Bounce = true;
        public override Vector2 direction
        {
            get { return speed; }
        }



        public AutomatedSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame, string collisionCueName, float scale,
            int scoreValue, int HP, int Damage)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, collisionCueName, scale, scoreValue, HP, Damage)
        {
        }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move sprite based on direction
            if (Bounce)
            {

                if (position.X > clientBounds.Width - frameSize.X || position.X < 0)
                    speed.X *= -1;

                if (position.Y > clientBounds.Height - frameSize.Y || position.Y < 0)
                    speed *= -1;
            }
            
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
