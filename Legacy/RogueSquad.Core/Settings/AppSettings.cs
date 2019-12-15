using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Settings
{
    public interface IGameSerializableObject
    {
        //marker interface
    }

    //loads from AppSettings.JSON

    [DataContract]
    public class GameSettings : IGameSerializableObject
    {
        [DataMember]
        public int GlobalVolume { get; set; }
        [DataMember]
        public int FxVolume { get; set; }
        [DataMember]
        public int MusicVolume { get; set; }
        [DataMember]
        public int SpeechVolume { get; set; }
        [DataMember]
        public int ResolutionH { get; set; }
        [DataMember]
        public int ResolutionW { get; set; }
        [DataMember]
        public bool EnableFullScreen { get; set; }
        [DataMember]
        public bool UseVsync { get; set; }

        public static GameSettings Default =>  new GameSettings{ GlobalVolume=100, FxVolume = 100, MusicVolume=100, SpeechVolume=100, ResolutionH=800, ResolutionW=600, EnableFullScreen = false, UseVsync=false };


    }






    public class AppSettings
    {
       
        public const string GAME_SETTINGS = "gameSettings.json";
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameSettings));

        public GameSettings LoadSettings()
        {
            if (!File.Exists(GAME_SETTINGS)) return GameSettings.Default;
            
            using (FileStream stream = new FileStream(GAME_SETTINGS, FileMode.Open))
            {
                return (GameSettings)serializer.ReadObject(stream);
            }
        }

        public void SaveSettings(GameSettings settings)
        {
            using (FileStream stream = new FileStream(GAME_SETTINGS, FileMode.Create))
            {
                serializer.WriteObject(stream, settings);
            }
        }       
        
        
    }
}
