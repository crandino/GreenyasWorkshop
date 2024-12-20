using UnityEngine;
using static Game;

namespace HexaLinks.Configuration
{
    [CreateAssetMenu(fileName = "GameConf")]
    public class Configuration : GameSystemScriptableObject
    {
        public Colors colors;
        public Parameters parameters;

        public override void InitSystem()
        {
            colors.Init();
        }       
    } 
}
