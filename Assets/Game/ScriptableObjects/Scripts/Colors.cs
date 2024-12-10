using HexaLinks.Ownership;
using System.Collections.Generic;
using UnityEngine;

namespace HexaLinks.Configuration
{
    [System.Serializable]
    public class Colors
    {
        [SerializeField] private Color noColor;
        [SerializeField] private Color playerOneColor;
        [SerializeField] private Color playerTwoColor;

        private Dictionary<Owner, Color> playerColors = null;

        public void Init()
        {
            playerColors = new Dictionary<Owner, Color>(new[]
            {
                new KeyValuePair<Owner, Color>(Owner.None, noColor),
                new KeyValuePair<Owner, Color>(Owner.PlayerOne, playerOneColor),
                new KeyValuePair<Owner, Color>(Owner.PlayerTwo, playerTwoColor)
            });
        }

        public Color this[Owner owner]
        {
            get => playerColors[owner];
        }
    } 
}
