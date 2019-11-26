using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    class Bird
    {
        Texture2D death;

        Animation birdAnim;

        Vector2 pos;

        SoundEffect soundDead, soundFlap;

        float velocity = 0, gravity = 1.4f, flapForce = -18, oldY;
        float rotationSpeed = .05f, rotation = 0;
        float scale;

        bool _isDead = false;

        int viewH;

        public float X { get => pos.X; }

        public bool isDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                if (_isDead)
                {
                    velocity = flapForce;
                    soundDead.Play();
                }
            }
        }

        public Rectangle Hitbox
        {
            get => new Rectangle((int)pos.X, (int)pos.Y - birdAnim.frameHeight / 2, birdAnim.frameWidth, birdAnim.frameHeight);
        }

        public Bird(Animation birdAnim, Vector2 pos, Texture2D death, int viewH, float scale, SoundEffect soundDead, SoundEffect soundFlap)
        {
            this.birdAnim = birdAnim;
            this.pos = pos;
            this.death = death;
            this.viewH = viewH;
            this.scale = scale;
            this.soundDead = soundDead;
            this.soundFlap = soundFlap;
        }

        public void Update(GameTime gt, bool jump,
            bool isPlaying, out bool isPlayingResult)
        {
            // have consistency between frames
            float update = (float)gt.ElapsedGameTime.TotalMilliseconds / 16f;

            isPlayingResult = isPlaying;

            #region Movement
            // gravity force
            velocity += gravity * update;

            if (!isDead)
            {
                // if Space is pressed
                if (jump)
                {
                    isPlayingResult = true;

                    // flap
                    velocity = flapForce;

                    soundFlap.Play();

                    rotation = -MathHelper.PiOver4;
                }
            }

            if (isPlaying)
            {
                oldY = pos.Y;
                pos.Y += velocity;

                // Make bird stay between limits
                pos.Y = MathHelper.Clamp(pos.Y, 0, viewH - birdAnim.frameHeight + 35);

                if (oldY == pos.Y)
                    velocity = 0;

                if (rotation <= MathHelper.PiOver2)
                    rotation += rotationSpeed;

                if (pos.Y >= viewH - birdAnim.frameHeight)
                    _isDead = true;
            }

            birdAnim.pos = pos;
            #endregion
            
            birdAnim.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!isDead)
                birdAnim.Draw(sb, rotation, new Vector2(birdAnim.frameWidth / 2, birdAnim.frameHeight / 2));
            else
                sb.Draw(death, new Rectangle((int)pos.X, (int)pos.Y, (int)(death.Width * scale), (int)(death.Height * scale)),
                    new Rectangle(0, 0, death.Width, death.Height), Color.White,
                    rotation, new Vector2(death.Width / 2, death.Height / 2), SpriteEffects.None, 0);
        }
    }
}