namespace Cavity.Build
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Cavity.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CRLF", Justification = "This is the intended casing.")]
    public sealed class CRLF : Task
    {
        [Required]
        public string Spec { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.Normal, Spec);
            Execute(new FileSpec(Spec));

            return true;
        }

        private void Execute(IEnumerable<FileInfo> spec)
        {
            System.Threading.Tasks.Parallel.ForEach(spec, Process);
        }

        private void Process(FileInfo file)
        {
            Log.LogMessage(MessageImportance.Normal, "[{0}] {1}".FormatWith(file.FixNewLine() ? '¤' : ' ', file.FullName));
        }
    }
}