﻿using System.Collections.Generic;

using DockPanelControler.Components;

namespace DockPanelControler
{
    internal class GlobalFormManager
    {
        public static List<FormDockHandler> FormDockHandlerCollection = new List<FormDockHandler>();

        public static List<DockPanel> DockPanels = new List<DockPanel>();

        public static void AddForm(FormDockHandler dockableFormBase)
        {
            var activeDockPanels = DockPanels.FindAll(x => x != null).FindAll(x => x.IsDisposed == false);

            foreach (var dockPanel in activeDockPanels)
            {
                dockPanel?.AddFormInternal(dockableFormBase);
            }
            FormDockHandlerCollection.Add(dockableFormBase);
        }
    }
}
