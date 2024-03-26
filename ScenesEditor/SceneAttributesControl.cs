using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using SceneModel;
using SceneServices;

namespace ScenesEditor
{
    public sealed partial class SceneAttributesControl : UserControl
    {
        public SceneAttributesControl()
        {
            InitializeComponent();
        }

        public event Action Changed; // TODO: use it


        public void Init(Scene scene)
        {
            customAttributesJsonEditor.SetJsonSource(scene.GetCustomAttributes());
        }
        public JObject CustomAttributes => (JObject)customAttributesJsonEditor.Root;
    }
}
