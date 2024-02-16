namespace ImageInfoService
{
    public record ImageInfo
    {
        public long PHash { get; set; } = 0;
        public string Artist { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string CopyrightNotice { get; set; } = string.Empty;
        public bool? CopyrightFlag { get; set; } = null;
        public int? ImageWidth { get; set; } = null;
        public int? ImageHeight { get; set; } = null;
        public bool? HasAlpha { get; set; } = null; // Indicates the image contains alpha transparency information
        public bool? IsAnimation { get; set; } = null; 
    }
}
