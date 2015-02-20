using System.Collections.Generic;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Microsoft.RightsManagement.UILibrary
{
    public sealed partial class PermissionsFlyout : ProtectionFlyout
    {
        private bool _isOpen = false;
        static private readonly List<string> SupportedRights = new List<string>();
        private List<Permission> _permissions;
        private UserPolicy _policy;

        static PermissionsFlyout()
        {
            SupportedRights.Add(CommonRights.View);
            // For demonstration purposes, rights are not necessarily enforced / used in this sample.
            SupportedRights.Add(EditableDocumentRights.Edit); 
            SupportedRights.Add(EditableDocumentRights.Print); 
            SupportedRights.Add(EditableDocumentRights.Extract);
        }

        public PermissionsFlyout()
        {
            InitializeComponent();
            Closed += PermissionsFlyout_Closed;
        }

        public UserPolicy Policy
        {
            get
            {
                return _policy;
            }

            set
            {
                _policy = value;

                if (_policy == null)
                {
                    Hide();
                }
                else
                {
                    PopulatePermissions();
                }
            }
        }


        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }

            set
            {
                if (value && _policy != null)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }

        // Note: FxCop for managed code complains that the event arguments are not derived from EventArgs. 
        // WinRT APIs use RoutedEventArgs (PolicyChangedEventArgs is derived from RoutedEventArgs), which is not derived from EventArgs.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]

        private new void Show()
        {
            base.Show();
            _isOpen = true;

            this.UpdateGrantedByTextBlockWidth();
        }

        private new void Hide()
        {
            base.Hide();
            _isOpen = false;
        }

        private void PopulatePermissions()
        {
            if (_policy == null)
            {
                Hide();
                return;
            }

            if (_policy.AccessCheck(CommonRights.Owner))
            {
                _permissionsCommentTextBlock.Text = LocalizedStrings.Get("PermissionsPopupOwnerComment");
                _templateNameTextBlock.Text = _policy.Name;
                _templateDescriptionTextBlock.Text = _policy.Description;
                _grantedByStackPanel.Visibility = Visibility.Collapsed;
                _permissionsListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                _permissionsCommentTextBlock.Text = LocalizedStrings.Get("PermissionsPopupNonOwnerComment");
                _templateNameTextBlock.Text = _policy.Name;
                _templateDescriptionTextBlock.Text = _policy.Description;
                _grantedByStackPanel.Visibility = Visibility.Visible;
                _grantedByTextBlock.Text = _policy.Owner;
                _permissionsListView.Visibility = Visibility.Visible;

                _permissions = new List<Permission>();
                Permission.CreateListFromPolicy(SupportedRights, _policy, _permissions);
                _permissionsListView.ItemsSource = _permissions;
            }

            UpdateGrantedByTextBlockWidth();
        }

        private void UpdateGrantedByTextBlockWidth()
        {
            // Set the granted by text block width such that it will get truncated properly
            _grantedByTextBlock.MaxWidth = FlyoutWidth
                                           - UIUtils.ControlMarginWidth(_grantedByTextBlock)
                                           - UIUtils.ControlSize(_grantedByLabelTextBlock).Width;
        }

        private void PermissionsFlyout_Closed(object sender, RoutedEventArgs e)
        {
            _isOpen = false;
        }
    }
}
