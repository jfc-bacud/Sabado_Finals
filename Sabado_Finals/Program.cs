namespace Sabado_Finals
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // DECLARATION OF THE MATRIX FOR ENCRYPTING AND DECRYPTING

            char[,] alphabetMatrix = new char[26, 26];
            string KeyPass = null;
            string userMessage = null;
            List<char> ProcessedMessage = new List<char>();

            for (int y = 0; y < 26; y++)
            {
                for (int x = 0; x < 26; x++)
                {
                    // BAKIT MAY '(CHAR)'? ITS TO CAST THE VALUE ON THE RIGHT SIDE AND TURN IT INTO A CHARACTER.
                    // BAKIT 65? KASI SA ASCII TABLE YUN NG STARTING POINT NG CHARACTER 'A' (YOU CAN CHANGE PAG GUSTO MO LOWER CASE, BASE KA LANG SA VALUES SA ASCII TABLE)

                    if (y < 1)
                    {
                        alphabetMatrix[y, x] = (char)(65 + x);
                    }

                    else if (y >= 1)
                    {
                        if ((x + y) < 26)
                        {
                            alphabetMatrix[y, x] = (char)(65 + (x + y)); // IT INPUTS CHARACTERS DEPENDING ON THE ROW. SO IF 2ND ROW, IT STARTS WITH 'B' AND CONTINUES ON
                        }
                        else // IN THE CASE WHERE THERE ARE CHARACTERS LEFT DUE TO ABOVE (E.G. IF IT STARTS WITH B, KULANG NG A) THIS HAPPENS
                        {
                            alphabetMatrix[y, x] = (char)((65 + x) - (26 - y)); // ILL BE HONEST, IDFK HOW I DID THIS SHIT IT JUST WORKED IN MY HEAD LMAO
                        }
                    }
                }
            }

            /*
             * FOR TESTING PURPOSES ONLY
            for (int x = 0; x < 26; x++)
            {
                for (int y = 0; y < 26; y++)
                {
                    Console.Write(alphabetMatrix[x, y] + " ");
                }
                Console.WriteLine();
            }*/

            Console.WriteLine("Message Encrypter / Decrypter\n");
            Console.Write("Select Mode:\n");
            Console.Write("[1] Encrypt\n");
            Console.Write("[2] Decrypt\n\n");
            Console.Write("Input: ");

            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.WriteLine("\nSelected Mode: Encryption");
                Console.WriteLine("\nGetting Key Pass...");

                using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\KeyPass.txt")) // READS KEY PASS FILE IN THE REPOS, YOU CAN CHANGE IT IF U LIKE
                {
                    KeyPass = sr.ReadLine().ToUpper();
                }

                List<char> keyChars = new List<char>();
                keyChars = KeyPass.ToList(); // TURNS STRING INTO LIST... I HOPE PWEDE ITO KASI HINDI ITO DINISCUSS

                for (int x = 0; x < keyChars.Count; x++) // FILTERS SPECIAL CHARACTERS
                {
                    int counter = 0;
                    while (counter < 26)
                    {
                        if (keyChars[x] == 65 + counter) // CHECKS IF THE CHARACTER WITHIN THE INDEX FALLS UNDER THE ALPHABETICAL CHARACTERS
                        {
                            break;
                        }
                        counter++;
                    }
                    if (counter < 26)
                    {
                        continue;
                    }
                    else
                    {
                        keyChars.RemoveAt(x); // REMOVES BASICALLY ANY INDEXES WHEREIN THE SPECIAL CHARACTER IS PRESENT
                        x--;
                    }
                }

                Console.Write("\nKey Pass: " + KeyPass);
                Console.WriteLine("\n");

                if (File.Exists(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\UnprocessedMessage.txt")) // READS IF THERE IS A MESSAGE WAITING TO BE DECRYPTED
                {
                    Console.Write("File 'UnprocessedMessage.txt' has been detected");
                    using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\UnprocessedMessage.txt"))
                    {
                        userMessage = sr.ReadLine().ToUpper();
                    }
                }

                else
                {
                    Console.Write("File 'UnprocessedMessage.txt' has not been detected. Moving to manual input...\n"); // ELSE, MANUAL INPUT
                    Console.Write("Input your message: ");
                    Console.WriteLine("\n");

                    using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\UnprocessedMessage.txt")) // SAVED AS THE UNPROCESSED OR "BEFORE ENCRYPTING" MESSAGE
                    {
                        userMessage = Console.ReadLine().ToUpper();
                        sw.Write(userMessage);
                    }
                }

                List<char> messageChars = new List<char>();
                messageChars = userMessage.ToList(); // TURNS FROM STRING INTO LIST

                for (int x = 0; x < messageChars.Count; x++) // FILTERS SPECIAL CHARACTERS
                {
                    int counter = 0;
                    while (counter < 26)
                    {
                        if (messageChars[x] == 65 + counter) // CHECKS IF THE CHARACTER WITHIN THE INDEX FALLS UNDER THE ALPHABETICAL CHARACTERS
                        {
                            break;
                        }
                        counter++;
                    }
                    if (counter < 26)
                    {
                        continue;
                    }
                    else
                    {
                        messageChars.RemoveAt(x); // REMOVES BASICALLY ANY INDEXES WHEREIN THE SPECIAL CHARACTER IS PRESENT
                        x--;
                    }
                }

                // CREATION OF KEY MASK

                List<char> KeyMask = new List<char>();

                while (KeyMask.Count != messageChars.Count) 
                {
                    int IndexCounter = 0;

                    while (IndexCounter < keyChars.Count && KeyMask.Count != messageChars.Count) // WHAT THIS DOES BASICALLY EMANS NA UULITIN NIYA NG KEYPASS PARA GAWIN NG KEY MASK UNTIL UMABOTO SA LENGTH NG MESSAGE
                    {
                        KeyMask.Add(keyChars[IndexCounter]);
                        IndexCounter++;
                    }
                }

                Console.Write("\n\nGenerated Key Mask: "); // DISPLAY
                foreach (char c in KeyMask)
                {
                    Console.Write(c);
                }

                // ENCRYPTION
                for (int x = 0; x < messageChars.Count && x < KeyMask.Count; x++) // HOW THIS WORKS -> BASICALLY HINAHANAP MO LANG NG INDEX NA NAKASPECIFY BY BOTH YINDEX AND XINDEX AND INPUTTING THE CHAR INTO THE LIST, WHICH WILL CREATE THE ENCRYPTION
                {
                    int xIndex = messageChars[x] - 65;
                    int yIndex = KeyMask[x] - 65;

                    ProcessedMessage.Add(alphabetMatrix[xIndex, yIndex]);
                }

                Console.WriteLine();
                Console.Write("Processed Message: "); // DISPLAY
                foreach (char c in ProcessedMessage)
                {
                    Console.Write(c);
                }

                using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\ProcessedMessage.txt")) // SAVES ENCRYPTED MESSAGE INTO A PROCESSED MESSAGE
                {
                    foreach (char c in ProcessedMessage)
                    {
                        sw.Write(c);
                    }
                }

                Console.WriteLine();
            }
            else if (choice == 2) // DECRYPTION
            {
                Console.WriteLine("\nSelected Mode: Decryption");
                Console.WriteLine("\nGetting Key Pass...");

                using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\KeyPass.txt")) // SAME SHIT AS THE ONE ABOVE
                {
                    KeyPass = sr.ReadLine().ToUpper();
                }

                List<char> keyChars = new List<char>();
                keyChars = KeyPass.ToList(); // TURNS FROM STRING INTO LIST

                for (int x = 0; x < keyChars.Count; x++) // FILTERS SPECIAL CHARACTERS
                {
                    int counter = 0;
                    while (counter < 26)
                    {
                        if (keyChars[x] == 65 + counter) // CHECKS IF THE CHARACTER WITHIN THE INDEX FALLS UNDER THE ALPHABETICAL CHARACTERS
                        {
                            break;
                        }
                        counter++;
                    }
                    if (counter < 26)
                    {
                        continue;
                    }
                    else
                    {
                        keyChars.RemoveAt(x); // REMOVES BASICALLY ANY INDEXES WHEREIN THE SPECIAL CHARACTER IS PRESENT
                        x--;
                    }
                }
                Console.Write("\nKey Pass: " + KeyPass);
                Console.WriteLine("\n");

                if (File.Exists(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\ProcessedMessage.txt")) // CHECKS  IF THE PROCESSED MESSAGE THAT WAS ENCRYPTED EARLIER EXISTS (IF USER ENCRYPTED EARLIER)
                {
                    Console.Write("File 'ProcessedMessage.txt' has been detected");
                    using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\ProcessedMessage.txt")) // READS IT 
                    {
                        userMessage = sr.ReadLine().ToUpper();
                    }
                }

                else
                {
                    Console.Write("File 'ProcessedMessage.txt' has not been detected. Moving to manual input...\n"); // ELSE, MANUAL INPUT THEN SAVES IT
                    Console.Write("Input your message: ");
                    Console.WriteLine("\n");

                    using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\UnprocessedMessage.txt"))
                    {
                        userMessage = Console.ReadLine().ToUpper();
                        sw.Write(userMessage);
                    }
                }

                List<char> messageChars = new List<char>();
                messageChars = userMessage.ToList(); // TURNS FROM STRING INTO LIST

                for (int x = 0; x < messageChars.Count; x++) // FILTERS SPECIAL CHARACTERS
                {
                    int counter = 0;
                    while (counter < 26)
                    { 
                        if (messageChars[x] == 65 + counter) // CHECKS IF THE CHARACTER WITHIN THE INDEX FALLS UNDER THE ALPHABETICAL CHARACTERS
                        {
                            break;
                        }
                        counter++;
                    }
                    if (counter < 26)
                    {
                        continue;
                    }
                    else
                    {
                        messageChars.RemoveAt(x); // REMOVES BASICALLY ANY INDEXES WHEREIN THE SPECIAL CHARACTER IS PRESENT
                        x--;
                    }
                }

                // CREATION OF KEY MASK

                List<char> KeyMask = new List<char>();

                while (KeyMask.Count != messageChars.Count)
                {
                    int IndexCounter = 0;

                    while (IndexCounter < keyChars.Count && KeyMask.Count != messageChars.Count)  // WHAT THIS DOES BASICALLY EMANS NA UULITIN NIYA NG KEYPASS PARA GAWIN NG KEY MASK UNTIL UMABOTO SA LENGTH NG MESSAGE
                    {
                        KeyMask.Add(keyChars[IndexCounter]);
                        IndexCounter++;
                    }
                }

                Console.Write("\n\nGenerated Key Mask: ");
                foreach (char c in KeyMask)
                {
                    Console.Write(c);
                }

                // DECRYPTION

                for (int x = 0; x < KeyMask.Count; x++)
                {
                    int IndexCounter = 0;

                    while (IndexCounter < 26)
                    {
                        if (alphabetMatrix[KeyMask[x] - 65, IndexCounter] == messageChars[x]) // BASICALLY, IT JUST SEARCHES FOR THE INDEX WITHIN THE MATRIX USING THE VALUE OF THE CHARACTER WITHIN THE KEY MASK AND THE MESSAGE
                        {
                            ProcessedMessage.Add((char)(IndexCounter + 65));
                        }
                        IndexCounter++;
                    }
                }

                Console.WriteLine();
                Console.Write("Processed Message: "); // DISPLAY
                foreach (char c in ProcessedMessage)
                {
                    Console.Write(c);
                }

                using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\Sabado_Finals\Sabado_Finals\ProcessedMessage.txt"))
                {
                    foreach (char c in ProcessedMessage)
                    {
                        sw.Write(c);
                    }
                }

                Console.WriteLine("\n");
            }
        }
    }
}