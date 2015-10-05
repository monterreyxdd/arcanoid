using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArcanoidGame.HotKeys;

namespace ArcanoidGame
{
    public partial class Form1 : Form
    {
        private const int DEFAULT_FPS = 120;

        private readonly Game _game;
        private readonly CustomHotkeys _hotkeys;
        private readonly Timer _fpsTimer;

        public Form1()
        {
            _game = new Game();
            _hotkeys = new CustomHotkeys();
            _fpsTimer = new Timer();

            InitializeComponent();
            SetupTimer(DEFAULT_FPS);
            SetupWindow();
            SetupHotkeys();
        }

        private void SetupTimer(int fps)
        {
            _fpsTimer.Interval = 1000/fps;
            _fpsTimer.Tick += (sender, args) => Refresh();
        }

        private void SetupWindow()
        {
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            Size = screen.Size;
            Location = screen.Location;

            BackColor = Color.Black;
        }

        private void SetupHotkeys()
        {
            _hotkeys.Register(new []
            {
                //Escape
                new Hotkey {Key = (char) 0x1B, PressAction = Close},
                new Hotkey {Key = 'A', PressAction = _game.MovePlatformLeft},
                new Hotkey {Key = 'D', PressAction = _game.MovePlatformRight},
                //Enter
                new Hotkey {Key = (char) 0x0D, ToggleAction = _game.TogglePause}
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _fpsTimer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle window = e.ClipRectangle;
            Graphics g = e.Graphics;

            _hotkeys.Check();
            _game.Update(g, window);
        }
    }
}