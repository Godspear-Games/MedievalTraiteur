using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeColourRandomiser : MonoBehaviour
{
    [SerializeField] private List<Color> _treeColours = new List<Color>();
    
    //randomise color when prefab is instantiated in editor
    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            RandomiseTreeColour();
        }
    }
    
    private void RandomiseTreeColour()
    {
        if (_treeColours.Count > 0)
        {
            int randomIndex = Random.Range(0, _treeColours.Count);
            GetComponent<SpriteRenderer>().color = _treeColours[randomIndex];
        }
    }
    
}
