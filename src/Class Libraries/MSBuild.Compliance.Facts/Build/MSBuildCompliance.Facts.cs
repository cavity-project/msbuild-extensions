namespace Cavity.Build
{
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using Cavity.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Moq;
    using Xunit;

    public sealed class MSBuildComplianceFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<MSBuildCompliance>()
                            .DerivesFrom<Task>()
                            .IsConcreteClass()
                            .IsSealed()
                            .HasDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void ctor()
        {
            Assert.NotNull(new MSBuildCompliance());
        }

        [Fact]
        public void op_Execute_IEnumerable()
        {
            using (var file = new TempFile())
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Cavity.Build.MSBuildCompliance.xml"))
                {
                    if (null != resource)
                    {
                        using (var reader = new StreamReader(resource))
                        {
                            file.Info.Append(reader.ReadToEnd());
                        }
                    }
                }

                var obj = new MSBuildCompliance
                              {
                                  BuildEngine = new Mock<IBuildEngine>().Object,
                                  Projects = new ITaskItem[]
                                                 {
                                                     new TaskItem(file.Info.FullName)
                                                 },
                                  XPath = "0=count(/b:Project/b:PropertyGroup[@Condition][not(b:WarningLevel[text()='4'])])"
                              };

                Assert.True(obj.Execute());
            }
        }

        [Fact]
        public void op_Execute_IEnumerableEmpty()
        {
            var obj = new MSBuildCompliance
                          {
                              BuildEngine = new Mock<IBuildEngine>().Object,
                              Projects = new ITaskItem[]
                                             {
                                             }
                          };

            Assert.True(obj.Execute());
        }

        [Fact]
        public void op_Execute_IEnumerableNull()
        {
            var obj = new MSBuildCompliance
                          {
                              BuildEngine = new Mock<IBuildEngine>().Object
                          };

            Assert.False(obj.Execute());
        }

        [Fact]
        public void op_Execute_IEnumerable_containingNullItem()
        {
            using (var file = new TempFile())
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Cavity.Build.MSBuildCompliance.xml"))
                {
                    if (null != resource)
                    {
                        using (var reader = new StreamReader(resource))
                        {
                            file.Info.Append(reader.ReadToEnd());
                        }
                    }
                }

                var obj = new MSBuildCompliance
                              {
                                  BuildEngine = new Mock<IBuildEngine>().Object,
                                  Projects = new ITaskItem[]
                                                 {
                                                     new TaskItem(file.Info.FullName),
                                                     null
                                                 },
                                  XPath = "0=count(/b:Project/b:PropertyGroup[@Condition][not(b:WarningLevel[text()='4'])])"
                              };

                Assert.True(obj.Execute());
            }
        }

        [Fact]
        public void op_Execute_IEnumerable_whenWarningLevelMissing()
        {
            using (var file = new TempFile())
            {
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Cavity.Build.MSBuildCompliance.xml"))
                {
                    if (null != resource)
                    {
                        using (var reader = new StreamReader(resource))
                        {
                            var xml = new XmlDocument();
                            xml.LoadXml(reader.ReadToEnd());
                            var namespaces = new XmlNamespaceManager(xml.NameTable);
                            namespaces.AddNamespace("b", "http://schemas.microsoft.com/developer/msbuild/2003");
                            var node = xml.SelectSingleNode("/b:Project/b:PropertyGroup[@Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \"]/b:WarningLevel", namespaces);
                            if (null != node &&
                                null != node.ParentNode)
                            {
                                node.ParentNode.RemoveChild(node);
                            }

                            file.Info.Append(xml.OuterXml);
                        }
                    }
                }

                var obj = new MSBuildCompliance
                              {
                                  BuildEngine = new Mock<IBuildEngine>().Object,
                                  Projects = new ITaskItem[]
                                                 {
                                                     new TaskItem(file.Info.FullName)
                                                 },
                                  Explanation = "This is a test",
                                  XPath = "0=count(/b:Project/b:PropertyGroup[@Condition][not(b:WarningLevel[text()='4'])])"
                              };

                Assert.False(obj.Execute());
            }
        }

        [Fact]
        public void prop_Explanation()
        {
            Assert.True(new PropertyExpectations<MSBuildCompliance>(p => p.Explanation)
                            .IsAutoProperty<string>()
                            .IsDecoratedWith<RequiredAttribute>()
                            .Result);
        }

        [Fact]
        public void prop_Projects()
        {
            Assert.True(new PropertyExpectations<MSBuildCompliance>(p => p.Projects)
                            .IsAutoProperty<ITaskItem[]>()
                            .IsDecoratedWith<RequiredAttribute>()
                            .Result);
        }

        [Fact]
        public void prop_Warning()
        {
            Assert.True(new PropertyExpectations<MSBuildCompliance>(p => p.Warning)
                            .IsAutoProperty<string>()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_XPath()
        {
            Assert.True(new PropertyExpectations<MSBuildCompliance>(p => p.XPath)
                            .IsAutoProperty<string>()
                            .IsDecoratedWith<RequiredAttribute>()
                            .Result);
        }
    }
}