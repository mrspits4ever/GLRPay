﻿using NFC7;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GLRPay_OplaadStation.pages
{
    /// <summary>
    /// Interaction logic for InitPinPage.xaml
    /// </summary>
    public partial class InitPinPage : Page
    {
        GLRPayCard card;

        public InitPinPage()
        {
            InitializeComponent();
            card = new GLRPayCard();
            card.HasCardInserted += Card_HasCardInserted;
        }

        private void Card_HasCardInserted(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //check of er iets in de blokken staat
                if (txtProductName.Text.Length == 0 || txtProductprice.Text.Length == 0) 
                    return;

                Frame ViewFrame = (Frame)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).FindName("PageViewer");

                //check of de kaart leeg is en of er iets staat in de general id locatie
                if (!card.CardValid) 
                    ViewFrame.Content = new NewCardPage();
                else 
                    ViewFrame.Content = new PinPage(new Product(float.Parse(txtProductprice.Text), txtProductName.Text), card);
            }));
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String))) {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!char.IsDigit(text, text.Length - 1)) {
                    e.CancelCommand();
                }
            }
            else {
                e.CancelCommand();
            }
        }


    }
}
