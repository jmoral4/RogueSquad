using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquad
{
    enum SpritePlayStates { Stop, Pause, Play }
    class BasicSprite
    {
        private readonly int MaxFrames;
        private int _currentFrame;
        private Point _size;
        public string TextureName { get; set; }
        public SpritePlayStates PlayState;
        private double _fps;
        private double _fpsMS;
        private double _lastTime;
        
        
        public BasicSprite(string textureName,  int frameHeight, int frameWidth, int frames, double fps, int startFrame = 0)
        {            
            TextureName = textureName;
            _size = new Point(frameWidth,frameHeight);
            MaxFrames = frames;
            _fps = fps;
            _fpsMS = 1000 / fps;
            _currentFrame = startFrame;           
            PlayState = SpritePlayStates.Stop;
        }

        public Rectangle CurrentFrame => new Rectangle(_currentFrame*_size.X, 0, _size.X, _size.Y);

        public void Play() => PlayState = SpritePlayStates.Play;

        public void Stop()
        {
            PlayState = SpritePlayStates.Stop;
            _currentFrame = 0;
        } 

        public void Pause() => PlayState = SpritePlayStates.Pause;

        public void Update(GameTime time)
        {

            if (PlayState != SpritePlayStates.Play)
                return;

            _lastTime += time.ElapsedGameTime.TotalMilliseconds;            

            if ( _lastTime >  _fpsMS)
            {
                _lastTime = 0;
                _currentFrame++;
                if (_currentFrame == MaxFrames)
                {
                    _currentFrame = 0;
                }
            }


        }

    }
}
