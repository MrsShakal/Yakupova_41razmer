using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
using System.Windows.Threading;

namespace Yakupova_41razmer
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
       
        private int time = 0;
        public DispatcherTimer timer = new DispatcherTimer();

        private bool needCap = false;
        private string cap = null;

        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PassTB.Text;

            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            User user = Yakupova41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {
                if (!needCap)
                {
                    Manager.MainFrame.Navigate(new ProductPage(user));
                    LoginTB.Text = "";
                    PassTB.Text = "";
                    CapTextBox.Text = "";
                    StackCAPTCHA.Visibility = Visibility.Hidden;
                    CapTextBox.Visibility = Visibility.Hidden;
                    


                    needCap = false;
                    LoginBtn.IsEnabled = true;
                }
                else
                {
                    
                    if (cap == CapTextBox.Text)
                    {
                        Manager.MainFrame.Navigate(new ProductPage(user));
                        LoginTB.Text = "";
                        PassTB.Text = "";
                        CapTextBox.Text = "";
                        StackCAPTCHA.Visibility = Visibility.Hidden;
                        CapTextBox.Visibility = Visibility.Hidden;
                        needCap = false;
                        LoginBtn.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Капча введена неправильно");
                        LoginBtn.IsEnabled = false;
                        time = 10;
                        RandGenCap();
                        TimerSeconds();
                    }
                }
            }
            else
            {
                if (needCap)
                {
                    RandGenCap();
                    MessageBox.Show("Введены неверные данные");
                    LoginBtn.IsEnabled = false;
                    time = 10;
                    TimerSeconds();
                }
                else
                {
                    MessageBox.Show("Введены неверные данные");
                    RandGenCap();
                    needCap = true;
                    StackCAPTCHA.Visibility = Visibility.Visible;
                    CapTextBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));

            LoginTB.Text = "";
            PassTB.Text = "";
            CapTextBox.Text = "";
            StackCAPTCHA.Visibility = Visibility.Hidden;
            CapTextBox.Visibility = Visibility.Hidden;
            


            needCap = false;
            LoginBtn.IsEnabled = true;
        }

        private void TimerSeconds()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (time > 0)
                time--;
            else
            {
                timer.Stop();
                LoginBtn.IsEnabled = true;
            }
        }

        public const string k = "123456789qwertyuopasdfghklzxcvbnm";
        private void RandGenCap()
        {
            Random random = new Random();
            captchaOneWord.Text = k[random.Next(0, k.Length)].ToString();
            captchaTwoWord.Text = k[random.Next(0, k.Length)].ToString();
            captchaThreeWord.Text = k[random.Next(0, k.Length)].ToString();
            captchaFourWord.Text = k[random.Next(0, k.Length)].ToString();

            cap = null;
            cap = captchaOneWord.Text + captchaTwoWord.Text + captchaThreeWord.Text + captchaFourWord.Text;
        }
    }
}

