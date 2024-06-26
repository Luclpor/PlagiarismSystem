﻿namespace Xylab.PlagiarismDetect.Frontend
{
    public abstract class Token
    {
        public Token(int type, int line, int column, int length, int fileId)
        {
            //Console.WriteLine("вызов конструктора Token Common");
            Type = type;
            Line = line > 0 ? line : 1;
            Column = column;
            Length = length;
            FileId = fileId;
        }

        public bool Marked { get; set; }
        public int Hash { get; set; } = -1;
        public int Type { get; internal set; }
        public virtual int Line { get; internal set; }
        public virtual int Column { get; internal set; }
        public virtual int Length { get; internal set; }
        public virtual int FileId { get; internal set; }

        protected virtual int Index => -1;

        public override string ToString() {
            return "<abstract>";
        }

        public virtual int NumberOfTokens() => 1;
    }
}
