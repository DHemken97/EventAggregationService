using System;
using System.Collections.Generic;
using System.Linq;

namespace EAS_Development_Interfaces.Models
{
    public class CommandElements
    {
        public CommandElements(string Command)
        {
            DoubleFlags = new Dictionary<string, string>();
            Flags = new List<string>();
            Arguments = new List<string>();

            var isCommand = true;
            var inQuotes = false;
            var inFlag = false;
            var inDoubleFlag = false;
            var inDoubleFlagKey = false;
            var constructedValue = string.Empty;
            var constructedKey = string.Empty;

            foreach (var chr in Command)
            {
                switch (chr)
                {
                    case '"':
                        inQuotes = !inQuotes;
                        break;
                    case '-' when (!inQuotes):

                        if (inFlag)
                        {
                            inDoubleFlag = true;
                            inFlag = false;
                            inDoubleFlagKey = true;
                        }

                        else
                            inFlag = true;
                        break;
                    case ':' when (!inQuotes && inDoubleFlagKey):
                        inDoubleFlagKey = false;
                        break;


                    case ' ':
                        if (inQuotes)
                        {
                            if (inDoubleFlagKey)
                                constructedKey += chr;
                            else
                                constructedValue += chr;
                        }
                        else
                        { //end of arg
                            AddValue(constructedValue,constructedKey,isCommand,inFlag,inDoubleFlag);

                            //reset
                            constructedValue = string.Empty;
                            constructedKey = string.Empty;
                            isCommand = false;
                            inDoubleFlagKey = false;
                            inDoubleFlag = false;
                            inFlag = false;   
                        }
                        break;
                    default:
                  
                            if (inDoubleFlagKey)
                                constructedKey += chr;
                            else
                                constructedValue += chr;
                        
                        break;
                         


                }
            }

            AddValue(constructedValue, constructedKey, isCommand, inFlag, inDoubleFlag);


        }

        private void AddValue(string constructedValue,string constructedKey,bool isCommand,bool inFlag,bool inDoubleFlag)
        {
            if (string.IsNullOrWhiteSpace(constructedValue)) return;
            if (isCommand)
                command = constructedValue;
            else if (inFlag)
                Flags.Add(constructedValue);
            else if (inDoubleFlag)
                DoubleFlags.Add(constructedKey, constructedValue);
            else Arguments.Add(constructedValue);
        }

        public string command { get; private set; }
        public List<string> Arguments { get; }
        public List<string> Flags { get; }
        public Dictionary<string, string> DoubleFlags { get; set; }
        public string ToFormattedString(bool removeCommand = false)
        {
            var cmd = removeCommand ? string.Empty : command;
            cmd += $" {string.Join(" ", Arguments.Select(a => $"\"{a}\""))}";
            cmd += $" {string.Join(" ", Flags.Select(f => $"-{f}"))}";
            cmd += $" {string.Join(" ", DoubleFlags.Select(f => $"--{f.Key}:{f.Value}"))}";

            cmd = cmd.Trim();
            while (cmd.IndexOf("  ") > -1)
                cmd.Replace("  ", " ");

            return cmd;
        }
    }
}
