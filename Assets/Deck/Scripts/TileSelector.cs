using Greenyas.Input;
using System;
using UnityEngine;
using Hexalinks.Tile;

public class TileSelector : MonoBehaviour
{
    // TODO: La clase Tile debería ser la encargada de distribuir toda la información a los diferentes
    // procesos. Desconectar sus conexiones, levantarla, pasar a controlarla, rotarla y recolocarla y activar
    // todo el proceso de dibujar los caminos completos

    private Tile currentSelectedTile = null;
    private InputManager input = null;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        input.OnSelect.OnButtonPressed += PickUpTile;
    }

    private void PickUpTile()
    {
        if (TileRaycast.CursorRaycastToTile(out currentSelectedTile))
        {
            input.OnSelect.OnButtonPressed -= PickUpTile;
            input.OnSelect.OnButtonPressed += ReleaseTile;

            currentSelectedTile.PickUp();
        }       
    }

    private void ReleaseTile()
    {
        if (!TileRaycast.CursorRaycastToTile())
        {
            input.OnSelect.OnButtonPressed -= ReleaseTile;
            input.OnSelect.OnButtonPressed += PickUpTile;

            currentSelectedTile.Release();
            currentSelectedTile = null;
        }
    }    
}
