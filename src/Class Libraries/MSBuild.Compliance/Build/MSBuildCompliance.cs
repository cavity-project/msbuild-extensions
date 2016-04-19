namespace Cavity.Build
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
#if !NET20
    using System.Linq;
#endif
    using System.Xml;
    using System.Xml.XPath;
    using Cavity.Properties;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public sealed class MSBuildCompliance : Task
    {
        [Required]
        public string Explanation { get; set; }

        [Required]
        public ITaskItem[] Projects { get; set; }

        public string Warning { get; set; }

        [Required]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "XPath", Justification = "Following the Microsoft naming convention.")]
        public string XPath { get; set; }

        public override bool Execute()
        {
            return Execute(Projects);
        }

        private bool Execute(IEnumerable<ITaskItem> projects)
        {
            if (null == projects)
            {
                Log.LogError(Resources.MSBuildCompliance_PathsNull_Message);
                return false;
            }

#if NET20
            var result = true;
            foreach (var project in projects)
            {
                if (Execute(project))
                {
                    continue;
                }

                if (BuildEngine.ContinueOnError)
                {
                    result = false;
                    continue;
                }

                return false;
            }
#else
            var result = 0 == projects.Count(project => !Execute(project));
#endif

            return result;
        }

        private bool Execute(ITaskItem path)
        {
            return null == path || Execute(new FileInfo(path.ItemSpec));
        }

        private bool Execute(FileSystemInfo file)
        {
            Log.LogMessage(MessageImportance.Normal, Resources.MSBuildCompliance_Execute_Message, file.FullName);
            Log.LogMessage(MessageImportance.Normal, XPath);
            var xml = new XmlDocument();
            xml.Load(file.FullName);

            return Execute(file, xml);
        }

        private bool Execute(FileSystemInfo file,
                             IXPathNavigable xml)
        {
            return Execute(file, xml.CreateNavigator());
        }

        private bool Execute(FileSystemInfo file,
                             XPathNavigator navigator)
        {
            return Execute(file, navigator, navigator.NameTable);
        }

        private bool Execute(FileSystemInfo file,
                             XPathNavigator navigator,
                             XmlNameTable nameTable)
        {
            var namespaces = new XmlNamespaceManager(nameTable);

            namespaces.AddNamespace("b", "http://schemas.microsoft.com/developer/msbuild/2003");

            return Execute(file, navigator, namespaces);
        }

        private bool Execute(FileSystemInfo file,
                             XPathNavigator navigator,
                             IXmlNamespaceResolver namespaces)
        {
            var o = navigator.Evaluate(XPath, namespaces);
            if (o != null && !(bool)o)
            {
                var message = string.Concat(Explanation, Resources.Colon, file.FullName);
                if (string.IsNullOrWhiteSpace(Warning) ||
                    !XmlConvert.ToBoolean(Warning))
                {
                    Log.LogError(message);
                    return false;
                }

                Log.LogWarning(message);
            }

            return true;
        }
    }
}