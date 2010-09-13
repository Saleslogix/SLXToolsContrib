using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage.Platform.QuickForms;
using Sage.Platform.QuickForms.Controls;
using System.Text.RegularExpressions;

namespace QuickFormDiff
{
    public class PropertyDifference
    {
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public PropertyDifference(string propertyName, string oldValue, string newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class ResourceDifference
    {
        public string ResourceName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public ResourceDifference(string resourceName, string oldValue, string newValue)
        {
            ResourceName = resourceName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class ControlPair
    {
        public IQuickFormsControl NewControl { get; private set; }
        public IQuickFormsControl OldControl { get; private set; }

        public ControlPair(IQuickFormsControl newControl, IQuickFormsControl oldControl)
        {
            NewControl = newControl;
            OldControl = oldControl;
        }
    }

    public class KeyComparer : IEqualityComparer<KeyValuePair<string, string>>
    {
        public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(KeyValuePair<string, string> obj)
        {
            return obj.Key.GetHashCode();
        }
    }

    public class QuickFormComparison
    {
        //old and new state
        private Dictionary<string, string> _oldFormProperties;
        private Dictionary<string, string> _newFormProperties;
        public Dictionary<string, string> OldFormResources { get; private set; }
        public Dictionary<string, string> NewFormResources { get; private set; }
        public IQuickFormDefinition OldForm { get; private set; }
        public IQuickFormDefinition NewForm { get; private set; }

        //property differences
        public IEnumerable<KeyValuePair<string, string>> AddedProperties { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> RemovedProperties { get; private set; }
        public IEnumerable<PropertyDifference> ModifiedProperties { get; private set; }
        
        //control/placement differences
        public IEnumerable<IQuickFormsControl> ControlsAdded { get; private set; }
        public IEnumerable<IQuickFormsControl> ControlsRemoved { get; private set; }
        public IEnumerable<ControlPair> ControlsPossiblyRenamed { get; private set; }
        public IEnumerable<ControlPair> ControlsMoved { get; private set; }
        public string RowAndColumnChangeSummary { get; private set; }
        
        //resource differences
        public IEnumerable<KeyValuePair<string, string>> ResourcesAdded { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> ResourcesRemoved { get; private set; }
        public IEnumerable<ResourceDifference> ResourcesModified { get; private set; }

        public QuickFormComparison(Dictionary<string, string> oldFormProperties, 
            Dictionary<string, string> newFormProperties,
            Dictionary<string, string> oldFormResources,
            Dictionary<string, string> newFormResources, 
            IQuickFormDefinition oldForm, 
            IQuickFormDefinition newForm)
        {
            _oldFormProperties = oldFormProperties;
            _newFormProperties = newFormProperties;
            OldFormResources = oldFormResources;
            NewFormResources = newFormResources;
            OldForm = oldForm;
            NewForm = newForm;
        }        

        public void Compare()
        {
            AddedProperties = _newFormProperties.Except(_oldFormProperties, new KeyComparer());
            RemovedProperties = _oldFormProperties.Except(_newFormProperties, new KeyComparer());
            ModifiedProperties = from newProperty in _newFormProperties
                                 join oldProperty in _oldFormProperties on newProperty.Key equals oldProperty.Key
                                 where newProperty.Value != oldProperty.Value
                                 select new PropertyDifference(newProperty.Key, oldProperty.Value, newProperty.Value);

            ControlsAdded = NewForm.AllControls.Except(OldForm.AllControls, new QFControlComparer());
            ControlsRemoved = OldForm.AllControls.Except(NewForm.AllControls, new QFControlComparer());
            
            //filter out added properties from added controls
            var addedControlSearchStrings =
                ControlsAdded.Select(control => string.Format("Elements[{0}]", control.ControlId));            
            var propsFromAddedControls = AddedProperties.Where(addedProp => 
                    addedControlSearchStrings.Where(searchValue =>
                                                    addedProp.Key.Contains(searchValue)).Any());
            AddedProperties = AddedProperties.Except(propsFromAddedControls);

            //filter out removed properties from removed controls
            var removedControlSearchStrings =
                ControlsRemoved.Select(control => string.Format("Elements[{0}]", control.ControlId));
            var propsFromRemovedControls = RemovedProperties.Where(removedProp =>
                    removedControlSearchStrings.Where(searchValue =>
                                                    removedProp.Key.Contains(searchValue)).Any());
            RemovedProperties = RemovedProperties.Except(propsFromRemovedControls);

            GenerateRowAndColumnChangeSummary();
            IdentifyControlsMoved();
            FindControlRenames();
            IdentifyResourceChanges();
        }

        private void IdentifyResourceChanges()
        {
            ResourcesAdded = NewFormResources.Where(pair => !OldFormResources.Keys.Contains(pair.Key)).OrderBy(pair => pair.Key);
            ResourcesRemoved = OldFormResources.Where(pair => !NewFormResources.Keys.Contains(pair.Key)).OrderBy(pair => pair.Key);
            ResourcesModified = from oldResource in OldFormResources
                                join newResource in NewFormResources
                                on oldResource.Key equals newResource.Key
                                where oldResource.Value != newResource.Value
                                orderby oldResource.Key
                                select new ResourceDifference(oldResource.Key, oldResource.Value, newResource.Value);
        }

        private void FindControlRenames()
        {
            ControlsPossiblyRenamed =
                from added in ControlsAdded
                join removed in ControlsRemoved on added.GetType() equals removed.GetType()
                where AnyMatchingDatabindings(removed.DataBindings, added.DataBindings)
                select new ControlPair(added, removed);
        }

        private bool AnyMatchingDatabindings(QuickFormDataBindings oldBindings, 
            QuickFormDataBindings newBindings)
        {
            return oldBindings.Join
                    (newBindings,
                    oldBinding => oldBinding,
                    newBinding => newBinding,
                    (newBinding, oldBinding) => newBinding,
                    new DataBindingComparer())
                 .Any();
        }

        private void IdentifyControlsMoved()
        {
            ControlsMoved = from oldControl in OldForm.AllControls
                            join newControl in NewForm.AllControls
                                on oldControl.ControlId equals newControl.ControlId
                            where (oldControl.Column != newControl.Column)
                                  || (oldControl.Row != newControl.Row)
                            select new ControlPair(newControl, oldControl);

            var regex = new Regex(@"Elements\[.*\]\(.*\)\.Control\(.*\)\.(Column|Row)\(Int32\)");
            ModifiedProperties = ModifiedProperties.Where(prop => !regex.IsMatch(prop.PropertyName));
        }

        private void GenerateRowAndColumnChangeSummary()
        {
            StringBuilder sb = new StringBuilder();

            int totalColumnDiff = NewForm.Columns.Count - OldForm.Columns.Count;
            if (totalColumnDiff == 0)
                sb.AppendLine("The total number of columns stayed the same.");
            else if (totalColumnDiff < 0)
            {                
                sb.AppendLine(string.Format("{0} columns were removed.", Math.Abs(totalColumnDiff)));

                var columns = OldForm.Columns;
                for (int i = columns.Count - Math.Abs(totalColumnDiff); i < columns.Count; i++)
                {
                    //remove entry from removed properties
                    int args = i;
                    RemovedProperties =
                        RemovedProperties.Where(prop => !prop.Key.StartsWith(string.Format("Columns[{0}]", args)));
                }
            }
            else
            {
                var columns = NewForm.Columns;
                for (int i = columns.Count - totalColumnDiff; i < columns.Count; i++)
                {
                    sb.AppendLine(string.Format("Column {0} was added (SizeType={1}, Width={2})",
                        i, columns[i].SizeType, columns[i].Width));

                    //remove entry from added properties
                    int args = i;
                    AddedProperties =
                        AddedProperties.Where(prop => !prop.Key.StartsWith(string.Format("Columns[{0}]", args)));
                }
            }

            int totalRowDiff = NewForm.Rows.Count - OldForm.Rows.Count;
            if (totalRowDiff == 0)
                sb.AppendLine("The total number of rows stayed the same.");
            else if (totalRowDiff < 0)
            {
                sb.AppendLine(string.Format("{0} rows were removed.", Math.Abs(totalRowDiff)));

                var rows = OldForm.Rows;
                for (int i = rows.Count - Math.Abs(totalRowDiff); i < rows.Count; i++)
                {
                    //remove entry from removed properties
                    int args = i;
                    RemovedProperties =
                        RemovedProperties.Where(prop => !prop.Key.StartsWith(string.Format("Rows[{0}]", args)));
                }
            }
            else
            {
                var rows = NewForm.Rows;
                for (int i = rows.Count - totalRowDiff; i < rows.Count; i++)
                {
                    sb.AppendLine(string.Format("Row {0} was added (SizeType={1}, Height={2})",
                        i, rows[i].SizeType, rows[i].Height));

                    //remove entry from added properties
                    int args = i;
                    AddedProperties =
                        AddedProperties.Where(prop => !prop.Key.StartsWith(string.Format("Rows[{0}]", args)));
                }
            }

            RowAndColumnChangeSummary = sb.ToString();
        }
    }

    public class QFControlComparer : IEqualityComparer<IQuickFormsControl>
    {
        public bool Equals(IQuickFormsControl x, IQuickFormsControl y)
        {
            return x.ControlId == y.ControlId;
        }

        public int GetHashCode(IQuickFormsControl obj)
        {
            return obj.ControlId.GetHashCode();
        }
    }

    public class DataBindingComparer : IEqualityComparer<IQuickFormDataBindingDefinition>
    {
        public bool Equals(IQuickFormDataBindingDefinition x, IQuickFormDataBindingDefinition y)
        {
            return x.ControlItemName == y.ControlItemName
                && x.DataItemName == y.DataItemName
                && x.DataSourceID == y.DataSourceID;
        }

        public int GetHashCode(IQuickFormDataBindingDefinition obj)
        {
            return (obj.ControlItemName + obj.DataItemName + obj.DataSourceID).GetHashCode();
        }
    }
}