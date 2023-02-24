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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using System.Diagnostics;
using Escape_Room.Global;

namespace Escape_Room.Windows.Maths_Quiz
{
    /// <summary>
    /// Interaction logic for MathsQuizMain.xaml
    /// </summary>
    public partial class MathsQuizMain : Window
    {
        Game game;
        Stopwatch timeOnQuiz = new Stopwatch();

        double timeToWinMS = 60000.0;
        int questionsToWin = 5;
        int numberCorrect = 0;
        int currentAnswer = 0;

        int minNumber = 0;
        int maxNumber = 100;
        Random random = new Random();
        DispatcherTimer timer;
        bool escapeKey = false; // if the escape key quits the quiz
        public enum operation
        {
            add,
            times,
            subtract,
        }
        
        public MathsQuizMain(ref Game E_game)
        {
            InitializeComponent();
            game = E_game;
            MessageBox.Show($"Answer {questionsToWin} within {(int)(timeToWinMS/1000)}s in order to get a key");
            timeOnQuiz.Start();
            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(tick);
            timer.Start();
            newQuestion();
            button btn = new button(_Canvas, 300, 100, "New Question");
            btn._Btn.Click += newQuestion;
        }


        public void tick(object sender, EventArgs e)
        {
            timeBox.Text = $"{((int)(timeOnQuiz.ElapsedMilliseconds / 1000)).ToString()}s";
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
        }

        public void newQuestion(object sender=null, RoutedEventArgs e=null)
        {
            scoreBox.Text = $"Correct: {numberCorrect}";
            int num1 = random.Next(minNumber, maxNumber);
            int num2 = random.Next(minNumber, maxNumber);
            operation Operation = (operation)random.Next(Enum.GetValues(typeof(operation)).Length);
            if (numberCorrect >= questionsToWin)
            {
                MessageBox.Show("You have been given the Red Key!");
                if (!game.Inventory.ContainsKey("Red"))
                {
                    game.Inventory.Add("Red", new Global.DataTypes.DoorKey(Brushes.Red));
                }
                game.redirect(new EscapeRoomMain(ref game), this, true);
                this.Close();
            }
            (int, string) a = generate(num1, num2, Operation);
            questionBox.Text = a.Item2;
            currentAnswer = a.Item1;

        }

        protected (int, string) generate(int num1, int num2, operation Operation)
        {
            string value;
            int answer;

            switch (Operation)
            {
                case operation.add:
                    value = $"{num1} + {num2}";
                    answer = num1 + num2;
                    break;
                case operation.subtract:
                    value = $"{num1} - {num2}";
                    answer = num1 - num2;
                    break;
                default:
                    num1 %= 10;
                    num2 %= 10;
                    value = $"{num1} x {num2}";
                    answer = num1 * num2;
                    break;
            }

            return (answer, value);
        }

        private void answerBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (timeOnQuiz.ElapsedMilliseconds > timeToWinMS)
            {
                timeOnQuiz.Stop();
                MessageBox.Show($"You ran out of time\nYou completed {numberCorrect} questions");
                game.redirect(new EscapeRoomMain(ref game), this, true);
                this.Close();
            }
            if (answerBox.Text == currentAnswer.ToString())
            {
                numberCorrect += 1;
                newQuestion();
                answerBox.Text = "";
            }
        }
    }
}
