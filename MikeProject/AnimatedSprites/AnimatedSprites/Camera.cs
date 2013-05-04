using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 center;

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void Update(GameTime gametime, Player player)
        {
            center = new Vector2(player.position.X + (player.collisionRect.Width/2) - 400 , 0);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0))*
                Matrix.CreateTranslation(new Vector3(-center.X,-center.Y,0));
        }
    }
}
