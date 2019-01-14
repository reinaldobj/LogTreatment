﻿using System;

namespace LogTreatment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello");
                Console.WriteLine("Enter the url of the log to download and map");
                string url = Console.ReadLine();

                Mapping mapping = new Mapping(url);
                string filename = mapping.MapMinhaCdnToAgora();

                Console.WriteLine($"The mapped log was saved as {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}
