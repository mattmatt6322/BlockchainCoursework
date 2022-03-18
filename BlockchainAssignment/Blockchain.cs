using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        private List<Block> blocks = new List<Block>();
        private List<Transaction> transactionPool = new List<Transaction>();
        private int transactionsPerBlock = 5;

        public Blockchain() {
            Block genesisBlock = new Block();
            blocks.Add(genesisBlock);
        }
        
        public void addBlock(Block block)
        {
            blocks.Add(block);
        }

        public bool addTransaction(Transaction transaction) {
            if (validateTransaction(transaction) == true)
            {
                transactionPool.Add(transaction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Transaction> selectTransactions() {
            int n = Math.Min(transactionPool.Count, transactionsPerBlock);
            List<Transaction> chosenTransactions = transactionPool.GetRange(0, n);
            transactionPool = transactionPool.Except(chosenTransactions).ToList();
            return chosenTransactions;
        }
    
        public bool validate() {

            bool validation = true;
            string previousHash = "";

            foreach (Block block in blocks)
            {
                
                if (validation == true)
                {

                    validation = validateContiguity(block, previousHash) 
                        && validateMerkleRoot(block) 
                        && validateHash(block)
                        && validateTransactions(block.getTransactions());
                    
                    previousHash = block.getHash();
                }
                else
                {
                    validation = false;
                    return validation;
                }
            }
            return validation;
        }
        

        public float getBalance(string publicKey)
        {
            float balance = 0;

            foreach (Block block in blocks)
            {
                List<Transaction> transactionList = block.getTransactions();
                foreach (Transaction transaction in transactionList)
                {
                    if (transaction.getRecipient() == publicKey)
                    {
                        balance += transaction.getAmount();
                    }
                    if (transaction.getSender() == publicKey)
                    {
                        balance -= (transaction.getAmount() + transaction.getFee());
                    }
                }
            }
            foreach (Transaction transaction in transactionPool)
            {
                if (transaction.getRecipient() == publicKey)
                {
                    balance += transaction.getAmount();
                }
                if (transaction.getSender() == publicKey)
                {
                    balance -= (transaction.getAmount() + transaction.getFee());
                }

               
            }
            return balance;
        }

        public Block getLastBlock()
        {
            return blocks[blocks.Count - 1];
        }

        public string printInfo()
        {
            string info = "";
            foreach (Block block in blocks)
            {
                info += block.getInfo() + "\n\n\n";
            }
            return info;
        }
        public string printInfo(int index)
        {
            string info = "";

            if (index < blocks.Count && index >= 0)
            {
                info = blocks[index].getInfo();
            }
            else
            {
                info = "Error";
            }
            return info;
        }


        private bool validateTransaction(Transaction transaction)
        {
            if (transaction.getSender() != "Mine Rewards")
            {
                if (validateSignature(transaction) == true)
                {

                    if (getBalance(transaction.getSender()) >= (transaction.getAmount() + transaction.getFee()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        private bool validateTransactions(List<Transaction> transactions)
        {
            foreach(Transaction transacion in transactions)
            {
                if (validateTransaction(transacion)==false)
                {
                    return false;
                }
            }
            return true;
        }
        private bool validateContiguity(Block block, string previousHash)
        {
            return block.getPreviousHash().Equals(previousHash);
        }
        private bool validateMerkleRoot(Block block)
        {
            return block.createMerkleRoot().Equals(block.getMerkleRoot());
        }
        private bool validateHash(Block block)
        {
            return block.createHash().Equals(block.getHash());
        }
        private bool validateTransactions(Block block)
        {
            return true;
        }
        private bool validateSignature(Transaction transaction)
        {
            return Wallet.Wallet.ValidateSignature(transaction.getSender(), transaction.getHash(), transaction.getSignature());
        }

        public List<Transaction> getTransactions()
        {
            return transactionPool;
        }
    }
}
