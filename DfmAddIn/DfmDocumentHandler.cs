using CodeStack.SwEx.AddIn.Core;
using SolidWorks.Interop.sldworks;
using System;
using Mechly.DfmAddIn.ViewModels;

namespace Mechly.DfmAddIn
{
    public class DfmDocumentHandler : DocumentHandler
    {
        public event Action<DfmIssuesVM> ShowIssues;

        private DfmIssuesVM m_IssuesVm;
        private DfmAnalysisEngine m_AnalysisEngine;

        public override void OnInit()
        {
            m_AnalysisEngine = new DfmAnalysisEngine();
            m_IssuesVm = new DfmIssuesVM();
            var part = this.Model as IPartDoc;
            if (part != null)
            {
                // Explicitly cast 'part' to the PartDoc CLASS to access the event
                ((PartDoc)part).FileRebuildNotify += OnModelRebuilt;
            }
        }

        private int OnModelRebuilt()
        {
            // Run the DFM checks on the current model
            var issues = m_AnalysisEngine.RunChecks(this.Model);

            // Update the UI with the new list of issues
            m_IssuesVm.UpdateIssues(issues);

            return 0;
        }

        public override void OnActivate()
        {
            // When the user switches to this document's window, update the Task Pane
            ShowIssues?.Invoke(m_IssuesVm);
        }

        public override void OnDestroy()
        {
            // Clean up the event subscription
            var part = this.Model as IPartDoc;
            if (part != null)
            {
                // Explicitly cast 'part' to the PartDoc CLASS to access the event
                ((PartDoc)part).FileRebuildNotify -= OnModelRebuilt;
            }
        }
    }
}