using System.Net.Http.Json;
using UnmatchedDeckTracker.Models;

namespace UnmatchedDeckTracker
{
    public partial class StartForm : Form
    {
        private HttpClient _client = new HttpClient();

        private Dictionary<string, UnmatchedDeck> _decks;
        private GameForm _currentGameForm;

        public StartForm()
        {
            InitializeComponent();

            _decks = new Dictionary<string, UnmatchedDeck>();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            var deckResponse = _client.GetFromJsonAsync<UnmatchedDeckResponse>("https://unmatched.cards/api/db/decks").Result;

            foreach (var deck in deckResponse.Decks.Where(it => it.Cards.Any()).OrderBy(it => it.Name))
            {
                comboBox1.Items.Add(deck.Name);
                comboBox2.Items.Add(deck.Name);

                _decks.Add(deck.Name, deck);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var deck1 = _decks[comboBox1.SelectedItem.ToString()];
            var deck2 = _decks[comboBox2.SelectedItem.ToString()];

            _currentGameForm = new GameForm(deck1, deck2);
            _currentGameForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _currentGameForm.Hide();;
            _currentGameForm = null;
        }
    }
}