﻿using System;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using MelonPrefManager.UI.InteractiveValues;
using UniverseLib.UI;
using UniverseLib;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace MelonPrefManager.UI
{
    public class CachedConfigEntry
    {
        public MelonPreferences_Entry RefConfig { get; }
        public InteractiveValue IValue;

        // UI
        public bool UIConstructed;
        public GameObject parentContent;
        public GameObject ContentGroup;
        public RectTransform ContentRect;
        public GameObject SubContentGroup;

        public Text mainLabel;

        internal GameObject UIroot;
        internal ButtonRef undoButton;

        public Type FallbackType => RefConfig.GetReflectedType();

        public CachedConfigEntry(MelonPreferences_Entry config, GameObject parent)
        {
            RefConfig = config;
            parentContent = parent;

            EnsureConfigValid();

            config.OnEntryValueChangedUntyped.Subscribe((object oldValue, object newValue) => { UpdateValue(); });

            CreateIValue(config.BoxedValue, FallbackType);
        }

        public void CreateIValue(object value, Type fallbackType)
        {
            IValue = InteractiveValue.Create(value, fallbackType);
            IValue.Owner = this;
            IValue.mainContentParent = ContentGroup;
            IValue.subContentParent = this.SubContentGroup;
        }

        private void EnsureConfigValid()
        {
            // MelonLoader does not support null config values. Ensure valid.
            if (RefConfig.BoxedValue == null)
            {
                if (FallbackType == typeof(string))
                    RefConfig.BoxedValue = "";
                else if (FallbackType.IsArray)
                    RefConfig.BoxedValue = Array.CreateInstance(FallbackType.GetElementType(), 0);
                else
                    RefConfig.BoxedValue = Activator.CreateInstance(FallbackType);

                RefConfig.BoxedEditedValue = RefConfig.BoxedValue;
            }
        }

        public void UpdateValue()
        {
            EnsureConfigValid();
            IValue.Value = RefConfig.BoxedEditedValue;

            IValue.OnValueUpdated();
            IValue.RefreshSubContentState();
        }

        public void SetValueFromIValue()
        {
            if (RefConfig.Validator != null)
                IValue.Value = RefConfig.Validator.EnsureValid(IValue.Value);

            var edited = RefConfig.BoxedEditedValue;
            if ((edited == null && IValue.Value == null) || (edited != null && edited.Equals(IValue.Value)))
                return;

            RefConfig.BoxedEditedValue = IValue.Value;
            UIManager.OnEntryEdit(this);
            undoButton.Component.gameObject.SetActive(true);
        }

        public void UndoEdits()
        {
            RefConfig.BoxedEditedValue = RefConfig.BoxedValue;
            IValue.Value = RefConfig.BoxedValue;
            IValue.OnValueUpdated();

            OnSaveOrUndo();
        }

        public void RevertToDefault()
        {
            RefConfig.ResetToDefault();
            RefConfig.BoxedEditedValue = RefConfig.BoxedValue;
            UpdateValue();
            OnSaveOrUndo();
        }

        internal void OnSaveOrUndo()
        {
            undoButton.Component.gameObject.SetActive(false);
            UIManager.OnEntryUndo(this);
        }

        public void Enable()
        {
            if (!UIConstructed)
            {
                ConstructUI();
                UpdateValue();
            }

            UIroot.SetActive(true);
            UIroot.transform.SetAsLastSibling();
        }

        public void Disable()
        {
            if (UIroot)
                UIroot.SetActive(false);
        }

        public void Destroy()
        {
            if (this.UIroot)
                GameObject.Destroy(this.UIroot);
        }

        internal void ConstructUI()
        {
            UIConstructed = true;

            UIroot = UIFactory.CreateVerticalGroup(parentContent, "ConfigEntry_" + this.RefConfig.Identifier, true, false, true, true, 0, 
                default, new Color(1,1,1,0));
            ContentRect = UIroot.GetComponent<RectTransform>();
            ContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25);
            UIFactory.SetLayoutElement(UIroot, minHeight: 25, flexibleHeight: 9999, minWidth: 100, flexibleWidth: 5000);
            //m_UIroot.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ContentGroup = UIFactory.CreateVerticalGroup(UIroot, "ConfigHolder", false, false, true, true, 5, new Vector4(2, 2, 5, 5),
                new Color(0.12f, 0.12f, 0.12f));
            //ContentGroup.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var horiGroup = UIFactory.CreateHorizontalGroup(ContentGroup, "ConfigEntryHolder", false, false, true, true,
                5, default, new Color(1, 1, 1, 0), TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(horiGroup, minHeight: 30, flexibleHeight: 0);

            // config entry label

            mainLabel = UIFactory.CreateLabel(horiGroup, "ConfigLabel", this.RefConfig.DisplayName, TextAnchor.MiddleLeft, 
                new Color(0.7f, 1, 0.7f));
            mainLabel.text += $" <i>({SignatureHighlighter.Parse(RefConfig.GetReflectedType(), false)})</i>";
            UIFactory.SetLayoutElement(mainLabel.gameObject, minWidth: 200, minHeight: 22, flexibleWidth: 9999, flexibleHeight: 0);

            // Undo button

            undoButton = UIFactory.CreateButton(horiGroup, "UndoButton", "Undo", new Color(0.3f, 0.3f, 0.3f));
            undoButton.OnClick += UndoEdits;
            undoButton.Component.gameObject.SetActive(false);
            UIFactory.SetLayoutElement(undoButton.Component.gameObject, minWidth: 80, minHeight: 22, flexibleWidth: 0);

            // Default button

            var defaultButton = UIFactory.CreateButton(horiGroup, "DefaultButton", "Default", new Color(0.3f, 0.3f, 0.3f));
            defaultButton.OnClick += RevertToDefault;
            UIFactory.SetLayoutElement(defaultButton.Component.gameObject, minWidth: 80, minHeight: 22, flexibleWidth: 0);

            // Description label

            if (RefConfig.Description != null)
            {
                var desc = UIFactory.CreateLabel(ContentGroup, "Description", $"<i>{RefConfig.Description}</i>", TextAnchor.MiddleLeft, Color.grey);
                UIFactory.SetLayoutElement(desc.gameObject, minWidth: 250, minHeight: 18, flexibleWidth: 9999, flexibleHeight: 0);
            }

            // subcontent

            SubContentGroup = UIFactory.CreateVerticalGroup(ContentGroup, "CacheObjectBase.SubContent", true, false, true, true, 0, default,
                new Color(1, 1, 1, 0));
            UIFactory.SetLayoutElement(SubContentGroup, minHeight: 30, flexibleHeight: 9999, minWidth: 125, flexibleWidth: 9000);

            SubContentGroup.SetActive(false);

            // setup IValue references

            if (IValue != null)
            {
                IValue.mainContentParent = ContentGroup;
                IValue.subContentParent = this.SubContentGroup;
            }
        }
    }
}
