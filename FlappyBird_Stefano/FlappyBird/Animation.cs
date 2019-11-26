using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    class Animation
    {
        #region vars
        // The image representing the collection of images used for animation
        public Texture2D texture;

        // The scale used to display the sprite strip
        float scale;

        // The time since we last updated the frame
        int elapsedTime;

        // The time we display a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int frameWidth;

        // Height of a given frame
        public int frameHeight;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 pos;
        #endregion

        public void Initialize(Texture2D texture, Vector2 position,
            int frameWidth, int frameHeight, int frameCount, int frameTime,
            Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in
            this.color = color;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;

            Looping = looping;
            pos = position;
            this.texture = texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false) return;

            // Update the elapsed time
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;

                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width 
            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)pos.X, (int)pos.Y,
                (int)(frameWidth * scale), (int)(frameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch, float rotate, Vector2 origin)
        {
            // Only draw the animation when we are active
            if (Active)
                spriteBatch.Draw(texture, destinationRect, sourceRect, color, rotate, origin, SpriteEffects.None, 0);
        }
    }
}