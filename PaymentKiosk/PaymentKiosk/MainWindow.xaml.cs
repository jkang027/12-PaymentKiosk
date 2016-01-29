using PaymentKiosk.Core.Domain;
using PaymentKiosk.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PaymentKiosk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<StoreMenuItem> checkoutList = new ObservableCollection<StoreMenuItem>();

        private StoreMenuItem currentlySelectedItem = null;

        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = checkoutList;
        }

        public static decimal totalCost;
        private void getTotal()
        {
            totalCost = 0;
            for (var i = 0; i < checkoutList.Count; i++)
            {
                totalCost = totalCost + checkoutList[i].ItemCost;
            }
            textBlockTotalCost.Text = "$" + totalCost;
            textBoxAmount.Text = totalCost.ToString();
        }

        private void buttonCoffee_Click(object sender, RoutedEventArgs e)
        {
            StoreMenuItem newCoffee = new StoreMenuItem
            {
                ItemName = "Coffee",
                ItemCost = 2.99M
            };
            checkoutList.Add(newCoffee);
            getTotal();
        }

        private void buttonMuffin_Click(object sender, RoutedEventArgs e)
        {
            StoreMenuItem newMuffin = new StoreMenuItem
            {
                ItemName = "Muffin",
                ItemCost = 4.99M
            };
            checkoutList.Add(newMuffin);
            getTotal();
        }

        private void buttonCroissant_Click(object sender, RoutedEventArgs e)
        {
            StoreMenuItem newCroissant = new StoreMenuItem
            {
                ItemName = "Croissant",
                ItemCost = 3.99M
            };
            checkoutList.Add(newCroissant);
            getTotal();
        }
        
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentlySelectedItem = (StoreMenuItem)dataGrid.SelectedItem;
        }

        private void buttonDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            checkoutList.Remove(currentlySelectedItem);
            getTotal();
            textBlockTotalCost.Text = "$" + totalCost;
            textBoxAmount.Text = totalCost.ToString();
        }

        private void buttonCheckout_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer
            {
                FirstName = textBoxFirstName.Text,
                LastName = textBoxLastName.Text,
                PhoneNumber = textBoxTelephone.Text
            };

            var creditCard = new CreditCard
            {
                CardNumber = passwordBox.Password,
                Cvc = textBoxCvc.Text,
                ExpirationDate = DateTime.Parse(textBoxExpiration.Text)
            };

            try
            {
                bool success = TransactionService.Charge(customer, creditCard, decimal.Parse(textBoxAmount.Text));
                if (success)
                {
                    MessageBox.Show("Transaction successful.");
                    var message = "\n" + "Your transaction was successful for the amount $" + decimal.Parse(textBoxAmount.Text);
                    SendSms.SendMessage(textBoxTelephone.Text, message);
                    textBoxAmount.Clear();
                    textBoxCvc.Clear();
                    textBoxExpiration.Clear();
                    textBoxFirstName.Clear();
                    textBoxLastName.Clear();
                    textBoxTelephone.Clear();
                    passwordBox.Clear();
                }
                else
                {
                    MessageBox.Show("Transaction unsuccessful.");
                    var message = "\n" + "Your transaction was unsuccessful.";
                    SendSms.SendMessage(textBoxTelephone.Text, message);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                var message = "\n" + "Your transaction was unsuccessful. \n" + exception.Message;
                SendSms.SendMessage(textBoxTelephone.Text, message);
            }
        }
    }
}
