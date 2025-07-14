namespace ATMTerminal
{
    public class User
    {
        public string Name { get; }
        public int Pin { get; }
        public int Balance { get; private set; }

        public User(string name, int pin, int initialBalance)
        {
            Name = name;
            Pin = pin;
            Balance = initialBalance;
        }

        public void Deposit(int amount)
        {
            if (amount > 0)
            {
                Balance += amount;
            }
        }

        public bool Withdraw(int amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }
}