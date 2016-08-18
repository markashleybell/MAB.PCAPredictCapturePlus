using System.Collections.Generic;

namespace MAB.PCAPredictCapturePlus
{
    public class CapturePlusRetrieveResult
    {
        public IEnumerable<CapturePlusRetrieveItem> Items { get; private set; }
        public CapturePlusError Error { get; private set; }

        public CapturePlusRetrieveResult(IEnumerable<CapturePlusRetrieveItem> items)
        {
            Items = items;
        }

        public CapturePlusRetrieveResult(CapturePlusError error)
        {
            Items = new List<CapturePlusRetrieveItem>();
            Error = error;
        }
    }
}
