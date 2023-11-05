using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Services;
using UnityEngine;

public class PlayerRoleManager : MonoBehaviour
{
    public string onlineScene;

    // Setting the player role to Overlord Role (P1)
    public void JoinAsP1()
    {
        PlayerPrefs.SetInt("PlayerRole", 1);
        PlayerPrefs.SetString("usernameLAN", "P1");
        FoundryApp.GetService<ISceneNavigator>().GoToAsync(onlineScene);
    }
    
    // Setting the player role to Top-Down-Character Role (P2)
    public void JoinAsP2()
    {
        PlayerPrefs.SetInt("PlayerRole", 2);
        PlayerPrefs.SetString("usernameLAN", "P2");
        FoundryApp.GetService<ISceneNavigator>().GoToAsync(onlineScene);
    }
    
    
}
