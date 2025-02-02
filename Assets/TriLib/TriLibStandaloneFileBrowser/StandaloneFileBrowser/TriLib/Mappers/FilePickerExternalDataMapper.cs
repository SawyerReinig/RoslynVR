using System;
using System.Collections.Generic;
using System.IO;
using TriLibCore.SFB;
using TriLibCore.Utils;
using UnityEngine;

namespace TriLibCore.Mappers
{
    /// <summary>
    /// Provides an external data mapping strategy for file picker–based workflows.
    /// This mapper searches through a collection of file items (each with an associated stream)
    /// to find one whose short filename matches the given <paramref name="originalFilename"/>.
    /// If a match is found, the file’s stream is returned, along with its full name as the final path.
    /// </summary>
    public class FilePickerExternalDataMapper : ExternalDataMapper
    {
        /// <inheritdoc />
        /// <remarks>
        /// This method looks for an external data source by comparing the short filename of the 
        /// <paramref name="originalFilename"/> (converted to lower-case) with the short filenames 
        /// of the files present in the custom context data (of type <see cref="IList{ItemWithStream}"/>).
        /// If a match is found and the file has valid data, it returns the open data stream from that file
        /// and sets <paramref name="finalPath"/> to the full file name.
        /// If no match is found or if the custom context data is missing, a warning is issued and <c>null</c>
        /// is returned.
        /// </remarks>
        /// <param name="assetLoaderContext">
        /// The <see cref="AssetLoaderContext"/> that contains overall model loading data,
        /// including custom context data holding references to selected files.
        /// </param>
        /// <param name="originalFilename">
        /// The original filename from the model data used to locate the corresponding file in the file picker selection.
        /// </param>
        /// <param name="finalPath">
        /// When a matching file is found, this output parameter is set to the file's full name or path.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> for the external data if a matching file is found; otherwise, <c>null</c>.
        /// </returns>
        public override Stream Map(AssetLoaderContext assetLoaderContext, string originalFilename, out string finalPath)
        {
            if (!string.IsNullOrEmpty(originalFilename))
            {
                var itemsWithStream = CustomDataHelper.GetCustomData<IList<ItemWithStream>>(assetLoaderContext.CustomData);
                if (itemsWithStream != null)
                {
                    var shortFileName = FileUtils.GetShortFilename(originalFilename).ToLowerInvariant();
                    foreach (var itemWithStream in itemsWithStream)
                    {
                        if (!itemWithStream.HasData)
                        {
                            continue;
                        }

                        var checkingFileShortName = FileUtils.GetShortFilename(itemWithStream.Name).ToLowerInvariant();
                        if (shortFileName == checkingFileShortName)
                        {
                            finalPath = itemWithStream.Name;
                            return itemWithStream.OpenStream();
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Missing custom context data.");
                }
            }
            finalPath = null;
            return null;
        }
    }
}
