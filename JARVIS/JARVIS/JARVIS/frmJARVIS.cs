using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Threading;


namespace JARVIS
{
    public partial class frmJARVIS : Form
    {
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        SpeechRecognitionEngine recognizer;

        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        Thread recordingThread;
        string input;

        public frmJARVIS()
        {
            InitializeComponent();
        }

        private void frmJARVIS_Load(object sender, EventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            Choices choices = new Choices();

            
            choices.Add(new string[] { "red", "Mriya" });

            GrammarBuilder gb = new GrammarBuilder();
            gb.AppendDictation();
            
            //gb.Append(choices);


            Grammar grammar = new Grammar(gb);

            recognizer.LoadGrammar(grammar);

            synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult, 0, System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

            recordingThread = new Thread(new ThreadStart(DetectSpeech));
            recordingThread.Start();

        }

        void DetectSpeech()
        {
            while (true)
            {
                try
                {
                    if (input != "")
                    {
                        recognizer.EmulateRecognize(input);
                        input = "";
                    }
                    recognizer.Recognize();
                }
                catch
                {

                }
                Thread.Sleep(5);

            }
        }

        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        SpeechRecognized(sender, e);
                    }
                    catch { }
                });
            }
            catch { }
            
        }

        void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string phrase = e.Result.Text;
            //phrase = ReplaceNumbers(phrase);
            lstVoice.Items.Add("You: " + phrase);
            lstVoice.SelectedIndex = lstVoice.Items.Count - 1;

            if (e.Result.Text.Contains("hello"))
            {
                Greet(phrase);
            }
            else if (e.Result.Text.Contains("multiply")){
                Multiply(phrase);
            }
            else if (e.Result.Text.Contains("divide")) {
                Divide(phrase);
            }
            else if(phrase.Contains("square root"))
            {
                SquareRoot(phrase);
            }
            else if(phrase.Contains("play") && phrase.Contains("music"))
            {
                PlayRandomMusic(phrase);
            }
            else if (phrase.Contains("pause") && phrase.Contains("music"))
            {
                PauseMusic(phrase);
            }
            else if (phrase.Contains("stop") && phrase.Contains("music"))
            {
                StopMusic(phrase);
            }

        }



        #region Complete

        void Speak(string input)
        {
            lstVoice.Items.Add("Mriya: " + input);
            lstVoice.SelectedIndex = lstVoice.Items.Count - 1;
            synthesizer.Speak(input);
        }

        #region Greetings
        void Greet(string input)
        {
            
            Speak("Hello! Mriya here. Nice to talk to you!");
        }
        #endregion Greetings

        #region Math_Functions


        public double WordsToDouble(string numberString)
        {
            string[] words = numberString.ToLower().Split(new char[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            Dictionary<string, int> modifiers = new Dictionary<string, int>() {
        {"billion", 1000000000},
        {"million", 1000000},
        {"thousand", 1000},
        {"hundred", 100}
    };

            if (numberString == "eleventy billion")
                return int.MaxValue; // 110,000,000,000 is out of range for an int!

            int result = 0;
            int currentResult = 0;
            int lastModifier = 1;

            foreach (string word in words)
            {
                if (modifiers.ContainsKey(word))
                {
                    lastModifier *= modifiers[word];
                }
                else
                {
                    int n;

                    if (lastModifier > 1)
                    {
                        result += currentResult * lastModifier;
                        lastModifier = 1;
                        currentResult = 0;
                    }

                    if ((n = Array.IndexOf(ones, word) + 1) > 0)
                    {
                        currentResult += n;
                    }
                    else if ((n = Array.IndexOf(teens, word) + 1) > 0)
                    {
                        currentResult += n + 10;
                    }
                    else if ((n = Array.IndexOf(tens, word) + 1) > 0)
                    {
                        currentResult += n * 10;
                    }
                    else if (word != "and")
                    {
                        //throw new ApplicationException("Unrecognized word: " + word);
                    }
                }
            }

            return result + currentResult * lastModifier;
        }
    

        void Multiply(string input)
        {
            //Break the string by its spaces
            string[] splitString = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Find the index of the word multiply
            int multiplyIndex = splitString.ToList().IndexOf("multiply");



            //Find the next numbers e.g. "multiply *24* time *38*"
            string firstWord = "";
            string secondWord = "";

            //Grab the strings up to "times"
            for(int i = multiplyIndex + 1; i< splitString.Length; i++)
            {

                if (WordsToDouble(splitString[multiplyIndex + i]) == 0 && splitString[multiplyIndex + i] != "zero") {
                    multiplyIndex += i;
                    break;
                }
                
                firstWord += splitString[multiplyIndex + i] + " ";
                
            }

            //Grab the strings past "times"
            for (int i = multiplyIndex + 1; i < splitString.Length; i++)
            {
                if (WordsToDouble(splitString[i]) == 0 && splitString[i] != "zero")
                {
                    multiplyIndex += i;
                    break;
                }
                secondWord += splitString[i] + " ";
                
            }

            //First find the number after multiply
            double firstNumber = WordsToDouble(firstWord);
            //Now the second number
            double secondNumber = WordsToDouble(secondWord);

            //Finally multiply the numbers together and politely return the result
            double result = firstNumber * secondNumber;
            Speak(firstNumber.ToString() + " times " + secondNumber.ToString() + " equals " + result.ToString());
        }

        void Divide(string input)
        {
            //Break the string by its spaces
            string[] splitString = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Find the index of the word multiply
            int multiplyIndex = splitString.ToList().IndexOf("divide");



            //Find the next numbers e.g. "multiply *24* time *38*"
            string firstWord = "";
            string secondWord = "";

            //Grab the strings up to "times"
            for (int i = multiplyIndex + 1; i < splitString.Length; i++)
            {

                if (WordsToDouble(splitString[multiplyIndex + i]) == 0 && splitString[multiplyIndex + i] != "zero")
                {
                    multiplyIndex += i;
                    break;
                }

                firstWord += splitString[multiplyIndex + i] + " ";

            }

            //Grab the strings past "times"
            for (int i = multiplyIndex + 1; i < splitString.Length; i++)
            {
                if (WordsToDouble(splitString[i]) == 0 && splitString[i] != "zero")
                {
                    multiplyIndex += i;
                    break;
                }
                secondWord += splitString[i] + " ";

            }

            //First find the number after multiply
            double firstNumber = WordsToDouble(firstWord);
            //Now the second number
            double secondNumber = WordsToDouble(secondWord);

            //Finally multiply the numbers together and politely return the result
            double result = firstNumber / secondNumber;
            Speak(firstNumber.ToString() + " divided by " + secondNumber.ToString() + " equals " + result.ToString());
        }

        void SquareRoot(string input)
        {
            //Break the string by its spaces
            string[] splitString = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //Find the index of the word multiply
            int multiplyIndex = splitString.ToList().IndexOf("square");



            //Find the next numbers e.g. "multiply *24* time *38*"
            string firstWord = "";

            

            //Grab the strings past "square root"
            for (int i = multiplyIndex + 3; i < splitString.Length; i++)
            {
                if (WordsToDouble(splitString[i]) == 0 && splitString[i] != "zero")
                {
                    multiplyIndex += i;
                    break;
                }
                firstWord += splitString[i] + " ";

            }

            //First find the number after multiply
            double firstNumber = WordsToDouble(firstWord);

            //Finally multiply the numbers together and politely return the result
            double result = Math.Sqrt(firstNumber);
            Speak("The square root of " + firstNumber.ToString() + " equals " + result.ToString());
        }

        #endregion Math_Functions

        #region File_Operations
        void PlayRandomMusic(string input)
        {
            Speak("Hold on, I'm finding your music");
            DriveInfo driveInfo = new DriveInfo(@"C:\");

            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Users\Michael\");
            

            List<FileInfo> allFiles = new List<FileInfo>();

            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                try
                {
                    FileInfo[] files;
                    files = directory.GetFiles("*.mp3", SearchOption.AllDirectories);
                    allFiles.AddRange(files);
                }
                catch
                {
                    //lstVoice.Items.Add(directory.FullName);
                }
            }

            Random random = new Random();
            
            wplayer.URL = allFiles[random.Next(0, allFiles.Count)].FullName;
            wplayer.controls.play();

            //Speak("Playing Music");

        }

        void PauseMusic(string input)
        {
            if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                wplayer.controls.pause();
            }
            Speak("Pausing Music");

        }

        void StopMusic(string input)
        {
            if(wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                wplayer.controls.stop();
            }
            Speak("Stopping Music");

        }

        #endregion File_Operations

        #endregion Complete

        private void frmJARVIS_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            input = txtInput.Text;
            txtInput.Text = "";
        }
    }
}
