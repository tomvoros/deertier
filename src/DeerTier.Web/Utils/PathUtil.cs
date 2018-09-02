using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DeerTier.Web.Utils
{
    public class PathUtil
    {
        private static readonly char[] _invalidFileNameChars;
        private static readonly string _appPath = HttpRuntime.AppDomainAppPath;
        
        static PathUtil()
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var invalidPathChars = Path.GetInvalidPathChars();

            _invalidFileNameChars = invalidFileNameChars
                .Concat(invalidPathChars)
                .Distinct()
                .ToArray();
        }

        public static string AppPath
        {
            get
            {
                return _appPath;
            }
        }

        /// <summary>
        /// Produces a valid filename by replacing all invalid characters with underscores.
        /// </summary>
        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            var fileNameBuilder = new StringBuilder(fileName.Length);

            var invalidSequence = false;
            foreach (var c in fileName)
            {
                if (_invalidFileNameChars.Contains(c))
                {
                    if (!invalidSequence)
                    {
                        fileNameBuilder.Append('_');
                        invalidSequence = true;
                    }
                }
                else
                {
                    fileNameBuilder.Append(c);
                    invalidSequence = false;
                }
            }

            return fileNameBuilder.ToString();
        }
    }
}