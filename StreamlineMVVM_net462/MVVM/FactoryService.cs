using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StreamlineMVVM
{
    public static class FactoryService
    {
        private class FactoryReference
        {
            public ViewModelBase ViewModelBaseReference { get; set; } = null;
            public Window WindowReference { get; set; } = null;
        }

        private static List<FactoryReference> factoryList = new List<FactoryReference>();

        public static void Register(ViewModelBase viewModelBase, Window window)
        {
            factoryList.Add(new FactoryReference { ViewModelBaseReference = viewModelBase, WindowReference = window });
        }

        public static void DeRegister(ViewModelBase viewModelBase)
        {
            try
            {
                int removed = factoryList.RemoveAll(f => f.ViewModelBaseReference == viewModelBase);
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error removing viewmodel from window factory reference list.", Ex);
            }
        }

        public static Window GetWindowReference(ViewModelBase viewModelBase)
        {
            if (viewModelBase == null)
            {
                return null;
            }

            try
            {
                Window window = factoryList.FirstOrDefault(w => w.ViewModelBaseReference == viewModelBase).WindowReference;
                return window;
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error getting window reference from window factory reference list.", Ex);
                return null;
            }
        }
    }
}
