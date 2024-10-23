using Greenyas.Hexagon;
using UnityEngine;

namespace HexaLinks.Tile.Extensions.Hexside
{
    public static class HexSideExtensions
    {
        public static bool IsOpposite(this HexSide.Side side, HexSide.Side opposite)
        {
            return Mathf.Abs(side - opposite) == 3;
        }

        public static HexSide.Side GetOpposite(this HexSide.Side side)
        {
            return (HexSide.Side)(((int)side + 3) % HexSide.TOTAL_SIDES) + (int)HexSide.Side.North;
        }
    } 
}
