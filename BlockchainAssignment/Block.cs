using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Block
    {
        private DateTime timestamp;
        private int index;
        private string myHash;
        private string previousHash;
        private List<Transaction> transactionList = new List<Transaction>();


        private int nonce = 0;
        private float difficultyThreshold = 4;
        private float reward = 1;
        private string merkleRoot;

        public Block(Block lastBlock, List<Transaction> transactions, string publicKey) {
            index = lastBlock.getIndex() + 1;
            previousHash = lastBlock.getHash();
            timestamp = DateTime.Now;
            transactionList = transactions;

            calculateRewards();
            Transaction rewardTransaction = new Transaction((reward), 0, publicKey, "Mine Rewards", "");
            transactionList.Add(rewardTransaction);
            merkleRoot = createMerkleRoot();
            
            myHash = mine();
    }

    public Block() {
            index = 0;
            previousHash = "";
            timestamp = DateTime.Now;
            merkleRoot = createMerkleRoot();
            myHash = mine();
        }

        
        public string createHash() {
            SHA256 hasher;
            hasher = SHA256Managed.Create();
            String input = index.ToString() + timestamp.ToString() + previousHash + nonce.ToString() + difficultyThreshold.ToString() + reward.ToString() + merkleRoot;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes((input)));

            String hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }
            return hash;
        }


        private string mine() {

            string hash = createHash();

            string start = new string('0',  (int)difficultyThreshold);

            while (!hash.StartsWith(start)) {
                nonce = nonce + 1;
                hash = createHash();
            }
           return hash;
        }
        
        public string createMerkleRoot() {

            List<string> hashes = new List<string>();

            foreach (Transaction t in transactionList) {
                hashes.Add(t.getHash());
            }

            if (hashes.Count() == 0)
            {
                return String.Empty;
            }
            else if (hashes.Count() == 1)
            {
                return hashes[0];
            }
            while (hashes.Count() != 1)
            {
                List<string> leaves = new List<string>();
                for (int i = 0; i < hashes.Count(); i = i + 2)
                {
                    if (hashes.Count()-1 == i)
                    {
                        leaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i]));
                    }
                    else
                    {
                        leaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i+1]));
                    }
                }

                hashes = leaves;
            }
            return hashes[0];
        }
        
        private void calculateRewards()
        {

            foreach (Transaction t in transactionList)
            {
                reward += t.getFee();
            }
        }
        
        public string getInfo() {

            string info = "Block index: " + index
                + "\nTimestamp: " + timestamp
                + "\nPrevious hash: " + previousHash
                + "\nHash: " + myHash
                + "\nNonce: " + nonce
                + "\nDifficulty: " + difficultyThreshold
                + "\nRewards: " + reward
                + "\nMerkle Root: " + merkleRoot;
            
            foreach (Transaction t in transactionList)
            {
                info += "\n\n" + t.getInfo();
            }
            return info;
        }

        public string getMerkleRoot()
        {
            return merkleRoot;
        }

        public float getReward()
        {
            return reward;
        }

        public List<Transaction> getTransactions()
        {
            return transactionList;
        }

        public string getHash()
        {
            return myHash;
        }
        public void setHash(string newHash)
        {
            myHash = newHash;
        }

        public string getPreviousHash()
        {
            return previousHash;
        }
        public void setPreviousHash(string newHash)
        {
            previousHash = newHash;
        }
        public int getIndex()
        {
            return index;
        }



    }
}
