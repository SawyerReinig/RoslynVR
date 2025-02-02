#pragma warning disable 672

using System;
using System.Collections.Generic;
using TriLibCore.SFB;
using TriLibCore.Interfaces;
using TriLibCore.Utils;
using UnityEngine;

namespace TriLibCore.Mappers
{
    /// <summary>
    /// Provides functionality to load textures from a file picker selection. This mapper searches through the 
    /// list of <see cref="ItemWithStream"/> objects (provided in the custom context data) to find a file whose 
    /// short filename matches the filename specified in the TriLib <see cref="ITexture"/>. If a match is found, 
    /// it opens the corresponding data stream.
    /// </summary>
    public class FilePickerTextureMapper : TextureMapper
    {
        /// <inheritdoc />
        /// <remarks>
        /// This method first checks if the texture's filename is provided. If it is, the method retrieves a list 
        /// of <see cref="ItemWithStream"/> objects from the <see cref="AssetLoaderContext.CustomData"/>. It then 
        /// converts the texture's filename and each candidate file's name to lowercase short filenames and compares them.
        /// If a match is found and the candidate file contains valid data, the texture's stream is set to the stream 
        /// provided by the candidate file.
        /// If the custom context data is missing, a warning is logged.
        /// </remarks>
        /// <param name="textureLoadingContext">
        /// The context containing the texture information, including the TriLib <see cref="ITexture"/>, associated 
        /// custom context data, and the output stream.
        /// </param>
        public override void Map(TextureLoadingContext textureLoadingContext)
        {
            if (string.IsNullOrEmpty(textureLoadingContext.Texture.Filename))
            {
                return;
            }
            var itemsWithStream = CustomDataHelper.GetCustomData<IList<ItemWithStream>>(textureLoadingContext.Context.CustomData);
            if (itemsWithStream != null)
            {
                var shortFileName = FileUtils.GetShortFilename(textureLoadingContext.Texture.Filename).ToLowerInvariant();
                foreach (var itemWithStream in itemsWithStream)
                {
                    if (!itemWithStream.HasData)
                    {
                        continue;
                    }
                    var checkingFileShortName = FileUtils.GetShortFilename(itemWithStream.Name).ToLowerInvariant();
                    if (shortFileName == checkingFileShortName)
                    {
                        textureLoadingContext.Stream = itemWithStream.OpenStream();
                    }
                }
            }
            else
            {
                Debug.LogWarning("Missing custom context data.");
            }
        }
    }
}
