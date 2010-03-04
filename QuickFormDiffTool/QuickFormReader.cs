using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Sage.Platform.QuickForms;
using System;
using Sage.Platform.QuickForms.ActionItems;
using Sage.Platform.QuickForms.Elements;
using Sage.Platform.QuickForms.Controls;
using Sage.Platform.FileSystem.Interfaces;
using System.Resources;

namespace QuickFormDiff
{
    public class QuickFormReader
    {
        public IQuickFormDefinition QuickForm { get; private set; }
        
        public Dictionary<string, string> FormProperties { get; private set; }
        public Dictionary<string, string> FormResources { get; private set; }

        public QuickFormReader(IQuickFormDefinition quickForm)
        {
            FormProperties = new Dictionary<string, string>();
            QuickForm = quickForm;
        }

        public Dictionary<string, string> ReadAllProperties()
        {
            FormProperties.Clear();
            AddPropertiesFromObject(QuickForm, string.Empty);
            ReadResources();
            return FormProperties;
        }

        private void ReadResources()
        {
            FormResources = new Dictionary<string, string>();

            var reader = new ResXResourceReader(QuickForm.FilePath.FullName + ".resx");
            foreach (DictionaryEntry resourceEntry in reader)
            {
                FormResources.Add((string)resourceEntry.Key, (string)resourceEntry.Value);
            }
        }

        private void AddPropertiesFromObject(object objectToRead, string parentName)
        {            
            var props = GetPropertyDescriptors(objectToRead);
            foreach (PropertyDescriptor property in props)
            {
                object propertyValue = property.GetValue(objectToRead);
                if (propertyValue == null)
                    continue;

                if (propertyValue is char && ((char)propertyValue == char.MinValue))
                    continue;

                if (propertyValue is ICollection)
                {
                    ReadCollection(parentName + property.Name + ".", propertyValue as ICollection);
                }
                else if (property.PropertyType.IsInterface)
                {
                    AddPropertiesFromObject(propertyValue, parentName + property.Name +
                        "(" + propertyValue.GetType().Name + ").");
                }
                else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    AddPropertiesFromObject(propertyValue, parentName + property.Name + 
                        "(" + propertyValue.GetType().Name + ").");
                }
                else
                {
                    string propertyTypeName = property.PropertyType.Name;
                    FormProperties.Add(parentName + property.Name + "(" + propertyTypeName + ")", propertyValue.ToString());
                }
            }
        }

