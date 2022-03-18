using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {

        Blockchain blockchain;
        String privKey;
        String publicKey;
        Wallet.Wallet myWallet;


        public BlockchainApp()
        {
            InitializeComponent();

            blockchain = new Blockchain();
            richTextBox1.Text = "New Blockchain Initialised!";


            myWallet = new Wallet.Wallet(out privKey);
            publicKey = myWallet.publicID;

            textBox3.Text = privKey;
            textBox2.Text = publicKey;
            passwordText.Text = privKey;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
        }



        private void printButton_Click(object sender, EventArgs e)
        {
            int blockIndex = 0;

            if (Int32.TryParse(textBox1.Text, out blockIndex))
            {
                richTextBox1.Text = blockchain.printInfo(blockIndex);
            }
        }
        private void printAllButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.printInfo();
        }


        private void createWalletButton_Click(object sender, EventArgs e)
        {
            myWallet = new Wallet.Wallet(out privKey);
            publicKey = myWallet.publicID;

            textBox3.Text = privKey;
            textBox2.Text = publicKey;
            passwordText.Text = privKey;
        }

        

        private void createTransactionButton_Click(object sender, EventArgs e)
        {
            float fee = 0;
            float amount = 0;
            string targetKey = "";
            string password = "";


            float.TryParse(textBox6.Text, out fee);
            float.TryParse(textBox5.Text, out amount);
            targetKey = textBox4.Text;
            password = passwordText.Text;

            if ((password.Length > 0) && (amount > 0) && (targetKey.Length > 0))
            {
                Transaction newTransaction = new Transaction(amount, fee, targetKey, publicKey, password);
                richTextBox1.Text = blockchain.addTransaction(newTransaction).ToString();
            }
            else {
                richTextBox1.Text = "Error: Please fill in 'To', 'Amount' and 'Password'.";
            }
        }

        private void mineBlockButton_Click(object sender, EventArgs e)
        {
            Block newBlock = new Block(blockchain.getLastBlock(), blockchain.selectTransactions(), publicKey);
            blockchain.addBlock(newBlock);
            richTextBox1.Text = "Added block: \n" + blockchain.getLastBlock().getIndex().ToString();
        }
        
        private void printTransactionsButton_Click(object sender, EventArgs e)
        {
            String info = "";
            foreach (Transaction transaction in blockchain.getTransactions())
            {
                info += transaction.getInfo() + "\n\n";
            }
            richTextBox1.Text = info;
        }
        

        private void validateChainButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.validate().ToString();
        }
        private void validateWalletButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = Wallet.Wallet.ValidatePrivateKey(privKey, publicKey).ToString();
        }



        private void checkBalanceButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.getBalance(publicKey).ToString();
        }

        private void fakeButton_Click(object sender, EventArgs e)
        {
            Block newBlock = new Block(blockchain.getLastBlock(), blockchain.selectTransactions(), publicKey);
            newBlock.setHash("Hello");
            blockchain.addBlock(newBlock);
            richTextBox1.Text = "Added block: \n" + blockchain.getLastBlock().getIndex().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Block newBlock = new Block(blockchain.getLastBlock(), blockchain.selectTransactions(), publicKey);
            newBlock.setPreviousHash("Hello");
            blockchain.addBlock(newBlock);
            richTextBox1.Text = "Added block: \n" + blockchain.getLastBlock().getIndex().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Transaction> fakeTransactions = new List<Transaction>();

            fakeTransactions.Add(new Transaction(1, 1, "Hello", "Hey", "Hi"));

            Block newBlock = new Block(blockchain.getLastBlock(), fakeTransactions, publicKey);
            blockchain.addBlock(newBlock);
            richTextBox1.Text = "Added block: \n" + blockchain.getLastBlock().getIndex().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            blockchain = new Blockchain();
            richTextBox1.Text = "New Blockchain Initialised!";
        }
    }
}