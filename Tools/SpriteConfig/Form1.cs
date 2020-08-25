using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace SpriteConfig
{

    public partial class Form1 : Form
    {
     
        AppState _appState;
        AnimatedSpriteInfo _animatedSpriteInfo;
        Image _spriteBitmap;
        // move most of this stuff into a spriteInfo or SpriteConfig class
        BufferedGraphics _bufferedAnimatedSprite;
        BufferedGraphics _bufferedOriginalFile;
        BufferedGraphicsContext bgc;
        float _rectWRatio = 1.0f; // the scaling ratio for the 'original sprite' box - used when drawing overlays in the sprite box
        float _rectHRatio = 1.0f;
        int _currentFrameCounter = 0;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _appState = new AppState();
            gTimer.Enabled = false;
            bgc = BufferedGraphicsManager.Current;
            _bufferedAnimatedSprite = bgc.Allocate(Graphics.FromHwnd(pnlAnimatedSprite.Handle), pnlAnimatedSprite.DisplayRectangle);
            _bufferedOriginalFile = bgc.Allocate(Graphics.FromHwnd(pnlGfx.Handle), pnlGfx.DisplayRectangle);          
            _appState.ParentSpriteLoaded = false;
            _appState.IsSaved = false;
            
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "*.png";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // load the image
                _spriteBitmap = Image.FromFile(ofd.FileName);

                _animatedSpriteInfo = new AnimatedSpriteInfo(ofd.FileName, ofd.SafeFileName, _spriteBitmap.Width, _spriteBitmap.Height); //re initialize on every new load..

                //update stats
                //estimated framecount based on above
                _animatedSpriteInfo.LastFrame = (_animatedSpriteInfo.SourceWidth / _animatedSpriteInfo.FrameWidth);
                gTimer.Interval = 1000 / _animatedSpriteInfo.DesiredFrameRate;

                // generate ratios for rendering overlay on source file (since it's scaled)
                _rectWRatio = (float)(pnlGfx.Width) / (float)_animatedSpriteInfo.SourceWidth;
                _rectHRatio = (float)pnlGfx.Height / (float)_animatedSpriteInfo.SourceHeight;

                //setup and bind our property grid             
                propertyGrid1.SelectedObject = _animatedSpriteInfo;
                propertyGrid1.Enabled = true;
                propertyGrid1.Visible = true;
                propertyGrid1.Name = "Animated Sprite Info";


                // update the name of the file we're editing
                lblEditing.Text = $"Editing: {_animatedSpriteInfo.SourceFullPath} + {_animatedSpriteInfo.SourceFileName}";

                gTimer.Enabled = true;
                _appState.ParentSpriteLoaded = true;
               
            }

        }


        private RectangleF GetScaledFrameRectangle()
        {
            return new RectangleF(
                _animatedSpriteInfo.GetCurrentFrame().X * _rectWRatio,
                _animatedSpriteInfo.GetCurrentFrame().Y * _rectHRatio,
                _animatedSpriteInfo.FrameWidth * _rectWRatio,
                _animatedSpriteInfo.FrameHeight * _rectHRatio
                );
        }



        private void GTimer_Tick(object sender, EventArgs e)
        {
            // draw per tick
            RenderFrame();

        }

        private void RenderFrame()
        {
            if (_appState.ParentSpriteLoaded)
            {
                
                _bufferedOriginalFile.Graphics.Clear(Color.White);
                _bufferedOriginalFile.Graphics.DrawImage(_spriteBitmap, pnlGfx.DisplayRectangle);

                //draw our current frame rect


                _bufferedAnimatedSprite.Graphics.Clear(Color.White);

                //update the current frame it it's different
                _animatedSpriteInfo.UpdateCurrentFrame(
                   new Rectangle(
                       _currentFrameCounter * _animatedSpriteInfo.FrameWidth,
                       _animatedSpriteInfo.FrameRow * _animatedSpriteInfo.FrameHeight,
                       _animatedSpriteInfo.FrameWidth,
                       _animatedSpriteInfo.FrameHeight));
                _bufferedOriginalFile.Graphics.DrawRectangles(Pens.Green, new RectangleF[] { GetScaledFrameRectangle() });
                _bufferedAnimatedSprite.Graphics.DrawImage(_spriteBitmap, pnlAnimatedSprite.DisplayRectangle, _animatedSpriteInfo.GetCurrentFrame(), GraphicsUnit.Pixel);
                _bufferedAnimatedSprite.Render();
                _bufferedOriginalFile.Render();
                
                _currentFrameCounter++;
                lblFrame.Text = "Frame: " + _currentFrameCounter;

                if (_currentFrameCounter >= _animatedSpriteInfo.LastFrame)
                {
                    _currentFrameCounter = _animatedSpriteInfo.FirstFrame;
                }
                
                DrawPlayButtonContents();
                btnPlay.Visible = true;
                btnStepSingle.Visible = true;
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fs = new Form1();
            fs.Show();
        }

        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //attempt to set FPS, rollback if invalid
            if (e.ChangedItem.Label == "DesiredFrameRate")
            {
                try
                {
                    int desiredFPS = (int)e.ChangedItem.Value;
                    if (desiredFPS != gTimer.Interval)
                    {
                        gTimer.Interval = 1000 / desiredFPS;
                    }
                }
                catch (Exception)
                {
                    _animatedSpriteInfo.DesiredFrameRate = (int)e.OldValue;
                }
            }

            ////update the rectangle if we changed height or width
            //if (e.ChangedItem.Label == "FrameWidth" || e.ChangedItem.Label == "FrameHeight")
            //{
            //    _animatedSpriteInfo.UpdateCurrentFrame(
            //        new Rectangle(
            //            _currentFrameCounter * _animatedSpriteInfo.FrameWidth,
            //            _animatedSpriteInfo.FrameRow * _animatedSpriteInfo.FrameHeight,
            //            _animatedSpriteInfo.FrameWidth,
            //            _animatedSpriteInfo.FrameHeight));
            //}


        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_appState.ParentSpriteLoaded)
                return;

            string output = JsonConvert.SerializeObject(_animatedSpriteInfo);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = _animatedSpriteInfo.AnimatedSpriteFilename;
            sfd.Title = "Save Animation Configuration";
            sfd.DefaultExt = ".sfx";

            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _animatedSpriteInfo.AnimatedSpriteFilename = sfd.FileName;
                File.WriteAllText(_animatedSpriteInfo.AnimatedSpriteFilename, output);
                _appState.IsSaved = true;
            }

            

            
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            gTimer.Enabled = !gTimer.Enabled;
            DrawPlayButtonContents();
        }

        private void DrawPlayButtonContents()
        {
            if (!gTimer.Enabled)
            {
                btnPlay.Font = new Font("Wingdings 3", 8.25f);
                btnPlay.Text = "u";
            }
            else
            {
                btnPlay.Font = new Font("Microsoft Sans Serif", 8.25f);
                btnPlay.Text = "■";
            }
        }

        private void btnStepSingle_Click(object sender, EventArgs e)
        {
            if (!gTimer.Enabled)
            {
                RenderFrame();
            }
        }
    }
}
