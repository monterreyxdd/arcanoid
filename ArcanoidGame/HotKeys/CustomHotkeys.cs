using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.HotKeys
{
    public class CustomHotkeys
    {
        private const short KEYPRESS_CODE = 128;
        private const short TOGGLE_CODE = 1;

        private readonly List<Hotkey> _hotkeys;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int virtualKeyCode);

        private bool IsPressed(char key)
        {
            return (GetKeyState(char.ToUpper(key)) & KEYPRESS_CODE) == KEYPRESS_CODE;
        }

        private bool IsActive(char key)
        {
            return (GetKeyState(char.ToUpper(key)) & TOGGLE_CODE) == TOGGLE_CODE;
        }
         
        public CustomHotkeys()
        {
            _hotkeys =  new List<Hotkey>();
        }

        public void Register(char keyCode, Action pressAction, Action<bool> toggleAction = null)
        {
            _hotkeys.Add(new Hotkey {Key = keyCode, PressAction = pressAction, ToggleAction = toggleAction, IsActive = IsActive(keyCode)});
        }

        public void Register(IEnumerable<Hotkey> collection)
        {
            foreach (Hotkey hotkey in collection)
            {
                hotkey.IsActive = IsActive(hotkey.Key);
                _hotkeys.Add(hotkey);
            }
        }

        public void Check()
        {
            foreach (var hotkey in _hotkeys)
            {
                if (IsPressed(hotkey.Key))
                {
                    hotkey.PressAction?.Invoke();
                }

                if (IsActive(hotkey.Key) != hotkey.IsActive)
                {
                    hotkey.IsActive = !hotkey.IsActive;
                    hotkey.ToggleAction?.Invoke(hotkey.IsActive);
                }
            }
        }
    }
}
