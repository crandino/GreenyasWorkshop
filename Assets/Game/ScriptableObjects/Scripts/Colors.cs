using HexaLinks.Ownership;
using System.Collections.Generic;
using UnityEngine;

namespace HexaLinks.Configuration
{
    [System.Serializable]
    public class Colors
    {
        [Header("Path colors")]
        [SerializeField] private Color pathNoColor;
        [SerializeField] private Color pathPlayerOneColor;
        [SerializeField] private Color pathPlayerTwoColor;

        [Header("Label colors")]
        [SerializeField] private Color labelsNoColor;
        [SerializeField] private Color labelPlayerOneColor;
        [SerializeField] private Color labelPlayerTwoColor;

        private Dictionary<Owner, ColorSet> playerColors = null;

        public struct ColorSet
        {
            public Color pathColor;
            public Color labelColor;

            public ColorSet(Color path, Color label)
            {
                pathColor = path;
                labelColor = label;
            }
        }

        public void Init()
        {
            playerColors = new Dictionary<Owner, ColorSet>(new[]
            {
                new KeyValuePair<Owner, ColorSet>(Owner.None, new(pathNoColor, labelsNoColor)),
                new KeyValuePair<Owner, ColorSet>(Owner.PlayerOne, new(pathPlayerOneColor, labelPlayerOneColor)),
                new KeyValuePair<Owner, ColorSet>(Owner.PlayerTwo, new(pathPlayerTwoColor, labelPlayerTwoColor))
            });
        }

        public ColorSet this[Owner owner]
        {
            get => playerColors[owner];
        }
    }
}
