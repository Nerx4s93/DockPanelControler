using System.Collections.Generic;

namespace DockPanelControler
{
    internal class GlobalFormManager
    {
        public static List<DockableFormBase> FormCollection = new List<DockableFormBase>();

        public static List<DockPanel> DockPanels = new List<DockPanel>();

        public static void AddForm(DockableFormBase form)
        {
            foreach (var dockPanel in DockPanels)
            {
                dockPanel?.AddFormInternal(form);
            }
            FormCollection.Add(form);
        }
    }
}
