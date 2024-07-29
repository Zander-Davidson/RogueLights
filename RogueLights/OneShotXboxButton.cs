using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RogueLights
{
    public class OneShotXboxButton
    {
        static GamePadState currentGamePadState;
        static GamePadState previousGamePadState;

        public static GamePadState GetState()
        {
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);

            return currentGamePadState;
        }

        public static bool IsPressed(Buttons button)
        {
            return currentGamePadState.IsButtonDown(button);
        }

        public static bool HasNotBeenPressed(Buttons button)
        {
            return IsPressed(button) && !previousGamePadState.IsButtonDown(button);
        }
    }
}
