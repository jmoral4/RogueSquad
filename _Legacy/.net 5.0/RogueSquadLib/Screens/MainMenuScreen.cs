
#region Using Statements
using Microsoft.Xna.Framework;
using RogueSquadLib.BaseServices;
using RogueSquadLib.Screens;
using System;

#endregion

namespace RogueSquadLib.Core.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Rogue Squad \nBuild: " + VersionInfo.GetVersionString())
        {
           
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("AI Testbed");
            MenuEntry animationTest = new MenuEntry("Sound and Animation Test");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry setpieceEditorMenuEntry = new MenuEntry("Future Feature...");
            MenuEntry npcEditorMenuEntry = new MenuEntry("Future Feature...");
            MenuEntry objectEditorMenuEntry = new MenuEntry("Future Feature...");            
            MenuEntry exitMenuEntry = new MenuEntry("Exit");                        
            


            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            animationTest.Selected += AnimationTest_Selected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            //setpieceEditorMenuEntry.Selected += SetpieceEditorMenuEntrySelected;
             npcEditorMenuEntry.Selected += NpcEditorMenuEntrySelected;
            //objectEditorMenuEntry.Selected += ObjectEditorMenuEntrySelected;            
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(animationTest);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(setpieceEditorMenuEntry);
            MenuEntries.Add(npcEditorMenuEntry);
            MenuEntries.Add(objectEditorMenuEntry);            
            MenuEntries.Add(exitMenuEntry);
        }

        private void AnimationTest_Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new SoundAndAnimationTestScreen());
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new AITestBed());
        }


 

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void SetpieceEditorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new OldCollisionTestScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void NpcEditorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new OldCollisionTestScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void ObjectEditorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new OldCollisionTestScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit the game?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
