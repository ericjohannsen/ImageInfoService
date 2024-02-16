using MetadataExtractor;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;
using System.Drawing;
using ImageMagick;

namespace ImageInfoService
{
    public class ImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ImageInfo> ComputeImageInfo(string url)
        {
            ImageInfo imageInfo = new ImageInfo();

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                using (var responseStream = await httpClient.GetStreamAsync(url))
                using (var memoryStream = new MemoryStream())
                {
                    // Copy the network stream to a memory stream
                    await responseStream.CopyToAsync(memoryStream);

                    // Ensure the memory stream is at the beginning before reading it
                    memoryStream.Position = 0;

                    // Extract metadata
                    var directories = ImageMetadataReader.ReadMetadata(memoryStream);
                    foreach (var directory in directories)
                    {
                        foreach (var tag in directory.Tags)
                        {
                            switch (tag.Name)
                            {
                                case "Artist":
                                    imageInfo.Artist = tag.Description ?? string.Empty;
                                    break;
                                case "Copyright":
                                    imageInfo.Copyright = tag.Description ?? string.Empty;
                                    break;
                                case "Copyright Notice":
                                    imageInfo.CopyrightNotice = tag.Description ?? string.Empty;
                                    break;
                                case "Copyright Flag":
                                    imageInfo.CopyrightFlag = ParseNullableBoolean(tag.Description);
                                    break;
                                case "Image Width":
                                    imageInfo.ImageWidth = ParseNullableInt(tag.Description);
                                    break;
                                case "Image Height":
                                    imageInfo.ImageHeight = ParseNullableInt(tag.Description);
                                    break;
                                case "Has Alpha":
                                    imageInfo.HasAlpha = ParseNullableBoolean(tag.Description);
                                    break;
                                case "Is Animation":
                                    imageInfo.IsAnimation = ParseNullableBoolean(tag.Description);
                                    break;
                            }
                        }
                    }

                    // Reset the memory stream position for PHash computation
                    memoryStream.Position = 0;

                    // Compute PHash (implementation not shown)
                    imageInfo.PHash = ComputePHash(memoryStream);
                }
            }

            return imageInfo;
        }

        private static bool? ParseNullableBoolean(string? description)
        {
            if (description == null)
                return null;

            return string.Equals(description, "true", StringComparison.InvariantCultureIgnoreCase);
        }
        private static int? ParseNullableInt(string? description)
        {
            if (description == null)
                return null;

            if (int.TryParse(description, out var value))
                return value;

            return null;
        }

        private static long ComputePHash(Stream imageStream)
        {
            return 0;
            //imageStream.Seek(0, SeekOrigin.Begin); // Reset stream position

            //// Create a Bitmap from the stream
            //using (var image = new MagickImage(imageStream))
            //{
            //    if (image is Bitmap)
            //    {
            //        var bmp = (Bitmap)image; // More compact in C#7 but not sure whether that's supported in AWS Lambda: if (image is Bitmap bmp)
            //        // Compute the PHash
            //        var digest = ImagePhash.ComputeDigest(bmp.ToLuminanceImage());

            //        // Convert the digest to a long for easier storage and comparison
            //        return ConvertDigestToLong(digest);
            //    }
            //    else
            //        throw new Exception($"Image type is unsupported {image.GetType()}");
            //}
        }
        private static long ConvertDigestToLong(Digest digest)
        {
            long result = 0;
            int shift = 0;
            foreach (var coeff in digest.Coefficents)
            {
                // This loop combines the coefficients into a single long value
                // Adjust the logic here depending on how you wish to handle the hash
                result |= ((long)coeff > 0 ? 1L : 0L) << shift;
                shift++;
            }
            return result;
        }
    }
}
