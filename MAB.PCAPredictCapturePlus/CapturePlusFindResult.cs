using System.Collections.Generic;

namespace MAB.PCAPredictCapturePlus
{
    public class CapturePlusFindResult
    {
        public IEnumerable<CapturePlusFindItem> Items { get; private set; }
        public CapturePlusError Error { get; private set; }

        public CapturePlusFindResult(IEnumerable<CapturePlusFindItem> items)
        {
            Items = items;
        }

        public CapturePlusFindResult(CapturePlusError error)
        {
            Items = new List<CapturePlusFindItem>();
            Error = error;
        }
    }
}
