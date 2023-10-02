using Microsoft.AspNetCore.Routing.Constraints;
using System.IO;
using System.Reflection;

public static class Logger{

    
    public static void WriteToLog(string content){

        string s = System.Reflection.Assembly.GetEntryAssembly().Location;

        string cwd = System.IO.Path.GetDirectoryName(s);

        using(StreamWriter sw = File.AppendText(System.IO.Path.Combine(cwd,"log.txt"))){

            sw.WriteLine(content);

        }
        Console.WriteLine(content);
        
    }


}