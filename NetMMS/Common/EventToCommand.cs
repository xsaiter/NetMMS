using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace NetMMS.Common {
    public class EventToCommand : TriggerAction<DependencyObject> {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null));

        public ICommand Command {
            get {
                return (ICommand)GetValue(CommandProperty);
            }
            set {
                SetValue(CommandProperty, value);
            }
        }

        protected override void Invoke(object parameter) {
            if (Command != null && Command.CanExecute(parameter)) {
                Command.Execute(parameter);
            }
        }
    }
}
