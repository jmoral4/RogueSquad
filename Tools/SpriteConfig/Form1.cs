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
        AnimatedSpriteModel _currentAnimatedSpriteModel;
        Image _spriteBitmap;
        // move most of this stuff into a spriteInfo or SpriteConfig class
        BufferedGraphics _bufferedAnimatedSprite;
        BufferedGraphics _bufferedOriginalFile;
        BufferedGraphicsContext bgc;
        float _rectWRatio = 1.0f; // the scaling ratio for the 'original sprite' box - used when drawing overlays in the sprite box
        float _rectHRatio = 1.0f;
        int _currentFrameCounter = 0;
        List<AnimatedSpriteModel> _animatedSpriteModels = new List<AnimatedSpriteModel>();
        

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

                _currentAnimatedSpriteModel = new AnimatedSpriteModel(ofd.SafeFileName, ofd.FileName, _spriteBitmap.Width, _spriteBitmap.Height); //re initialize on every new load..

                //update stats
                //estimated framecount based on above
                _currentAnimatedSpriteModel.LastFrame = (_currentAnimatedSpriteModel.SourceWidth / _currentAnimatedSpriteModel.FrameWidth);
                gTimer.Interval = 1000 / _currentAnimatedSpriteModel.DesiredFrameRate;

                // generate ratios for rendering overlay on source file (since it's scaled)
                _rectWRatio = (float)(pnlGfx.Width) / (float)_currentAnimatedSpriteModel.SourceWidth;
                _rectHRatio = (float)pnlGfx.Height / (float)_currentAnimatedSpriteModel.SourceHeight;

                //setup and bind our property grid             
                propertyGrid1.SelectedObject = _currentAnimatedSpriteModel;
                propertyGrid1.Enabled = true;
                propertyGrid1.Visible = true;
                propertyGrid1.Name = "Animated Sprite Info";


                // update the name of the file we're editing
                lblEditing.Text = $"Editing: {_currentAnimatedSpriteModel.SourceFullPath} + {_currentAnimatedSpriteModel.SourceFileName}";

                gTimer.Enabled = true;
                _appState.ParentSpriteLoaded = true;
               
            }

        }

        private void NewAnimatedSpriteModel()
        { 
        
        }



        private RectangleF GetScaledFrameRectangle()
        {
            return new RectangleF(
                _currentAnimatedSpriteModel.GetCurrentFrame().X * _rectWRatio,
                _currentAnimatedSpriteModel.GetCurrentFrame().Y * _rectHRatio,
                _currentAnimatedSpriteModel.FrameWidth * _rectWRatio,
                _currentAnimatedSpriteModel.FrameHeight * _rectHRatio
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
                _currentAnimatedSpriteModel.UpdateCurrentFrame(
                   new Rectangle(
                       _currentFrameCounter * _currentAnimatedSpriteModel.FrameWidth,
                       _currentAnimatedSpriteModel.FrameRow * _currentAnimatedSpriteModel.FrameHeight,
                       _currentAnimatedSpriteModel.FrameWidth,
                       _currentAnimatedSpriteModel.FrameHeight));
                _bufferedOriginalFile.Graphics.DrawRectangles(Pens.Green, new RectangleF[] { GetScaledFrameRectangle() });
                _bufferedAnimatedSprite.Graphics.DrawImage(_spriteBitmap, pnlAnimatedSprite.DisplayRectangle, _currentAnimatedSpriteModel.GetCurrentFrame(), GraphicsUnit.Pixel);
                _bufferedAnimatedSprite.Render();
                _bufferedOriginalFile.Render();
                
                _currentFrameCounter++;
                lblFrame.Text = "Frame: " + _currentFrameCounter;

                if (_currentFrameCounter >= _currentAnimatedSpriteModel.LastFrame)
                {
                    _currentFrameCounter = _currentAnimatedSpriteModel.FirstFrame;
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
                    _currentAnimatedSpriteModel.DesiredFrameRate = (int)e.OldValue;
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

            string output = JsonConvert.SerializeObject(_currentAnimatedSpriteModel, Formatting.Indented);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = _currentAnimatedSpriteModel.AnimatedSpriteFilename;
            sfd.Title = "Save Animation Configuration";
            sfd.DefaultExt = ".sfx";

            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _currentAnimatedSpriteModel.AnimatedSpriteFilename = sfd.FileName;
                File.WriteAllText(_currentAnimatedSpriteModel.AnimatedSpriteFilename, output);
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

        private void addAnimationButton_Click(object sender, EventArgs e)
        {
            //first see if we already have it in our list.
            if (_animatedSpriteModels.Contains(_currentAnimatedSpriteModel))
            {
                MessageBox.Show("Model already exists in list.");
            }
            else
            {
                _animatedSpriteModels.Add(_currentAnimatedSpriteModel);
                animationsListBox.Items.Add(_currentAnimatedSpriteModel.AnimationName);
                _currentAnimatedSpriteModel = new AnimatedSpriteModel(_currentAnimatedSpriteModel.SourceFileName, _currentAnimatedSpriteModel.SourceFullPath, _currentAnimatedSpriteModel.SourceWidth, _currentAnimatedSpriteModel.SourceHeight);
                propertyGrid1.SelectedObject = _currentAnimatedSpriteModel;
            }
        }

        private void removeAnimation_Click(object sender, EventArgs e)
        {
            _animatedSpriteModels.Remove(_currentAnimatedSpriteModel);
            animationsListBox.Items.Remove(_currentAnimatedSpriteModel.AnimationName);
        }

        private void animationsListBox_Click(object sender, EventArgs e)
        {
            var animationName = animationsListBox.SelectedItem as string;
            if (!String.IsNullOrEmpty(animationName))
            {
                  var toBeSelected= _animatedSpriteModels.Where(x => x.AnimationName == animationName).FirstOrDefault();
                if (toBeSelected != null)
                {
                    _currentAnimatedSpriteModel = toBeSelected;
                }
            }
        }
    }
}
