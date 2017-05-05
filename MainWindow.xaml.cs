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
using System.Windows.Navigation;
using System.Windows.Shapes;
using STS_Web_App_Mockup.Scripts;
using System.IO;
using System.Windows.Threading;

namespace STS_Web_App_Mockup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NodeManager slides;
        string correctAnswer = "A";
        bool canTransition = false;
        TimeSpan time;
        DispatcherTimer timer;

        private int timerLength = 0;
        private string Slidefolder = "\\Slides\\";

        public MainWindow()
        {
            slides = new NodeManager();
            InitializeComponent();
            for(int i = 0; i < Directory.GetFiles(Directory.GetCurrentDirectory() + Slidefolder).Length; i++)
            {
                Node n = new Scripts.Node("File " + i, null, null, "Slide " + i, NodeType.Text);
                string[] slideData = slides.generateNodeData(i);
                n.writeToData(slideData);
                slides.AddNode(n);
            }

            time = TimeSpan.FromSeconds(timerLength);
            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (time == TimeSpan.Zero)
                {
                    timer.Stop();
                    if(slides.currentNode().type != NodeType.Question)
                        canTransition = true;
                    Next.Content = "NEXT ->";

                }
                time = time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            timer.Start();

            constructMenu();

            slides.transition(true);
            switchScreens();
        }

        private void switchScreens()
        {
            switch (slides.currentNode().type)
            {
                case NodeType.Text:
                    TextContainer.Visibility = Visibility.Visible;
                    ImageContainer.Visibility = Visibility.Collapsed;
                    QuestionContainer.Visibility = Visibility.Collapsed;
                    MenuContainer.Visibility = Visibility.Collapsed;
                    OnlyImageContainer.Visibility = Visibility.Collapsed;

                    MainText.Text = slides.currentNode().readData()[0];
                    Title.Content = slides.currentNode().nodeName;
                    Next.Content = "WAIT";

                    time = TimeSpan.FromSeconds(slides.currentNode().timer);
                    timer.Start();

                    break;
                case NodeType.Image:
                    TextContainer.Visibility = Visibility.Collapsed;
                    ImageContainer.Visibility = Visibility.Visible;
                    QuestionContainer.Visibility = Visibility.Collapsed;
                    MenuContainer.Visibility = Visibility.Collapsed;
                    OnlyImageContainer.Visibility = Visibility.Collapsed;

                    ImageText.Text = slides.currentNode().readData()[0];
                    Title.Content = slides.currentNode().nodeName;

                    string imagePath = Directory.GetCurrentDirectory() + "\\Images\\" + slides.currentNode().readData()[1];
                    SlideImage.Source = new ImageSourceConverter().ConvertFromString(imagePath) as ImageSource;

                    Next.Content = "WAIT";

                    time = TimeSpan.FromSeconds(slides.currentNode().timer);
                    timer.Start();

                    break;
                case NodeType.Question:
                    TextContainer.Visibility = Visibility.Collapsed;
                    ImageContainer.Visibility = Visibility.Collapsed;
                    QuestionContainer.Visibility = Visibility.Visible;
                    MenuContainer.Visibility = Visibility.Collapsed;
                    OnlyImageContainer.Visibility = Visibility.Collapsed;

                    A.IsChecked = false;
                    B.IsChecked = false;
                    C.IsChecked = false;
                    D.IsChecked = false;

                    question.Text = slides.currentNode().readData()[0];
                    Title.Content = slides.currentNode().nodeName;

                    A.Content = slides.currentNode().readData()[2];
                    B.Content = slides.currentNode().readData()[3];
                    C.Content = slides.currentNode().readData()[4];
                    D.Content = slides.currentNode().readData()[5];
                    correctAnswer = slides.currentNode().readData()[6];

                    Next.Content = "WAIT";

                    time = TimeSpan.FromSeconds(slides.currentNode().timer);
                    timer.Start();

                    break;
                case NodeType.Menu:
                    TextContainer.Visibility = Visibility.Collapsed;
                    ImageContainer.Visibility = Visibility.Collapsed;
                    QuestionContainer.Visibility = Visibility.Collapsed;
                    MenuContainer.Visibility = Visibility.Visible;
                    OnlyImageContainer.Visibility = Visibility.Collapsed;
                    Title.Content = slides.currentNode().nodeName;
                    canTransition = true;

                    break;
                case NodeType.OnlyImage:
                    TextContainer.Visibility = Visibility.Collapsed;
                    ImageContainer.Visibility = Visibility.Collapsed;
                    QuestionContainer.Visibility = Visibility.Collapsed;
                    MenuContainer.Visibility = Visibility.Collapsed;
                    OnlyImageContainer.Visibility = Visibility.Visible;

                    Title.Content = slides.currentNode().nodeName;

                    string imagePathO = Directory.GetCurrentDirectory() + "\\Images\\" + slides.currentNode().readData()[1];
                    OnlyImageContainer.Source = new ImageSourceConverter().ConvertFromString(imagePathO) as ImageSource;

                    Next.Content = "WAIT";

                    time = TimeSpan.FromSeconds(slides.currentNode().timer);
                    timer.Start();

                    break;

                default:
                    Console.WriteLine("How does one even get here? Enums are pretty solid in structure...");
                    break;
            }
        }

        private void constructMenu()
        {
            Console.WriteLine(Directory.GetFiles(Directory.GetCurrentDirectory() + Slidefolder).Length);
            for(int i = 0; i < Directory.GetFiles(Directory.GetCurrentDirectory() + Slidefolder).Length;i++) //I hate local Directory Management
            {
                if (i != 0)
                {
                    Console.WriteLine("FILE: " + Directory.GetFiles(Directory.GetCurrentDirectory() + Slidefolder)[i]);
                    Button btn = new Button();
                    btn.Content = slides.slideN(i).nodeName;
                    btn.Tag = i;
                    btn.Click += new RoutedEventHandler(MenuBtn_Click);
                    btn.Margin = new Thickness(3);
                    MenuStack.Children.Add(btn);
                }

            }
        }



        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (canTransition)
            {
                slides.transition(true);
                canTransition = false;
                switchScreens();
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            slides.transition(false);
            canTransition = false;
            switchScreens();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            slides.goToHead();
            canTransition = true;
            switchScreens();
            time = TimeSpan.Zero;
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            switch(correctAnswer)
            {
                case "A":
                    if((bool)A.IsChecked)
                    {
                        MessageBox.Show("Correct!");
                        canTransition = true;
                    }
                    else
                    {
                        MessageBox.Show("Sorry. That is incorrect.");
                    }
                    break;
                case "B":
                    if ((bool)B.IsChecked)
                    {
                        MessageBox.Show("Correct!");
                        canTransition = true;
                    }
                    else
                    {
                        MessageBox.Show("Sorry. That is incorrect.");
                    }
                    break;
                case "C":
                    if ((bool)C.IsChecked)
                    {
                        MessageBox.Show("Correct!");
                        canTransition = true;
                    }
                    else
                    {
                        MessageBox.Show("Sorry. That is incorrect.");
                    }
                    break;
                case "D":
                    if ((bool)D.IsChecked)
                    {
                        MessageBox.Show("Correct!");
                        canTransition = true;
                    }
                    else
                    {
                        MessageBox.Show("Sorry. That is incorrect.");
                    }
                    break;
                default:
                    MessageBox.Show("There appears to be an error.");
                    canTransition = true;
                    break;
            }
        }

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Button for Slide # " + (sender as Button).Tag);
            slides.goToN((string)(sender as Button).Content);
            switchScreens();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
