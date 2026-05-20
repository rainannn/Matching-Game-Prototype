#region

using UnityEngine;

#endregion

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T s_instance = null;


    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<T>(false);
                if (s_instance == null)
                {
                    Debug.LogWarning("Instance not found! " + typeof(T));
                    return null;
                }

#if UNITY_EDITOR
                if (Application.isPlaying)
                {
#endif
                    s_instance.transform.SetParent(null);

#if UNITY_EDITOR
                }
#endif
            }

            return s_instance;
        }
    }
}