﻿

#pragma checksum "C:\Users\umara\OneDrive\[GU WORK]\Development\Gamer Desk\GamerDesk 0.90\GamerDesk\pAddGame.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2275296E642527393E684B52E7B2BB7B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GamerDesk
{
    partial class classAddGame : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 223 "..\..\pAddGame.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.lstFoundGames_Tapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 210 "..\..\pAddGame.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnAddGame_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 182 "..\..\pAddGame.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnPathBrowse_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 136 "..\..\pAddGame.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnImgSelect_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 96 "..\..\pAddGame.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnSearchGame_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

