using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SSCEOfflineRegSchApp.Tools
{
    public class ClickableHeaderListView
    {
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public void Click(UserControl page, object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction = ListSortDirection.Ascending;
            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                }

                var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                var lv = sender as ListView;
                Sort(lv, sortBy, direction);

                if (direction == ListSortDirection.Ascending)
                {
                    headerClicked.Column.HeaderTemplate =
                      page.Resources["HeaderTemplateArrowUp"] as DataTemplate;
                }
                else
                {
                    headerClicked.Column.HeaderTemplate =
                      page.Resources["HeaderTemplateArrowDown"] as DataTemplate;
                }

                // Remove arrow from previously sorted header  
                if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                {
                    _lastHeaderClicked.Column.HeaderTemplate = null;
                }

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }

        public void WClick(Window page, object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction = ListSortDirection.Ascending;
            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                }

                var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                var lv = sender as ListView;
                Sort(lv, sortBy, direction);

                if (direction == ListSortDirection.Ascending)
                {
                    headerClicked.Column.HeaderTemplate =
                      page.Resources["HeaderTemplateArrowUp"] as DataTemplate;
                }
                else
                {
                    headerClicked.Column.HeaderTemplate =
                      page.Resources["HeaderTemplateArrowDown"] as DataTemplate;
                }

                // Remove arrow from previously sorted header  
                if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                {
                    _lastHeaderClicked.Column.HeaderTemplate = null;
                }

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }

        public static void Sort(ListView lv, string sortBy, ListSortDirection direction)
        {
            try
            {
                ICollectionView dataView =
             CollectionViewSource.GetDefaultView(lv.ItemsSource);

                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();
            }
            catch (Exception ex)
            {
               SafeGuiWpf.ShowWarning(ex.Message);
            }
        }
    }
}
