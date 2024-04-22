using UnityEngine;

public class FSMSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if(instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("FSMSingleton Error");
                        return instance;
                    }

                    if(instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        singleton.hideFlags = HideFlags.HideAndDontSave;
                    }
                    else
                    {
                        Debug.LogError("FSMSinglton already exists");
                    }
                }
                return instance;
            }
        }
    }
}
