using Microsoft.Xna.Framework;
namespace Cyberpunch_game
{
    interface IControllable
    {
        PlayerIndex PlayerIndex { get; set; }
        void SetInput(GameAction Action);
    }
}
