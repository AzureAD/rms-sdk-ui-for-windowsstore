
namespace Microsoft.RightsManagement.UILibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Windows.UI.Xaml;

    /// <summary>
    /// Permission is used to populate UI controls (GridView or ListView) using the binding mechanism of the Xaml framework.
    /// </summary>
    internal class Permission : INotifyPropertyChanged
    {
        public Permission(String right, bool granted)
        {
            _right = right;
            _granted = granted;
            try
            {
                _rightDisplayName = RightsToBeDisplayed[right];
            }
            catch (Exception)
            {
                _rightDisplayName = right;
            }

            NotifyPropertyChanged("Right");
            NotifyPropertyChanged("DisplayName");
            NotifyPropertyChanged("GrantedVisibility");
            NotifyPropertyChanged("NotGrantedVisibility");
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", _granted, DisplayName);
        }

        /// <summary>
        /// The underline right of the permission
        /// </summary>
        public String Right
        {
            get
            {
                return _right;
            }

            set
            {
                _right = value;
                NotifyPropertyChanged("Right");
            }
        }

        /// <summary>
        /// Display name of the permission
        /// </summary>
        public String DisplayName
        {
            get
            {
                return _rightDisplayName;
            }

            set
            {
                _rightDisplayName = value;
                NotifyPropertyChanged("DisplayName");
            }
        }

        /// <summary>
        /// Indicates if the "granted" version of the permission should be shown.
        /// </summary>
        public Visibility GrantedVisibility
        {
            get
            {
                return _granted ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Indicates if the "not granted" version of the permission should be shown.
        /// </summary>
        public Visibility NotGrantedVisibility
        {
            get
            {
                return !_granted ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a list of permissions using the specified list of supported rights and the specified protection policy
        /// </summary>
        /// <param name="supportedRights">The supported rights</param>
        /// <param name="policy">The current protection policy</param>
        /// <param name="permissions">The output permissions collection</param>
        public static void CreateListFromPolicy(IEnumerable<string> supportedRights, UserPolicy policy, IList<Permission> permissions)
        {
            permissions.Clear();

            if (policy == null || policy.AccessCheck(CommonRights.Owner))
            {
                // If this is the owner, show only the owner right
                permissions.Add(new Permission(CommonRights.Owner, true));
            }
            else
            {
                foreach (var right in supportedRights)
                {
                    // Don't show the owner right
                    if (String.Compare(right, CommonRights.Owner, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        permissions.Add(new Permission(right, policy.AccessCheck(right)));
                    }
                }
            }
        }

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        readonly private bool _granted = false;
        private String _rightDisplayName = null;
        private String _right = null;

        static private readonly Dictionary<string, string>  RightsToBeDisplayed = new Dictionary<string, string>();

        static Permission()
        {
            RightsToBeDisplayed.Add(EditableDocumentRights.Edit, LocalizedStrings.Get("EditRightText"));
            RightsToBeDisplayed.Add(EditableDocumentRights.Extract, LocalizedStrings.Get("ExtractRightText"));
            RightsToBeDisplayed.Add(EditableDocumentRights.Print, LocalizedStrings.Get("PrintRightText"));
            RightsToBeDisplayed.Add(CommonRights.Owner, LocalizedStrings.Get("OwnerRightText"));
            RightsToBeDisplayed.Add(CommonRights.View, LocalizedStrings.Get("ViewRightText"));
        }
    }

}
