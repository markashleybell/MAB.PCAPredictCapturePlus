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
        /// A summary of the address data.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// A list of number ranges identifying the characters to highlight in the Text response (zero-based start position and end).
        /// </summary>
        public string Highlight { get; set; }
        /// <summary>
        /// A zero-based position in the Text response indicating the suggested position of the cursor if this item is selected. A -1 response indicates no suggestion is available.
        /// </summary>
        public int Cursor { get; set; }
        /// <summary>
        /// Descriptive information about the address, typically if it's a container.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The next step of the search process (Find or Retrieve).
        /// </summary>
        public string Next { get; set; }
    }
}
