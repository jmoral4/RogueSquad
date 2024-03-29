
#region Using Statements
using RogueSquadLib.Util;
using Microsoft.Xna.Framework;
using RogueSquadLib.BaseServices;


#endregion

namespace RogueSquadLib.Core.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    public class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry ungulateMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry frobnicateMenuEntry;
        MenuEntry elfMenuEntry;
        MenuEntry cpuMenuEntry;
        MenuEntry gpuMenuEntry;
        MenuEntry diskSpaceMenuEntry;
        MenuEntry osMenuEntry;

        enum Ungulate
        {
            BactrianCamel,
            Alpaca,
            Llama,
        }

        static Ungulate currentUngulate = Ungulate.Llama;

        static string[] languages = { "C#", "French", "Deoxyribonucleic acid" };
        static int currentLanguage = 0;

        static bool frobnicate = true;

        static int elf = 23;

        #endregion

        #region Initialization
        //GameSettings gameSettings;
        //AppSettings settings;

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Nonsense Options")
        {
            //settings = new AppSettings();
            //gameSettings = settings.LoadSettings();
            // Create our menu entries.
            ungulateMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            frobnicateMenuEntry = new MenuEntry(string.Empty);
            elfMenuEntry = new MenuEntry(string.Empty);

            cpuMenuEntry = new MenuEntry(string.Empty); 
            gpuMenuEntry = new MenuEntry(string.Empty); 
            diskSpaceMenuEntry = new MenuEntry(string.Empty); 
            osMenuEntry = new MenuEntry(string.Empty); 

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            ungulateMenuEntry.Selected += UngulateMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            frobnicateMenuEntry.Selected += FrobnicateMenuEntrySelected;
            elfMenuEntry.Selected += ElfMenuEntrySelected;
            back.Selected += Back_Selected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            //MenuEntries.Add(ungulateMenuEntry);
            //MenuEntries.Add(languageMenuEntry);
            //MenuEntries.Add(frobnicateMenuEntry);
            //MenuEntries.Add(elfMenuEntry);
            MenuEntries.Add(cpuMenuEntry);
            MenuEntries.Add(gpuMenuEntry);
            MenuEntries.Add(diskSpaceMenuEntry);
            MenuEntries.Add(osMenuEntry);
            MenuEntries.Add(back);
        }

        private void Back_Selected(object sender, PlayerIndexEventArgs e)
        {
            //save settings
            //gameSettings.ResolutionH = Engine.Instance.ScreenHeight;
            //gameSettings.ResolutionW = Engine.Instance.ScreenWidth;
            //settings.SaveSettings(gameSettings);
           
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            ungulateMenuEntry.Text = "Preferred ungulate: " + currentUngulate;
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            frobnicateMenuEntry.Text = "Frobnicate: " + (frobnicate ? "on" : "off");
            elfMenuEntry.Text = "elf: " + elf;

            cpuMenuEntry.Text = $"CPU: {OSInfo.Instance.ProcessArchitecture} Cores:{OSInfo.Instance.CPUCores}";
            gpuMenuEntry.Text = $"GPU: {OSInfo.Instance.DefaultGraphicsCard.Description}";
            diskSpaceMenuEntry.Text = $"Disk Free: {OSInfo.Instance.GetFreeDiskSpace() / 1024 / 1024 / 1044}GB";
            osMenuEntry.Text = $"OS: {OSInfo.Instance.OSDescription}";
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentUngulate++;

            if (currentUngulate > Ungulate.Llama)
                currentUngulate = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            frobnicate = !frobnicate;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            elf++;

            SetMenuEntryText();
        }


        #endregion
    }
}
