using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MechanicShop.Services
{
    public static class Validate
    {
        // This class is for input validation for the various
        // entries in the GUI, This way I only need to write the code
        // Once
        public static string Name(string input) //Chloe
        {
            if (input == null || input == "") { return ""; }
            // Regular expression pattern to allow only alphabetic characters, space, hyphen, apostrophe, and period
            string pattern = @"^[a-zA-ZÀ-ÿ\s'\-\.\,]+$";
            // Check if the entered text matches the pattern
            if (!Regex.IsMatch(input, pattern))
            {
                // If the entered text contains disallowed characters, remove them
                var newText = Regex.Replace(input, @"[^a-zA-ZÀ-ÿ\s'\-\.\,]", "");
                // Update the entry's text with the sanitized text
                return newText;
            }
            return input;
        }


        public static string Phone(string input) //Chloe
        {
            if (input == null) { return ""; }
            // Remove non-digit characters
            var newText = new string(input.Where(char.IsDigit).ToArray());
            // Limit maximum length to 10 digits
            if (newText.Length > 10)
            {
                newText = newText.Substring(0, 10);
            }
            // Automatically insert dashes
            if (newText.Length > 3)
            {
                newText = newText.Insert(3, "-");
                if (newText.Length > 7)
                {
                    newText = newText.Insert(7, "-");
                }
            }
            return newText;
        }

        public static string VIN(string input) //Chloe
        {
            if (input == null || input == "") { return ""; }
            // Convert the entered text to uppercase
            string newText = input.ToUpper();
            // Ensure that the entered text contains only numbers and capital letters
            string validCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string filteredText = "";
            foreach (char c in newText)
            {
                if (validCharacters.Contains(c))
                {
                    filteredText += c;
                }
            }
            // Update the entry's text with the filtered text
            return filteredText;
        }

        public static string Description(string input) //Chloe
        {
            if (string.IsNullOrEmpty(input)) { return ""; }
            // Regular expression pattern to match disallowed characters
            string pattern = @"[^a-zA-ZÀ-ÿ\s'\-.,\d]";
            // Check if the entered text contains disallowed characters
            if (Regex.IsMatch(input, pattern))
            {
                // If disallowed characters are found, remove them
                var newText = Regex.Replace(input, pattern, "");
                // Update the entry's text with the sanitized text
                return newText;
            }
            return input;
        }

        public static string Currency(string input) //Chloe
        {
            if (input == null) { return ""; }
            // Remove non-digit characters
            var newText = new string(input.Where(char.IsDigit).ToArray());
            // Limit maximum length to 5 digits
            if (newText.Length > 5)
            {
                newText = newText.Substring(0, 5);
            }
            // Automatically insert decimal
            if (newText.Length >= 3 && newText.Length <= 5 && newText.IndexOf('.') == -1)
            {
                newText = newText.Insert(newText.Length - 2, ".");
            }
            return newText;
        }
    }

    
}
