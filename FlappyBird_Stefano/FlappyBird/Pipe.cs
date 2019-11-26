using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    class Pipe
    {
        Texture2D texture;
        
        public Pipe(Texture2D texture)
        {
            this.texture = texture;
        }
        
        public void Draw(SpriteBatch sb, float rotation, Vector2 pos, SpriteEffects spriteEffect)
        {
            sb.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height),
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), spriteEffect, 0);
        }
    }
}