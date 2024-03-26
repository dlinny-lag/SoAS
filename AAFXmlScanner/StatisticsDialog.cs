using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AAF.Services.AAFImport;
using AAF.Services.Errors;
using AAFModel;

namespace AAFXmlScanner
{
    public partial class StatisticsDialog : Form
    {
        public StatisticsDialog(AAFData aafModel, ImportResult result)
        {
            InitializeComponent();
            statisticsText.Lines = GenerateText(aafModel, result);
        }

        private static string[] GenerateText(AAFData aafModel, ImportResult result)
        {
            List<string> retVal = new List<string>(10);

            retVal.Add($"Total files: {aafModel.Files.Count}. Failed to proceed: {aafModel.FailedFiles.Count}");
            retVal.Add($"Animation declarations: {aafModel.Animations.Count}. Unique animation declarations: {result.Animations.Count}");
            retVal.Add($"Animation group declarations: {aafModel.AnimationGroups.Count}. Unique animation group declarations: {result.AnimationGroups.Count}");
            retVal.Add($"Position tree declarations: {aafModel.PositionTrees.Count}. Unique position tree declarations: {result.PositionsTrees.Count}");
            retVal.Add($"Position declarations: {aafModel.Positions.Count}. Unique position declarations: {result.Positions.Count}");

            var acceptableByAff = result.Positions.Where(
                pair => !result.Errors.ValidationErrors.TryGetValue(pair.Key, out var errs) || // is not erroneous
                        // see aaf.model.xml.position.PositionNode.initialize
                        // AAF rejects positions with missing reference and with reference to a tree with missing references
                        errs.All(e => !(e is MissingReferenceError) && !(e is TreeHasPositionWithMissingReference))
            );
            retVal.Add($"Positions available in AAF: {acceptableByAff.Count()}");

            var dangerous = result.Errors.ValidationErrors.Where(
                    pair => pair.Value.Any(e => !(e is MissingReferenceError) && !(e is TreeHasPositionWithMissingReference)))
                .ToList();

            retVal.Add($"Position(s) that may broke AAF: {dangerous.Count}");
            for (int i = 0; i < dangerous.Count; i++)
            {
                retVal.Add($"  {dangerous[i].Key}");
            }

            return retVal.ToArray();
        }

    }
}
