namespace MAB.PCAPredictCapturePlus
{
    /// <summary>
    /// Search result data returned by a successful Find query.
    /// </summary>
    public class CapturePlusFindItem
    {
        /// <summary>
        /// The unique address identifier.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The result type (Address, Postcode, Locality)
        /// </summary>
        public CapturePlusFindItemType Type { get; set; }
        /// <summary>
        /// A summary of the address data.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// A list of number ranges identifying the characters to highlight in the Text response (zero-based start position and end).
        /// </summary>
        public string Highlight { get; set; }
        /// <summary>
        /// Descriptive information about the address, typically if it's a container.
        /// </summary>
        public string Description { get; set; }
    }
}
