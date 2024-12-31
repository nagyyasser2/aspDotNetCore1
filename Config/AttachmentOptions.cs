namespace aspDotNetCore.Config
{
    public class AttachmentOptions
    {
        public string AllowedExtentions { get; set; }
        public int MaxSizeInMegaBytes { get; set; }
        public bool EnableCompression { get; set; }
    }
}
