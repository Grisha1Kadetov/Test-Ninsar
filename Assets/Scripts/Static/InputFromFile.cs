using UnityEngine;
using File = System.IO.File;

namespace TestNinsar
{
    public static class InputFromFile
    {
        public static readonly string Path =  "/Resources/input.txt";
        
        public static string GetInput()
        {
            return File.ReadAllText(Application.dataPath+Path);
        }

        public static void SetInput(string input)
        { 
            File.WriteAllText(Application.dataPath+Path,input);
        }
    }
}