﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class MainMenu
    {
        public bool selected = false;
        public string text = "MainMenu";
        public Rectangle area;

        public MainMenu(Rectangle Area, string Text)
        {
            area = Area;
            text = Text;
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
       

            public void Draw (SpriteBatch spriteBatch, SpriteFont font)
            {
                if (selected == false)
                {
                    spriteBatch.DrawString(font, text, new Vector2(area.X, area.Y), Color.Red);
                 
                }
                if (selected == true)
                {
                    spriteBatch.DrawString(font, text, new Vector2(area.X, area.Y), Color.White);
                }
            }
        }
    }