        private static IEnumerable<PropertyDescriptor> GetPropertyDescriptors(object objectToRead)
        {
            var QFPropertiesToRead = new List<string>
                                       {
                                           "Id",
                                           "Name",
                                           "EntityTypeName",
                                           "DefaultNamespace",
                                           "AssemblyName",
                                           "UseEntityNameAsTitle",
                                           "ImagePath",
                                           "LoadAction",
                                           "DefinitionType",
                                           "Columns",
                                           "Rows",
                                           "GeneratePrintView",
                                           "Description",
                                           "ActiveControl",
                                           "LoadActions",
                                           "Elements",
                                           "ToolElements"
                                       };

            var InsertAssociationActionProps = new List<string>
                                                   {
                                                        "ParentRelationshipPropertyName",
                                                        "ChildRelationshipPropertyName",
                                                        "DataSource",
                                                        "SmartPart",
                                                        "Height",
                                                        "Width",
                                                        "Top",
                                                        "Left",
                                                        "ResourceKey",
                                                        "CenterDialog",
                                                        "PassResultsToNextAction",
                                                        "OnCompleteActionItem",
                                                   };

            var InsertChildDialogActionProps = new List<string>
                                                   {
                                                       "SmartPart",
                                                       "Height",
                                                       "Width",
                                                       "Top",
                                                       "Left",
                                                       "ResourceKey",
                                                       "ParentRelationshipPropertyName",
                                                       "DataSource",
                                                       "CenterDialog"
                                                   };

            var BusinessRuleActionProps = new List<string>
                                              {
                                                "BusinessRule",
                                                "ObjectName",
                                                "Parameters",
                                                "PassResultsToNextAction",
                                                "OnCompleteActionItem"
                                              };

            var CodeSnippetActionItemProps = new List<string>
                                                 {
                                                     "MethodId"
                                                 };

            var CSharpSnippetActionProps = new List<string>
                                                 {
                                                     "CSharpCodeSnippet"
                                                 };

            var DialogActionProps = new List<string>
                                        {
                                            "SmartPart",
                                            "Height", 
                                            "Width",
                                            "Top",
                                            "Left",
                                            "EntityType", 
                                            "ResourceKey",
                                            "CenterDialog"
                                        };

            var MessageActionProps = new List<string>
                                        {
                                            "ResourceKey"
                                        };

            var RedirectActionProps = new List<string>
                                         {
                                            "MainViewEntityName",
                                            "RawURL",
                                            "EntityViewMode",
                                            "UseCurrentIdInLink"
                                         };

            var RefreshDataActionProps = new List<string>
                                          {
                                          };

            var ResetEntityActionProps = new List<string>
                                             {
                                             };

            var UIActionProps = new List<string>
                                    {
                                        "PropertySettings"
                                    };

            var ValidationActionProps = new List<string>
                                    {
                                        "ValidationMethod",
                                        "OnFail",
                                        "OnSuccess"
                                    };

            var QuickFormElementProps = new List<string>
                                            {
                                                "EntityMappingType",
                                                "EntityTypeName",
                                                "EntityNamespace",
                                                "EntityAssemblyName",
                                                "DefaultNamespace",
                                                "AssemblyName",
                                                "EntityReferenceName",
                                                "ValueMember",
                                                "DesignerDisplayMember",
                                                "Control"
                                            };

            var props = TypeDescriptor.GetProperties(objectToRead)
                .Cast<PropertyDescriptor>()
                .Where(prop => !prop.Attributes.OfType<XmlIgnoreAttribute>().Any());

            //var props = props.Where(prop => prop.Attributes.)
            List<string> filterList = null;
            if (objectToRead is IQuickFormDefinition)
                filterList = QFPropertiesToRead;
            else if (objectToRead is InsertAssociationAction)
                filterList = InsertAssociationActionProps;
            else if (objectToRead is InsertChildDialogActionItem)
                filterList = InsertChildDialogActionProps;
            else if (objectToRead is BusinessRuleActionItem)
                filterList = BusinessRuleActionProps;
            else if (objectToRead is CodeSnippetActionItem)
                filterList = CodeSnippetActionItemProps;
            else if (objectToRead is CSharpSnippetActionItem)
                filterList = CSharpSnippetActionProps;
            else if (objectToRead is DialogActionItem)
                filterList = DialogActionProps;
            else if (objectToRead is MessageActionItem)
                filterList = MessageActionProps;
            else if (objectToRead is RedirectActionItem)
                filterList = RedirectActionProps;
            else if (objectToRead is RefreshDataAction)
                filterList = RefreshDataActionProps;
            else if (objectToRead is ResetEntityActionItem)
                filterList = ResetEntityActionProps;
            else if (objectToRead is UIActionItem)
                filterList = UIActionProps;
            else if (objectToRead is ValidationActionItem)
                filterList = ValidationActionProps;
            else if (objectToRead is QuickFormElement)
                filterList = QuickFormElementProps;
            
            if (filterList != null)
                return props.Where(prop => filterList.Contains(prop.Name));
            
            var nonBrowsableProps = props.Where(prop => prop.Attributes
                                            .OfType<BrowsableAttribute>()
                                            .Any(attrib => attrib.Browsable == false));
            props = props.Except(nonBrowsableProps);
            
            return props;
        }

        private void ReadCollection(string parentName, ICollection values)
        {
            int i = 0;
            foreach (var value in values)
            {
                Type valueType = value.GetType();
                string index = i.ToString();
                QuickFormElement element = value as QuickFormElement;
                if (element != null && element.Control != null && !string.IsNullOrEmpty(element.Control.ControlId))
                {
                    index = element.Control.ControlId;
                }
                string listItemTypeName = valueType.Name;
                AddPropertiesFromObject(value,
                    string.Format("{0}[{1}]({2}).", parentName.TrimEnd('.'), index, listItemTypeName));

                i++;
            }
        }
    }
}