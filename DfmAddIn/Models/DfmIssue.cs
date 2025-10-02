using Mechly.DfmAddIn.Models;
using SolidWorks.Interop.sldworks;

namespace Mechly.DfmAddIn.Models
{
    // This class holds the information for a single DFM problem.
    // It's a simple data container.
    public class DfmIssue
    {
        public string Description { get; set; }
        public Severity_e Severity { get; set; }
        public IEntity EntityToSelect { get; set; }
    }
}