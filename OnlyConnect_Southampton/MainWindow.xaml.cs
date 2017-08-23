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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OnlyConnect_Southampton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int clickCounts = 0;
        int correctCounts = 0;
        bool fadeout_done = true;
        int chancesLeft = 3;
        //Random rnd = new Random();
        List<Button> ClickedButtons = new List<Button>();
        string[] words = {"Cuckoo", "Common skate", "Giant panda", "Steller's sea cow","Atlantic halibut (wild)", "African elephant", "Passenger pigeon", "Shag", "European eel", "Dodo", "Turtle dove", "Koala", "Polar bear", "Kittiwake", "Pyrenean ibex", "Bluefin tuna" };
        //string[] words;
        DispatcherTimer timer = new DispatcherTimer();
        List<string> group1 = new List<string> { "Cuckoo", "Turtle dove", "Kittiwake", "Shag" };
        List<string> group2 = new List<string> { "Atlantic halibut (wild)", "European eel", "Common skate", "Bluefin tuna" };
        List<string> group3 = new List<string> { "Dodo", "Passenger pigeon", "Pyrenean ibex", "Steller's sea cow" };
        List<string> group4 = new List<string> { "Polar bear", "Giant panda", "African elephant", "Koala" };

        

        private void gameArea_Loaded(object sender, RoutedEventArgs e)
        {
            int wordIndex = 0;
            //words = ori_words.OrderBy(x => rnd.Next()).ToArray();
            messageArea.Text = "Click any button to start the game!";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button thisButton = new Button();
                    thisButton.SetValue(Grid.RowProperty, i);
                    thisButton.SetValue(Grid.ColumnProperty, j);
                    thisButton.Content = "Only Connect!";
                    thisButton.Background = Brushes.Azure;
                    thisButton.FontSize = 40;
                    thisButton.FontFamily = new FontFamily("Arial Rounded MT Bold");
                    thisButton.Click += newGame;
                    thisButton.Style = (Style)FindResource("GelButton");
                    gameArea.Children.Add(thisButton);
                    wordIndex++;
                }
            }            
        }

        private void newGame(object sender, RoutedEventArgs e)
        {
            restart();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeBar.Value > 0)
            {
                timeBar.Value -= 0.016;
            }
            else
            {
                losing("Time's Up!", "Sorry, time is up! Press reset to play again!");
            }
        }

        private void losing(string reason, string message)
        {
            popUpStatus(reason, 2);
            messageArea.Text = message;
            foreach(Button singlebutton in gameArea.Children)
            {
                singlebutton.IsEnabled = false;
                singlebutton.Foreground = Brushes.Black;
                singlebutton.Background = Brushes.Azure;
                singlebutton.Content = "Only Connect!";
            }
        }

        private void popUpStatus(string text, int seconds)
        {
            if(fadeout_done)
            {
                fadeout_done = false;
                Status.Content = text;
                Status.Visibility = Visibility.Visible;
                DoubleAnimation fadeout = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(seconds)));
                fadeout.Completed += Fadeout_Completed;
                Status.BeginAnimation(Grid.OpacityProperty, fadeout);
            }
           
        }

        private void Fadeout_Completed(object sender, EventArgs e)
        {
            Status.Visibility = Visibility.Collapsed;
            fadeout_done = true;
        }

        private void ThisButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            //popUpStatus(clickedButton.Name.ToString(), 2);
            //Status.Content = clickedButton.Name.ToString();
            if (clickCounts < 3)
            {
                if (correctCounts == 0)
                {
                    clickedButton.Background = Brushes.MidnightBlue;
                }
                else if (correctCounts == 1)
                {
                    clickedButton.Background = Brushes.Teal;
                }
                else if (correctCounts == 2)
                {
                    clickedButton.Background = Brushes.DarkViolet;
                }
                else
                {
                    //clickedButton.Background = Brushes.LightSteelBlue;
                }
                clickedButton.Foreground = Brushes.White;
                //clickedButton.Click -= ThisButton_Click;
                if (ClickedButtons.Contains(clickedButton))
                {
                    ClickedButtons.Remove(clickedButton);
                    clickedButton.Background = Brushes.Azure;
                    clickedButton.Foreground = Brushes.Black;
                    clickCounts--;
                }
                else
                {
                    ClickedButtons.Add(clickedButton);
                    clickCounts++;
                }
                
            }
            else
            {
                if (correctCounts == 0)
                {
                    clickedButton.Background = Brushes.MidnightBlue;
                }
                else if (correctCounts == 1)
                {
                    clickedButton.Background = Brushes.Teal;
                }
                else if (correctCounts == 2)
                {
                    clickedButton.Background = Brushes.DarkViolet;
                }
                else
                {
                    clickedButton.Background = Brushes.LightSteelBlue;
                }
                clickedButton.Foreground = Brushes.White;
                //clickedButton.Click -= ThisButton_Click;
                if(ClickedButtons.Contains(clickedButton))
                {
                    ClickedButtons.Remove(clickedButton);
                    clickedButton.Background = Brushes.Azure;
                    clickedButton.Foreground = Brushes.Black;
                    clickCounts--;
                }
                else
                {
                    ClickedButtons.Add(clickedButton);
                    clickCounts++;
                    clickReached();
                }             
                
            }

  
        }

        private void clickReached()
        {
            List<string> selectedItems = new List<string>();
            foreach (var item in ClickedButtons)
            {
                selectedItems.Add(item.Content.ToString());
            }
            if(!selectedItems.Except(group1).Any())
            {
                CorrectGroup();
            }
            else if (!selectedItems.Except(group2).Any())
            {
                CorrectGroup();
            }
            else if(!selectedItems.Except(group3).Any())
            {
                CorrectGroup();
            }
            else if (!selectedItems.Except(group4).Any())
            {
                CorrectGroup();
            }
            else
            {
                clearButtons();
            }
            ClickedButtons.Clear();
            clickCounts = 0;
        }

        private void CorrectGroup()
        {
            int col = 0;
            if (correctCounts < 2)
            {
                popUpStatus("Correct!", 2);
            }
            else
            {
                popUpStatus("You win!", 5);
                timer.Stop();
            }
            
            foreach (var button in ClickedButtons)
            {
                if (correctCounts == 0)
                {
                    messageArea.Text = "That's great! 3 more groups to go!";
                    button.Background = Brushes.MidnightBlue;
                }
                else if(correctCounts == 1)
                {
                    messageArea.Text = "Now you have 3 chances to work out the last 2 groups";
                    live1.Visibility = Visibility.Visible;
                    live2.Visibility = Visibility.Visible;
                    live3.Visibility = Visibility.Visible;
                    button.Background = Brushes.Teal;
                }
                else if(correctCounts == 2)
                {
                    messageArea.Text = "Great work! You win! Press reset to play again!";
                    button.Background = Brushes.DarkViolet;
                }
                else
                {
                    //button.Background = Brushes.LightSteelBlue;
                }
                button.Click -= ThisButton_Click;
                string butName = "Button" + correctCounts.ToString() + col.ToString();
                Button theButton = LogicalTreeHelper.FindLogicalNode(gameArea, butName) as Button;
                Fadeout(theButton, button);
                int i = (int)button.GetValue(Grid.RowProperty);
                int j = (int)button.GetValue(Grid.ColumnProperty);
                theButton.SetValue(Grid.RowProperty, i);
                theButton.SetValue(Grid.ColumnProperty, j);
                button.SetValue(Grid.RowProperty, correctCounts);
                button.SetValue(Grid.ColumnProperty, col);
                button.Name = "Correct" +correctCounts.ToString() + col.ToString();
                theButton.Name = "Button" + i.ToString() + j.ToString();
                col++;
                Fadein(theButton, button);
            }
            correctCounts++;
            if (correctCounts == 3)
            {
                for(int i = 0; i<4; i++)
                {
                    string butName = "Button" + "3"+ i.ToString();
                    Button theButton = LogicalTreeHelper.FindLogicalNode(gameArea, butName) as Button;
                    theButton.Background = Brushes.Green;
                    theButton.Click -= ThisButton_Click;
                }
            }

        }

        private void Fadeout(Button but1, Button but2)
        {
            DoubleAnimation da = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(2)));
            but1.BeginAnimation(Grid.OpacityProperty, da);
            but2.BeginAnimation(Grid.OpacityProperty, da);
        }
        private void Fadein(Button but1, Button but2)
        {
            DoubleAnimation da = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(2)));
            but1.BeginAnimation(Grid.OpacityProperty, da);
            but2.BeginAnimation(Grid.OpacityProperty, da);
        }

        private void restart()
        {
            gameArea.Children.Clear();
            //startMusic.Play();
            int wordIndex = 0;
            clickCounts = 0;
            chancesLeft = 3;
            correctCounts = 0;
            messageArea.Text = "Select any four cards... you have 3 minutes to group them into four groups of four!";
            timeBar.Value = 180;
            timer.Interval = TimeSpan.FromMilliseconds(1.0);
            timer.Tick += Timer_Tick;
            timer.Start();
            live1.Visibility = Visibility.Hidden;
            live2.Visibility = Visibility.Hidden;
            live3.Visibility = Visibility.Hidden;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button thisButton = new Button();
                    thisButton.SetValue(Grid.RowProperty, i);
                    thisButton.SetValue(Grid.ColumnProperty, j);
                    //thisButton.Content = i.ToString() + j.ToString();
                    thisButton.Content = words[wordIndex];
                    //ImageBrush newBrush = new ImageBrush();
                    //newBrush.ImageSource = new BitmapImage(new Uri());
                    thisButton.Background = Brushes.Azure;
                    thisButton.FontSize = 35;
                    //thisButton.
                    //thisButton.SetValue(, TextWrapping.Wrap);
                    thisButton.FontFamily = new FontFamily("Arial Rounded MT Bold");
                    thisButton.Click += ThisButton_Click;
                    thisButton.Name = "Button" + i.ToString() + j.ToString();
                    thisButton.Style = (Style)FindResource("GelButton");
                    gameArea.Children.Add(thisButton);
                    wordIndex++;
                }
            }
        }


        private void clearButtons()
        {
            foreach (var button in ClickedButtons)
            {
                button.Background = Brushes.Azure;
                button.Foreground = Brushes.Black;
            }
            if (correctCounts == 2 && chancesLeft > 2)
            {
                chancesLeft--;
                messageArea.Text = "Oops wrong! Try again! You have " + chancesLeft.ToString() + " chances left";
                if(chancesLeft == 2)
                {
                    live1.Visibility = Visibility.Hidden;
                }
                else
                {
                    live2.Visibility = Visibility.Hidden;
                }
                popUpStatus("WRONG!", 2);
            }
            else if(correctCounts == 2 && chancesLeft == 2)
            {
                chancesLeft--;
                messageArea.Text = "Last chance! You'll lose if this is wrong!";
                popUpStatus("LAST CHANCE!", 2);
                live3.Visibility = Visibility.Hidden;
            }
            else if (correctCounts == 2 && chancesLeft == 1)
            {
                losing("YOU LOSE!", "Sorry, you ran out of chances! Press reset to play again!");
                timer.Tick -= Timer_Tick;
            }
        }

        private void ResetBut_Click(object sender, RoutedEventArgs e)
        {
            timer.Tick -= Timer_Tick;
            restart();
        }

        private void ExitBut_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
