using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage.Platform.Application.UI;

namespace QuickFormDiff
{
    public class SimpleEditorService : IEditorService
    {
        public System.Collections.Generic.IEnumerable<IEditorInfo> AvailableEditorsForItem(IEditableItem editableItem)
        {
            throw new NotImplementedException();
        }

        public IEditorInfo DefaultEditorForItem(IEditableItem editableItem)
        {
            throw new NotImplementedException();
        }

        public void EditItemWithEditor(IEditorInfo editor, IEditableItem editableItem)
        {
            throw new NotImplementedException();
        }

        public IEditorInfo FindEditorInfoForEditor(object editor)
        {
            throw new NotImplementedException();
        }

        public void RegisterEditor(IEditorInfo editorInfo)
        {
            throw new NotImplementedException();
        }
    }
}
