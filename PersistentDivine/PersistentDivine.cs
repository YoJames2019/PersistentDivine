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

                bool nightmareLanternLit = pd.GetBool("nightmareLanternLit");

                bool completedQuest = pd.GetBool("defeatedNightmareGrimm") || pd.GetBool("destroyedNightmareLantern");
                bool finishedMiscDivineLoreInteractions = pd.GetBool("legEaterLeft") && pd.GetBool("divineFinalConvo");
                bool completedAllDivineInteractions = completedQuest && finishedMiscDivineLoreInteractions;

                // to make sure divines tent doesnt just poof out of existence
                // as soon as we leave the tent after having the final convo
                bool loadingTownFromDivine = previousScene == "Grimm_Divine" && loadingScene == "Town";

                // if we've started the grimm troupe quest and we haven't completed all divine interactions
                // or we're loading into the town from divine
                bool shouldShowDivine = (nightmareLanternLit && !completedAllDivineInteractions) || loadingTownFromDivine;

                return shouldShowDivine;
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
