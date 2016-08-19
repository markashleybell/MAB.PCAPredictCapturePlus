using System.Collections.Generic;

namespace MAB.PCAPredictCapturePlus
{
    /// <summary>
    /// Represents the result of a Capture Plus Retrieve query.
    /// </summary>
    public class CapturePlusRetrieveResult
    {
        /// <summary>
        /// A list of <see cref="CapturePlusRetrieveItem"/> objects representing the query results.
        /// </summary>
        public IEnumerable<CapturePlusRetrieveItem> Items { get; private set; }
        /// <summary>
        /// A <see cref="CapturePlusError"/> containing error information.
        /// </summary>
        public CapturePlusError Error { get; private set; }

        /// <summary>
        /// Constructor for a successful query result.
        /// </summary>
        /// <param name="items">A list of <see cref="CapturePlusRetrieveItem"/> objects representing the query results.</param>
        public CapturePlusRetrieveResult(IEnumerable<CapturePlusRetrieveItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// Constructor for a query which resulted in an error.
        /// </summary>
        /// <param name="error">The <see cref="CapturePlusError"/> containing error information.</param>
        public CapturePlusRetrieveResult(CapturePlusError error)
        {
            Items = new List<CapturePlusRetrieveItem>();
            Error = error;
        }
    }
}
