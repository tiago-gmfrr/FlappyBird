using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    class Button
    {
        Rectangle rect;
        string text;
        Texture2D texture, pressed, hover;
        SpriteFont font;
        Color color;
        bool isPressed = false, isHovering;

        public Point pos => new Point(rect.X, rect.Y);
        
        public Button(Point pos, Texture2D texture, Texture2D pressed, Texture2D hover, float scale)
            : this(pos, texture, pressed, hover, scale, "", null, Color.Black) { }

        public Button(Point pos, Texture2D texture, Texture2D pressed, Texture2D hover, float scale, string text, SpriteFont font, Color color)
        {
            rect = new Rectangle(pos, new Point((int)(texture.Width * scale), (int)(texture.Height * scale)));

            this.texture = texture;
            this.pressed = pressed;
            this.hover = hover;

            this.text = text;
            this.font = font;
            this.color = color;
        }

        /// <summary>
        /// Return true if button is pressed
        /// </summary>
        /// <param name="mState"></param>
        /// <param name="oldMState"></param>
        /// <returns></returns>
        public bool Update(MouseState mState, MouseState oldMState)
        {
            isPressed = false;
            isHovering = false;

            if (rect.Intersects(new Rectangle(mState.X, mState.Y, 1, 1)))
            {
                isHovering = true;
                isPressed = mState.LeftButton == ButtonState.Pressed;
                
                return oldMState.LeftButton == ButtonState.Pressed
                    && mState.LeftButton == ButtonState.Released;
            }

            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            Texture2D tmp = texture;
            if (isHovering)
                tmp = hover;
            if (isPressed)
                tmp = pressed;

            sb.Draw(tmp, rect, Color.White);

            if (font != null)
                sb.DrawString(font, text, new Vector2(rect.X, rect.Y), color);
        }
    }
}