using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using MoleCC.Exceptions;
using System.Text.RegularExpressions;

namespace MoleCC.Models
{
    public class TrdTranslationParser
    {

        public List<Dictionary<string, string>> ParseStream(StreamReader fileStream)
        {
           string rawText = GetTextFromStream(fileStream);
           return ParseString(rawText);
        }

        private string GetTextFromStream(StreamReader stream)
        {
            return stream.ReadToEnd();
        }

        public List<Dictionary<string,string>> ParseString(string rawText)
        {
            string[] textInLines = rawText.Split('|');
            var resultDictionary = new List<Dictionary<string, string>>();


            for (int i = 0; i < textInLines.Length; i++)
            {
                string[] wordsInLine = textInLines[i].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> lineDictionary = new Dictionary<string, string>();
                for (int j = 0; j < wordsInLine.Length; j++)
                {
                    string[] words = wordsInLine[j].Split('-');
                    if (lineDictionary.ContainsKey(words[0].Trim())) continue;

                    lineDictionary.Add(words[0].Trim(), words[1].Trim());
                }
                resultDictionary.Add(lineDictionary);
            }
            return resultDictionary;
        }
    }
}
