using System;
using System.Collections.Generic;
using System.IO;
using AAF.Services.AAFImport;
using AAF.Services.Errors;
using AAFModel;

namespace AAF.Services
{
    public static class FileExport
    {
        public static void ExportText(this Stream stream, AAFData aafModel, ImportResult result)
        {
            if (aafModel == null)
                throw new ArgumentNullException(nameof(aafModel));
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            StreamWriter sw = new StreamWriter(stream);
            sw.WriteAAFErrors(aafModel);
            sw.WriteImportErrors(result.Errors);
            sw.Flush();
        }

        private static void WriteImportErrors(this StreamWriter sw, AAFImportErrors errors)
        {
            foreach (var pair in errors.RaceDuplications)
            {
                sw.WriteLine(pair.Value.Report());
            }
            foreach (var pair in errors.AnimationDuplications)
            {
                sw.WriteLine(pair.Value.Report());
            }
            foreach (var pair in errors.AnimationGroupDuplications)
            {
                sw.WriteLine(pair.Value.Report());
            }
            foreach (var pair in errors.PositionTreeDuplications)
            {
                sw.WriteLine(pair.Value.Report());
            }
            foreach (var pair in errors.PositionDuplications)
            {
                sw.WriteLine(pair.Value.Report());
            }

            foreach (var pair in errors.ValidationErrors)
            {
                foreach (PositionError e in pair.Value)
                {
                    sw.WriteLine(e.Report());
                }
            }
        }

        private static void WriteAAFErrors(this StreamWriter sw, AAFData aafModel)
        {
            sw.WriteFileErrors(aafModel.FailedFiles);
            sw.WriteLine();
            sw.WriteErrorList(aafModel.Errors);
            sw.WriteLine();
            sw.WriteErrorList(aafModel.Warnings);
            sw.WriteLine();
        }

        private static void WriteFileErrors(this StreamWriter sw, IDictionary<string, LoadException> errors)
        {
            foreach (var pair in errors)
            {
                sw.WriteLine();
                sw.WriteLine(pair.Key); // same as in FailedFilesList.DataGridOnCellValueNeeded. TODO: avoid duplication
                sw.Write("\t");
                sw.WriteLine(pair.Value.InnerException.Message); // same as in FileLoadFailureList.GetData. TODO: avoid duplication
            }
        }

        private static void WriteErrorList(this StreamWriter sw, IDictionary<string, IList<string>> errs)
        {
            foreach (var pair in errs)
            {
                sw.WriteLine();
                sw.WriteLine(pair.Key);
                foreach (string err in pair.Value)
                {
                    sw.WriteLine(err);
                }
            }
        }
    }
}