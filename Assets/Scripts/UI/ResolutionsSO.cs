using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Resolution SO", menuName = "Resolution SO")]
public class ResolutionsSO : ScriptableObject
{
    [System.Serializable]
    public class ResolutionSetting
    {
        public string name
        {
            get
            {
                return width + "x" + height;
            }
        }
        public int width = 0;
        public int height = 0;
    }
    public ResolutionSetting[] AvailableResolutions;
}
