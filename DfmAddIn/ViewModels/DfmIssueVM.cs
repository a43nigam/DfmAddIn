using Mechly.DfmAddIn.Models;
using SolidWorks.Interop.sldworks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mechly.DfmAddIn.ViewModels
{
    // This class wraps a single DfmIssue for the UI.
    public class DfmIssueVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DfmIssue m_Issue;

        public DfmIssueVM(DfmIssue issue)
        {
            m_Issue = issue;
        }

        public string Description => m_Issue.Description;
        public Severity_e Severity => m_Issue.Severity;
        public IEntity EntityToSelect => m_Issue.EntityToSelect;
    }
}