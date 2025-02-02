using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using TriLibCore.Utils;

namespace TriLibCore.Mappers
{
    /// <summary>
    /// Provides an external data mapping strategy for extracting data from Zip files.
    /// This mapper searches through the entries of a Zip file (provided via custom context data)
    /// for an entry whose short filename matches the specified <paramref name="originalFilename"/>.
    /// If a match is found, it opens a stream for that Zip entry.
    /// </summary>
    public class ZipFileExternalDataMapper : ExternalDataMapper
    {
        /// <inheritdoc />
        /// <remarks>
        /// This method retrieves the custom context data of type <see cref="ZipLoadCustomContextData"/> from the 
        /// <see cref="AssetLoaderContext.CustomData"/>. It then validates that the Zip file instance is available.
        /// The method converts the <paramref name="originalFilename"/> to a lower-case short filename and iterates 
        /// through each file entry in the Zip. When an entry with a matching short filename is found, the method 
        /// sets <paramref name="finalPath"/> to the name of the Zip file and returns a stream opened from that entry.
        /// If no matching entry is found, <paramref name="finalPath"/> is set to <c>null</c> and <c>null</c> is returned.
        /// </remarks>
        /// <param name="assetLoaderContext">
        /// The context containing model loading data and custom context data, including the Zip file reference.
        /// </param>
        /// <param name="originalFilename">
        /// The original filename from the model data used to locate the corresponding Zip entry.
        /// </param>
        /// <param name="finalPath">
        /// When a matching entry is found, this output parameter is set to the Zip file's name.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> to the matched Zip entry if found; otherwise, <c>null</c>.
        /// </returns>
        public override Stream Map(AssetLoaderContext assetLoaderContext, string originalFilename, out string finalPath)
        {
            var zipLoadCustomContextData = CustomDataHelper.GetCustomData<ZipLoadCustomContextData>(assetLoaderContext.CustomData);
            if (zipLoadCustomContextData == null)
            {
                throw new Exception("Missing custom context data.");
            }

            var zipFile = zipLoadCustomContextData.ZipFile;
            if (zipFile == null)
            {
                throw new Exception("Zip file instance is null.");
            }

            var shortFileName = FileUtils.GetShortFilename(originalFilename).ToLowerInvariant();
            foreach (ZipEntry zipEntry in zipFile)
            {
                if (!zipEntry.IsFile)
                {
                    continue;
                }
                var checkingFileShortName = FileUtils.GetShortFilename(zipEntry.Name).ToLowerInvariant();
                if (shortFileName == checkingFileShortName)
                {
                    finalPath = zipFile.Name;
                    string _;
                    return AssetLoaderZip.ZipFileEntryToStream(out _, zipEntry, zipFile);
                }
            }
            finalPath = null;
            return null;
        }
    }
}
