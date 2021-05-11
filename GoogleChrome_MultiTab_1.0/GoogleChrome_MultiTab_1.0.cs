// Chrome Multi-Tab Script version 1.0
// Open a random number of tabs in Chrome and select a website from a list at random to open
// Scroll up and down through the page
// Uses loginVSI standard template website / pages 

using LoginPI.Engine.ScriptBase;
using System.Text.RegularExpressions;

public class Chrome_MultiTab : ScriptBase
{
    //Static Random Object
    static System.Random decisionrnd = new System.Random();
    
    private void Execute()
    {           
        // Define environment variable to use with Workload
        var temp = GetEnvironmentVariable("TEMP");       
        
        // Download the VSIwebsite.zip from the appliance and unzip in the %temp% folder
        if(!(DirectoryExists($"{temp}\\LoginPI\\vsiwebsite")))
        {
            Log("Downloading File");
            CopyFile(KnownFiles.WebSite, $"{temp}\\LoginPI\\vsiwebsite.zip");
            UnzipFile($"{temp}\\LoginPI\\vsiwebsite.zip", $"{temp}\\LoginPI\\vsiwebsite");
        }
        else
        {
            Log("File already exists");
        }
        
        //Generate an array of websites to be used        
        string[] allWebsites = 
            {
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\website\\index.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\index.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\logonpage.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\articlepage.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\website\\index.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\index.html",                
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\logonpage.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\articlepage.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\website\\index.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\index.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\logonpage.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\videopage.html",
                $"{temp}\\LoginPI\\vsiwebsite\\ChromeScript\\articlepage.html"
            };       
        
        //How many tabs should be launch
        int minimumTabs = 1;
        int maximumTabs = 10;
        
        // Start Chrome
        START();   
        
        //Select the number of tabs that will be opened and log it
        int randomTabs = decisionrnd.Next(minimumTabs,maximumTabs);
        Log($"Number of tabs selected to be opened - {randomTabs}");
        int x = minimumTabs;
        while( x != randomTabs ) {       
            //Open a new tab
            MainWindow.FindControl(className : "Button", title : "New Tab").Click();
            
            //Select the address bar
            MainWindow.FindControl(className : "Edit", title : "Address and search bar").Click();
            
            //Highlight the text
            MainWindow.Type("{CTRL+A}".Repeat(2));
            
            //Type the random website address
            string websiteSelected = Random_Website(allWebsites);
            MainWindow.Type(websiteSelected);
            
            //Write it out to the screen
            System.Console.WriteLine(Random_Website(allWebsites));            
            MainWindow.Type("{ENTER}");
            
            //Navigate through the tab
            MainWindow.MoveMouseToCenter();        
            MainWindow.Type("{PAGEDOWN}".Repeat(5));
            MainWindow.Type("{PAGEUP}".Repeat(5));
            
            //If its the video page, just wait a while and let it play
            if ((Regex.Match(websiteSelected,"videopage",RegexOptions.IgnoreCase).Success)) {
                Log("Its a video page so we're going to watch it");
                Wait(30);
            }
            
            //Wait for a random number of seconds
            int waitTime = decisionrnd.Next(1,20);
            Wait(waitTime); 
            
            Log($"Random wait time - {waitTime}");
            
            //Incremend the loop counter
            x++;
        } 

        // Stop the browser
        Wait(2);
        STOP();

    }
    
    //Private function for selecting a random website from a string array of websites
    private string Random_Website(string [] allWebsites)
    {            
        //Generate a random number and pick out that string from the array   
        int randomDecision = decisionrnd.Next(0,((allWebsites.Length)-1));
        string website = (allWebsites[randomDecision].ToString());
        
        //Return the website selected
        return website;
    }
}
