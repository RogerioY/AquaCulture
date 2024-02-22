using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    public class OptionUI : MonoBehaviour
    {
        public void ExitPause()
        {
            PlayerCharacter.PlayerInstance.Unpause();
        }

        public void RestartLevel()
        {
            ExitPause();
            SceneController.RestartZone();
        }
    }
}