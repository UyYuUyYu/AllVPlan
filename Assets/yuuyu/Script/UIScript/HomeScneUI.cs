using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScneUI : MonoBehaviour
{
    [SerializeField] Text koeruSubScriberText,playerPointText;
    int _koeruSubscriber;
    int _playerPoint;
    // Start is called before the first frame update
    void Awake()
    {

        _koeruSubscriber=GameManager.koeruSubscriber;
        _playerPoint=GameManager.playerPoint;
       koeruSubScriberText.text=_koeruSubscriber.ToString("d5");
       playerPointText.text=_playerPoint.ToString("d5");
    }

  
}
