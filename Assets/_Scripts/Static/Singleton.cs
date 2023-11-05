/*
 *  - Singleton.cs is an abstract, generic class that is used for creating a singleton class. Another class can derive from SingleTon<T> to become a singleton class.
 *  - Inspiration from https://youtu.be/ErJgQY5smnw
 */

using UnityEngine;

/// <summary>
/// Abstract class for creating a singleton class. It is used to create a class that can only have one instance.
/// Any other scripts wanting access to the class that is inheriting this script should call GetInstance() to return its instance.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    private static bool m_applicationIsQuitting = false;

    /// <summary>
    /// Returns the instance of the singleton class.
    /// </summary>
    /// <returns>The instance of the singleton class.</returns>
    public static T GetInstance()
    {
        if (m_applicationIsQuitting)
        {
            return null;
        }

        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
            }
        }
        return instance;
    }

    /* IMPORTANT!!! To use Awake in a derived class you need to do it this way
     * protected override void Awake()
     * {
     *     base.Awake();
     *     //Your code goes here
     * }
     * */

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Use protected override void Awake() and base.Awake() in derived classes
    /// to use the Awake function and keep the singleton functionality.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this as T)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Called when the application is quitting.
    /// </summary>
    private void OnApplicationQuit()
    {
        m_applicationIsQuitting = true;
    }
}