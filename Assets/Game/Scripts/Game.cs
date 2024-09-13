using Greenyas.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class Game : MonoBehaviour
{
    public static Game Instance
    {
        private set; get;
    }

    private readonly Dictionary<Type, SubSystem> systems = new Dictionary<Type, SubSystem>();

    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
            InitializeSubSystems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSubSystems()
    {
        RegisterSubSystem<InputManager>();
        RegisterSubSystem<TilePlacement>();
    }

    private void RegisterSubSystem<T>() where T : SubSystem, new()
    {
        T subSystem = new T();
        subSystem.InitSystem();
        systems.Add(typeof(T), subSystem);
    }

    public T GetSystem<T>() where T : SubSystem
    {
        try
        {
            return (T)systems[typeof(T)];
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }

        return null;

    }

    public abstract class SubSystem
    {
        private bool Initialized { get; set; }

        public void InitSystem()
        {
            if (!Initialized)
                Initialized = TryInitSystem();
        }

        protected abstract bool TryInitSystem();
    }
}


