﻿using System.Collections.Generic;

namespace DockPanelControler
{
    internal class GlobalFormManager
    {
        public static List<DockableFormBase> FormCollection = new List<DockableFormBase>();

        public static List<DockPanel> DockPanels = new List<DockPanel>();

        public static void AddForm(DockableFormBase dockableFormBase)
        {
            var activeDockPanels = DockPanels.FindAll(x => x != null).FindAll(x => x.IsDisposed == false);

            foreach (var dockPanel in activeDockPanels)
            {
                dockPanel?.AddFormInternal(dockableFormBase);
            }
            FormCollection.Add(dockableFormBase);
        }
    }
}
