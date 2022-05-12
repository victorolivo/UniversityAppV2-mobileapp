using System;
using Xamarin.Forms;
using UniversityAppV2.Services;
using UniversityAppV2.Models;
using System.Threading.Tasks;

namespace UniversityAppV2
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();

            termList.ItemsSource = TermsDB.GetAllTerms();
        }

        private void AddTermBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddModTerm(false, null));
        }

        private void termList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            ((ListView)sender).SelectedItem = null;
        }

        public async void TermsRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            termList.ItemsSource = TermsDB.GetAllTerms();
            TermsRefreshView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            termList.ItemsSource = TermsDB.GetAllTerms();
            base.OnAppearing();
        }

        private void ModifyTerm_Clicked(object sender, EventArgs e)
        {
            var term = ((MenuItem)sender).BindingContext as Term;
            Navigation.PushAsync(new AddModTerm(true, term));

        }

        private void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var term = ((MenuItem)sender).BindingContext as Term;

            TermsDB.RemoveTerm(term.Id);

            TermsRefreshView_Refreshing(sender, e);
        }

        private async void termList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var term = ((ListView)sender).SelectedItem as Term;
            if (term == null)
                return;
            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new CoursesPage(term));
        }
    }
}
