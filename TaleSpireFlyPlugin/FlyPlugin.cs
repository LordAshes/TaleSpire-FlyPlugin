using BepInEx;
using BepInEx.Configuration;

using UnityEngine;

namespace LordAshes
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency(LordAshes.FileAccessPlugin.Guid)]
    [BepInDependency(RadialUI.RadialUIPlugin.Guid)]
    public partial class FlyPlugin : BaseUnityPlugin
    {
        // Plugin info
        public const string Name = "Fly Plug-In";               
        public const string Guid = "org.lordashes.plugins.fly";
        public const string Version = "1.1.0.0";                

        /// <summary>
        /// Function for initializing plugin
        /// This function is called once by TaleSpire
        /// </summary>
        void Awake()
        {
            UnityEngine.Debug.Log("Fly Plugin: Active.");

            // Remove original fly menu option
            if(Config.Bind("Settings","Remove GM Only Fly Option", true).Value==true)
            {
                RadialUI.RadialUIPlugin.AddOnRemoveCharacter(FlyPlugin.Guid, "Fly Toggle", null);
            }

            // Add replacement
            RadialUI.RadialUIPlugin.AddOnCharacter(FlyPlugin.Guid, new MapMenu.ItemArgs()
            {
                Action = (mmi, obj) =>
                {
                    CreatureBoardAsset asset = null;
                    CreaturePresenter.TryGetAsset(new CreatureGuid(RadialUI.RadialUIPlugin.GetLastRadialTargetCreature()), out asset);
                    if(asset!=null)
                    {
                        asset.EnableFlying(!asset.IsFlying);
                    }
                },
                CloseMenuOnActivate = true,
                Icon = FileAccessPlugin.Image.LoadSprite("Fly.png"),
                Title = "Fly"
            }, (a,b)=> { return LocalClient.HasControlOfCreature(new CreatureGuid(RadialUI.RadialUIPlugin.GetLastRadialTargetCreature())); });
                
            Utility.PostOnMainPage(this.GetType());
        }

        /// <summary>
        /// Function for determining if view mode has been toggled and, if so, activating or deactivating Character View mode.
        /// This function is called periodically by TaleSpire.
        /// </summary>
        void Update()
        {
        }
    }
}
