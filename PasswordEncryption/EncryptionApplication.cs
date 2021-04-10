using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasswordEncryption
{
    public static class EncryptionApplication
    {
        private static List<User> _profiles = new List<User>();
        private const string key = "adef@@kfxcbv@";

        public static void StartUp()
        {
            MainScreen();
        }

        private static void MainScreen()
        {
            Console.WriteLine("\nDarius' Password Authentication System\n");
            Console.WriteLine("1. Create account");
            Console.WriteLine("2. Log in with existing Account");
            Console.WriteLine("3. Exit Application");
            Console.Write("\nplease select an option above by entering corresponding value: ");
            try
            {
                var selection = int.Parse(Console.ReadLine() ?? "0");

                switch (selection)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        LogIn();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }

                MainScreen();
            }
            catch (Exception)
            {
                Console.WriteLine("\nBe sure to use the menu numbers to make a selection.\n");

                MainScreen();
            }
        }

        private static void CreateAccount()

        {
            var user = new User();
            var password = "";
            var verificationPassword = "";
            Console.WriteLine("\nCreate a New Account\n");

            do
            {
                Console.Write("\nPlease enter desired username: ");
                user.UserName = Console.ReadLine();
            } while (IsUnique(user.UserName) && !string.IsNullOrEmpty(user.UserName));

            do
            {
                Console.Write("Please enter a password: ");
                password = Console.ReadLine();
                Console.Write("Please Re-enter your password: ");
                verificationPassword = Console.ReadLine();

                if (password != verificationPassword)
                    Console.WriteLine("\nPasswords do not match: Please try again.\n");
            } while (password != verificationPassword && !string.IsNullOrEmpty(password));

            if (password == verificationPassword)
            {
                user.Password = EncryptPassword(password);
                AddUser(user);
                Console.WriteLine(
                    $"\nThank you {user.UserName}, Your account has been Created. Please try to log in via the main menu.\n");
                MainScreen();
            }
        }

        private static void LogIn()
        {
            Console.Write("\nPlease Enter Your Username: ");
            var lUserName = Console.ReadLine();

            Console.Write("\nPlease enter password: ");
            var password = Console.ReadLine();

            if (IsValid(new User {UserName = lUserName, Password = EncryptPassword(password)}))
                Console.WriteLine($"\nYou have successfully logged in as {lUserName}\n");
            else
                Console.WriteLine("\nUsername with that password does not exist in our system.\n");
        }

        private static bool IsValid(User user)
        {
            var exists = _profiles.Exists(p => p.UserName == user.UserName && p.Password == user.Password);

            return exists;
        }

        private static bool IsUnique(string user)
        {
            var exists = _profiles.Exists(p => p.UserName == user);
            if (exists) Console.WriteLine("\nuser name already exists, please input new user name\n");
            return exists;
        }

        private static void AddUser(User newUser)
        {
            _profiles.Add(newUser);
        }

        private static string EncryptPassword(string password)
        {
            password += key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        private static string DecryptPassword(string encryptedPassword)
        {
            var base64EncodeBytes = Convert.FromBase64String(encryptedPassword);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - key.Length);
            return result;
        }
    }
}