using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementReactionLibrary : MonoBehaviour
{
    public delegate void ElementReaction(); // public delegate void ElementReaction( Element a, Element b)
    //public enum ElementName { Null, Ice, Fire, Length}

    [SerializeField] GameObject waterPrefab;
    GameObject waterPrefabInstance;
    


    private Quaternion rotation = Quaternion.identity;
    /// <summary>
    /// from here on all of these functions contain the reactions from 2 elements
    /// they are static becasue the same thing will happen every single time
    /// and this way we can call these functions from different classes
    /// 
    ///  to add in future: maybe take in the position of collision parameter?
    /// </summary>

    public void FireIceReaction(Vector3 Position)
    {
        Instantiate(waterPrefab, Position, rotation);
    }

    
}
