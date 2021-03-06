﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Commands
{
    public class ConsoleCommand
    {
        #region Private Variables

        private List<string> _arguments;

        #endregion

        #region Constructors

        public ConsoleCommand(string input)
        {
            this._arguments = new List<string>();
            // Ugly regex to split string on spaces, but preserve quoted text intact:
            var stringArray = Regex.Split(
                                          input,
                                          "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            for (int i = 0; i < stringArray.Length; i++)
            {
                // The first element is always the command:
                if (i == 0)
                {
                    this.Name = stringArray[i];

                    // Set the default:
                    this.LibraryClassName = "CommandPresenter";
                    string[] s = stringArray[0].Split('.');

                    if (s.Length == 2)
                    {
                        this.LibraryClassName = s[0];
                        this.Name = s[1];
                    }
                }
                else
                {
                    var inputArgument = stringArray[i];
                    string argument = inputArgument;

                    // Is the argument a quoted text string?
                    var regex = new Regex("\"(.*?)\"", RegexOptions.Singleline);
                    var match = regex.Match(inputArgument);

                    if (match.Captures.Count > 0)
                    {
                        // Get the unquoted text:
                        var captureQuotedText = new Regex("[^\"]*[^\"]");
                        var quoted = captureQuotedText.Match(match.Captures[0].Value);
                        argument = quoted.Captures[0].Value;
                    }

                    this._arguments.Add(argument);
                }
            }
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public string LibraryClassName { get; set; }

        public IEnumerable<string> Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}