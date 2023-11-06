using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Services;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string nextScene;
    
    public void ChangeToNextScene()
    {
        FoundryApp.GetService<ISceneNavigator>().GoToAsync(nextScene);
    }
}
