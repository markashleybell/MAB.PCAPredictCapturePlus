namespace MAB.PCAPredictCapturePlus
{
    internal class CapturePlusFindItemRaw
    {
        // For successful results
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Highlight { get; set; }

        // Used by both error and successful results
        public string Description { get; set; }

        // For error results
        public int? Error { get; set; }
        public string Cause { get; set; }
        public string Resolution { get; set; }
    }
}
