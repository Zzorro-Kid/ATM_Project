#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>

int main() {

    std::vector<std::string> users;
    std::vector<int> pins;
    std::vector<int> balances;

   
    std::ifstream codes_file("codes.txt");

    if (!codes_file.is_open()) {
        std::cerr << "Failed to open codex.txt\n";
        return 1;
    
    }

    std::string line;

    while (std::getline(codes_file, line)) {

        std::istringstream iss(line);
        std::string name;
        int pin;
        if (iss >> name >> pin) {
            users.push_back(name);
            pins.push_back(pin);
            balances.push_back(1000); 
        }
    }

    codes_file.close();

    if (users.empty()) {

        std::cerr << "No users loaded from codex.txt\n";
        return 1;
    }

    int action;
    int deposit_add;
    int deposit_withdraw;
    int pin_code;
    int user_index = -1;

    for (int i = 3; i > 0; i--) {
        std::cout << "Enter PIN: ";
        std::cin >> pin_code;

        for (size_t j = 0; j < pins.size(); j++) {
            if (pin_code == pins[j]) {
                user_index = static_cast<int>(j);
                break;
            }
        }

        if (user_index != -1) {

            std::cout << "\nWelcome, " << users[user_index] << "!\n";
            break;
        } else {

            std::cout << "Wrong PIN! Attempts left: " << i - 1 << "\n";
        }

        if (i == 1) {

            std::cout << "\nAccess Denied. Too many attempts.\n";
            return 0;
        }
    }

    while (true) {

        std::cout << "\n=== ATM Terminal ===\n";
        std::cout << "1. Show balance\n";
        std::cout << "2. Refill account\n";
        std::cout << "3. Withdrawal\n";
        std::cout << "4. Exit\n";
        std::cout << "\nSelect an action: ";
        std::cin >> action;

        switch (action) {
            case 1:
                std::cout << "\nYour balance: $" << balances[user_index] << "\n";
                break;

            case 2:
                std::cout << "\nEnter the recharge amount: ";
                std::cin >> deposit_add;

                if (deposit_add <= 0) {

                    std::cout << "\nWrong amount! Try again.\n";

                } else {

                    balances[user_index] += deposit_add;
                    std::cout << "\nBalance successfully charged!\n";
                }
                break;

            case 3:
                std::cout << "\nEnter the withdrawal amount: ";
                std::cin >> deposit_withdraw;

                if (deposit_withdraw > balances[user_index]) {
                    std::cout << "\nInsufficient funds for withdrawal! Try again.\n";
                } else {

                    balances[user_index] -= deposit_withdraw;
                    std::cout << "\nYou took: $" << deposit_withdraw << "\n";
                    std::cout << "New Balance: $" << balances[user_index] << "\n";
                }
                break;

            case 4:
                std::cout << "\nThank you for using our ATM. Goodbye!\n";
                return 0;

            default:
                std::cout << "\nWrong choice! Try again.\n";
                break;
        }
    }

    return 0;
}
