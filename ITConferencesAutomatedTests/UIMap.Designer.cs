﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 14.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace ITConferencesAutomatedTests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public partial class UIMap
    {
        
        /// <summary>
        /// MainPageSearchTest - Use 'MainPageSearchTestParams' to pass parameters into this method.
        /// </summary>
        public void MainPageSearchTest()
        {
            #region Variable Declarations
            WinWindow uIHomePageMyASPNETApplWindow1 = this.UIHomePageMyASPNETApplWindow.UIHomePageMyASPNETApplWindow1;
            WinControl uIChromeLegacyWindowDocument = this.UIHomePageMyASPNETApplWindow.UIChromeLegacyWindowWindow.UIChromeLegacyWindowDocument;
            #endregion

            // Click 'Home Page - My ASP.NET Application - Google Chrome' window
            Mouse.Click(uIHomePageMyASPNETApplWindow1, new Point(874, 715));

            // Type 'conf' in 'Chrome Legacy Window' document
            Keyboard.SendKeys(uIChromeLegacyWindowDocument, this.MainPageSearchTestParams.UIChromeLegacyWindowDocumentSendKeys, ModifierKeys.None);

            // Click 'Home Page - My ASP.NET Application - Google Chrome' window
            Mouse.Click(uIHomePageMyASPNETApplWindow1, new Point(978, 718));

            // Click 'Home Page - My ASP.NET Application - Google Chrome' window
            Mouse.Click(uIHomePageMyASPNETApplWindow1, new Point(474, 360));
        }
        
        /// <summary>
        /// EnterSearchTest - Use 'EnterSearchTestParams' to pass parameters into this method.
        /// </summary>
        public void EnterSearchTest()
        {
            #region Variable Declarations
            WinWindow uIConferencesMyASPNETAWindow1 = this.UIHomePageMyASPNETApplWindow.UIConferencesMyASPNETAWindow1;
            WinControl uIChromeLegacyWindowDocument = this.UIHomePageMyASPNETApplWindow.UIChromeLegacyWindowWindow.UIChromeLegacyWindowDocument;
            #endregion

            // Click 'Conferences - My ASP.NET Application - Google Chro...' window
            Mouse.Click(uIConferencesMyASPNETAWindow1, new Point(513, 205));

            // Type '{Back}{Back}{Back}{Back}{Back}{Back}{Back}{Back}{Enter}' in 'Chrome Legacy Window' document
            Keyboard.SendKeys(uIChromeLegacyWindowDocument, this.EnterSearchTestParams.UIChromeLegacyWindowDocumentSendKeys, ModifierKeys.None);

            // Click 'Conferences - My ASP.NET Application - Google Chro...' window
            Mouse.Click(uIConferencesMyASPNETAWindow1, new Point(261, 357));
        }
        
        #region Properties
        public virtual MainPageSearchTestParams MainPageSearchTestParams
        {
            get
            {
                if ((this.mMainPageSearchTestParams == null))
                {
                    this.mMainPageSearchTestParams = new MainPageSearchTestParams();
                }
                return this.mMainPageSearchTestParams;
            }
        }
        
        public virtual EnterSearchTestParams EnterSearchTestParams
        {
            get
            {
                if ((this.mEnterSearchTestParams == null))
                {
                    this.mEnterSearchTestParams = new EnterSearchTestParams();
                }
                return this.mEnterSearchTestParams;
            }
        }
        
        public UIHomePageMyASPNETApplWindow UIHomePageMyASPNETApplWindow
        {
            get
            {
                if ((this.mUIHomePageMyASPNETApplWindow == null))
                {
                    this.mUIHomePageMyASPNETApplWindow = new UIHomePageMyASPNETApplWindow();
                }
                return this.mUIHomePageMyASPNETApplWindow;
            }
        }
        #endregion
        
        #region Fields
        private MainPageSearchTestParams mMainPageSearchTestParams;
        
        private EnterSearchTestParams mEnterSearchTestParams;
        
        private UIHomePageMyASPNETApplWindow mUIHomePageMyASPNETApplWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'MainPageSearchTest'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class MainPageSearchTestParams
    {
        
        #region Fields
        /// <summary>
        /// Type 'conf' in 'Chrome Legacy Window' document
        /// </summary>
        public string UIChromeLegacyWindowDocumentSendKeys = "conf";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'EnterSearchTest'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class EnterSearchTestParams
    {
        
        #region Fields
        /// <summary>
        /// Type '{Back}{Back}{Back}{Back}{Back}{Back}{Back}{Back}{Enter}' in 'Chrome Legacy Window' document
        /// </summary>
        public string UIChromeLegacyWindowDocumentSendKeys = "{Back}{Back}{Back}{Back}{Back}{Back}{Back}{Back}{Enter}";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIHomePageMyASPNETApplWindow : WinWindow
    {
        
        public UIHomePageMyASPNETApplWindow()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Home Page - My ASP.NET Application - Google Chrome";
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "Chrome_WidgetWin_1";
            this.WindowTitles.Add("Home Page - My ASP.NET Application - Google Chrome");
            this.WindowTitles.Add("Conferences - My ASP.NET Application - Google Chrome");
            #endregion
        }
        
        #region Properties
        public WinWindow UIHomePageMyASPNETApplWindow1
        {
            get
            {
                if ((this.mUIHomePageMyASPNETApplWindow1 == null))
                {
                    this.mUIHomePageMyASPNETApplWindow1 = new WinWindow(this);
                    #region Search Criteria
                    this.mUIHomePageMyASPNETApplWindow1.SearchProperties[WinWindow.PropertyNames.Name] = "Home Page - My ASP.NET Application - Google Chrome";
                    this.mUIHomePageMyASPNETApplWindow1.SearchConfigurations.Add(SearchConfiguration.DisambiguateChild);
                    this.mUIHomePageMyASPNETApplWindow1.WindowTitles.Add("Home Page - My ASP.NET Application - Google Chrome");
                    this.mUIHomePageMyASPNETApplWindow1.WindowTitles.Add("Conferences - My ASP.NET Application - Google Chrome");
                    #endregion
                }
                return this.mUIHomePageMyASPNETApplWindow1;
            }
        }
        
        public UIChromeLegacyWindowWindow UIChromeLegacyWindowWindow
        {
            get
            {
                if ((this.mUIChromeLegacyWindowWindow == null))
                {
                    this.mUIChromeLegacyWindowWindow = new UIChromeLegacyWindowWindow(this);
                }
                return this.mUIChromeLegacyWindowWindow;
            }
        }
        
        public WinWindow UIConferencesMyASPNETAWindow1
        {
            get
            {
                if ((this.mUIConferencesMyASPNETAWindow1 == null))
                {
                    this.mUIConferencesMyASPNETAWindow1 = new WinWindow(this);
                    #region Search Criteria
                    this.mUIConferencesMyASPNETAWindow1.SearchProperties[WinWindow.PropertyNames.Name] = "Conferences - My ASP.NET Application - Google Chrome";
                    this.mUIConferencesMyASPNETAWindow1.SearchConfigurations.Add(SearchConfiguration.DisambiguateChild);
                    this.mUIConferencesMyASPNETAWindow1.WindowTitles.Add("Conferences - My ASP.NET Application - Google Chrome");
                    #endregion
                }
                return this.mUIConferencesMyASPNETAWindow1;
            }
        }
        #endregion
        
        #region Fields
        private WinWindow mUIHomePageMyASPNETApplWindow1;
        
        private UIChromeLegacyWindowWindow mUIChromeLegacyWindowWindow;
        
        private WinWindow mUIConferencesMyASPNETAWindow1;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIChromeLegacyWindowWindow : WinWindow
    {
        
        public UIChromeLegacyWindowWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlId] = "587458624";
            this.WindowTitles.Add("Home Page - My ASP.NET Application - Google Chrome");
            #endregion
        }
        
        #region Properties
        public WinControl UIChromeLegacyWindowDocument
        {
            get
            {
                if ((this.mUIChromeLegacyWindowDocument == null))
                {
                    this.mUIChromeLegacyWindowDocument = new WinControl(this);
                    #region Search Criteria
                    this.mUIChromeLegacyWindowDocument.SearchProperties[UITestControl.PropertyNames.ControlType] = "Document";
                    this.mUIChromeLegacyWindowDocument.WindowTitles.Add("Home Page - My ASP.NET Application - Google Chrome");
                    #endregion
                }
                return this.mUIChromeLegacyWindowDocument;
            }
        }
        #endregion
        
        #region Fields
        private WinControl mUIChromeLegacyWindowDocument;
        #endregion
    }
}
