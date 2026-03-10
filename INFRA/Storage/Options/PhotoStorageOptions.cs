using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
namespace INFRA
{
    public sealed class PhotoStorageOptions
    {
        public const string SectionName = "PhotoStorage";

        public string Endpoint { get; set; } = string.Empty;   // localhost:9000
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Bucket { get; set; } = "animal-photos";
        public bool UseSsl { get; set; } = false;
        public bool PublicRead { get; set; } = false;
        public string? PublicBaseUrl { get; set; }             // opcional
    }
}
