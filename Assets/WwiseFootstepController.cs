using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WwiseFootstepController : MonoBehaviour
{
    [System.Serializable]
    public struct TileToFootstepSwitch
    {
        public TileBase tile;
        public string AKSwitchName;
    }
    public TileToFootstepSwitch[] TileSwitchAssociations;
    protected Dictionary<TileBase, string> m_lookupSwitches;
    TileBase lastSurface = null;
    string debugMes = "no value";
    

    void Awake()
    {
        m_lookupSwitches = new Dictionary<TileBase, string>();

        for (int i = 0; i < TileSwitchAssociations.Length; ++i)  //A for loop for filling up the dictionary. For every element in overrides array run the loop
        {
            if (TileSwitchAssociations[i].tile == null)  // skip this iteration of the loop is this element is null (empty)
                continue;

            m_lookupSwitches[TileSwitchAssociations[i].tile] = TileSwitchAssociations[i].AKSwitchName; //In our dictionary, each tile in the overrides array is now linked to the set of switches you filled out in the same element. 
        }
    }

   public void PlayWwiseFootstep(TileBase surface = null)
    {
        string AkSwitchTemp = null;
        string AkSwitchToUse = null;
        if ((surface != lastSurface) && (surface != null) && (m_lookupSwitches.TryGetValue(surface, out AkSwitchTemp)))
        {
            AkSwitchToUse = AkSwitchTemp;
            AkSoundEngine.SetSwitch("FS_Surfaces", AkSwitchToUse, gameObject);
        }
        lastSurface = surface;
        AkSoundEngine.PostEvent("Play_Footstep", gameObject);
      
     
    }

}

