using Modding;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PersistentDivine
{
    public class PersistentDivineMod : Mod
    {
        private static PersistentDivineMod? _instance;
        private string? loadingScene;
        private string? previousScene;
        private bool overrideDivine;

        internal static PersistentDivineMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(PersistentDivineMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public PersistentDivineMod() : base("PersistentDivine")
        {
            _instance = this;
            overrideDivine = false;
        }

        public override void Initialize()
        {
            Log("Initializing");

            ModHooks.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.BeforeSceneLoadHook += BeforeSceneLoad;

            Log("Initialized");
        }
        private bool OnGetPlayerBoolHook(string target, bool orig)
        {

            if (target == "divineInTown")
            {
                PlayerData pd = PlayerData.instance;
                return !(
                            (
                                pd.GetBool("legEaterLeft") && 
                                (
                                    pd.GetBool("defeatedNightmareGrimm") || 
                                    pd.GetBool("destroyedNightmareLantern")
                                ) && 
                                pd.GetBool("divineFinalConvo") && 
                                !(
                                    previousScene == "Grimm_Divine" && 
                                    loadingScene == "Town"
                                 )
                            ) || 
                            !pd.GetBool("nightmareLanternLit")
                       ) || overrideDivine;
            }
            return orig;
        }
        private string BeforeSceneLoad(string scName)
        {
            previousScene = loadingScene;
            loadingScene = scName;
            return scName;
        }
    }
}
