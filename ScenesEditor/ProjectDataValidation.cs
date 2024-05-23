using System;
using System.Linq;
using SceneModel;
using ScenesEditor.Data;

namespace ScenesEditor
{
    [Flags]
    public enum ValidationChanges
    {
        None = 0,
        ContactId = 1,
    }

    public static class ProjectDataValidation
    {
        /// <summary>
        /// Returns true if validation process changes something
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static ValidationChanges ValidateData(this Project project)
        {
            ValidationChanges change = ValidationChanges.None;
            change |= InitContactIds(project);
            return change;
        }

        private static ValidationChanges InitContactIds(Project project)
        {
            ValidationChanges change = ValidationChanges.None;
            foreach (IHasId contact in project.Scenes.SelectMany(s => s.ActorsContacts.Union<IHasId>(s.EnvironmentContacts)))
            {
                if (contact.Id == Guid.Empty)
                {
                    change = ValidationChanges.ContactId;
                    contact.Id = Guid.NewGuid();
                }
            }

            return change;
        }
    }
}