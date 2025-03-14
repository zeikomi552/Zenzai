using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zenzai.Common.Utilities;

namespace Zenzai.Common.Actions
{
    public class ClipboardCopyAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty CopyTextProperty =
        DependencyProperty.Register("CopyText", typeof(string), typeof(ClipboardCopyAction), new UIPropertyMetadata());

        public string CopyText
        {
            get { return (string)GetValue(CopyTextProperty); }
            set { SetValue(CopyTextProperty, value); }
        }

        protected override void Invoke(object obj)
        {
            try
            {
                if (CopyText != null)
                {
                    Clipboard.SetText(CopyText);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
    }
}
