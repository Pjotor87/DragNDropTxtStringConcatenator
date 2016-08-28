using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DragNDropTxtStringConcatenator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
                try
                {
                    foreach (string filePath in args)
                        if (!string.IsNullOrEmpty(filePath) && Path.GetExtension(filePath) == ".txt")
                            if (File.Exists(filePath))
                            {
                                Console.WriteLine(string.Format("Concatenating file: {0}", filePath));
                                string[] linesInFile = File.ReadAllLines(filePath, Encoding.Default);

                                if (linesInFile != null)
                                {
                                    IList<string> concatenatedStrings = new List<string>();
                                    int firstWhiteSpaceIndex_Or_SectionLength = -1;

                                    {// GET FIRST SECTION
                                        for (int i = 0; i < linesInFile.Length; i++)
                                            if (!string.IsNullOrEmpty(linesInFile[i]))
                                                concatenatedStrings.Add(linesInFile[i]);
                                            else
                                            {
                                                firstWhiteSpaceIndex_Or_SectionLength = i;
                                                break;
                                            }
                                    }
                                    {// GET ALL OTHER SECTIONS
                                        int currentSection = 2;
                                        for (int i = firstWhiteSpaceIndex_Or_SectionLength + 1; i < linesInFile.Length; i++)
                                            if (!string.IsNullOrEmpty(linesInFile[i]))
                                                concatenatedStrings[i % (firstWhiteSpaceIndex_Or_SectionLength + 1)] += linesInFile[i];
                                            else
                                                currentSection++;
                                    }
                                    {// WRITE CONCATENATED STRING FILE
                                        if (firstWhiteSpaceIndex_Or_SectionLength > -1 && 
                                            concatenatedStrings.Count > 0)
                                            try
                                            {
                                                File.WriteAllLines(Path.ChangeExtension(filePath, "CONCAT.txt"), concatenatedStrings, Encoding.Default);
                                            }
                                            catch { throw; }
                                    }
                                }
                            }
                            else
                                throw new FileNotFoundException(string.Format("No file at path: {0} does not exist.", filePath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
        }
    }
}