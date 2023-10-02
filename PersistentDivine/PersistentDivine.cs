using Modding;
using System;
using UnityEngine.SceneManagement;

namespace PersistentDivine
{
    public class PersistentDivineMod : Mod
    {
        private static PersistentDivineMod? _instance;

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

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneUnload;

            Log("Initialized");
        }

        public void OnSceneUnload(Scene scf, Scene sct) {

            PlayerData pd = PlayerData.instance;

            if ((pd.legEaterLeft && pd.defeatedNightmareGrimm && pd.divineFinalConvo && !(scf.name == "Grimm_Divine" && sct.name == "Town")) || !pd.nightmareLanternLit)
            {
                Log("Divine is nowhere to be found");
                pd.divineInTown = false;
            }
            else
            {
                Log("The lantern was lit, and the Divine quest not complete, Divine will stay");
                pd.divineInTown = true;
            }

        }

    }
}
