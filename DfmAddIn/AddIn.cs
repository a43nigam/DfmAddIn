using CodeStack.SwEx.AddIn;
using CodeStack.SwEx.AddIn.Attributes;
using CodeStack.SwEx.AddIn.Base;
using CodeStack.SwEx.AddIn.Core;
using Mechly.DfmAddIn.Properties;
using Mechly.DfmAddIn.ViewModels;
using Mechly.DfmAddIn.Views;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;

// The namespace is simplified to match the rest of your project
namespace Mechly.DfmAddIn
{
    [AutoRegister("DFM Validator")]
    [Guid("160F5F50-CB41-4604-8DED-8E4989E6B572")] // This GUID should be unique for your add-in
    [ComVisible(true)]
    public class AddIn : SwAddInEx
    {
        // --- All of the obsolete button command logic has been removed ---
        // The IssuesMgrCommands_e enum has been deleted.
        // The OnTaskPaneButtonClicked method has been deleted.

        private IDocumentsHandler<DfmDocumentHandler> m_DocHandler;
        private IssuesControl m_IssuesControl;

        public override bool OnConnect()
        {
            // Note the class name here should match the class in your IssuesDocument.cs file
            m_DocHandler = CreateDocumentsHandler<DfmDocumentHandler>();
            m_DocHandler.HandlerCreated += OnDocHandlerCreated;

            // This call is now simplified. It creates the Task Pane without any command buttons.
            IssuesControlHost ctrlHost;
            CreateTaskPane<IssuesControlHost>(out ctrlHost);

            m_IssuesControl = ctrlHost.IssuesControl;
            return true;
        }

        private void OnDocHandlerCreated(DfmDocumentHandler docHandler)
        {
            docHandler.Destroyed += OnIssuesDocDestroyed;
            docHandler.ShowIssues += OnShowIssues;
        }

        private void OnIssuesDocDestroyed(DocumentHandler docHandler)
        {
            // This logic is still good. It clears the UI when the last document is closed.
            if (App.IFrameObject().GetModelWindowCount() == 1)
            {
                m_IssuesControl.DataContext = null;
            }
        }

        private void OnShowIssues(DfmIssuesVM issuesVm)
        {
            // This is the final step: the data from a document (the ViewModel)
            // is assigned to the UI's DataContext, making the issues appear in the list.
            m_IssuesControl.DataContext = issuesVm;
        }
    }
}