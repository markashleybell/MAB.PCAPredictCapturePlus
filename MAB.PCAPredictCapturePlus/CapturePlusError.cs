namespace MAB.PCAPredictCapturePlus
{
    /// <summary>
    /// Information about an error which occurred while querying the Capture Plus API.
    /// </summary>
    public class CapturePlusError
    {
        #pragma warning disable 1591

        public int Error { get; set; }
        public string Description { get; set; }
        public string Cause { get; set; }
        public string Resolution { get; set; }

        #pragma warning restore 1591
    }
}
