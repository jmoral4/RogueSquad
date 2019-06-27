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

namespace SpriteConfig
{

    public partial class Form1 : Form
    {
        SpriteFile _spriteFile;
        bool _spriteLoaded;
        AppState _appState;
        AnimatedSpriteInfo _animatedSpriteInfo;
        Image _spriteBitmap;
        Rectangle _currentFrame;
        // move most of this stuff into a spriteInfo or SpriteConfig class
        int _maxFrames = 0;
        int _startFrame = 0;
        BufferedGraphics _bufferedAnimatedSprite;
        BufferedGraphics _bufferedOriginalFile;
        BufferedGraphicsContext bgc;
        float rectWRatio = 1.0f; // the scaling ratio for the 'original sprite' box - used when drawing overlays in the sprite box
        float rectHRatio = 1.0f;

        ConcurrentQueue<Action> _actionQueue;
        
 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _appState = new AppState();
            _spriteFile = new SpriteFile();
            gTimer.Enabled = false;
            bgc = BufferedGraphicsManager.Current;
            _bufferedAnimatedSprite = bgc.Allocate(Graphics.FromHwnd(pnlAnimatedSprite.Handle), pnlAnimatedSprite.DisplayRectangle);
            _bufferedOriginalFile = bgc.Allocate(Graphics.FromHwnd(pnlGfx.Handle), pnlAnimatedSprite.DisplayRectangle);
            _spriteLoaded = false;
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
                _animatedSpriteInfo = new AnimatedSpriteInfo(); //re initialize on every new load..
                _spriteFile.FileFullPath = ofd.FileName;
                _spriteFile.FileName = ofd.SafeFileName;
                _spriteLoaded = true;
                lblEditing.Text = $"Editing: {_spriteFile.FileFullPath} + {_spriteFile.FileName}";
                _spriteBitmap = Image.FromFile(_spriteFile.FileFullPath);
                _spriteFile.Width = _spriteBitmap.Width;
                _spriteFile.Height = _spriteBitmap.Height;

                //update stats
                //estimated framecount based on above
                txtFrames.Text = (_spriteFile.Width / Int32.Parse(txtWidth.Text)).ToString();
                lblMaxFrames.Text = txtFrames.Text;
                _currentFrame = new Rectangle(0, 0, GetFrameWidth(), GetFrameHeight());
                //generate rectangle for initial sprite
                _maxFrames = Int32.Parse(txtFrames.Text);

                rectWRatio = (float)(pnlGfx.Width) / (float)_spriteFile.Width;
                rectHRatio = (float)pnlGfx.Height / (float)_spriteFile.Height;

                UpdateAllSpriteData();
                propertyGrid1.SelectedObject = _animatedSpriteInfo;
                propertyGrid1.Enabled = true;
                propertyGrid1.Visible = true;
                propertyGrid1.Name = "Animated Sprite Info";

                gTimer.Enabled = true;
            }

        }

        private int GetFrameWidth()
        {
            return Int32.Parse(txtWidth.Text);
        }

        private int GetFrameHeight()
        {
            return Int32.Parse(txtHeight.Text);
        }

        private int GetRow()
        {
            return Int32.Parse(txtRow.Text);
        }

        private int GetFrameCount()
        {
            return Int32.Parse(txtFrames.Text);
        }

        private int GetFPS()
        {
            return Int32.Parse(txtFPS.Text);
        }

        private int GetStartFrame()
        {
            return Int32.Parse(txtStartFrame.Text);
        }

      

        private void UpdateAllSpriteData()
        {
            try
            {
                

                _currentFrame = new Rectangle(_animatedSpriteInfo.CurrentFrameCounter * GetFrameWidth(), GetRow() * GetFrameHeight(), GetFrameWidth(), GetFrameHeight());
                
                _animatedSpriteInfo.CurrentFrame = new Rectangle(_animatedSpriteInfo.CurrentFrameCounter * GetFrameWidth(), GetRow() * GetFrameHeight(), GetFrameWidth(), GetFrameHeight());
                _animatedSpriteInfo.FrameHeight = GetFrameHeight();
                _animatedSpriteInfo.FrameWidth = GetFrameWidth();
                _animatedSpriteInfo.FrameRow = GetRow();
                _animatedSpriteInfo.FirstFrame = GetStartFrame();
                _animatedSpriteInfo.LastFrame = GetFrameCount();
                

                //update max frames if we've updated everything else
                _maxFrames = GetFrameCount();
                _startFrame = Int32.Parse(txtStartFrame.Text);

                
            }
            catch (Exception ex)
            { }

            //let's catch fps exceptions indepdentnyly so that we'll simply not change fps until it's corect
            try
            {
                int desiredFPS = GetFPS();
                if (desiredFPS != gTimer.Interval)
                {
                    gTimer.Interval = 1000 / desiredFPS;
                }
            }
            catch (Exception)
            { }

        }

        private RectangleF GetScaledFrameRectangle()
        {
            return new RectangleF(
                _currentFrame.X * rectWRatio,
                _currentFrame.Y * rectHRatio,
                _currentFrame.Width * rectWRatio,
                _currentFrame.Height * rectHRatio

                );
        }



        private void GTimer_Tick(object sender, EventArgs e)
        {

            // draw per tick
            if (_spriteLoaded)
            {
                _bufferedOriginalFile.Graphics.Clear(Color.White);
                _bufferedOriginalFile.Graphics.DrawImage(_spriteBitmap, pnlGfx.DisplayRectangle);

                //draw our current frame rect

                _bufferedOriginalFile.Graphics.DrawRectangles(Pens.Green, new RectangleF[] { GetScaledFrameRectangle() });

                _bufferedAnimatedSprite.Graphics.Clear(Color.White);
                _bufferedAnimatedSprite.Graphics.DrawImage(_spriteBitmap, pnlAnimatedSprite.DisplayRectangle, _currentFrame, GraphicsUnit.Pixel);
                _bufferedAnimatedSprite.Render();
                _bufferedOriginalFile.Render();
                //_animatedSpriteInfo.CurrentFrameCounter++;
                _animatedSpriteInfo.CurrentFrameCounter++;

                if (_animatedSpriteInfo.CurrentFrameCounter >= _maxFrames)
                {
                    _animatedSpriteInfo.CurrentFrameCounter = _startFrame;
                }

                //get all updates if the user changed stuff 
                UpdateAllSpriteData();
            }

        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fs = new Form1();
            fs.Show();
        }

        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // TODO: should do this in a cleaner way
            //update everything on the form if we updateed a property (achieving the 2 way binding)
            lock (_animatedSpriteInfo)
            {
                txtFPS.Text = _animatedSpriteInfo.DesiredFrameRate.ToString();
                txtHeight.Text = _animatedSpriteInfo.FrameHeight.ToString();
                txtWidth.Text = _animatedSpriteInfo.FrameWidth.ToString();
                txtStartFrame.Text = _animatedSpriteInfo.FirstFrame.ToString();
                txtRow.Text = _animatedSpriteInfo.FrameRow.ToString();
                txtFrames.Text = _animatedSpriteInfo.LastFrame.ToString();               
            }
         
        }
    }
}
