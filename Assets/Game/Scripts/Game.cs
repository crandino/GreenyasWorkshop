using Greenyas.Input;
using HexaLinks.Tile.Events;
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
        TileEvents.Clear();

        RegisterSystem<InputManager>();
        RegisterSystem<TilePlacement>();

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
        system.InitSystem();
        systems.Add(system.GetType(), system);
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
            SceneManager.LoadScene("Assets/Scenes/Playground.unity");
#endif
    }

    public interface IGameSystem 
    {
        void InitSystem();
    }   
    
    public abstract class GameSystemMonobehaviour : MonoBehaviour, IGameSystem
    {
        public abstract void InitSystem();
    }

    public abstract class GameSystemScriptableObject : ScriptableObject, IGameSystem
    {
        public abstract void InitSystem();
    }

}


