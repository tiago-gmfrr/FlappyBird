using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    class DoublePipe
    {
        Vector2 pos;

        Texture2D texture;

        Pipe down, up;

        public static int speed = 7, offset = 550;

        public bool isPointAdded = false;

        Vector2 posUp => pos - new Vector2(0, offset);
        Vector2 posDown => pos + new Vector2(0, offset);

        Rectangle rectUp =>
            new Rectangle((int)posUp.X, (int)posUp.Y - texture.Height / 2, texture.Width, texture.Height);

        Rectangle rectDown =>
            new Rectangle((int)posDown.X, (int)posDown.Y - texture.Height / 2, texture.Width, texture.Height);

        public float X => pos.X;

        public DoublePipe(Texture2D texture, Vector2 pos)
        {
            this.texture = texture;
            this.pos = pos;

            up = new Pipe(texture);
            down = new Pipe(texture);
        }

        public void Update() => pos.X -= speed;

        public void Draw(SpriteBatch sb)
        {
            up.Draw(sb, 0, posUp, SpriteEffects.None);
            down.Draw(sb, MathHelper.Pi, posDown, SpriteEffects.FlipHorizontally);
        }

        public bool IsOut => pos.X + texture.Width < 0;

        public bool IsCollinding(Rectangle hitbox)
            => rectUp.Intersects(hitbox) || rectDown.Intersects(hitbox);
    }
}