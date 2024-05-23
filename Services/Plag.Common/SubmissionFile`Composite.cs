using Antlr4.Runtime;
using System;
using System.Collections.Generic;

namespace Xylab.PlagiarismDetect.Frontend
{
    public class SubmissionComposite : List<ISubmissionFile>, ISubmissionFile
    {
        public string Path => "./";

        public bool IsLeaf => false;

        public int Id => -1;

        public ICharStream Open() => throw new InvalidOperationException();

        public static IEnumerable<ISubmissionFile> ExtendToLeaf(ISubmissionFile file)
        {
            if (file.IsLeaf)
            {
                //Console.WriteLine("in submissionFileComposite");
                yield return file;
            }
            else
            {
                foreach (var item in file)
                {
                    Console.WriteLine(item.ToString());
                    foreach (var item2 in ExtendToLeaf(item))
                    {
                        yield return item2;
                    }
                }
            }
        }
    }
}
