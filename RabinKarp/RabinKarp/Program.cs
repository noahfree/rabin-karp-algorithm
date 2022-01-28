using System;
using System.Collections.Generic;

namespace RabinKarp
{
    class Program
    {
        // RUNNING TIME: O(m + n)
        // n = the length of the constitution
        // m = the length of the pattern
        static void Main(string[] args)
        {

            // create a list to hold the output indices
            List<int> output = new List<int>();

            // initialize the initial text string to ""
            string text = "";
            char[] letters;
            // foreach loop reads in the text from the constitution.txt file, line by line & character by character
            foreach (string line in System.IO.File.ReadLines("constitution.txt"))
            {
                // letters are converted to lowercase
                letters = line.ToLower().ToCharArray();
                foreach (char letter in letters)
                {
                    text += letter;
                }
            }

            // while loop ensure pattern length is greater than 0
            string pattern;
            while (true)
            {
                // prompts the user to input a pattern
                Console.WriteLine("\nPlease input a pattern: ");
                pattern = Console.ReadLine().ToLower();

                if (pattern.Length > 0) break; 
            }

            // calculate hash value for the pattern:
            // each pattern's hash is equal to (char ascii value)*26^n-1 + (char ascii value)*26^n-2 + ... + (char ascii value)*26^0
            double key = 0;
            int counter = pattern.Length - 1;
            foreach (char letter in pattern)
            {
                key += (int)letter * Math.Pow(26, counter);
                counter--;
            }

            // calculate hash value for the first pattern.Length characters in text
            // the same hash formula as above ((char ascii value)*26^n-1 + (char ascii value)*26^n-2 + ... + (char ascii value)*26^0) is used
            double sum = 0;
            counter = pattern.Length - 1;
            for (int i = 0; i < text.Length; i++)
            {
                sum += (int)text[i] * Math.Pow(26, counter);
                if (counter == 0) break;
                else counter--;
            }

            // start at i = 1 since we already computed the hash for characters 0-(pattern.Length-1) in the text
            //
            // for every iteration of the for loop, the current hash value for the text section is modified in order to subtact the 
            // leftmost character and add the next character in the text to the section being tested
            //
            // this can be done by first subtracting (char ascii value)*26^(n-1)
            // next, multiply the hash value by 10 in order to increment the exponents on 26
            // finally, add the ascii value of the next character in the text to complete the shift
            for (int i = 1; text.Length >= i + pattern.Length; i++)
            {
                // for each iteration, the key (which is the hash value of the pattern) is compared to the sum (which is the hash value
                // for the current section of text being compared)
                // 
                // if the hash values are equal, a Verify() function verifies that the pattern has been found, and the index is added to the
                // output array
                if (key == sum && Verify(pattern, text, i-1))
                {
                        output.Add(i - 1);
                }

                // if the the text length is equal to the inrementer (i) + the pattern length, then we have reached the end of the text
                if (text.Length == i + pattern.Length) break;

                // the following 3 statements modify the existing hash value for the text to remove the leftmost character and add the next character
                // 
                // as described above, the logic is as follows:
                //
                // for example, for a string of all the same character, such as AAAAAAAAAAAAAA, this is true:
                // (hash) = ((hash)-((ascii value)*(26^n-1)))*26 + ((ascii value)*(26^0))
                // 
                // now consider the string ABCDEFGHI; suppose "old hash" is ABC's hash value, and new hash is BCD's hash value:
                // (new hash) = ((old hash)-((A ascii value)*(26^n-1)))*26 + ((D ascii value)*(26^0))
                sum -= (int)text[i-1] * Math.Pow(26, pattern.Length - 1);
                sum *= 26;
                sum += (int)text[i - 1 + pattern.Length];
            }

            // the following lines are used to generate the output
            Console.WriteLine("\nPattern has " + output.Count + " instances: ");
            foreach (int element in output)
            {
                string str = "   Index " + element + " -> ";
                // the for loop adds the pattern's surrounding characters to the output string 
                for (int i = element - 30; i < element + 30 + pattern.Length && i < text.Length; i++)
                {
                    if (i < 0) continue;
                    str += text[i];
                }
                Console.WriteLine(str);
            }
            Console.WriteLine("\nTotal instances found: " + output.Count + "\n");

            // user is prompted to run the program again
            Console.WriteLine("\nWould you like to run the program again? (y/n)");
            try
            {
                // user input read from the console
                char toggle = Char.Parse(Console.ReadLine());
                // input must be 'y' or 'Y' to run again
                if (toggle == 'y' || toggle == 'Y')
                {
                    Console.WriteLine();
                    Main(args);
                }
            }
            catch (Exception e)
            {

            }
        }

        // function Verify() simply verifies that the found pattern in the text is truly the same as the actual pattern
        static bool Verify(string pattern, string text, int index)
        {
            // if statement ensures that the pattern comes after a space
            if (index != 0 && Char.IsLetter(text[index - 1])) return false;
            // a simple foreach() loop compares the letter in pattern to the next character in the text
            foreach (char letter in pattern)
            {
                // if any characters don't match, false is returned
                if (letter != text[index]) return false;
                // the inputted index is incremented
                index++;
            }

            // in order for the word match to be correct, the next value must not be a letter
            return !Char.IsLetter(text[index]);
        }
    }

}
