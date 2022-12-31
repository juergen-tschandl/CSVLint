﻿using System;
using System.ComponentModel;
using CsvQuery.PluginInfrastructure;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Manages application settings
    /// </summary>
    public class Settings : SettingsBase
    {
        [Description("Maximum unique values when reporting or detecting coded values, if column contains more than it's not reported."),
            Category("Analyze"), DefaultValue(15)]
        public int UniqueValuesMax { get; set; }

        //[Description("Maximum rows to analyze to automatically detect data types. Set to 0 to analyze all rows, set to 1000 for better performance with large files."),
        //    Category("Analyze"), DefaultValue(0)]
        //public int ScanRows { get; set; }

        [Description("Maximum amount of digits for integer values, if a value has more then it's considered a text value. Applies to both autodetecting datatypes and validating data. Useful to distinguish (bar)codes and actual numeric values."),
            Category("Analyze"), DefaultValue(12)]
        public int IntegerDigitsMax { get; set; }

        [Description("Maximum amount of decimals for decimal values, if a value has more then it's considered a text value. Applies to both autodetecting datatypes and validating data."),
            Category("Analyze"), DefaultValue(20)]
        public int DecimalDigitsMax { get; set; }

        [Description("Decimal values must have leading zero, set to false to accept values like .5 or .01"),
            Category("Analyze"), DefaultValue(true)]
        public bool DecimalLeadingZero { get; set; }

        private int _ErrorTolerance;
        public float _ErrorTolerancePerc;

        [Description("Error tolerance percentage, when analyzing allow X % errors. For example when a column with a 1000 values contains all integers except for 9 or fewer non-integer values, then it's still interpreted as an integer column."), Category("Analyze"), DefaultValue(1)]
        public int ErrorTolerance
        {
            get
            {
                return this._ErrorTolerance;
            }
            set
            {
                // set string representation, may include "SysYear"
                this._ErrorTolerance = value;
                this._ErrorTolerancePerc = (float)0.01 * value;
            }
        }

        [Description("When detecting or validating date or datetime values, years smaller than this value will be considered as out-of-range."),
            Category("Analyze"), DefaultValue(1900)]
        public int YearMinimum { get; set; }

        [Description("When detecting or validating date or datetime values, years larger than this value will be considered as out-of-range."),
            Category("Analyze"), DefaultValue(2050)]
        public int YearMaximum { get; set; }

        [Description("Maximum year for two digit year date values. For example, when set to 2024 the year values 24 and 25 will be interpreted as 2024 and 1925. Set as SysYear for current year."),
            Category("Edit"), DefaultValue("SysYear")]
        public string TwoDigitYearMax
        {
            get
            {
                return this._strTwoDigitYearMax;
            }
            set
            {
                // set string representation, may include "SysYear"
                this._strTwoDigitYearMax = this.CheckYearString(value);
                // set actual integer value
                this.intTwoDigitYearMax = this.GetYearFromString(value);
            }
        }

        // actual year value as int
        public int intTwoDigitYearMax;
        private string _strTwoDigitYearMax;

        [Description("Comment character, the first lines starting with this character will be skipped"), Category("General"), DefaultValue('#')]
        public char CommentCharacter { get; set; }

        //[Description("Decimal values remove leading zero, for example output 0.5 as .5"), Category("Edit"), DefaultValue(false)]
        //public bool DecimalLeadingZeroOut { get; set; }

        [Description("Default quote escape character when quotes exists inside text"),
            Category("General"), DefaultValue('"')]
        public char DefaultQuoteChar { get; set; }

        //[Description("Default font for text boxes in CSV Lint docking window. Changing this requires closing and opening the CSV docked window."), Category("General"), DefaultValue("Courier, 11.25pt")]
        //public string Font { get; set; }

        private const string FontDockDefault = "Courier New, 11.25pt";

        [Description("Default font for text boxes in CSV Lint docking window. Changing the font requires closing and opening the CSV docked window."), Category("General"), DefaultValue(typeof(Font), FontDockDefault)]
        public Font FontDock { get; set; }

        [Description("Keyword for empty values or null values in the csv data, case-sensitive."),
            Category("General"), DefaultValue("NaN")]
        public string NullValue { get; set; }

        [Description("Include separator in syntax highlighting colors. Set to false and the separator characters are not colored."),
            Category("General"), DefaultValue(false)]
        public bool SeparatorColor { get; set; }

        [Description("Preferred characters when automatically detecting the separator character. For special characters like tab, use \\t or \\u0009."),
            Category("General"), DefaultValue(",;\\t|")]
        public string Separators
        {
            get
            {
                return this._strSeparators;
            }
            set
            {
                // set string representation, may include "SysYear"
                this._strSeparators = value;
                if (this._strSeparators.Trim() == "") this._strSeparators = ",;\\t|"; // default

                // clear characters list
                this._charSeparators = "";

                // set actual character values
                // replace escaped special characters, example "\t" "\x1b" etc.
                Regex ItemRegex = new Regex(@"(\\u[0-9a-fA-F]{4}|\\x[0-9a-fA-F]{2}|\\[trnae]|.)", RegexOptions.Compiled);
                foreach (Match ItemMatch in ItemRegex.Matches(this._strSeparators))
                {
                    char c = '\0';
                    if (ItemMatch.Length > 1)
                    {
                        string ltr = ItemMatch.ToString().Substring(1, 1);

                        // check for default characters
                        if (ltr == "t") c = '\t'; // tab
                        if (ltr == "r") c = '\r'; // carriage return
                        if (ltr == "n") c = '\n'; // line feed
                        if (ltr == "a") c = '\a'; // bell
                        if (ltr == "e") c = '\u0027'; // escape

                        // check for u0009 or x09 etc.
                        if ((ltr == "u") || (ltr == "x") )
                        {
                            string hexnr = ItemMatch.ToString().Substring(2, ItemMatch.Length - 2);
                            int number = Convert.ToInt32(hexnr, 16);
                            c = Convert.ToChar(number);
                        }
                    }
                    else
                    {
                        c = ItemMatch.ToString()[0];
                    }
                    // add to list
                    if (c != '\0')
                    {
                        this._charSeparators += c;
                    }
                }
            }
        }
        // actual year value as int
        public string _charSeparators;
        private string _strSeparators;

        [Description("Transparent cursor line, changing this setting will require a restart of Notepad++."), Category("General"), DefaultValue(true)]
        public bool TransparentCursor { get; set; }

        [Description("Trim values before analyzing or editing, only applies to csv because for fixed width data it is always trimmed internally."),
            Category("General"), DefaultValue(true)]
        public bool TrimValues { get; set; }

        [Description("Detect columns automatically, uncheck this option to manually set the column separator and header names parameters before detecting column datatypes."), Category("General"), DefaultValue(true)]
        public bool AutoDetectColumns { get; set; }

        //[Description("Maximum errors output, limit errors logging, or 0 for no limit."),
        //    Category("Validate"), DefaultValue(0)]
        //public int MaxErrors { get; set; }

        //[Description("Month abbreviations for detecting or generating date format 'mmmm', comma separated list of 12 names."),
        //    Category("Validate"), DefaultValue("jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec")]
        //public String MonthAbbrev { get; set; }

        // SORT user preferences
        [Description("Sort data, selected column name."), Category("UserPref"), DefaultValue("")]
        public string SortColName { get; set; }

        [Description("Sort data, sort ascending."), Category("UserPref"), DefaultValue(true)]
        public bool SortAscending { get; set; }

        // SPLIT COLUMN user preferences
        [Description("Split column, selected column name."), Category("UserPref"), DefaultValue("")]
        public string SplitColName { get; set; }

        [Description("Split column, selected option."), Category("UserPref"), DefaultValue(0)]
        public int SplitOption { get; set; }

        [Description("Edit column, pad with character."), Category("UserPref"), DefaultValue("0")]
        public string EditColPad { get; set; }

        [Description("Edit column, pad with character count."), Category("UserPref"), DefaultValue(10)]
        public int EditColPadLength { get; set; }

        [Description("Edit column, search for value."), Category("UserPref"), DefaultValue("abc")]
        public string EditColSearch { get; set; }

        [Description("Edit colun, replace with value."), Category("UserPref"), DefaultValue("xyz")]
        public string EditColReplace { get; set; }

        [Description("Split column, split on character."), Category("UserPref"), DefaultValue("/")]
        public string SplitChar { get; set; }

        [Description("Split column, split on position."), Category("UserPref"), DefaultValue(3)]
        public int SplitPos { get; set; }

        [Description("Split column, remove original column."), Category("UserPref"), DefaultValue(false)]
        public bool SplitRemoveOrg { get; set; }

        // REFORMAT user preferences
        [Description("Reformat dataset, checkbox options."), Category("UserPref"), DefaultValue("")]
        public string ReformatOptions { get; set; }

        [Description("Reformat dataset, date format."), Category("UserPref"), DefaultValue("yyyy-MM-dd")]
        public string ReformatDateFormat { get; set; }

        [Description("Reformat dataset, decimal separator."), Category("UserPref"), DefaultValue(".")]
        public string ReformatDecSep { get; set; }

        [Description("Reformat dataset, column separator."), Category("UserPref"), DefaultValue(";")]
        public string ReformatColSep { get; set; }

        [Description("Reformat dataset, apply quotes option."), Category("UserPref"), DefaultValue(0)]
        public int ReformatQuotes { get; set; }

        [Description("Reformat dataset, replace carriage return line feeds with this string value."), Category("UserPref"), DefaultValue("<br>")]
        public string ReformatReplaceCrLf { get; set; }

        // DETECT COLUMNS user preferences
        [Description("Detect columns, contains separator character."), Category("UserPref"), DefaultValue(",")]
        public string DetectColumnSep { get; set; }

        [Description("Detect columns, fixed width column positions, absolute positions and comma separated."), Category("UserPref"), DefaultValue("")]
        public string DetectColumnWidths { get; set; }

        [Description("Detect columns, contains header names."), Category("UserPref"), DefaultValue(false)]
        public bool DetectColumnHeader { get; set; }

        [Description("Detect columns, skip lines."), Category("UserPref"), DefaultValue(0)]
        public int DetectSkipLines { get; set; }

        // COUNT UNIQUE user preferences
        [Description("Count unique values, list of selected columns."), Category("UserPref"), DefaultValue("")]
        public string UniqueColumns { get; set; }

        [Description("Count unique values, sort result."), Category("UserPref"), DefaultValue(true)]
        public bool UniqueSortBy { get; set; }

        [Description("Count unique values, sort by value or count."), Category("UserPref"), DefaultValue(false)]
        public bool UniqueSortValue { get; set; }

        [Description("Count unique values, sort ascending or descending."), Category("UserPref"), DefaultValue(false)]
        public bool UniqueSortAsc { get; set; }

        [Description("Data convert, convert to type."), Category("UserPref"), DefaultValue(0)]
        public int DataConvertType { get; set; }

        [Description("Data convert, SQL tablename or XML tag name."), Category("UserPref"), DefaultValue("")]
        public string DataConvertName { get; set; }

        private int _sqlbatch;

        [Description("Maximum records per SQL insert batch, minimum batch size is 10."),
            Category("UserPref"), DefaultValue(1000)]
        public int DataConvertBatch
        {
            get { return _sqlbatch; }
            set
            {
                _sqlbatch = Math.Max(value, 10);
            }
        }

        [Description("Convert data to SQL, database type mySQL, MS-SQL or PostgreSQL (0, 1 or 2)."), Category("UserPref"), DefaultValue(0)]
        public int DataConvertSQL { get; set; }

        [Description("Metadata generate type."), Category("UserPref"), DefaultValue(0)]
        public int MetadataType { get; set; }

        // helper function for "SysYear" as year values
        private int GetYearFromString(string yr)
        {
            // test if it's a valid int
            int.TryParse(yr, out int ret);

            // replace SysYear with current year, for example 2020
            if ((ret == 0) || (yr.ToLower() == "sysyear"))
            {
                ret = DateTime.Now.Year;
            };

            return ret;
        }

        private string CheckYearString(string yr)
        {
            string ret = yr.Trim();

            // test if it's a valid int
            int.TryParse(ret, out int test);

            // replace SysYear with current year, for example 2020
            if ((test == 0) || (yr.ToLower() == "sysyear"))
            {
                ret = "SysYear"; // default value
            };
        
            return ret;
        }
    }
}
