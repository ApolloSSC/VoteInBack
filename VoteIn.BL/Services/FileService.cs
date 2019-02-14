using Microsoft.AspNetCore.Hosting;
using System;
using System.Globalization;
using System.IO;
using VoteIn.BL.Interfaces.Services;

namespace VoteIn.BL.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// The env
        /// </summary>
        private readonly IHostingEnvironment _env;

        #region Public Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="FileService"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public FileService(IHostingEnvironment env)
        {
            _env = env;
        }
        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="Exception">File not exists at {path}</exception>
        public string LoadFile(string path, CultureInfo culture = null)
        {
            if (culture == null) culture = new CultureInfo("en");
            var pathList = path.Split('\\');
            var fullPath = Path.Combine(_env.WebRootPath, pathList[0],culture.TwoLetterISOLanguageName, pathList[1]);

            if (!File.Exists(fullPath))
            {
                fullPath = Path.Combine(_env.WebRootPath, path);

                if (!File.Exists(fullPath))
                    throw new Exception($"File not exists at {path}");
            }

            return File.ReadAllText(fullPath);
        }
        #endregion
    }
}
