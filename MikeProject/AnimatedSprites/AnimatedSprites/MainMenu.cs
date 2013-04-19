using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    class MainMenu
    {
        public bool selected = false;
        public string text = "MainMenu";
        public Rectangle area;

        public bool enabled { get; set; }

        public MainMenu(Rectangle Area, string Text)
        {
            area = Area;
            text = Text;
            enabled = true;
        }

        public void mouseOver(MouseState mbd)
        {
            if (mbd.X > area.X && mbd.X < area.X + area.Width && mbd.Y > area.Y && mbd.Y > area.Y + area.Height)
            {
                selected = true;
            }
            else
            {
                selected = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (selected == false && enabled)
            {
                spriteBatch.DrawString(font, text, new Vector2(area.X, area.Y), Color.Red);

            }
            if (selected == true && enabled)
            {
                spriteBatch.DrawString(font, text, new Vector2(area.X, area.Y), Color.White);
            }
        }
    }
}
