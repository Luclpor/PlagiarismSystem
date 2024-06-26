﻿using Antlr4.Grammar.Cpp;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;

namespace Xylab.PlagiarismDetect.Frontend.Cpp
{
    public class Language : ILanguage
    {
        public IReadOnlyCollection<string> Suffixes { get; } = new[] { ".CPP", ".H", ".C", ".HPP", ".CC" };

        public string Name => "C++ 14";

        public string ShortName => "cpp";

        public int MinimalTokenMatch => 12;

        public Func<Structure, ICPP14Listener> ListenerFactory { get; }

        public bool SupportsColumns => true;

        public bool IsPreformated => true;

        public bool UsesIndex => true;

        public int CountOfTokens => (int)TokenConstants.NUM_DIFF_TOKENS;

        public Language() : this(s => new JplagListener(s))
        {
        }

        public Language(Func<Structure, ICPP14Listener> listenerImpl)
        {
            Console.WriteLine("Вызов конструктора Language");
            ListenerFactory = listenerImpl;
        }

        static Language()
        {
            CPP14Parser.InitSharedContextCache();
            CPP14Lexer.InitSharedContextCache();
        }



        public Structure Parse(ISubmissionFile files)
        {
            var structure = new Structure();
            var outputWriter = new StringWriter(structure.OtherInfo);
            var errorWriter = new StringWriter(structure.ErrorInfo);
            var listener = ListenerFactory(structure);//create obj JPlagListener

            foreach (var item in SubmissionComposite.ExtendToLeaf(files))
            {
                var lexer = new CPP14Lexer(item.Open(), outputWriter, errorWriter);
                var parser = new CPP14Parser(new CommonTokenStream(lexer), outputWriter, errorWriter);
            
                parser.AddErrorListener(structure);
                parser.AddParseListener(listener);
                structure.FileId = item.Id;

                var root = parser.TranslationUnit();
                parser.ErrorListeners.Clear();
                parser.ParseListeners.Clear();
                if (!structure.EndWithEof)
                    structure.AddToken(new Token(TokenConstants.FILE_END, 0, 0, 0, item.Id));
            }

            return structure;
        }

        public string TypeName(int type) => Token.TypeToString((TokenConstants)type);

        public Frontend.Token CreateToken(int type, int line, int column, int length, int fileId)
        {
            return new Token((TokenConstants)type, line, column, column + length - 1, fileId);
        }

        public void Cleanup()
        {
            CPP14Parser.ResetSharedContextCache();
            CPP14Lexer.ResetSharedContextCache();
        }
    }
}
