using Mechly.DfmAddIn.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mechly.DfmAddIn.ViewModels
{
    // This is the main ViewModel for the UI. It holds the list of all issues.
    public class DfmIssuesVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DfmIssueVM m_ActiveIssue;

        public DfmIssuesVM()
        {
            Issues = new ObservableCollection<DfmIssueVM>();
        }

        public ObservableCollection<DfmIssueVM> Issues { get; private set; }

        public DfmIssueVM ActiveIssue
        {
            get => m_ActiveIssue;
            set
            {
                m_ActiveIssue = value;
                NotifyChanged();

                // When an issue is selected in the list, highlight it in SOLIDWORKS
                if (m_ActiveIssue?.EntityToSelect != null)
                {
                    m_ActiveIssue.EntityToSelect.Select(true);
                }
            }
        }

        // This is called from the rebuild event to refresh the list in the UI
        public void UpdateIssues(List<DfmIssue> newIssues)
        {
            Issues.Clear();

            foreach (var issueData in newIssues)
            {
                var issueVm = new DfmIssueVM(issueData);
                Issues.Add(issueVm);
            }
        }

        private void NotifyChanged([CallerMemberName] string prpName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prpName));
        }
    }
}