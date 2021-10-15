﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Kbg.NppPluginNET;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace NppPluginNET.PluginInfrastructure
{
    internal static class ILexer
    {
        public static readonly string Name = "CSVLint\0";
        public static readonly string StatusText = "CSV Linter and validator\0";

        public static char separatorChar = ';';
        public static List<int> fixedWidths;

        // Properties
        static readonly Dictionary<string, bool> SupportedProperties = new Dictionary<string, bool>
        {
            { "fold", true},
            { "fold.compact", false},
            { "separatorcolor", false},
            { "separator", false}
    };
        static readonly Dictionary<string, string> PropertyDescription = new Dictionary<string, string>
        {
            { "fold", "Enable or disable the folding functionality."},
            { "fold.compact", "If set to 0 closing tag is visible when collapsed else hidden." },
            { "separatorcolor", "Include separator in syntax highlighting colors." },
            { "separator", "Separator character for syntax highlighting csv data"}
        };
        static readonly Dictionary<string, int> PropertyTypes = new Dictionary<string, int>
        {
            { "fold", (int)SciMsg.SC_TYPE_BOOLEAN},
            { "fold.compact", (int)SciMsg.SC_TYPE_BOOLEAN },
            { "separatorcolor", (int)SciMsg.SC_TYPE_BOOLEAN },
            { "separator", (int)SciMsg.SC_TYPE_STRING }
        };

        // Styles
        static List<string> NamedStylesList = new List<string> { "SCE_CSVLINT_DEFAULT" };
        static int NamedStylesListCount = NamedStylesList.Count;
        static List<string> TagsOfStyleList = new List<string> { "default" };
        static List<string> DescriptionOfStyleList = new List<string> { "Default style" };

        // 1. since cpp defines these as interfaces, ILexer and IDocument, with virtual functions, 
        //      there is an implicit first parameter, the class instance
        // 2. according to c# documentation delegates are used to simulate function pointers

        #region IDocument
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentVersion(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void IDocumentSetErrorStatus(IntPtr instance, int status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr IDocumentLength(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void IDocumentGetCharRange(IntPtr instance, IntPtr buffer, IntPtr position, IntPtr lengthRetrieve);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate char IDocumentStyleAt(IntPtr instance, IntPtr position);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr IDocumentLineFromPosition(IntPtr instance, IntPtr position);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr IDocumentLineStart(IntPtr instance, IntPtr line);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentGetLevel(IntPtr instance, IntPtr line);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentSetLevel(IntPtr instance, IntPtr line, int level);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentGetLineState(IntPtr instance, IntPtr line);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentSetLineState(IntPtr instance, IntPtr line, int state);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void IDocumentStartStyling(IntPtr instance, IntPtr position);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool IDocumentSetStyleFor(IntPtr instance, IntPtr length, char style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool IDocumentSetStyles(IntPtr instance, IntPtr length, IntPtr styles);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void IDocumentDecorationSetCurrentIndicator(IntPtr instance, int indicator);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void IDocumentDecorationFillRange(IntPtr instance, IntPtr position, int value, IntPtr fillLength);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void IDocumentChangeLexerState(IntPtr instance, IntPtr start, IntPtr end);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentCodePage(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool IDocumentIsDBCSLeadByte(IntPtr instance, char ch);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr IDocumentBufferPointer(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentGetLineIndentation(IntPtr instance, IntPtr line);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr IDocumentLineEnd(IntPtr instance, IntPtr line);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr IDocumentGetRelativePosition(IntPtr instance, IntPtr positionStart, IntPtr characterOffset);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int IDocumentGetCharacterAndWidth(IntPtr instance, IntPtr position, IntPtr pWidth);


        [StructLayout(LayoutKind.Sequential)]
        public struct IDocumentVtable
        {
            public IDocumentVersion Version;
            public IDocumentSetErrorStatus SetErrorStatus;
            public IDocumentLength Length;
            public IDocumentGetCharRange GetCharRange;
            public IDocumentStyleAt StyleAt;
            public IDocumentLineFromPosition LineFromPosition;
            public IDocumentLineStart LineStart;
            public IDocumentGetLevel GetLevel;
            public IDocumentSetLevel SetLevel;
            public IDocumentGetLineState GetLineState;
            public IDocumentSetLineState SetLineState;
            public IDocumentStartStyling StartStyling;
            public IDocumentSetStyleFor SetStyleFor;
            public IDocumentSetStyles SetStyles;
            public IDocumentDecorationSetCurrentIndicator DecorationSetCurrentIndicator;
            public IDocumentDecorationFillRange DecorationFillRange;
            public IDocumentChangeLexerState ChangeLexerState;
            public IDocumentCodePage CodePage;
            public IDocumentIsDBCSLeadByte IsDBCSLeadByte;
            public IDocumentBufferPointer BufferPointer;
            public IDocumentGetLineIndentation GetLineIndentation;
            public IDocumentLineEnd LineEnd;
            public IDocumentGetRelativePosition GetRelativePosition;
            public IDocumentGetCharacterAndWidth GetCharacterAndWidth;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IDocument
        {
            public IntPtr VTable;
        }
        #endregion IDocument


        #region ILexer
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerVersion(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerRelease(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPropertyNames(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerPropertyType(IntPtr instance, IntPtr name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescribeProperty(IntPtr instance, IntPtr name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPropertySet(IntPtr instance, IntPtr key, IntPtr val);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescribeWordListSets(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerWordListSet(IntPtr instance, int kw_list_index, IntPtr key_word_list);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerLex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerFold(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPrivateCall(IntPtr instance, int operation, IntPtr pointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerLineEndTypesSupported(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerAllocateSubStyles(IntPtr instance, int style_base, int number_styles);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerSubStylesStart(IntPtr instance, int style_base);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerSubStylesLength(IntPtr instance, int style_base);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerStyleFromSubStyle(IntPtr instance, int sub_style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerPrimaryStyleFromStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerFreeSubStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerSetIdentifiers(IntPtr instance, int style, IntPtr identifiers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerDistanceToSecondaryStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerGetSubStyleBases(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerNamedStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerNameOfStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerTagsOfStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescriptionOfStyle(IntPtr instance, int style);

        [StructLayout(LayoutKind.Sequential)]
        public struct ILexer4
        {
            public ILexerVersion Version;
            public ILexerRelease Release;
            public ILexerPropertyNames PropertyNames;
            public ILexerPropertyType PropertyType;
            public ILexerDescribeProperty DescribeProperty;
            public ILexerPropertySet PropertySet;
            public ILexerDescribeWordListSets DescribeWordListSets;
            public ILexerWordListSet WordListSet;
            public ILexerLex Lex;
            public ILexerFold Fold;
            public ILexerPrivateCall PrivateCall;
            public ILexerLineEndTypesSupported LineEndTypesSupported;
            public ILexerAllocateSubStyles AllocateSubStyles;
            public ILexerSubStylesStart SubStylesStart;
            public ILexerSubStylesLength SubStylesLength;
            public ILexerStyleFromSubStyle StyleFromSubStyle;
            public ILexerPrimaryStyleFromStyle PrimaryStyleFromStyle;
            public ILexerFreeSubStyles FreeSubStyles;
            public ILexerSetIdentifiers SetIdentifiers;
            public ILexerDistanceToSecondaryStyles DistanceToSecondaryStyles;
            public ILexerGetSubStyleBases GetSubStyleBases;
            public ILexerNamedStyles NamedStyles;
            public ILexerNameOfStyle NameOfStyle;
            public ILexerTagsOfStyle TagsOfStyle;
            public ILexerDescriptionOfStyle DescriptionOfStyle;
        }
        #endregion ILexer

        static ILexer4 ilexer4 = new ILexer4 { };
        static IntPtr vtable_pointer = IntPtr.Zero;

        public static IntPtr ILexerImplementation()
        {
            if (vtable_pointer == IntPtr.Zero)
            {
                // simulate a c++ vtable by creating an array of 25 function pointers
                ilexer4.Version = new ILexerVersion(Version);
                ilexer4.Release = new ILexerRelease(Release);
                ilexer4.PropertyNames = new ILexerPropertyNames(PropertyNames);
                ilexer4.PropertyType = new ILexerPropertyType(PropertyType);
                ilexer4.DescribeProperty = new ILexerDescribeProperty(DescribeProperty);
                ilexer4.PropertySet = new ILexerPropertySet(PropertySet);
                ilexer4.DescribeWordListSets = new ILexerDescribeWordListSets(DescribeWordListSets);
                ilexer4.WordListSet = new ILexerWordListSet(WordListSet);
                ilexer4.Lex = new ILexerLex(Lex);
                ilexer4.Fold = new ILexerFold(Fold);
                ilexer4.PrivateCall = new ILexerPrivateCall(PrivateCall);
                ilexer4.LineEndTypesSupported = new ILexerLineEndTypesSupported(LineEndTypesSupported);
                ilexer4.AllocateSubStyles = new ILexerAllocateSubStyles(AllocateSubStyles);
                ilexer4.SubStylesStart = new ILexerSubStylesStart(SubStylesStart);
                ilexer4.SubStylesLength = new ILexerSubStylesLength(SubStylesLength);
                ilexer4.StyleFromSubStyle = new ILexerStyleFromSubStyle(StyleFromSubStyle);
                ilexer4.PrimaryStyleFromStyle = new ILexerPrimaryStyleFromStyle(PrimaryStyleFromStyle);
                ilexer4.FreeSubStyles = new ILexerFreeSubStyles(FreeSubStyles);
                ilexer4.SetIdentifiers = new ILexerSetIdentifiers(SetIdentifiers);
                ilexer4.DistanceToSecondaryStyles = new ILexerDistanceToSecondaryStyles(DistanceToSecondaryStyles);
                ilexer4.GetSubStyleBases = new ILexerGetSubStyleBases(GetSubStyleBases);
                ilexer4.NamedStyles = new ILexerNamedStyles(NamedStyles);
                ilexer4.NameOfStyle = new ILexerNameOfStyle(NameOfStyle);
                ilexer4.TagsOfStyle = new ILexerTagsOfStyle(TagsOfStyle);
                ilexer4.DescriptionOfStyle = new ILexerDescriptionOfStyle(DescriptionOfStyle);
                IntPtr vtable = Marshal.AllocHGlobal(Marshal.SizeOf(ilexer4));
                Marshal.StructureToPtr(ilexer4, vtable, false);
                vtable_pointer = Marshal.AllocHGlobal(Marshal.SizeOf(vtable));
                Marshal.StructureToPtr(vtable, vtable_pointer, false);
            }
            // return the address of the fake vtable
            return vtable_pointer;
        }

        // virtual int SCI_METHOD Version() const = 0
        public static int Version(IntPtr instance)
        {
            /*returns an enumerated value specifying which version of the interface is implemented: 
             * lvRelease5 for ILexer5 and lvRelease4 for ILexer4. 
             * ILexer5 must be provided for Scintilla version 5.0 or later.
             */
            //GC.Collect();  // test to see if the methods do get garbage collected
            return 2;
        }

        // virtual void SCI_METHOD Release() = 0
        public static void Release(IntPtr instance)
        {
            // is called to destroy the lexer object.
        }

        // virtual const char * SCI_METHOD PropertyNames() = 0
        public static IntPtr PropertyNames(IntPtr instance)
        {
            /*  returns a string with all of the valid properties separated by "\n".
             *  If the lexer does not support this call then an empty string is returned. 
             */
            return Marshal.StringToHGlobalAnsi(string.Join(Environment.NewLine, SupportedProperties.Keys));
        }

        // virtual int SCI_METHOD PropertyType(const char *name) = 0
        public static int PropertyType(IntPtr instance, IntPtr name)
        {
            // Properties may be boolean (SC_TYPE_BOOLEAN), integer (SC_TYPE_INTEGER), or string (SC_TYPE_STRING) 
            return PropertyTypes[Marshal.PtrToStringAnsi(name)];
        }

        // virtual const char * SCI_METHOD DescribeProperty(const char *name) = 0
        public static IntPtr DescribeProperty(IntPtr instance, IntPtr name)
        {
            // A description of a property in English
            return Marshal.StringToHGlobalAnsi(PropertyDescription[Marshal.PtrToStringAnsi(name)]);
        }

        // virtual i64 SCI_METHOD PropertySet(const char *key, const char *val) = 0
        public static IntPtr PropertySet(IntPtr instance, IntPtr key, IntPtr val)
        {
            /* The return values from PropertySet and WordListSet are used to indicate 
             * whether the change requires performing lexing or folding over any of the document. 
             * It is the position at which to restart lexing and folding or -1 
             * if the change does not require any extra work on the document. 
             * A simple approach is to return 0 if there is any possibility that a change 
             * requires lexing the document again while an optimisation could be to remember 
             * where a setting first affects the document and return that position.
             */
            string name = Marshal.PtrToStringAnsi(key);
            string value = Marshal.PtrToStringAnsi(val);

            if ((name == "separator") && (value.Length > 0))
            {
                separatorChar = value[0];
            }
            else if (name == "fixedwidths")
            {
                fixedWidths = value.Split(',').Select(Int32.Parse).ToList();
                separatorChar = '\0';
            }
            else
            {
                SupportedProperties[name] = value == "0" ? false : true;
            }

            return IntPtr.Zero;
        }

        // virtual const char * SCI_METHOD DescribeWordListSets() = 0
        public static IntPtr DescribeWordListSets(IntPtr instance)
        {
            // ? A string describing the different keywords, but how to separate is the question
            return IntPtr.Zero;
        }

        // virtual i64 SCI_METHOD WordListSet(int n, const char *wl) = 0
        public static IntPtr WordListSet(IntPtr instance, int kw_list_index, IntPtr key_word_list)
        {
            // CSV Lint doesn't use keyword lists
            return IntPtr.Zero;
        }


        // virtual void SCI_METHOD Lex(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Lex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            /* main lexing method. 
             * start_pos is always the startposition of a line
             * length_doc is NOT the total length of a document but the size of the text to be styled
             * init_style is the style of last styled byte
             * p_access is the pointer of the IDocument cpp class
             */

            // algorithm in part based on "How can I parse a CSV string with JavaScript, which contains comma in data?"
            // answer by user Bachor https://stackoverflow.com/a/58181757/1745616

            int length = (int)length_doc;
            int start = (int)start_pos;

            // allocate a buffer
            IntPtr buffer_ptr = Marshal.AllocHGlobal(length);
            if (buffer_ptr == IntPtr.Zero) { return; }

            // create the IDocument interface (struct) from the provided p_access pointer
            IDocument idoc = (IDocument)Marshal.PtrToStructure(p_access, typeof(IDocument));
            // create/simulate the vtable of the IDocument interface
            IDocumentVtable vtable = (IDocumentVtable)Marshal.PtrToStructure((IntPtr)idoc.VTable, typeof(IDocumentVtable));

            // scintilla fills the allocated buffer
            vtable.GetCharRange(p_access, buffer_ptr, (IntPtr)start, (IntPtr)length);
            if (buffer_ptr == IntPtr.Zero) { return; }

            // convert the buffer into a managed string
            string content = Marshal.PtrToStringAnsi(buffer_ptr, length);

            // column color index
            int idx = 1;
            bool isEOL = false;

            int start_col = 0;
            int end_col = 0;
            int i = 0;
            bool bNextCol = false;

            // fixed width or separator
            if (separatorChar == '\0')
            {
                int colidx = 0;
                int widthcount = fixedWidths[colidx];

                // fixed widths
                while (i < length)
                {
                    // next character
                    char cur = content[i];
                    i++;

                    // new line
                    if ((cur == '\n') || (cur == '\r')) { isEOL = true; end_col = i; }

                    // end of column
                    widthcount--;
                    if (widthcount == 0) { bNextCol = true; end_col = i; }

                    // if next col or next line
                    if (bNextCol || isEOL)
                    {
                        // style this column
                        if (end_col != start_col)
                        {
                            vtable.StartStyling(p_access, (IntPtr)(start + start_col));
                            vtable.SetStyleFor(p_access, (IntPtr)(end_col - start_col), (char)idx);
                        }

                        // next column width
                        colidx++;
                        if (isEOL) colidx = 0; // reset to first column at end of line

                        // next color, cycle colors or reset at end of line
                        idx++;
                        if ((idx > 8) || (isEOL)) idx = 1;

                        // next width, or take the rest after last column width 
                        if (colidx < fixedWidths.Count)
                        {
                            widthcount = fixedWidths[colidx];
                        }
                        else
                        {
                            widthcount = 9999; // rest of line
                            idx = 0; // white, no color to indicate incorrect column
                        }

                        // reset variables
                        bNextCol = false;
                        isEOL = false;
                        start_col = i;
                    }
                }
            }
            else
            {
                // JAVASCRIPT
                bool quote = false;
                bool sepcol = SupportedProperties["separatorcolor"];

                for (i = 0; i < length - 1; i++)
                {
                    char cur = content[i];
                    char next = content[i + 1];

                    if (!quote)
                    {
                        //const cellIsEmpty = line[line.length - 1].length === 0;
                        bool cellIsEmpty = (i - start_col == 0);

                        if ((cur == '"') && cellIsEmpty) { quote = true; }
                        else if (cur == separatorChar) { bNextCol = true; end_col = i; }
                        else if ((cur == '\r') && (next == '\n')) { isEOL = true; end_col = i; i++; }
                        else if ((cur == '\n') || (cur == '\r')) { isEOL = true; end_col = i; }
                        //else line[line.length - 1] += cur;
                    }
                    else
                    {
                        if ((cur == '"') && (next == '"')) { i++; }
                        else if (cur == '"') quote = false;
                        //else line[line.length - 1] += cur;
                    }

                    // if next col or next line
                    if (bNextCol || isEOL)
                    {
                        // include separator character in syntax highlighting color
                        if (sepcol && !isEOL) end_col++;

                        // style this column
                        vtable.StartStyling(p_access, (IntPtr)(start + start_col));
                        vtable.SetStyleFor(p_access, (IntPtr)(end_col - start_col), (char)idx);

                        // separator character is white/unstyled
                        if (!sepcol)
                        {
                            // style empty value between columns
                            vtable.StartStyling(p_access, (IntPtr)(start + i));
                            vtable.SetStyleFor(p_access, (IntPtr)1, (char)0); // 0 = white
                        }

                        // next color
                        idx++;

                        if ((idx > 8) || (isEOL)) idx = 1; // reset end of line

                        bNextCol = false;
                        isEOL = false;
                        start_col = i + 1;
                    }
                }
            }

            // style the last column
            if (length - start_col > 0)
            {
                vtable.StartStyling(p_access, (IntPtr)(start + start_col));
                vtable.SetStyleFor(p_access, (IntPtr)(length - start_col), (char)idx);
            }

            // free allocated buffer
            Marshal.FreeHGlobal(buffer_ptr);

            //Debug.WriteLine("{0} -- Lex finished!", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        // virtual void SCI_METHOD Fold(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Fold(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            /* 
             * is called with the exact range that needs folding. 
             * Previously, lexers were called with a range that started one line 
             * before the range that needs to be folded as this allowed fixing up 
             * the last line from the previous folding. 
             * The new approach allows the lexer to decide whether to backtrack 
             * or to handle this more efficiently.
             * 
             * Lessons I have learned so far are
             * - do not start with a base level of 0 to simplify the arithmetic int calculation
             * - scintilla recommends to use 0x400 as a base level
             * - when the value becomes smaller than the base value, set the base value
             * - create an additional margin in which you set the levels of the respective lines, 
             *      so it is easy to see when something breaks.
             */

            // CSV Lint -> no folding
            // TODO: maybe add folding to group 10x records or something?
            // 
            //int start = (int)start_pos;
            //int length = (int)length_doc;
            //IntPtr range_ptr = editor.GetRangePointer(start, length);
            //string content = Marshal.PtrToStringAnsi(range_ptr, length);
            //
            //
            //int cur_level = (int)SciMsg.SC_FOLDLEVELBASE;
            //int cur_line = editor.LineFromPosition(start);
            //
            //if (cur_line > 0)
            //{
            //    int prev_level = (int)editor.GetFoldLevel(cur_line - 1);
            //    bool header_flag_set = (prev_level & (int)SciMsg.SC_FOLDLEVELHEADERFLAG) == (int)SciMsg.SC_FOLDLEVELHEADERFLAG;
            //
            //    if (header_flag_set)
            //    {
            //        cur_level = (prev_level & (int)SciMsg.SC_FOLDLEVELNUMBERMASK) + 1;
            //    }
            //    else
            //    {
            //        cur_level = (prev_level & (int)SciMsg.SC_FOLDLEVELNUMBERMASK);
            //    }
            //}
            //
            //int next_level = cur_level;
            //
            //for (int i = 0; i < length; i++)
            //{
            //
            //    if (!SupportedProperties["fold"])
            //    {
            //        editor.SetFoldLevel(cur_line, (int)SciMsg.SC_FOLDLEVELBASE);
            //        while (i < length)
            //        {
            //            // read rest of the line
            //            if (content[i] == '\n') { break; }
            //            i++;
            //        }
            //        cur_line++;
            //        continue;
            //    }
            //
            //    string tag = "";
            //    if (i + 2 < length) { tag = content.Substring(i, 3); }
            //
            //
            //    if (FoldOpeningTags.Contains(tag))
            //    {
            //        next_level++;
            //        cur_level |= (int)SciMsg.SC_FOLDLEVELHEADERFLAG;
            //    }
            //    else if (FoldClosingTags.Contains(tag))
            //    {
            //        next_level--;
            //        if (SupportedProperties["fold.compact"]) { cur_level--; }
            //        cur_level &= (int)SciMsg.SC_FOLDLEVELNUMBERMASK;
            //    }
            //    else
            //    {
            //        cur_level &= (int)SciMsg.SC_FOLDLEVELNUMBERMASK;
            //    }
            //
            //    while (i < length)
            //    {
            //        // read rest of the line
            //        if (content[i] == '\n') { break; }
            //        i++;
            //    }
            //    // set fold level
            //    if (cur_level < (int)SciMsg.SC_FOLDLEVELBASE)
            //    {
            //        cur_level = (int)SciMsg.SC_FOLDLEVELBASE;
            //    }
            //    editor.SetFoldLevel(cur_line, cur_level);
            //    cur_line++;
            //    cur_level = next_level;
            //
            //}
        }

        // virtual int SCI_METHOD NamedStyles() = 0;
        public static int NamedStyles(IntPtr instance)
        {
            // Retrieve the number of named styles for the lexer.
            return NamedStylesListCount;
        }

        // virtual const char * SCI_METHOD NameOfStyle(int style) = 0;
        public static IntPtr NameOfStyle(IntPtr instance, int style)
        {
            // Retrieve the name of a style.
            return Marshal.StringToHGlobalAnsi(NamedStylesList[style]);
        }

        // virtual const char * SCI_METHOD TagsOfStyle(int style) = 0;
        public static IntPtr TagsOfStyle(IntPtr instance, int style)
        {
            /*
             * Retrieve the tags of a style.
             * This is a space-separated set of words like "comment documentation".
             */
            return Marshal.StringToHGlobalAnsi(TagsOfStyleList[style]);
        }

        // virtual const char * SCI_METHOD DescriptionOfStyle(int style) = 0;
        public static IntPtr DescriptionOfStyle(IntPtr instance, int style)
        {
            /* 
             * Retrieve an English-language description of a style which may be suitable 
             * for display in a user interface.
             * This looks like "Doc comment: block comments beginning with /** or /*!".
             */
            return Marshal.StringToHGlobalAnsi(DescriptionOfStyleList[style]);
        }

        // virtual int SCI_METHOD LineEndTypesSupported() = 0;
        public static int LineEndTypesSupported(IntPtr instance)
        {
            /*
             *  reports the different types of line ends supported by the current lexer. 
             *  This is a bit set although there is currently only a single choice
             *  with either SC_LINE_END_TYPE_DEFAULT (0) or SC_LINE_END_TYPE_UNICODE (1). 
             *  These values are also used by the other messages concerned with Unicode line ends.
             */
            return (int)SciMsg.SC_LINE_END_TYPE_DEFAULT;
        }

        // virtual void * SCI_METHOD PrivateCall(int operation, void *pointer) = 0;
        public static IntPtr PrivateCall(IntPtr instance, int operation, IntPtr pointer)
        {
            /* allows for direct communication between the application and a lexer.
             * An example would be where an application maintains a single large data structure 
             * containing symbolic information about system headers(like Windows.h) 
             * and provides this to the lexer where it can be applied to each document.
             * This avoids the costs of constructing the system header information for each document.
             * This is invoked with the SCI_PRIVATELEXERCALL API.
             */
            return IntPtr.Zero;
        }


        /***********************************************************************************************
         * 
         * NOTE:
         * 
         * THE FOLLOWING METHODS ARE NEEDED ONLY IF THE DOCUMENT IS TO BE LEXED WITH DIFFERENT LEXERS,
         * LIKE THE HTML LEXER DOES, WHICH BESIDES HTML CODE ALSO HAS TO LEX PHP, JS etc.
         * 
         * THE IMPLEMENTATION IS LEFT TO THOSE INTERESTED :)
         * 
         * *********************************************************************************************/


        // virtual int SCI_METHOD AllocateSubStyles(int styleBase, int numberStyles) = 0;
        public static int AllocateSubStyles(IntPtr instance, int style_base, int number_styles)
        {
            /* Allocate some number of substyles for a particular base style, 
             * returning the first substyle number allocated.
             * Substyles are allocated contiguously.
             */
            return -1;
        }

        // virtual int SCI_METHOD SubStylesStart(int styleBase) = 0;
        public static int SubStylesStart(IntPtr instance, int style_base)
        {
            // Return the start of the substyles allocated for a base style.
            return -1;
        }

        // virtual int SCI_METHOD SubStylesLength(int styleBase) = 0;
        public static int SubStylesLength(IntPtr instance, int style_base)
        {
            // Return the length of the substyles allocated for a base style.
            return 0;
        }

        // virtual int SCI_METHOD StyleFromSubStyle(int subStyle) = 0;
        public static int StyleFromSubStyle(IntPtr instance, int sub_style)
        {
            // For a sub style, return the base style, else return the argument.
            return 0;
        }

        // virtual int SCI_METHOD PrimaryStyleFromStyle(int style) = 0;
        public static int PrimaryStyleFromStyle(IntPtr instance, int style)
        {
            // For a secondary style, return the primary style, else return the argument.
            return 0;
        }

        // virtual void SCI_METHOD FreeSubStyles() = 0;
        public static void FreeSubStyles(IntPtr instance)
        {
            // Free all allocated substyles.
        }

        // virtual void SCI_METHOD SetIdentifiers(int style, const char *identifiers) = 0;
        public static void SetIdentifiers(IntPtr instance, int style, IntPtr identifiers)
        {
            /* Similar to SCI_SETKEYWORDS but for substyles. 
             * The prefix feature available with SCI_SETKEYWORDS is not implemented for SCI_SETIDENTIFIERS.
             */
        }

        // virtual int SCI_METHOD DistanceToSecondaryStyles() = 0;
        public static int DistanceToSecondaryStyles(IntPtr instance)
        {
            // Returns the distance between a primary style and its corresponding secondary style.
            return 0;
        }

        // virtual const char * SCI_METHOD GetSubStyleBases() = 0;
        public static IntPtr GetSubStyleBases(IntPtr instance)
        {
            // Fill styles with a byte for each style that can be split into substyles.
            return IntPtr.Zero;
        }
    }
}
