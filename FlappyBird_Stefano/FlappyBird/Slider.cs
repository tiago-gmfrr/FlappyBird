using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    class Slider
    {
        float min, max, value, pool;
        Color leftColor, rightColor, toggleColor;
        Rectangle rect;
        Texture2D pixel;
        bool isHorizontal, isPressed;

        int valueLength => (int)((value - min) / pool * (isHorizontal ? rect.Width : rect.Height));

        Rectangle left => new Rectangle(rect.X, rect.Y, isHorizontal ? valueLength : rect.Width,
            isHorizontal ? rect.Height : valueLength);

        Rectangle right => new Rectangle(rect.X + (isHorizontal ? valueLength : 0),
            rect.Y + (isHorizontal ? 0 : valueLength),
            rect.Width - (isHorizontal ? valueLength : 0), rect.Height - (isHorizontal ? 0 : valueLength));

        Rectangle toggle
        {
            get
            {
                float size1 = (isHorizontal ? rect.Height : rect.Width) * 1.5f;
                int size2 = (isHorizontal ? rect.Width : rect.Height) / 8;

                if (isHorizontal)
                    return new Rectangle(rect.X + valueLength - size2 / 2, rect.Y - (int)(size1 / 6), size2, (int)size1);
                else
                    return new Rectangle(rect.X - (int)(size1 / 6), rect.Y + valueLength - size2 / 2, (int)size1, size2);
            }
        }

        public Slider(float min, float max, float value, Rectangle rect, bool isHorizontal) :
            this(min, max, value, rect, isHorizontal, Color.Green, Color.LightGray, Color.DarkGreen)
        { }

        public Slider(float min, float max, float value, Rectangle rect, bool isHorizontal,
            Color leftColor, Color rightColor, Color toggleColor)
        {
            this.min = min;
            this.max = max;
            pool = max - min;
            this.value = value;
            this.rect = rect;
            this.isHorizontal = isHorizontal;
            this.leftColor = leftColor;
            this.rightColor = rightColor;
            this.toggleColor = toggleColor;
        }

        public void LoadContent(ContentManager Content)
        {
            pixel = Content.Load<Texture2D>("pixel");
        }

        public void AddValue(float value)
        {
            this.value += value;

            if (this.value < min)
                this.value = min;
            else if (this.value > max)
                this.value = max;
        }

        public void SetValue(MouseState mState)
        {
            int pos = isHorizontal ? mState.X - rect.X : mState.Y - rect.Y;
            Rectangle mouseRect = new Rectangle(mState.X, mState.Y, 1, 1);

            if (mState.LeftButton == ButtonState.Released)
                isPressed = false;

            if (mState.LeftButton == ButtonState.Pressed && (mouseRect.Intersects(toggle) || isPressed))
            {
                isPressed = true;

                if ((isHorizontal && mouseRect.X >= rect.X && mouseRect.X <= rect.X + rect.Width) ||
                (!isHorizontal && mouseRect.Y >= rect.Y && mouseRect.Y <= rect.Y + rect.Height))
                    value = pos * 1f / (isHorizontal ? rect.Width : rect.Height) * pool + min;
            }
        }

        public float Value => value;

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, left, leftColor);
            sb.Draw(pixel, right, rightColor);
            sb.Draw(pixel, toggle, toggleColor);
        }
    }
}