using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image _joystick;
    [SerializeField] private Image _brakeButton;
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
                Debug.Log("Unity Editor");
        #endif

        #if UNITY_IOS
              Debug.Log("Iphone");
        #endif

        #if UNITY_STANDALONE_WIN
            Debug.Log("Stand Alone Windows");
            _joystick.enabled = false;
            _brakeButton.enabled = false;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
