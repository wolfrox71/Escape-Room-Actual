using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Escape_Room.Global;
using Escape_Room.Global.DataTypes;

namespace Escape_Room.Windows.MasterMind
{
    /// <summary>
    /// Interaction logic for MasterMindMain.xaml
    /// </summary>
    
    public enum Colours
    {
        Red,
        Blue,
        Green,
        Orange,
        Pink,
        Black,
        Grey,
    }

    public partial class MasterMindMain : Window
    {
        Game game;

        Colours[] answer;

        List<Colours> currentGuess = new List<Colours>();


        static int colours_per_guess = 4;
        static int maxNumberOfGuesses = 10;
        int numberOfGueses = 0;
        List<Colours[]> guesses = new List<Colours[]>();
        List<Colours[]> rightSide = new List<Colours[]>();
        Random random = new Random();
        DispatcherTimer timer;
        bool placedBlocks = false;

        Colours correctColour = Colours.Green;
        Colours incorrectPlace = Colours.Orange;
        Colours fullyIncorrect = Colours.Red;

        bool escapeKey = false;

        public MasterMindMain(ref Game _game)
        {
            InitializeComponent();
            game = _game;

            answer = randomGuess();

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(tick);
            timer.Start();
        }

        public void tick(object sender, EventArgs e)
        {
            if (!placedBlocks)
            {
                drawButtons();
                placedBlocks = true;
            }
            // if pressing the escape key exits the code
            // uses more cpu as constantly checking if pressed down
            if (escapeKey)
            {
                // if the escape key is held down
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    // leave the game
                    timer.Stop();
                    game.redirect(new EscapeRoomMain(ref game), this, false);
                    this.Close();
                }
            }
            // if not
            else
            {
                timer.Stop(); // stop the timer and the checking
            }
        }

        public void showCurrent()
        {
            for (int number_in_guess = 0; number_in_guess < currentGuess.Count; number_in_guess++)
            {
                int y = 100;
                int x = (number_in_guess * 20) + 300;
                SolidColorBrush colour = colour_converter(currentGuess[number_in_guess]);
                new Block(_Canvas, x, y, colour);

            }
        }

        public void drawButtons()
        {
            int numberOfColours = (Enum.GetNames(typeof(Colours))).Length;
            int numberOfButtons = numberOfColours + 3; // + 3 for answer
            for (int i = 0; i < numberOfButtons; i++)
            {
                int x = 300 + (i / 2 * 70);
                int y = 200;
                if (i % 2 == 0)
                {
                    y -= 70;
                }
                string content = "";
                if (i >= numberOfColours)
                {
                    switch (i - numberOfColours)
                    {
                        case 0:
                            content = "Enter";
                            break;
                        case 1:
                            content = "Delete";
                            break;
                        case 2:
                            content = "Answer";
                            break;
                    }
                }
                else
                {
                    content = ((Colours)i).ToString();
                }
                button btn = new button(_Canvas, x, y, content);
                btn._Btn.Click += Btn_clicked;
            }
        }

        public void Btn_clicked(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            String buttonName = clicked.Content.ToString();
            // if a colour button was pressed
            switch (buttonName)
            {
                case "Enter":
                    if (currentGuess.Count != colours_per_guess)
                    {
                        return;
                    }
                    addGuess(currentGuess.ToArray());
                    currentGuess.Clear();
                    break;
                case "Delete":
                    if (currentGuess.Count > 0)
                    {
                        // remove the last item
                        currentGuess.RemoveAt(currentGuess.Count-1);
                        //currentGuess.RemoveAt(currentGuess.Count-1);
                    }
                    break;
                case "Answer":
                    MessageBox.Show($"The answer was {answer[0].ToString()},{answer[1].ToString()},{answer[2].ToString()},{answer[3].ToString()}");
                    return;
                default:
                    if (currentGuess.Count < colours_per_guess)
                    {
                        currentGuess.Add(colour_converter(buttonName));
                    }
                    break;
            }
            showGuesses();
            //run();

        
        }

        public Colours[] randomGuess()
        {
            Colours[] guess = new Colours[colours_per_guess];
            for (int i = 0; i < colours_per_guess; i++)
            {
                guess[i] = (Colours)random.Next(Enum.GetValues(typeof(Colours)).Length);
            }
            return guess;
        }

        public void addGuess(Colours[] guess)
        {
            numberOfGueses++;
            int numberIncorrect = 0;
            for (int i = 0; i < colours_per_guess; i++)
            {
                if (guess[i] != answer[i])
                {
                    numberIncorrect++;
                }
            }
            // if they didnt get any wrong
            if (numberIncorrect == 0)
            {
                won();
                return;
            }
            // add the guess to the guesses
            guesses.Add(guess);
            // and caluclate the right side
            calcualteRightside(guess);


            // if they dont have any guesses left
            if (!(numberOfGueses < maxNumberOfGuesses))
            { 
                // stop the timer
                timer.Stop();
                // show the answer
                MessageBox.Show($"The answer was {answer[0].ToString()},{answer[1].ToString()},{answer[2].ToString()},{answer[3].ToString()}");
                //MasterMindMain a = new MasterMindMain(ref game);

                // and move them to the main window
                EscapeRoomMain a = new EscapeRoomMain(ref game);
                a.Show();
                this.Close();
            }
           
            return;
        }

