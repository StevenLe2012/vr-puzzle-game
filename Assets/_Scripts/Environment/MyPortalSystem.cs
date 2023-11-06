using System;
using Foundry;
using Foundry.Networking;
using Foundry.Services;
using UnityEngine;

public class MyPortalSystem : FoundryScript
{
    [System.Serializable]
    public enum PortalType
    {
        Teleport,
        SceneChange,
        GoBack,
        GoForward
    }

    [Header("Portal Type")]
    public PortalType portalType;

    [Header("TeleportSystem")]
    public Transform teleportPoint;
    public PortalSystem linkedPortal;

    [Header("SceneChange")]
    public string sceneName;

    private void OnEnable()
    {
        GetComponent<NewSceneNetworkedEvent>().NewSceneEvent.AddListener(ChangeToSelectScene);
    }

    private void OnDisable()
    {
        GetComponent<NewSceneNetworkedEvent>().NewSceneEvent.RemoveListener(ChangeToSelectScene);
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && player.GetComponent<NetworkObject>().IsOwner)
        {
            if (portalType == PortalType.Teleport)
            {
                player.TeleportLook(linkedPortal.teleportPoint.position, linkedPortal.teleportPoint.forward, linkedPortal.teleportPoint.up);
                Debug.Log("Teleporting player to " + linkedPortal.teleportPoint.position + " from " +
                          teleportPoint.position);
            }
            else
            {
                // Get the navigator service
                var navigator = FoundryApp.GetService<ISceneNavigator>();

                if (portalType == PortalType.SceneChange)
                {

                    // Go to new scene
                    Debug.Log("Loading new scene " + sceneName);

                    
                    GetComponent<NewSceneNetworkedEvent>().NewSceneEvent?.Invoke(true);
                }
                else if (portalType == PortalType.GoBack)
                {
                    // If we can't go back, warn
                    if (!navigator.CanGoBack)
                    {
                        Debug.LogError("Cannot navigate back. Ignoring teleport request.");
                        return;
                    }

                    // Go back
                    Debug.Log("Going back");
                    await navigator.GoBackAsync();
                }
                else if (portalType == PortalType.GoForward)
                {
                    // If we can't go forward, warn
                    if (!navigator.CanGoForward)
                    {
                        Debug.LogError("Cannot navigate forward. Ignoring teleport request.");
                        return;
                    }

                    // Go back
                    Debug.Log("Going forward");
                    await navigator.GoForwardAsync();
                }
            }
        }
    }

    public void ChangeToSelectScene(NetEventSource netEventSource, bool isTrue)
    {
        if (isTrue)
        {
            ChangeToSelectSceneAsync();
        }
        // var navigator = FoundryApp.GetService<ISceneNavigator>();
        
    }
    
    public async void ChangeToSelectSceneAsync()
    {
        var navigator = FoundryApp.GetService<ISceneNavigator>();
        await navigator.GoToAsync(sceneName);
    }
}