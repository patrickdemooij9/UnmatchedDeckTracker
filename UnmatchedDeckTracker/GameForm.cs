using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnmatchedDeckTracker.Models;
using Button = System.Windows.Forms.Button;
using Size = System.Drawing.Size;

namespace UnmatchedDeckTracker
{
    public partial class GameForm : Form
    {
        private Dictionary<Button, int> _amounts;
        private PictureBox _currentPictureBox;

        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        private readonly UnmatchedDeck _deck1;
        private readonly UnmatchedDeck _deck2;

        public GameForm(UnmatchedDeck deck1, UnmatchedDeck deck2)
        {
            _deck1 = deck1;
            _deck2 = deck2;
            _amounts = new Dictionary<Button, int>();
            InitializeComponent();

            Shown += GameForm_Shown;
        }

        private void GameForm_Shown(object? sender, EventArgs e)
        {
            LoadDeck(_deck1, 0);
            LoadDeck(_deck2, Width - 205);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            BackColor = Color.Magenta;
            TransparencyKey = Color.Magenta;
            TopMost = true;
            //FormBorderStyle = FormBorderStyle.None;

            //var initialStyle = GetWindowLong(Handle, -20);
            //SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);

            var gameHandle = FindWindow(null, "Tabletop Simulator");
            GetWindowRect(gameHandle, out var rect);
            Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            Top = rect.Top;
            Left = rect.Left;
        }

        private void LoadDeck(UnmatchedDeck deck, int startingX)
        {
            var maxHeight = 800;
            var maxHeightPerButton = maxHeight / deck.Cards.Length;
            var cards = deck.Cards.OrderBy(it => it.Type).ToArray();

            for (var i = 0; i < cards.Length; i++)
            {
                var card = cards[i];

                var button = new Button();
                button.Top = i * maxHeightPerButton;
                button.Left = startingX;
                button.Size = new Size(200, maxHeightPerButton);
                button.Text = $"{card.Title} ({card.Quantity})";
                button.TextAlign = ContentAlignment.MiddleLeft;
                button.BackColor = card.GetColor();
                
                button.MouseUp += (sender, args) =>
                {
                    MouseEventArgs me = (MouseEventArgs)args;
                    if (me.Button == MouseButtons.Left)
                        _amounts[button] -= 1;
                    else if (me.Button == MouseButtons.Right)
                        _amounts[button] += 1;
                    else if (me.Button == MouseButtons.Middle)
                    {
                        ShowImage(card);
                        return;
                    }
                    button.Text = $"{card.Title} ({_amounts[button]})";
                    if (_amounts[button] > 0)
                    {
                        button.BackColor = card.GetColor();
                    }
                    else
                    {
                        button.BackColor = Color.Gray;
                    }
                };
                button.MouseLeave += (sender, args) => RemoveImage();

                _amounts.Add(button, card.Quantity);
                Controls.Add(button);
            }
        }

        private void ShowImage(UnmatchedCard card)
        {
            if (_currentPictureBox != null)
            {
                Controls.Remove(_currentPictureBox);
            }

            _currentPictureBox = new PictureBox();
            _currentPictureBox.Size = new Size(300, 500);
            _currentPictureBox.Left = Width / 2 - 150;
            _currentPictureBox.LoadAsync(card.Image);

            Controls.Add(_currentPictureBox);
        }

        private void RemoveImage()
        {
            if (_currentPictureBox != null)
            {
                Controls.Remove(_currentPictureBox);
            }
        }
    }
}
