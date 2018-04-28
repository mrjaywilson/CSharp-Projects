namespace Client
{
    class ROT13
    {
        /// <summary>
        /// Method to convert string with Rotating 13 cypher
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Transform(string message)
        {
            // Store each character of message in array
            char[] array = message.ToCharArray();

            // Iterate through array
            for (int i = 0; i < array.Length; i++)
            {
                // Get number of letter
                int letterNumber = (int)array[i];

                // Check number of letter for conversion
                if (letterNumber >= 'a' && letterNumber <= 'z')
                {
                    if (letterNumber > 'm')
                    {
                        // If the letter is greater than 'm' subtract 13
                        letterNumber -= 13;
                    }
                    else
                    {
                        // If not, add 13
                        letterNumber += 13;
                    }
                }
                else if (letterNumber >= 'A' && letterNumber <= 'Z') // Also check caps
                {
                    // If the capital letter is 'M', subtract 13
                    if (letterNumber > 'M')
                    {
                        letterNumber -= 13;
                    }
                    else
                    {
                        // If not, add 13
                        letterNumber += 13;
                    }
                }

                // Cast number to Character and store in array
                array[i] = (char)letterNumber;
            }

            // Return string from array.
            return new string(array);
        }
    }
}
