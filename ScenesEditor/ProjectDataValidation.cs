using System;
using System.Linq;
using SceneModel;
using ScenesEditor.Data;

namespace ScenesEditor
{
    public static class ProjectDataValidation
    {
        public static bool ValidateData(this Project project)
        {
            bool changed = false;
            changed |= InitContactIds(project);
            return changed;
        }

        private static bool InitContactIds(Project project)
        {
            bool changed = false;
            foreach (IHasId contact in project.Scenes.SelectMany(s => s.ActorsContacts.Union<IHasId>(s.EnvironmentContacts)))
            {
                if (contact.Id == Guid.Empty)
                {
                    changed = true;
                    contact.Id = Guid.NewGuid();
                }
            }

            return changed;
        }
    }
}