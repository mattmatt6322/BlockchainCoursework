using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Transaction
    {
        private string hash;
        private string signature;
        private string senderAddress;
        private string recipientAddress;
        private DateTime timestamp;
        private float amount;
        private float fee;
        string name = "JustANumber";


        public Transaction(float Amount, float Fee, string RecipientAdress, string SenderAdress, string SendersKey) {
            senderAddress = SenderAdress;
            recipientAddress = RecipientAdress;
            amount = Amount;
            fee = Fee;
            timestamp = DateTime.Now;
            hash = createHash();
            signature = Wallet.Wallet.CreateSignature(senderAddress, SendersKey, hash);
        }
        

        private string createHash() {

            SHA256 hasher;
            hasher = SHA256Managed.Create();
            String input = amount.ToString() + timestamp.ToString() + fee.ToString() + senderAddress + recipientAddress;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes((input)));

            String hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }
            return hash;
        }

        public string getInfo(){
            string info = "TRANSACTION " + hash
                + "\nSignature: " + signature
                + "\nTimestamp: " + timestamp
                + "\nTo: " + recipientAddress
                + "\nFrom: " + senderAddress
                + "\nAmount: " + amount + " " + name
                + "\nFee: " + fee;
            return info;
        }

        public float getFee()
        {
            return fee;
        }
        public float getAmount()
        {
            return amount;
        }
        public float getAmountPlusFee()
        {
            return amount+fee;
        }
        public string getHash()
        {
            return hash;
        }
        public string getRecipient()
        {
            return recipientAddress;
        }
        public string getSender()
        {
            return senderAddress;
        }
        public string getSignature()
        {
            return signature;
        }


    }
}
