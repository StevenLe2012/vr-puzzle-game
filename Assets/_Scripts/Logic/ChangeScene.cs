using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Services;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string nextScene;
    // public NetworkEvent<bool> NextSceneEvent;
    //
    // private void OnEnable()
    // {
    //     // GetComponent<PushPad>().OnPadPushedEvent.AddListener(ChangeToSelectScene);
    // }
    //
    // private void OnDisable()
    // {
    //     // GetComponent<PushPad>().OnPadPushedEvent.RemoveListener(ChangeToSelectScene);
    // }
    
    // public void TriggerNextSelectedSceneForAll()
    // {
    //     NextSceneEvent?.Invoke(true);
    // }
    
    
    public void ChangeToSelectScene(NetEventSource netEventSource, bool isTrue)
    {
        print("IsTrue: " + isTrue);
        if (isTrue)
        {
            ChangeToSelectSceneAsync();
        }
    }

    private async void ChangeToSelectSceneAsync()
    {
        var navigator = FoundryApp.GetService<ISceneNavigator>();
        // Go to new scene
        Debug.Log("Loading new scene " + nextScene);
        await navigator.GoToAsync(nextScene);
    }
}
