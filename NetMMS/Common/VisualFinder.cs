using System;
using System.Windows;
using System.Windows.Media;

namespace NetMMS.Common {
    public static class VisualFinder {
        public static T GetAncestor<T>(DependencyObject obj) where T : DependencyObject {
            if (obj == null) {
                throw new ArgumentNullException("currentObject");
            }
            do {
                if (obj is T) {
                    return (T)obj;
                }
                obj = VisualTreeHelper.GetParent(obj);
            } while (obj != null);
            return null;
        }

        public static T FindChild<T>(DependencyObject depObj, string childName) where T : DependencyObject {
            if (depObj == null) {
                return null;
            }
            if (depObj is T && ((FrameworkElement)depObj).Name == childName) {
                return depObj as T;
            }
            var n = VisualTreeHelper.GetChildrenCount(depObj);
            for (int i = 0; i < n; i++) {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var obj = FindChild<T>(child, childName);
                if (obj != null) {
                    return obj;
                }
            }
            return null;
        }
    }
}
