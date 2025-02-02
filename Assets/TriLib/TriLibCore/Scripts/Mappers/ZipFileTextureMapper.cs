#pragma warning disable 672

using System;
using ICSharpCode.SharpZipLib.Zip;
using TriLibCore.General;
using TriLibCore.Utils;
using UnityEngine;

namespace TriLibCore.Mappers
{
    /// <summary>
    /// Provides a texture mapping strategy for extracting texture data from Zip files.
    /// This mapper retrieves a Zip file from the custom context data and iterates through its entries,
    /// comparing their names to the texture's filename. If a matching file is found, its stream is opened
    /// and assigned to the texture loading context.
    /// </summary>
    public class ZipFileTextureMapper : TextureMapper
    {
        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// This method begins by retrieving the custom context data as an instance of
        /// <see cref="ZipLoadCustomContextData"/>. It verifies that the Zip file instance is valid.
        /// </para>
        /// <para>
        /// The method then obtains the filename (without extension) of the current zip entry that was used to load
        /// the model and compares it (in lower-case) to the short filename of the texture. It iterates through all file 
        /// entries in the Zip and, for each valid texture file type, checks if either:
        /// <list type="bullet">
        /// <item><description>For diffuse textures, the filename (without extension) matches the model's zip entry filename,</description></item>
        /// <item><description>or the short filenames match directly.</description></item>
        /// </list>
        /// If a match is found, the texture's <see cref="TextureLoadingContext.Stream"/> is set to the stream obtained from the matching Zip entry.
        /// </para>
        /// </remarks>
        /// <param name="textureLoadingContext">
        /// The context containing texture information, including the TriLib <see cref="ITexture"/>,
        /// the target texture type, and where the stream is to be assigned.
        /// </param>
        public override void Map(TextureLoadingContext textureLoadingContext)
        {
            var zipLoadCustomContextData = CustomDataHelper.GetCustomData<ZipLoadCustomContextData>(textureLoadingContext.Context.CustomData);
            if (zipLoadCustomContextData == null)
            {
                throw new Exception("Missing custom context data.");
            }
            var zipFile = zipLoadCustomContextData.ZipFile;
            if (zipFile == null)
            {
                throw new Exception("Zip file instance is null.");
            }
            if (string.IsNullOrWhiteSpace(textureLoadingContext.Texture.Filename))
            {
                if (textureLoadingContext.Context.Options.ShowLoadingWarnings)
                {
                    Debug.LogWarning("Texture name is null.");
                }
                return;
            }
            // Get the file name (without extension) from the zip entry used for the model
            var modelFilenameWithoutExtension = FileUtils.GetFilenameWithoutExtension(zipLoadCustomContextData.ZipEntry.Name).ToLowerInvariant();
            // Get the short filename for the texture
            var textureShortName = FileUtils.GetShortFilename(textureLoadingContext.Texture.Filename).ToLowerInvariant();

            foreach (ZipEntry zipEntry in zipFile)
            {
                if (!zipEntry.IsFile)
                {
                    continue;
                }
                var checkingFileShortName = FileUtils.GetShortFilename(zipEntry.Name).ToLowerInvariant();
                var checkingFilenameWithoutExtension = FileUtils.GetFilenameWithoutExtension(zipEntry.Name).ToLowerInvariant();
                if ((TextureUtils.IsValidTextureFileType(checkingFileShortName) &&
                     textureLoadingContext.TextureType == TextureType.Diffuse &&
                     modelFilenameWithoutExtension == checkingFilenameWithoutExtension)
                    || textureShortName == checkingFileShortName)
                {
                    textureLoadingContext.Stream = AssetLoaderZip.ZipFileEntryToStream(out _, zipEntry, zipFile);
                }
            }
        }
    }
}
