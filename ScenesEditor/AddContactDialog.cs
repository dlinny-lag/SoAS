using System.Drawing;
using System.Windows.Forms;
using SceneModel;

namespace ScenesEditor
{
    public sealed class AddContactDialog : Form
    {
        private ContactEditorControl editor;
        private Button okBtn;
        private Button cancelBtn;

        private void Initialize(bool environment)
        {
            editor = new ContactEditorControl(environment, true);
            okBtn = new Button();
            okBtn.Text = "OK";
            okBtn.Width = 50;
            okBtn.Click += OkBtn_Click;
            cancelBtn = new Button();
            cancelBtn.Text = "Cancel";
            cancelBtn.Width = 50;
            cancelBtn.Click += CancelBtn_Click;

            Size = new Size(270, 200);
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;

            Controls.Add(editor);
            Controls.Add(okBtn);
            Controls.Add(cancelBtn);
        }

        void SetSize()
        {
            ClientSize = new Size(editor.Width, editor.Height + okBtn.Height + Margin.Vertical * 2);
            SetButtonsLocation();
            MinimumSize = Size;
            ClientSizeChanged += (sender, args) => SetButtonsLocation();
        }

        private void CancelBtn_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OkBtn_Click(object sender, System.EventArgs e)
        {
            if (editor.IsEnvironmentContact)
            {
                if (Environmental?.Details.Contact == null)
                    return;
            }
            else
            {
                if (ActorsContact?.From.Contact == null || ActorsContact?.To.Contact == null)
                    return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SetButtonsLocation()
        {
            int xCenter = ClientSize.Width / 2;
            okBtn.Location = new Point(xCenter-okBtn.Width-Margin.Horizontal, ClientSize.Height - okBtn.Height - Margin.Vertical);
            cancelBtn.Location = new Point(xCenter+Margin.Horizontal, ClientSize.Height - cancelBtn.Height - Margin.Vertical);
        }

        public AddContactDialog(bool environmentContact)
        {
            Initialize(environmentContact);
            SetButtonsLocation();
        }

        public void InitEnvironmentContact(Scene scene, int actorIndex)
        {
            Environmental = new EnvironmentContact
            {
                Details =
                {
                    ParticipantIndex = actorIndex,
                    Participant = scene.Participants[actorIndex]
                }
            };
            editor.InitEnvironmentContact(scene, Environmental);
            SetSize();
            Text = scene.ParticipantTitle(actorIndex);
        }

        public bool FromActor { get; set; }

        private void UpdateTitle(int index)
        {
            Text = $"{(FromActor?"From":"To")} {editor.Scene.ParticipantTitle(index)}";
        }

        public void InitActorsContact(Scene scene, int fromActorIndex, int toActorIndex)
        {
            ActorsContact = new ActorsContact()
            {
                From = { ParticipantIndex = fromActorIndex },
                To = { ParticipantIndex = toActorIndex }
            };
            ActorsContact.MakeAlive(scene.Participants);
            editor.InitActors(scene, FromActor?ActorsContact.From:ActorsContact.To, FromActor?ActorsContact.To:ActorsContact.From);
            SetSize();
            UpdateTitle(fromActorIndex);
        }

        public EnvironmentContact Environmental { get; private set; }
        
        public ActorsContact ActorsContact { get; private set; }
    }
}