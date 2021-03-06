﻿using NFC7;
using System;

namespace GLRPay_OplaadStation
{
    public class GLRPayCard
    {
        const string ValidGeneralId = "0a8a35a8fe49788a";

        private NFC7handler NFCreader;

        public string Identifier { get; private set; }
        public bool CardValid { get; private set; } = false;

        public event EventHandler HasCardInserted;

        private void CardWasInserted(EventArgs arg)
        {
            HasCardInserted?.Invoke(this, arg);
        }

        public GLRPayCard()
        {
            NFCreader = new NFC7handler();
            NFCreader.CardInsertEvent += NFCreader_CardInsertEvent;
        }

        private void NFCreader_CardInsertEvent(object sender, EventArgs args)
        {
            //er is een nieuwe kaart gevonden
            //check of de kaart correct is
            CardValid = CheckValid();
            //zet de unique identifier van deze kaart
            Identifier = NFCreader.UID;
            //laat iedereen weten dat deze kaart gevuld is
            CardWasInserted(EventArgs.Empty);
            //zet de unique identifier van deze kaart
            NFCreader.CardInsertEvent -= NFCreader_CardInsertEvent;
        }

        public bool CheckValid()
        {
            string cardId = StringConverter(NFCreader.ReadBlockText(1, 0));
            if (string.IsNullOrWhiteSpace(cardId)) {
                return false;
            }

            return Validate(cardId);
        }

        // convert a stringarray to a single string
        private string StringConverter(string[] StringArray)
        {
            string FullString = "";
            foreach (string element in StringArray) {
                FullString += element;
            }
            return FullString;
        }

        // function to check if card is supported
        private bool Validate(string GeneralID)
        {
            if (GeneralID == ValidGeneralId) {
                return true;
            }
            else { return false; }
        }
    }
}