        public void calcualteRightside(Colours[] guess)
        {
            /*
            {
                List<int> correct = new List<int>();
                List<Colours> right = new List<Colours>();
                // go through to see what is in the correct place
                for (int i = 0; i < colours_per_guess; i++)
                {
                    if (guess[i] == answer[i])
                    {
                        correct.Add(i);
                        right.Add(correctColour);
                    }
                }
                for (int i = 0; i < colours_per_guess; i++)
                {
                    // if this one was already in the correct place
                    if (correct.Contains(i))
                    {
                        // move to the next location
                        continue;
                    }
                    // go through the answer to see if this colour is in somewhere
                    for (int j = 0; j < colours_per_guess; j++)
                    {
                        // if the colour is in the wrong place but the answer has that colour
                        if (guess[i] == answer[j] && !correct.Contains(i) && !correct.Contains(j))
                        {
                            // add that that index has been used so repeats doesnt break
                            correct.Add(j);
                            correct.Add(i);
                            // add the correct colour on the other side
                            right.Add(incorrectPlace);
                        }
                    }
                }
                // go through each colour that is completely wrong
                for (int i = 0; i < colours_per_guess - right.Count; i++)
                {
                    // and add that colour
                    right.Add(fullyIncorrect);
                }
                rightSide.Add(right.ToArray());
            }
            */
            List<Colours> Right = new List<Colours>();
            List<Colours> guess_List = guess.ToList();
            List<Colours> answer_clone = new List<Colours>();
            foreach (Colours colour in answer) { answer_clone.Add(colour); }
            // checking for in the right place
            for (int i = guess_List.Count-1; i >= 0; i--)
            {
                if (guess_List[i] == answer_clone[i])
                {
                    Right.Add(correctColour);
                    guess_List.RemoveAt(i);
                    answer_clone.RemoveAt(i);
                }
            }
            // checking everything that is in the wrong place
            for (int i = guess_List.Count-1; i >= 0; i--)
            {
                Colours colour = guess_List[i];
                if (answer_clone.Contains(colour))
                {
                    Right.Add(incorrectPlace);
                    guess_List.RemoveAt(i);
                    answer_clone.Remove(colour);
                }
            }
            // all the ones that werent in the answer at all
            foreach(Colours colour in guess_List)
            {
                Right.Add(fullyIncorrect);
            }
            rightSide.Add(Right.ToArray());
        }

        public void won()
        {
            timer.Stop();
            MessageBox.Show($"Won in {numberOfGueses} attempts!");
            // give them the blue key if they dont have it already
            if (!game.Inventory.ContainsKey("Blue"))
            {
                 MessageBox.Show("You got the Blue Key");
                game.Inventory.Add("Blue", new DoorKey(Brushes.Blue));
            }
           
            EscapeRoomMain a = new EscapeRoomMain(ref game);
            a.Show();
            this.Close();
        }
        public void showGuesses()
        {
           
            _Canvas.Children.Clear();
            drawButtons();
            showCurrent();
            if (guesses.Count == 0)
            {
                return;
            }
            for (int attemptNumber = 0; attemptNumber < numberOfGueses; attemptNumber++)
            {
                for (int number_in_guess = 0; number_in_guess < colours_per_guess * 2; number_in_guess++)
                {
                    int y = (attemptNumber * 20) + 20;
                    int x = (number_in_guess * 20) + 20;
                    SolidColorBrush colour;
                    if (number_in_guess < colours_per_guess)
                    {
                        colour = colour_converter(guesses[attemptNumber][number_in_guess]);
                    }
                    else
                    {
                        x += 50;
                        colour = colour_converter(rightSide[attemptNumber][number_in_guess % colours_per_guess]);
                    }
                    new Block(_Canvas, x, y, colour);

                }

            }
        }

        public SolidColorBrush colour_converter(Colours colour)
        {
            switch (colour)
            {
                case Colours.Red:
                    return Brushes.Red;
                case Colours.Blue:
                    return Brushes.Blue;
                case Colours.Green:
                    return Brushes.Green;
                case Colours.Orange:
                    return Brushes.Orange;
                case Colours.Pink:
                    return Brushes.Pink;
                //case Colours.White:
                //  return Brushes.White;
                case Colours.Grey:
                    return Brushes.Gray;
                default:
                    return Brushes.Black;
            
            }
        }
        public Colours colour_converter(String colour)
        {
            switch (colour)
            {
                case "Red":
                    return Colours.Red;
                case "Blue":
                    return Colours.Blue;
                case "Green":
                    return Colours.Green;
                case "Orange":
                    return Colours.Orange;
                case "Pink":
                    return Colours.Pink;
                //case Colours.White:
                //  return Brushes.White;
                case "Grey":
                    return Colours.Grey;
                default:
                    return Colours.Black;

            }
        }
    }
}