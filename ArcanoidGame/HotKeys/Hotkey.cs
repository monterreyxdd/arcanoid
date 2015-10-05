using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.HotKeys
{
    public class Hotkey
    {
        public char Key { get; set; }
        public bool IsActive { get; set; }
        public Action PressAction { get; set; }
        public Action<bool> ToggleAction { get; set; }
    }
}
