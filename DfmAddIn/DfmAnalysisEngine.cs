using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using Mechly.DfmAddIn.Models;

namespace Mechly.DfmAddIn
{
    public class DfmAnalysisEngine
    {
        // Rule 1: Holes must be >= 3mm in diameter.
        private const double MIN_HOLE_DIAMETER = 3.0 / 1000; // SOLIDWORKS API uses meters

        // This is the main analysis method. It takes the active model and returns a list of problems.
        public List<DfmIssue> RunChecks(IModelDoc2 model)
        {
            var issues = new List<DfmIssue>();
            var part = model as IPartDoc;

            if (part == null)
            {
                return issues; // Not a part document, do nothing.
            }

            // --- MINIMUM HOLE DIAMETER CHECK ---
            var bodies = part.GetBodies2((int)swBodyType_e.swSolidBody, false) as object[];
            if (bodies == null) return issues;

            foreach (IBody2 body in bodies)
            {
                var faces = body.GetFaces() as object[];
                if (faces == null) continue;

                foreach (IFace2 face in faces)
                {
                    var surface = face.GetSurface() as ISurface;


                    // Check if the face is a cylinder and is an internal face (a hole)
                    if (surface != null && surface.IsCylinder() && !face.FaceInSurfaceSense())
                    {
                        var cylindricalSurface = surface as ICylindricalSurface;
                        var cylinderParams = cylindricalSurface.GetCylinderParams() as double[];
                        double diameter = cylinderParams[3] * 2;

                        if (diameter < MIN_HOLE_DIAMETER)
                        {
                            var issue = new DfmIssue()
                            {
                                Description = $"Hole is too small: {diameter * 1000:F2}mm",
                                Severity = Severity_e.High,
                                EntityToSelect = face as IEntity
                            };
                            issues.Add(issue);
                        }
                    }
                }
            }

            // --- ADD MORE CHECKS HERE IN THE FUTURE ---

            return issues;
        }
    }
}