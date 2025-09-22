using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    [SerializeField] private List<IManager> managers;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        managers = FindAllManagers();

        foreach (IManager manager in managers)
        {
            manager.Initialize();
        }
    }

    private List<IManager> FindAllManagers()
    {
        IEnumerable<IManager> Managers = FindObjectsOfType<MonoBehaviour>().OfType<IManager>();

        return new List<IManager>(Managers);
    }
}
