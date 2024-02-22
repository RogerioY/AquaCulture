using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AquaCulture
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        public struct TileOverride // construct a type called TileOverride and give it 2 fields: one of the type TileBase called 'tile', and the other an Array of audio clips. 
        {
            public TileBase tile;           //this doesn't show up in the inspector or anything, it's just like creating a class.
            public AudioClip[] clips;
        }

        public AudioClip[] clips;           /*create and array of AudioClips called 'clips'. These will be the default sounds to use later if the PlayRandomSound function is called with no surface variable
                                             the inspector will now contain a dropdown for 'clips', the first field is 'size' which will determine how many elements there will be in the array. If you set size to 4, there will be 4 elements for you to drag audioclips into. */

        public TileOverride[] overrides;  //Create and array of the type TileOverride called 'overrides' for you to populate in the inspector. Because of the above struct, in the inspector each element in this array will have space for Tilebase (in the tile box) and a nested array of Audioclips.

        public bool randomizePitch = false;  //Allows you to decide if you want to enable randomised pitch
        public float pitchRange = 0.2f;   //determine the maximum deviation from original pitch if you have pitch randomisation on. That is plus and minus zero. So this default value of 0.2f would mean it gives a random pitch betweem -0.8 and 1.2

        protected AudioSource m_Source;  //create an audiosource called m_source . This is what will get filled with audioclips. 
        protected Dictionary<TileBase, AudioClip[]> m_LookupOverride;  //declares a dictionary. A dictionary takes 'keys' and uses them to access values. In this case it takes Tilebases and arrays of AudioClips. This creates dictionary called m_LookupOverride  , but all its values are null at the moment. 

        private void Awake()
        {
            m_Source = GetComponent<AudioSource>();   //fill up our audiosource variable with the audiosource compnenet on this gameObject
            m_LookupOverride = new Dictionary<TileBase, AudioClip[]>();  //create an instance of the above dictionary that we are going to fill with values. 

            for(int i = 0; i < overrides.Length; ++i)  //A for loop for filling up the dictionary. For every element in overrides array run the loop
            {
                if (overrides[i].tile == null)  // skip this iteration of the loop is this element is null (empty)
                    continue;

                m_LookupOverride[overrides[i].tile] = overrides[i].clips; //In our dictionary, each tile in the overrides array is now linked to the set of audio clips you filled out in the same element. 
            }
        }

        public void PlayRandomSound(TileBase surface = null)  //the function that plays the sound. Being called from another script. This can be called with a Tilebase value eg: PlayRandomSound (wood), or without one eg PlayRandomSound(), in which case, this temporary variable called surface is null.
        {
            AudioClip[] source = clips;  // create and array called source and copy all the vaues from 'clips'. Source is the array of clips that will actually get played, so give it the default values first. 

            AudioClip[] temp;   //create a 2nd array called temp to fill up and use if we can
            if (surface != null && m_LookupOverride.TryGetValue(surface, out temp)) //was the function called with a value? If so, can we find that value in our dictionary? if we can, it's gonna be the tile we use as a key to point to the associated array of audioclips, so fill up the 'temp' array with the audio clips associated with the tile that's been called with the function
                source = temp; //temp now holds all the audioclips we're going to want so set source to  temp.

            int choice = Random.Range(0, source.Length); //create an int called choice and assign it a random value between 0 and the number of element in the source array.

            if(randomizePitch) 
                m_Source.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange); //do the randomised pitch if you decided that you want to.

            m_Source.PlayOneShot(source[choice]); //on m_source, play an audioclip selected randomly from the source array. (well really you're selecting the element number that got spat out at line 51) 
        }

        public void Stop()
        {
            m_Source.Stop();
        }
    }
}