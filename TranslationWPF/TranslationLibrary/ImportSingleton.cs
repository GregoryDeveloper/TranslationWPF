using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TranslationLibrary
{
    public sealed class ImportSingleton
    {
        private static ImportSingleton instance = null;

        private ImportSingleton()
        {

        }

        public static ImportSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImportSingleton();
                }
                return instance;
            }
        }

        //private string GetWord(string line)
        //{
        //    string word = "";
        //    string[] equalsSplit = SplitLine(line);

        //    word = ExtractWord(equalsSplit[0]);
        //    return RemoveExtraInformation(word);

        //}

        public (string,string) SplitLine(string line)
        {
            line.Replace("\t", "");
            string[] splitLine =  line.Split('=');

            switch (splitLine.Length)
            {
                case 2:
                    return (splitLine[0], splitLine[1]);
                case 3:
                    if (splitLine[0].Last() == '(')
                        return (splitLine[0] + "=" + splitLine[1], splitLine[2]);
                    else
                        return (splitLine[0], splitLine[1] + "=" + splitLine[2]);
                case 4:
                    return  (splitLine[0]+"="+ splitLine[1], splitLine[2]+"="+ splitLine[3]);
                     
                default:
                    break;

            }
            return ("", "");
        
        }

        //private List<string> GetSynonyms(string line)
        //{
        //    string[] equalsSplit = SplitLine(line);


        //}

        public (string, string) ExtractComment(string line)
        {
            string comment = "";
            string wordLine = "";

            if (!line.Contains("(") || !line.Contains(")")||line.Contains('='))
                return ("",line);

            wordLine=ExtractUntilCaractere(line, 0, '(');
            comment = ExtractUntilCaractere(line, line.IndexOf('(')+1, ')');
            wordLine+=ExtractUntilCaractere(line, line.IndexOf(')') + 1);


            //comment = line.Substring(line.IndexOf('(')+1, line.IndexOf(')') - line.IndexOf('(')-1);
            return (comment, wordLine);
        }

        public (string,string) ExtractExample(string line )
        {
            string[] lines;

            if (!line.Contains("ex:") && !line.Contains("example:") && !line.Contains("exemple:"))
                return ("", line);

            lines = line.Split(':');

            if (!CheckIfExample(lines[0]))
                return ("", line);

            return (lines[1], lines[0].Remove(lines[0].Length - 2));


        }

        private bool CheckIfExample(string line)
        {
            if (line[line.Length-2]=='e'&& line[line.Length - 1] == 'x')
                return true;
            if ( line.Contains("example") || line.Contains("exemple"))
                return true;
            return false;
        }

        private string ExtractUntilCaractere(string line,int index, char? caractere = null)
        {
            StringBuilder sb = new StringBuilder();

            while (index < line.Length && line[index] != caractere)
            {
                sb.Append(line[index]);
                index++;
            }
            return sb.ToString();
        }

        private string RemoveExtraInformation(string line)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;

            while (i < line.Length && line[i] != '(' && line[i] != ')')
            {
                sb.Append(line[i]);
                i++;
            }

            return sb.ToString();
        }

        public (List<string>,string) ExtractSynonyms(string line)
        {
            string wordLine = line;
            List<string> synonyms = new List<string>();
            if (wordLine.Contains("(") || wordLine.Contains(")") || wordLine.Contains('='))
            {
                wordLine = ExtractUntilCaractere(line, 0, '(');
                synonyms.Add(ExtractUntilCaractere(line, line.IndexOf('=')+1,')'));
            }

            if (wordLine.Contains(','))
            {
                string[] words = wordLine.Split(',');
                wordLine = words[0];
                synonyms.AddRange(words.Skip(1));
            }
            
            return (synonyms, wordLine);

        }

        private string ExtractWord(string line)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                
                if (IsWordCharacter(line[i]))             
                    sb.Append(line[i]);
                
            }
            return sb.ToString();
        }

        private bool IsWordCharacter(char letter)
        {
            if ((int)letter >= 'A' && (int)letter <= 'Z')
                return true;
            if ((int)letter >= 'a'&& (int)letter <= 'z')
                return true;
            if (letter =='[' || letter == ']'|| letter== ' ')
                return true;

            return false;
        }

        private bool IsSynonym(string []splitLine,int index)
        {
            if (index == 0)
                return false;
                
            if (splitLine[index - 1].ElementAt(splitLine[index - 1].Length - 1) == '(' || splitLine[index - 1].ElementAt(splitLine[index - 1].Length - 2) == '(')
                return true;
                      
            return false;
        }

        public string RemoveTabs(string line)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {

                if (line[i]!='\t')
                    sb.Append(line[i]);

            }
            return sb.ToString();
        }

       
    }
}
