using System.Collections.Generic;

namespace MAB.PCAPredictCapturePlus
{
    /// <summary>
    /// Represents the result of a Capture Plus Find query.
    /// </summary>
    public class CapturePlusFindResult
    {
        /// <summary>
        /// A list of <see cref="CapturePlusFindItem"/> objects representing the query results.
        /// </summary>
        public IEnumerable<CapturePlusFindItem> Items { get; private set; }
        /// <summary>
        /// A <see cref="CapturePlusError"/> containing error information.
        /// </summary>
        public CapturePlusError Error { get; private set; }

        /// <summary>
        /// Constructor for a successful query result.
        /// </summary>
        /// <param name="items">A list of <see cref="CapturePlusFindItem"/> objects representing the query results.</param>
        public CapturePlusFindResult(IEnumerable<CapturePlusFindItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// Constructor for a query which resulted in an error.
        /// </summary>
        /// <param name="error">The <see cref="CapturePlusError"/> containing error information.</param>
        public CapturePlusFindResult(CapturePlusError error)
        {
            Items = new List<CapturePlusFindItem>();
            Error = error;
        }
    }
}
