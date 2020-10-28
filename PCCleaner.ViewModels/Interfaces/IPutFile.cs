using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCleaner.ViewModels.Interfaces
{
    public interface IPutFile
    {
        public string HelloText { get; set; }
        public string LetsPutThingsInOrderText { get; set; }
        public ICommand FileOpenFolderButton { get; set; }
        public string AddFoldersText { get; set; }
    }
}
