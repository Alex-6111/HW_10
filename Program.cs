using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HW_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SearchFile();
        }

        public static void SearchFile()
        {
            Console.Write("Enter directory path: ");

            string path = Console.ReadLine();  

            
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Catalog not found!");
                Console.ReadKey();
                return;
            }

            
            Console.Write("Enter a mask to search for files:");

            string mask = Console.ReadLine();  

             
            if (!Regex.IsMatch(mask, @"^[\w\.\*\?]+$"))
            {
                Console.WriteLine("Invalid mask format!");
                Console.ReadKey();
                return;
            }

            
            Console.Write("Enter the start date of the search range for files in the format (dd.mm.yyyy):");
            DateTime dateStart;
            try
            {
                dateStart = DateTime.Parse(Console.ReadLine());  
                 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

             
            Console.Write("Enter the date of the end of the range to search for files in the format (dd.mm.yyyy): ");
            DateTime dateEnd;
            try
            {
                dateEnd = DateTime.Parse(Console.ReadLine());  
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
             if (dateEnd < dateStart)
            {
                Console.WriteLine("The end date of the range must be greater than the start date of the range.");
                Console.ReadKey();
                return;
            }

             
            string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories)
                .Where(f => File.GetLastWriteTime(f) >= dateStart && File.GetLastWriteTime(f) <= dateEnd)
                .ToArray();

            string reportFile = Path.Combine(path, "report.txt");
            StreamWriter sw = new StreamWriter(reportFile, false, Encoding.UTF8);

             
            bool firstMatсh = true;
            if (files.Length == 0)
            {
                Console.WriteLine("Files not found.");
            }
            else
            {
                Console.WriteLine("Found files:");
                foreach (string file in files)
                {
                     
                    Console.WriteLine(file);

          
                    if (firstMatсh)
                    {
                        sw.WriteLine($"Finding files in a directory: {path}");
                        sw.WriteLine($"Mask requested file: {mask}");
                        sw.WriteLine($"Range start date for file search: {dateStart}");
                        sw.WriteLine($"Date of the end of the range for searching files:{dateEnd}");
                        sw.WriteLine();
                        sw.WriteLine("Found files:");
                    }
                    sw.WriteLine(file);
                    firstMatсh = false;
                }
            }

           
            Console.Write("\nEnter text to search files: ");
            string text;
            try
            {
                text = Console.ReadLine();  
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                text = "";
            }
          
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("Search text cannot be empty.");
                Console.ReadKey();
                return;
            }
          
            firstMatсh = true;
            foreach (string file in files)
            {
                 
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                     
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains(text))
                        {
                           
                            if (firstMatсh)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Found text files\"{text}\":");
                            }
                            Console.WriteLine(file);

                            
                            if (firstMatсh)
                            {
                                sw.WriteLine();
                                sw.WriteLine($"Found text files\"{text}\":");
                                firstMatсh = false;
                            }
                            sw.WriteLine(file);
                            break;
                        }
                    }
                }
            }
            sw.Close();
        
        }

        public static void SearchAndDelete()
        {
            Console.Write("Enter directory path:");

            string path = Console.ReadLine();  
     
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Catalog not found.");
                Console.ReadKey();
                return;
            }

            
            Console.Write("Enter a mask to search for files:");

            string mask = Console.ReadLine();  
          

             if (!Regex.IsMatch(mask, @"^[\w\.\*\?]+$"))
            {
                Console.WriteLine("Invalid mask format.");
                Console.ReadKey();
                return;
            }

 
            string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories);

             if (files.Length == 0)
            {
                Console.WriteLine("Files not found.");
            }
            else
            {
                Console.WriteLine("Found files:");
                for (var index = 0; index < files.Length; index++)
                {
                     
                    Console.WriteLine($"{index}\t{files[index]}");
                }
            }

            if (files.Length == 0)
            {
                Console.WriteLine("The search returned no results, nothing to delete.");
            }
            else
            {
                 Console.WriteLine("\nEnter the file number to delete (-1 - delete all found files):");
                string input = Console.ReadLine();

                 if (!Regex.IsMatch(input, @"^-?\d+$"))
                {
                    Console.WriteLine("Invalid input format.");
                    Console.ReadKey();
                    return;
                }

                 if (input == "-1")
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                     int index = int.Parse(input);
                    if (index >= 0 && index < files.Length)
                    {
                        File.Delete(files[index]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid file number.");
                        Console.ReadKey();
                        return;
                    }
                }
            }
        
        }
    }
}
