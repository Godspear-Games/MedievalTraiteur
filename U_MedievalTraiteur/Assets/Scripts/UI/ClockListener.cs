using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockListener : MonoBehaviour
{
    [SerializeField] private TMP_Text _clockText = null;
    [SerializeField] private Image _clockImage = null;
    [SerializeField] private GameObject _clockPointer = null;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnUpdateTimer += ClockTick;
    }
    
    private void OnDisable()
    {
        EventManager.Instance.OnUpdateTimer -= ClockTick;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnUpdateTimer -= ClockTick;
    }

    private void ClockTick(float time, float maxtime)
    {
        _clockText.text = time.ToString();
        _clockImage.fillAmount = (float)time/maxtime;
        _clockPointer.transform.localEulerAngles = new Vector3(0,0,360f-(time/maxtime*360f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
