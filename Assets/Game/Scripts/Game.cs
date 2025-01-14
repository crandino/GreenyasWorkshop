using Greenyas.Input;
using HexaLinks.Path.Finder;
using HexaLinks.Turn;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Game : SingletonMonobehaviour<Game>
{
    [SerializeField, Header("ScriptableObjects")] private GameSystemScriptableObject[] gameSystemScriptableObject;
    [SerializeField, Header("Monobehaviours")] private GameSystemMonobehaviour[] gameSystemMonobehaviour;

    private readonly Dictionary<Type, IGameSystem> systems = new Dictionary<Type, IGameSystem>();

    protected override void OnInitialization()
    {
        RegisterSystem<InputManager>();
        RegisterSystem<TilePlacement>();
        RegisterSystem<PathIterator>();

        foreach (var system in gameSystemScriptableObject)
            RegisterSystem(system);

        foreach (var system in gameSystemMonobehaviour)
            RegisterSystem(system);

        GetSystem<TurnManager>().StartGame();
    }

    private void RegisterSystem<T>() where T : IGameSystem, new()
    {
        T subSystem = new T();
        RegisterSystem(subSystem);
    }

    private void RegisterSystem<T>(T system) where T : IGameSystem
    {
        systems.Add(system.GetType(), system);
        system.InitSystem();
    }

    private void UnregisterAll()
    {
        foreach(var system in systems.Values)
            system.TerminateSystem();

        systems.Clear();
    }

    public T GetSystem<T>() where T : IGameSystem
    {
        try
        {
            return (T)systems[typeof(T)];
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return default;
        }
    }

    private void Update()
    {
#if DEBUG
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            UnregisterAll();
            SceneManager.LoadScene("Assets/Scenes/Playground.unity");
        }
#endif
    }

    public interface IGameSystem 
    {
        void InitSystem();
        void TerminateSystem();
    }

    public abstract class GameSystemMonobehaviour : MonoBehaviour, IGameSystem
    {
        public virtual void InitSystem() { }
        public virtual void TerminateSystem() { }
    }

    public abstract class GameSystemScriptableObject : ScriptableObject, IGameSystem
    {
        public virtual void InitSystem() { }
        public virtual void TerminateSystem() { }
    }

}


