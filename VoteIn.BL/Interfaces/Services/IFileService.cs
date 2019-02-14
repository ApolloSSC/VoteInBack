using System.Globalization;

namespace VoteIn.BL.Interfaces.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string LoadFile(string path, CultureInfo culture = null);
    }
}
