using System;
using System.Windows.Forms;
using SceneModel;

namespace ScenesEditor
{
    public sealed partial class ParticipantTabPage : TabPage
    {
        private readonly Participant participant;
        private readonly int index;

        public ParticipantTabPage(Scene scene, int participantIndex)
        {
            InitializeComponent();
            index = participantIndex;
            participant = scene.Participants[index];
            Text = scene.ParticipantTitle(index);
            FillAttributes();
        }
        
        private void FillAttributes()
        {
            victimCheckBox.Checked = participant.IsVictim;
            aggressorCheckBox.Checked = participant.IsAggressor;
        }

        public event Action<Participant, int> Changed;

        public void UpdateAfterChange()
        {
            FillAttributes();
        }

        private void victimCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            participant.IsVictim = victimCheckBox.Checked;
            Changed?.Invoke(participant, index);
        }

        private void aggressorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            participant.IsAggressor = aggressorCheckBox.Checked;
            Changed?.Invoke(participant, index);
        }
    }
}
