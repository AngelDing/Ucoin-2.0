using System;
using System.IO;
using System.Runtime.Serialization;

namespace Ucoin.ServiceModel.Server.Runtime
{
    [Serializable]
    public class ServicePackage
    {
        private readonly string name;
        private readonly string fullName;
        private readonly string assembly;

        public ServicePackage(string serviceFile, string baseAddress)
        {
            fullName = serviceFile;
            name = Path.GetFileName(serviceFile);
            if (IsAssemblyFileType(serviceFile))
            {
                assembly = serviceFile;
            }
            BaseAddress = baseAddress;
        }

        [DataMember]
        public string FullName
        {
            get { return fullName; }
        }

        [IgnoreDataMember]
        public int Id
        {
            get { return fullName.GetHashCode(); }
        }

        [IgnoreDataMember]
        public string BaseAddress { get; set; }

        [IgnoreDataMember]
        public string Name
        {
            get { return name; }
        }

        [IgnoreDataMember]
        public string AssemblyFile
        {
            get { return assembly; }
        }

        private static bool IsAssemblyFileType(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            return extension == ".dll" || extension == ".exe";
        }
    }
}
